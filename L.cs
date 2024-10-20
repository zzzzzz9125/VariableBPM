namespace VariableBpm
{
    public static class L
    {
        public static string Font, VariableBpm, VariableBpmManualCmd, VariableBpmAuto, VariableBpmManual, Metronome, FileGroup, SelectFile, ImportFromFile, ImportStart, ImportEnd, ToProjectPosition, Import, ExportToFile, Export, Settings, Interval, AutoStart, MidiCompatibilityMode, ChangeBpmTo, InsertFromFile, RippleForMarkers;
        public static string[] FileDialogFilters, FileImportMessages, FileExportMessages, ImportRangeMidiStartType, ImportRangeMidiEndType, ToProjectType, ClearRangeType;

        // Some text localization.
        public static void Localize()
        {
            switch (VariableBpmCommon.Settings.CurrentLanguage)
            {
                case "zh":
                    Font = "Microsoft Yahei UI";
                    VariableBpm = "可变 BPM"; VariableBpmManualCmd = "可变 BPM - BPM 网格手动刷新"; VariableBpmAuto = "自动刷新"; VariableBpmManual = "BPM 网格手动刷新"; Metronome = "节拍器";
                    FileGroup = "速度标记导入/导出"; SelectFile = "选择文件"; ImportFromFile = "从文件导入"; ImportStart = "导入起点"; ImportEnd = "导入终点"; ToProjectPosition = "到项目位置"; Import = "导入"; ExportToFile = "导出到文件"; Export = "导出"; Settings = "设置"; Interval = "自动刷新检测间隔 (ms)"; AutoStart = "启动时默认启用自动刷新"; MidiCompatibilityMode = "MIDI 最大兼容模式";
                    ChangeBpmTo = "更改 BPM 至 {0}"; InsertFromFile = "从 {0} 插入 BPM"; RippleForMarkers = "标记自动跟进";
                    FileDialogFilters = new string[]
                    {
                        "MIDI 文件|*.mid;*.midi|标记列表文件|*.json|所有文件|*.*",
                        "MIDI 文件|*.mid;*.midi|标记列表文件|*.json"
                    };
                    FileImportMessages = new string[]
                    {
                        "文件导入失败！文件 {0} 不存在！",
                        "文件导入失败！文件 {0} 不是可导入的文件类型。",
                        "所选的 MIDI 文件不受支持，无法正常读取。可以启用设置中的 MIDI 最大兼容模式，再尝试导入。或者到其他 DAW 内重新导出后再导入。若多次出现该问题，请提交反馈。"
                    };
                    FileExportMessages = new string[]
                    {
                        "文件导出成功！是否打开所在文件夹？",
                        "文件导出失败！请确认路径合法，且项目中存在 BPM 标记！"
                    };
                    ImportRangeMidiStartType = new string[] { "MIDI 起始位置", "第一个音符", "自定义" };
                    ImportRangeMidiEndType = new string[] { "MIDI 终止位置", "最后一个音符", "自定义" };
                    ToProjectType = new string[] { "起始位置", "光标位置" };
                    ClearRangeType = new string[] { "清除已有全部标记", "仅清除导入范围内的", "不清除" };
                    break;

                default:
                    Font = "Arial";
                    VariableBpm = "Variable BPM"; VariableBpmManualCmd = "Variable BPM - BPM Grid Manual Refresh"; VariableBpmAuto = "Auto Refresh"; VariableBpmManual = "BPM Grid Manual Refresh"; Metronome = "Metronome";
                    FileGroup = "Tempo Markers Import/Export"; SelectFile = "Select File"; ImportFromFile = "Import From"; ImportStart = "Start Point"; ImportEnd = "End Point"; ToProjectPosition = "To Project"; Import = "Import"; ExportToFile = "Export To"; Export = "Export"; Settings = "Settings"; Interval = "Auto Detection Interval (ms)"; AutoStart = "Enable Auto Refresh When Starting"; MidiCompatibilityMode = "MIDI Max Compatibility Mode";
                    ChangeBpmTo = "Change BPM To {0}"; InsertFromFile = "Insert BPM From {0}"; RippleForMarkers = "Auto Ripple For Markers";
                    FileDialogFilters = new string[]
                    {
                        "MIDI files|*.mid;*.midi|MarkerList files|*.json|All files|*.*",
                        "MIDI files|*.mid;*.midi|MarkerList files|*.json"
                    };
                    FileImportMessages = new string[]
                    {
                        "File Import Failed! File {0} Not Exists!",
                        "File Import Failed! File {0} isn't an importable file type.",
                        "The selected MIDI file is unsupported and cannot be read properly. You can enable MIDI Max Compatibility Mode in Settings and try importing again. Or you can export it from another DAW and import it again. If this problem occurs several times, please submit an issue."
                    };
                    FileExportMessages = new string[]
                    {
                        "File Export Success! Do you want to open the folder?",
                        "File Export Failed! Please check that the file path is valid and that BPM Markers exist in this project!"
                    };
                    ImportRangeMidiStartType = new string[] { "MIDI Start", "First Note", "Custom" };
                    ImportRangeMidiEndType = new string[] { "MIDI End", "Last Note", "Custom" };
                    ToProjectType = new string[] { "Start Position", "Cursor Position" };
                    ClearRangeType = new string[] { "Clear All Existing Markers", "Clear Only Those Within Range", "Do Not Clear" };
                    break;
            }
        }
    }
}