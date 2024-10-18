namespace VariableBpm
{
    public static class L
    {
        public static string Font, VariableBpmManual, VariableBpmImport, VariableBpmExport, VariableBpmSettings, Interval, AutoStart, OK, VariableBpmEnable, VariableBpmDisable, ChangeBpmTo, InsertFromFile, RippleForMarkers;
        public static string[] FileDialogFilters, FileImportErrors;

        // Some text localization.
        public static void Localize()
        {
            switch (Common.Settings.CurrentLanguage)
            {
                case "zh":
                    Font = "Microsoft Yahei UI";
                    VariableBpmManual = "可变 BPM - 手动"; VariableBpmImport = "可变 BPM - 从文件导入"; VariableBpmExport = "可变 BPM - 导出到文件"; VariableBpmSettings = "可变 BPM - 设置"; Interval = "间隔 (ms)"; AutoStart = "启动工程时默认启用"; OK = "确定";
                    VariableBpmEnable = "可变 BPM - 启用"; VariableBpmDisable = "可变 BPM - 禁用"; ChangeBpmTo = "改变 BPM 至 {0}";
                    InsertFromFile = "从 {0} 插入 BPM"; RippleForMarkers = "标记自动跟进";
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
                    VariableBpmManual = "Variable BPM - Manually"; VariableBpmImport = "Variable BPM - Import From..."; VariableBpmExport = "Variable BPM - Export To..."; VariableBpmSettings = "Variable BPM Settings"; Interval = "Interval (ms)"; AutoStart = "Enabled When Project Starts"; OK = "OK";
                    VariableBpmEnable = "Variable BPM - Enable"; VariableBpmDisable = "Variable BPM - Disable"; ChangeBpmTo = "Change BPM To {0}";
                    InsertFromFile = "Insert BPM From {0}"; RippleForMarkers = "Auto Ripple For Markers";
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