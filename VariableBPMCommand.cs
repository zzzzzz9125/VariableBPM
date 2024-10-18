#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System;
using System.Collections.Generic;
using System.Drawing;
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

    public class VariableBpmCommand
    {
        private Vegas myVegas;
        BpmPointList bpmPointList;
        Timer BpmTimer;
        BpmPoint PointSave;
        bool Enable = false, Rippling = false;
        bool Activated = true;
        MarkerInfoList RippleMarkersSave = null;

        internal void VariableBpm(Vegas Vegas, CommandCategory category, ref List<CustomCommand> CustomCommands)
        {
            myVegas = Vegas;
            L.Localize();

            CustomCommand cmd = new CustomCommand(category, "VariableBPM") { DisplayName = Enable ? L.VariableBpmDisable : L.VariableBpmEnable };
            cmd.Invoked += delegate (object o, EventArgs e) { Enable = !Enable; cmd.DisplayName = Enable ? L.VariableBpmDisable : L.VariableBpmEnable; RefreshBpmList(); };
            CustomCommands.Add(cmd);

            CustomCommand cmdManual = new CustomCommand(category, "VariableBPM_Manual") { DisplayName = L.VariableBpmManual };
            cmdManual.Invoked += delegate (object o, EventArgs e) { RefreshBpmList(true); };
            CustomCommands.Add(cmdManual);

            CustomCommand cmdImport = new CustomCommand(category, "VariableBPM_Import") { DisplayName = L.VariableBpmImport };
            cmdImport.Invoked += delegate (object o, EventArgs e)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog { Filter = L.FileDialogFilters[0] };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(openFileDialog.FileName))
                    {
                        MessageBox.Show(L.FileImportErrors[0]);
                        return;
                    }

                    bool enableSave = Enable;
                    Enable = false;
                    FileType type = FileType.NotSupported;
                    switch (openFileDialog.FilterIndex)
                    {
                        case 1:
                            type = FileType.Midi;
                            break;

                        case 2:
                            type = FileType.MarkerInfoList;
                            break;

                        default:
                            switch (Path.GetExtension(openFileDialog.FileName).ToLower())
                            {
                                case ".mid":
                                case ".midi":
                                    type = FileType.Midi;
                                    break;

                                case ".json":
                                    type = FileType.MarkerInfoList;
                                    break;

                                default:
                                    MessageBox.Show(L.FileImportErrors[1]);
                                    break;
                            }
                            break;
                    }
                    ImportMarkersFrom(openFileDialog.FileName, type);
                    Enable = enableSave;
                }
            };
            CustomCommands.Add(cmdImport);

            CustomCommand cmdExport = new CustomCommand(category, "VariableBPM_Export") { DisplayName = L.VariableBpmExport };
            cmdExport.Invoked += delegate (object o, EventArgs e)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = L.FileDialogFilters[1], FileName = Path.GetFileNameWithoutExtension(myVegas.Project.FilePath) ?? "Untitled" };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileType type = FileType.NotSupported;
                    switch (saveFileDialog.FilterIndex)
                    {
                        case 1:
                            type = FileType.Midi;
                            break;

                        case 2:
                            type = FileType.MarkerInfoList;
                            break;

                        default:
                            break;
                    }
                    ExportMarkersTo(saveFileDialog.FileName, type);
                }
            };
            CustomCommands.Add(cmdExport);

            if (Common.Settings.AutoStart)
            {
                myVegas.ProjectOpened += delegate (object o, EventArgs e)
                {
                    // When Vegas app is closed, strangely enough, ProjectOpened will be called again.
                    // Therefore, it's necessary to use Activated to determine whether Vegas is still active.
                    if (Activated)
                    {
                        Enable = true;
                        cmd.DisplayName = Enable ? L.VariableBpmDisable : L.VariableBpmEnable;
                        RefreshBpmList();
                    }
                };
            }

            myVegas.ProjectClosed += delegate (object o, EventArgs e)
            {
                if (BpmTimer != null)
                {
                    BpmTimer.Stop();
                    BpmTimer.Dispose();
                    BpmTimer = null;
                    GC.Collect();
                }
            };

            myVegas.AppActivated += delegate (object o, EventArgs e) { Activated = true; };
            myVegas.AppDeactivate += delegate (object o, EventArgs e) { Activated = false; };

            CustomCommand cmdSettings = new CustomCommand(category, "VariableBPMSettings") { DisplayName = L.VariableBpmSettings };
            cmdSettings.Invoked += delegate (object o, EventArgs e)
            {
                Color[] colors = myVegas.GetColors();
                Form form = new Form
                {
                    ShowInTaskbar = false,
                    AutoSize = true,
                    BackColor = colors[0],
                    ForeColor = colors[1],
                    Font = new Font(L.Font, 9),
                    Text = L.VariableBpmSettings,
                    FormBorderStyle = FormBorderStyle.FixedToolWindow,
                    StartPosition = FormStartPosition.CenterScreen,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };

                Panel p = new Panel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };
                form.Controls.Add(p);

                TableLayoutPanel l = new TableLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                    ColumnCount = 2
                };
                l.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
                l.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
                p.Controls.Add(l);

                Label label = new Label
                {
                    Margin = new Padding(6, 10, 0, 6),
                    Text = L.Interval,
                    AutoSize = true
                };
                l.Controls.Add(label);

                int interval = Common.Settings.Interval;
                TextBox intervalBox = new TextBox
                {
                    AutoSize = true,
                    Margin = new Padding(9, 6, 6, 6),
                    Text = interval.ToString()
                };
                l.Controls.Add(intervalBox);

                CheckBox autoStartBox = new CheckBox
                {
                    Text = L.AutoStart,
                    Margin = new Padding(6, 3, 0, 3),
                    AutoSize = true,
                    Checked = Common.Settings.AutoStart
                };

                l.Controls.Add(autoStartBox);
                l.SetColumnSpan(autoStartBox, 2);

                CheckBox autoRippleBox = new CheckBox
                {
                    Text = L.RippleForMarkers,
                    Margin = new Padding(6, 3, 0, 3),
                    AutoSize = true,
                    Checked = Common.Settings.RippleForMarkers
                };

                l.Controls.Add(autoRippleBox);
                l.SetColumnSpan(autoRippleBox, 2);

                FlowLayoutPanel panel = new FlowLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Anchor = AnchorStyles.None,
                    Font = new Font(L.Font, 8)
                };
                l.Controls.Add(panel);
                l.SetColumnSpan(panel, 2);

                Button ok = new Button
                {
                    Text = L.OK,
                    DialogResult = DialogResult.OK
                };
                panel.Controls.Add(ok);
                form.AcceptButton = ok;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Common.Settings.AutoStart = autoStartBox.Checked;
                    Common.Settings.RippleForMarkers = autoRippleBox.Checked;

                    if (int.TryParse(intervalBox.Text, out int inter) && inter > 0 && inter != interval)
                    {
                        Common.Settings.Interval = inter;
                        if (Enable)
                        {
                            RefreshBpmList();
                        }
                    }
                    Common.Settings.SaveToFile();
                }
            };
            CustomCommands.Add(cmdSettings);

            myVegas.MarkersChanged += delegate (object o, EventArgs e)
            {
                if (Common.Settings.RippleForMarkers)
                {
                    RippleForMarkers();
                    RippleMarkersSave = MarkerInfoList.GetFrom(myVegas.Project.Markers);
                }

                RefreshBpmList();
            };
        }

        public void RippleForMarkers()
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
                    markers[index].Position += markers[index].Position - RippleMarkersSave[index].Position;
                }
            }

            Enable = enableSave;
            Rippling = false;
        }

        public void RefreshBpmList(bool manual = false)
        {
            if (BpmTimer != null)
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
            if (manual || !list.IsTheSame(bpmPointList))
            {
                bpmPointList = list;
                RefreshBpmGrid();
            }

            if (!manual)
            {
                BpmTimer = new Timer() { Interval = Common.Settings.Interval };
                BpmTimer.Tick += delegate (object o, EventArgs e) { RefreshBpmGrid(); };
                BpmTimer.Start();
            }
        }

        public BpmPointList GetBpmPointList(BaseMarkerList<Marker> markers)
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

        public void ImportMarkersFrom(string filePath, FileType type)
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

                RefreshBpmList(true);
            }
        }


        public void ExportMarkersTo(string filePath, FileType type)
        {
            if (bpmPointList.Count == 0)
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
                        foreach (BpmPoint p in bpmPointList)
                        {
                            tempoMapManager.SetTempo(new MetricTimeSpan((p.Position - bpmPointList[0].Position).Nanos / 10), Tempo.FromBeatsPerMinute(p.Bpm));
                        }
                    }
                    midi.Write(filePath, true);

                    break;

                case FileType.MarkerInfoList:
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(bpmPointList.MarkerInfos, Formatting.Indented));
                        fileStream.Write(buffer, 0, buffer.Length);
                    }
                    break;

                default: return;
            }


        }

        public BpmPoint GetCursorBpmPoint()
        {
            if (bpmPointList == null || bpmPointList.Count == 0 || myVegas == null)
            {
                return null;
            }

            int j = bpmPointList.Count - 1;

            for (int i = 0; i < bpmPointList.Count; i++)
            {
                if (bpmPointList[i].Position > myVegas.Transport.PlayCursorPosition)
                {
                    j = i - 1;
                    break;
                }
            }

            j = Math.Max(0, j);

            if (bpmPointList[j].Bpm == 0)
            {
                int m = j;
                do { m--; } while (m > 0 && bpmPointList[m].Bpm == 0);
                if (bpmPointList[m].Bpm == 0)
                {
                    m = j;
                    do { m++; } while (m < bpmPointList.Count - 1 && bpmPointList[m].Bpm == 0);
                }
                j = m;
            }

            return bpmPointList[j];
        }
        public void RefreshBpmGrid()
        {
            BpmPoint cursor = GetCursorBpmPoint();

            if (cursor != null && cursor.Bpm != 0 && PointSave != cursor)
            {
                using (UndoBlock undo = new UndoBlock(string.Format(L.ChangeBpmTo, cursor.Bpm)))
                {
                    UpdateBpmGrid(cursor);
                }
                PointSave = cursor;
            }
        }

        public void UpdateBpmGrid(BpmPoint p)
        {
            myVegas.Project.Ruler.Format = RulerFormat.MeasuresAndBeats;
            myVegas.Project.Ruler.BeatsPerMinute = p.Bpm;
            myVegas.Project.Ruler.BeatsPerMeasure = p.Beats;
            myVegas.Project.Ruler.StartTime = p.StartTime + (string.IsNullOrEmpty(p.Offset) ? new Timecode(0) : Timecode.FromString(p.Offset, RulerFormat.MeasuresAndBeats));
        }
    }
}