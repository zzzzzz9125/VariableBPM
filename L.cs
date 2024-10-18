namespace VariableBpm
{
    public static class L
    {
        public static string Font, VariableBpm, VariableBpmManualCmd,  VariableBpmAuto, VariableBpmManual, FileGroup, ImportFrom, ExportTo, Settings, Interval, AutoStart, ChangeBpmTo, InsertFromFile, RippleForMarkers;
        public static string[] FileDialogFilters, FileImportErrors;

        // Some text localization.
        public static void Localize()
        {
            switch (VariableBpmCommon.Settings.CurrentLanguage)
            {
                case "zh":
                    Font = "Microsoft Yahei UI";
                    VariableBpm = "可变 BPM"; VariableBpmManualCmd = "可变 BPM - BPM 网格手动刷新"; VariableBpmAuto = "启用 BPM 网格自动刷新功能"; VariableBpmManual = "BPM 网格手动刷新";
                    FileGroup = "速度标记导入/导出"; ImportFrom = "从文件导入"; ExportTo = "导出到文件"; Settings = "设置"; Interval = "检测间隔 (ms)"; AutoStart = "启动时默认启用自动刷新";
                    ChangeBpmTo = "更改 BPM 至 {0}"; InsertFromFile = "从 {0} 插入 BPM"; RippleForMarkers = "标记自动跟进";
                    FileDialogFilters = new string[]
                    {
                        "MIDI 文件|*.mid;*.midi|标记列表文件|*.json|所有文件|*.*",
                        "MIDI 文件|*.mid;*.midi|标记列表文件|*.json"
                    };
                    FileImportErrors = new string[]
                    {
                        "文件导入失败！文件 {0} 不存在！",
                        "文件导入失败！文件 {0} 不是可导入的文件类型。"
                    };

                    break;

                default:
                    Font = "Arial";
                    VariableBpm = "Variable BPM"; VariableBpmManualCmd = "Variable BPM - BPM Grid Manual Refresh"; VariableBpmAuto = "Enable BPM Grid Auto Refresh"; VariableBpmManual = "BPM Grid Manual Refresh";
                    FileGroup = "Tempo Marker Import/Export"; ImportFrom = "Import From Files"; ExportTo = "Export To Files"; Settings = "Settings"; Interval = "Detection Interval (ms)"; AutoStart = "Enable Auto Refresh When Starting";
                    ChangeBpmTo = "Change BPM To {0}"; InsertFromFile = "Insert BPM From {0}"; RippleForMarkers = "Auto Ripple For Markers";
                    FileDialogFilters = new string[]
                    {
                        "MIDI files|*.mid;*.midi|MarkerList files|*.json|All files|*.*",
                        "MIDI files|*.mid;*.midi|MarkerList files|*.json"
                    };
                    FileImportErrors = new string[]
                    {
                        "File Import Failed! File {0} Not Exists!",
                        "File Import Failed! File {0} isn't an importable file type."
                    };
                    break;
            }
        }
    }
}