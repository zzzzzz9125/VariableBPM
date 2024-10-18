#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Linq;

namespace VariableBpm
{
    public enum FileType { NotSupported, Midi, MarkerInfoList };

    public static class VariableBpmCommon
    {
        public static BpmPointList CurrentBpmPointList;
        public static Timer BpmTimer;
        public static BpmPoint PointSave;
        public static bool Enable = false, Rippling = false, Activated = true;
        public static MarkerInfoList RippleMarkersSave = null;
        public static VariableBpmSettings Settings = new VariableBpmSettings().LoadFromFile();


        public static void RippleForMarkers(this Vegas myVegas)
        {
            if (Rippling)
            {
                return;
            }

            BaseMarkerList<Marker> markers = myVegas.Project.Markers;

            if (RippleMarkersSave?.Count != markers.Count)
            {
                return;
            }

            int index = -1;
            for (int i = 0; i < markers.Count; i++)
            {
                if (markers[i].Label != RippleMarkersSave[i].Label)
                {
                    return;
                }

                if (markers[i].Position != RippleMarkersSave[i].Position)
                {
                    index = i;
                }
            }

            if (index < 0)
            {
                return;
            }

            bool enableSave = Enable;
            Enable = false;
            Rippling = true;

            using (UndoBlock undo = new UndoBlock(L.RippleForMarkers))
            {
                for (int i = index + 1; i < markers.Count; i++)
                {
                    markers[i].Position += markers[index].Position - RippleMarkersSave[index].Position;
                }
            }

            Enable = enableSave;
            Rippling = false;
        }

        public static void RefreshBpmList(this Vegas myVegas, bool manual = false)
        {
            if (BpmTimer != null && !(manual && Enable))
            {
                BpmTimer.Stop();
                BpmTimer.Dispose();
                BpmTimer = null;
                GC.Collect();
            }

            if (!Enable && !manual)
            {
                return;
            }

            BpmPointList list = GetBpmPointList(myVegas.Project.Markers);

            // Avoid problems that user can't undo. When user undos, MarkersChanged will be also called once, causing a conflict.
            if (manual || !list.IsTheSame(CurrentBpmPointList))
            {
                CurrentBpmPointList = list;
                myVegas.RefreshBpmGrid();
            }

            if (!manual)
            {
                BpmTimer = new Timer() { Interval = Settings.Interval };
                BpmTimer.Tick += delegate (object o, EventArgs e) { myVegas.RefreshBpmGrid(); };
                BpmTimer.Start();
            }
        }

        public static BpmPointList GetBpmPointList(BaseMarkerList<Marker> markers)
        {
            BpmPointList list = new BpmPointList();
            Timecode offset = new Timecode(0);
            uint beats = 4;

            foreach (Marker m in markers)
            {
                string s = m.Label.ToUpper();
                if (!s.Contains("BPM") && !s.Contains("£Â£Ð£Í"))
                {
                    continue;
                }

                s = s.Replace("BPM", "").Replace("£Â£Ð£Í", "");

                string offsetStr = Regex.Match(s, @"[+-] *\d+(\.\d+){0,2}").Value;
                if (!string.IsNullOrEmpty(offsetStr))
                {
                    s = s.Replace(offsetStr, "");

                    switch (offsetStr.Split('.').Length)
                    {
                        case 1:
                            offsetStr += ".0.000";
                            break;

                        case 2:
                            offsetStr += ".000";
                            break;

                        default:
                            break;
                    }
                }

                string meterStr = Regex.Match(s, @"BEATS *= *[1-9]\d*").Value;
                if (!string.IsNullOrEmpty(meterStr) && uint.TryParse(Regex.Match(meterStr, @"(?<=BEATS *= *)[1-9]\d*").Value, out uint tmp))
                {
                    beats = tmp;
                    s = s.Replace(meterStr, "");
                }

                if (double.TryParse(s.Replace("RESET", "").Replace(" ", "").Replace("=", "").Replace(",", "").Replace("£¬", ""), out double n))
                {
                    bool reset = s.Contains("RESET");
                    if (reset) { offset = new Timecode(0); }
                    offset += string.IsNullOrEmpty(offsetStr) ? new Timecode(0) : Timecode.FromString(offsetStr, RulerFormat.MeasuresAndBeats, false);
                    list.Add(new BpmPoint(m.Position, n, offset.ToString(RulerFormat.MeasuresAndBeats), reset, beats, m));
                }
            }

            list.SetStartTime();

            return list;
        }

        public static void ImportMarkersFrom(this Vegas myVegas, string filePath, FileType type)
        {
            List<Marker> markerList = new List<Marker>();
            switch (type)
            {
                case FileType.Midi:
                    MidiFile midi = MidiFile.Read(filePath);
                    TempoMap tempoMap = midi.GetTempoMap();

                    Timecode offset = new Timecode(0);
                    foreach (ValueChange<Tempo> change in tempoMap.GetTempoChanges())
                    {
                        markerList.Add(new Marker(new Timecode(TimeConverter.ConvertTo<MetricTimeSpan>(change.Time, tempoMap).TotalMilliseconds) + offset,
                                                  string.Format("BPM = {0}", change.Value.BeatsPerMinute)));
                    }
                    break;

                case FileType.MarkerInfoList:
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        List<MarkerInfo> infos = JsonConvert.DeserializeObject<List<MarkerInfo>>(reader.ReadToEnd());
                        if (infos != null)
                        {
                            foreach (MarkerInfo info in infos)
                            {
                                markerList.Add(new Marker(Timecode.FromSeconds(info?.Seconds ?? 0), info?.Label));
                            }
                        }
                    }
                    break;

                default: return;

            }


            if (markerList.Count == 0)
            {
                return;
            }

            using (UndoBlock undo = new UndoBlock(string.Format(L.InsertFromFile, Path.GetFileName(filePath))))
            {
                foreach (BpmPoint p in GetBpmPointList(myVegas.Project.Markers))
                {
                    if (p.Marker != null && p.Marker.IsValid())
                    {
                        myVegas.Project.Markers.Remove(p.Marker);
                    }
                }

                foreach (Marker m in markerList)
                {
                    myVegas.Project.Markers.Add(m);
                }

                myVegas.RefreshBpmList(true);
            }
        }


        public static void ExportMarkersTo(string filePath, FileType type)
        {
            if (CurrentBpmPointList.Count == 0)
            {
                return;
            }

            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            switch (type)
            {
                case FileType.Midi:
                    MidiFile midi = new MidiFile(new TrackChunk());

                    using (TempoMapManager tempoMapManager = new TempoMapManager(midi.TimeDivision, midi.GetTrackChunks().Select(c => c.Events)))
                    {
                        TempoMap tempoMap = tempoMapManager.TempoMap;
                        foreach (BpmPoint p in CurrentBpmPointList)
                        {
                            tempoMapManager.SetTempo(new MetricTimeSpan((p.Position - CurrentBpmPointList[0].Position).Nanos / 10), Tempo.FromBeatsPerMinute(p.Bpm));
                        }
                    }
                    midi.Write(filePath, true);

                    break;

                case FileType.MarkerInfoList:
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(CurrentBpmPointList.MarkerInfos, Formatting.Indented));
                        fileStream.Write(buffer, 0, buffer.Length);
                    }
                    break;

                default: return;
            }


        }

        public static BpmPoint GetCursorBpmPoint(this Vegas myVegas)
        {
            if (CurrentBpmPointList == null || CurrentBpmPointList.Count == 0)
            {
                return null;
            }

            int j = CurrentBpmPointList.Count - 1;

            for (int i = 0; i < CurrentBpmPointList.Count; i++)
            {
                if (CurrentBpmPointList[i].Position > myVegas.Transport.PlayCursorPosition)
                {
                    j = i - 1;
                    break;
                }
            }

            j = Math.Max(0, j);

            if (CurrentBpmPointList[j].Bpm == 0)
            {
                int m = j;
                do { m--; } while (m > 0 && CurrentBpmPointList[m].Bpm == 0);
                if (CurrentBpmPointList[m].Bpm == 0)
                {
                    m = j;
                    do { m++; } while (m < CurrentBpmPointList.Count - 1 && CurrentBpmPointList[m].Bpm == 0);
                }
                j = m;
            }

            return CurrentBpmPointList[j];
        }
        public static void RefreshBpmGrid(this Vegas myVegas)
        {
            BpmPoint cursor = myVegas.GetCursorBpmPoint();

            if (cursor != null && cursor.Bpm != 0 && PointSave != cursor)
            {
                using (UndoBlock undo = new UndoBlock(string.Format(L.ChangeBpmTo, Math.Round(cursor.Bpm, 3))))
                {
                    myVegas.UpdateBpmGrid(cursor);
                }
                PointSave = cursor;
            }
        }

        public static void UpdateBpmGrid(this Vegas myVegas,BpmPoint p)
        {
            myVegas.Project.Ruler.Format = RulerFormat.MeasuresAndBeats;
            myVegas.Project.Ruler.BeatsPerMinute = p.Bpm;
            myVegas.Project.Ruler.BeatsPerMeasure = p.Beats;
            myVegas.Project.Ruler.StartTime = p.StartTime + (string.IsNullOrEmpty(p.Offset) ? new Timecode(0) : Timecode.FromString(p.Offset, RulerFormat.MeasuresAndBeats));
        }
    }
}