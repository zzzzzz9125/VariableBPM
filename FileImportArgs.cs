#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif


using System.IO;
using System.Windows.Forms;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace VariableBpm
{
    public enum FileType { NotSupported, Midi, MarkerInfoList };

    public class FileImportArgs
    {
        public string FilePath
        {
            get { return path; }
            set
            {
                if (!File.Exists(value))
                {
                    MessageBox.Show(string.Format(L.FileImportMessages[0], value));
                    return;
                }
                switch (Path.GetExtension(value))
                {
                    case ".mid":
                    case ".midi":
                        ReadingSettings readingSettings = VariableBpmCommon.Settings.MidiCompatibilityMode ? new ReadingSettings
                        {
                            InvalidChannelEventParameterValuePolicy = InvalidChannelEventParameterValuePolicy.ReadValid,
                            InvalidChunkSizePolicy = InvalidChunkSizePolicy.Ignore,
                            InvalidMetaEventParameterValuePolicy = InvalidMetaEventParameterValuePolicy.SnapToLimits,
                            MissedEndOfTrackPolicy = MissedEndOfTrackPolicy.Ignore,
                            NoHeaderChunkPolicy = NoHeaderChunkPolicy.Ignore,
                            NotEnoughBytesPolicy = NotEnoughBytesPolicy.Ignore,
                            UnexpectedTrackChunksCountPolicy = UnexpectedTrackChunksCountPolicy.Ignore,
                            UnknownChannelEventPolicy = UnknownChannelEventPolicy.SkipStatusByteAndOneDataByte,
                            UnknownChunkIdPolicy = UnknownChunkIdPolicy.ReadAsUnknownChunk,
                        } : new ReadingSettings();
                        try
                        {
                            Midi = MidiFile.Read(value, readingSettings);
                            FileType = FileType.Midi;
                        }

                        catch
                        {
                            MessageBox.Show(L.FileImportMessages[2]);
                        }
                        break;

                    case ".json":
                        FileType = FileType.MarkerInfoList;
                        break;

                    default:
                        MessageBox.Show(L.FileImportMessages[1]);
                        return;
                }
                path = value;
            }
        }

        private string path;

        public FileType FileType;
        public Timecode ToProjectPosition = new Timecode(0);
        public BarBeatFractionTimeSpan MidiImportStart, MidiImportEnd;
        public int ClearRangeChoice;
        public MidiFile Midi;

        public void GetParameters(Timecode toProjectPosition, int clearRangeChoice, string midiImportStartStr, string midiImportEndStr)
        {
            ToProjectPosition = toProjectPosition;
            ClearRangeChoice = clearRangeChoice;
            MidiImportStart = midiImportStartStr.ConvertToBarBeatFraction(true);
            MidiImportEnd = midiImportEndStr.ConvertToBarBeatFraction(true);
        }
    }
}
