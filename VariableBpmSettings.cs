using System.IO;
using Newtonsoft.Json;

namespace VariableBpm
{
    public class VariableBpmSettings
    {
        public string CurrentLanguage = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        public int Interval = 1;
        public bool AutoStart = false, RippleForMarkers = false, MidiCompatibilityMode = false;
        public int[] ImportMidiRangeChoices = new int[] { 0, 0 };
        public int ToProjectChoice = 0, ClearRangeChoice = 0;

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