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
                    VariableBpmManual = "�ɱ� BPM - �ֶ�"; VariableBpmImport = "�ɱ� BPM - ���ļ�����"; VariableBpmExport = "�ɱ� BPM - �������ļ�"; VariableBpmSettings = "�ɱ� BPM - ����"; Interval = "��� (ms)"; AutoStart = "��������ʱĬ������"; OK = "ȷ��";
                    VariableBpmEnable = "�ɱ� BPM - ����"; VariableBpmDisable = "�ɱ� BPM - ����"; ChangeBpmTo = "�ı� BPM �� {0}";
                    InsertFromFile = "�� {0} ���� BPM"; RippleForMarkers = "����Զ�����";
                    FileDialogFilters = new string[]
                    {
                        "MIDI �ļ�|*.mid;*.midi|����б��ļ�|*.json|�����ļ�|*.*",
                        "MIDI �ļ�|*.mid;*.midi|����б��ļ�|*.json"
                    };
                    FileImportErrors = new string[]
                    {
                        "�ļ�����ʧ�ܣ��ļ� {0} �����ڣ�",
                        "�ļ�����ʧ�ܣ��ļ� {0} ���ǿɵ�����ļ����͡�"
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