#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace VariableBpm
{
    public static class Common
    {
        public static int VegasVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileMajorPart;
        public static string AppFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        public static string SettingsFolder = Path.Combine(VegasVersion < 14 ? Path.Combine(AppFolder, "Sony") : AppFolder, "VEGAS Pro", VegasVersion + ".0");
        public static VariableBpmSettings Settings = new VariableBpmSettings().LoadFromFile();

        public static Color[] GetColors(this Vegas myVegas)
        {
            int darkModeType = 0;
            if (darkModeType < 0 || darkModeType > 4)
            {
                darkModeType = 0;
            }

            int myVegasVersion = VegasVersion;
#if !Sony
            if (darkModeType == 0 && myVegasVersion > 14)
            {
                darkModeType = myVegas.GetDarkType() + 1;
            }
#endif
            if (myVegasVersion > 18)
            {
                darkModeType += 4;
            }

            int[,] colorValues = new int[9, 2]
            {
            // {back, fore}
                {153, 25} , // Earlier, for Vegas Pro 13 - 14
                {45, 220} , // DarkEarly, for Vegas Pro 15 - 18
                {94, 220} , // MediumEarly
                {146, 29} , // LightEarly
                {210, 35} , // WhiteEarly
                {34, 220} , // Dark, for Vegas Pro 19 - 22
                {68, 255} , // Medium
                {187, 17} , // Light
                {238, 51}   // White
            };

            return new Color[] { Color.FromArgb(colorValues[darkModeType, 0], colorValues[darkModeType, 0], colorValues[darkModeType, 0]), Color.FromArgb(colorValues[darkModeType, 1], colorValues[darkModeType, 1], colorValues[darkModeType, 1]) };
        }

#if !Sony
        // not supported in older versions, so I used a single method separately
        public static int GetDarkType(this Vegas myVegas)
        {
            myVegas.GetInterfaceType(out InterfaceType type);
            return (int)type;
        }
#endif

    }
}