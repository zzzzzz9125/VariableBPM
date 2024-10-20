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
                    VariableBpm = "�ɱ� BPM"; VariableBpmManualCmd = "�ɱ� BPM - BPM �����ֶ�ˢ��"; VariableBpmAuto = "�Զ�ˢ��"; VariableBpmManual = "BPM �����ֶ�ˢ��"; Metronome = "������";
                    FileGroup = "�ٶȱ�ǵ���/����"; SelectFile = "ѡ���ļ�"; ImportFromFile = "���ļ�����"; ImportStart = "�������"; ImportEnd = "�����յ�"; ToProjectPosition = "����Ŀλ��"; Import = "����"; ExportToFile = "�������ļ�"; Export = "����"; Settings = "����"; Interval = "�Զ�ˢ�¼���� (ms)"; AutoStart = "����ʱĬ�������Զ�ˢ��"; MidiCompatibilityMode = "MIDI ������ģʽ";
                    ChangeBpmTo = "���� BPM �� {0}"; InsertFromFile = "�� {0} ���� BPM"; RippleForMarkers = "����Զ�����";
                    FileDialogFilters = new string[]
                    {
                        "MIDI �ļ�|*.mid;*.midi|����б��ļ�|*.json|�����ļ�|*.*",
                        "MIDI �ļ�|*.mid;*.midi|����б��ļ�|*.json"
                    };
                    FileImportMessages = new string[]
                    {
                        "�ļ�����ʧ�ܣ��ļ� {0} �����ڣ�",
                        "�ļ�����ʧ�ܣ��ļ� {0} ���ǿɵ�����ļ����͡�",
                        "��ѡ�� MIDI �ļ�����֧�֣��޷�������ȡ���������������е� MIDI ������ģʽ���ٳ��Ե��롣���ߵ����� DAW �����µ������ٵ��롣����γ��ָ����⣬���ύ������"
                    };
                    FileExportMessages = new string[]
                    {
                        "�ļ������ɹ����Ƿ�������ļ��У�",
                        "�ļ�����ʧ�ܣ���ȷ��·���Ϸ�������Ŀ�д��� BPM ��ǣ�"
                    };
                    ImportRangeMidiStartType = new string[] { "MIDI ��ʼλ��", "��һ������", "�Զ���" };
                    ImportRangeMidiEndType = new string[] { "MIDI ��ֹλ��", "���һ������", "�Զ���" };
                    ToProjectType = new string[] { "��ʼλ��", "���λ��" };
                    ClearRangeType = new string[] { "�������ȫ�����", "��������뷶Χ�ڵ�", "�����" };
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