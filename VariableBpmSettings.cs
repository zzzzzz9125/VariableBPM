using System.IO;
using Newtonsoft.Json;

namespace VariableBpm
{
    [JsonObject(MemberSerialization.OptOut)]
    public class VariableBpmSettings
    {
        public string CurrentLanguage = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        public int Interval = 1;
        public bool AutoStart = false, MidiCompatibilityMode = false;
        public int[] ImportMidiRangeChoices = new int[] { 0, 0 };

        public int AutoLogicChoice
        {
            get { return Common.VegasVersion < 19 ? 0 : autoLogicChoice; }
            set { autoLogicChoice = Common.VegasVersion < 19 ? 0 : value; }
        }

        public int ToProjectChoice = 0, ClearRangeChoice = 0, RippleForMarkersChoice = 0;

        private int autoLogicChoice = Common.VegasVersion < 19 ? 0 : 1;

        public static VariableBpmSettings LoadFromFile(string filePath = null)
        {
            filePath = filePath ?? Path.Combine(Common.SettingsFolder, "VariableBpmSettings.json");
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return JsonConvert.DeserializeObject<VariableBpmSettings>(reader.ReadToEnd());
                }
            }
            else
            {
                return new VariableBpmSettings();
            }
        }

        public void SaveToFile(string filePath = null)
        {
            filePath = filePath ?? Path.Combine(Common.SettingsFolder, "VariableBpmSettings.json");
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(this, Formatting.Indented));
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}