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
                    VariableBpm = "�ɱ� BPM"; VariableBpmManualCmd = "�ɱ� BPM - BPM �����ֶ�ˢ��"; VariableBpmAuto = "���� BPM �����Զ�ˢ�¹���"; VariableBpmManual = "BPM �����ֶ�ˢ��";
                    FileGroup = "�ٶȱ�ǵ���/����"; ImportFrom = "���ļ�����"; ExportTo = "�������ļ�"; Settings = "����"; Interval = "����� (ms)"; AutoStart = "����ʱĬ�������Զ�ˢ��";
                    ChangeBpmTo = "���� BPM �� {0}"; InsertFromFile = "�� {0} ���� BPM"; RippleForMarkers = "����Զ�����";
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