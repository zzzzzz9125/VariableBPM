#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace VariableBpm
{
	sealed partial class VariableBpm
    {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

        private void InitializeComponent()
		{
            Color[] colors = myVegas.GetColors();
            this.SuspendLayout();

            this.AutoScaleMode = AutoScaleMode.Font;
            this.MinimumSize = new Size(433, 172);
            this.BackColor = colors[0];
            this.ForeColor = colors[1];
            this.DisplayName = string.Format("{0} {1}", L.VariableBpm, VariableBpmCommon.VERSION);
            this.Font = new Font(L.Font, 9);

            TableLayoutPanel l = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                ColumnCount = 2,
                Dock = DockStyle.Fill
            };
            l.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            l.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            this.Controls.Add(l);

            Button manualButton = new Button
            {
                Text = L.VariableBpmManual,
                Margin = new Padding(3, 5, 0, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat
            };
            manualButton.FlatAppearance.BorderSize = 1;
            manualButton.FlatAppearance.BorderColor = Color.FromArgb(127, 127, 127);
            l.Controls.Add(manualButton);

            CheckBox autoBox = new CheckBox
            {
                Text = L.VariableBpmAuto,
                Margin = new Padding(6, 9, 6, 3),
                AutoSize = true,
                Checked = VariableBpmCommon.Settings.AutoStart
            };

            l.Controls.Add(autoBox);

            autoBox.CheckedChanged += delegate (object o, EventArgs e)
            {
                VariableBpmCommon.Enable = autoBox.Checked;
                myVegas.RefreshBpmList();
                SetFocusToMainTrackView();
            };

            manualButton.Click += delegate (object o, EventArgs e)
            {
                myVegas.RefreshBpmList(true);
                SetFocusToMainTrackView();
            };

            GroupBox fileGroup = new GroupBox
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill,
                Text = L.FileGroup,
                ForeColor = colors[1]
            };

            l.Controls.Add(fileGroup);
            l.SetColumnSpan(fileGroup, 2);

            TableLayoutPanel filePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                Anchor = (AnchorStyles.Top | AnchorStyles.Left),
                GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                ColumnCount = 3
            };
            filePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            filePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            filePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            fileGroup.Controls.Add(filePanel);

            Label label = new Label
            {
                Margin = new Padding(6, 9, 6, 6),
                Text = L.ImportFromFile,
                AutoSize = true
            };
            filePanel.Controls.Add(label);

            TextBox importPathBox = new TextBox
            {
                AutoSize = true,
                Margin = new Padding(9, 6, 11, 6),
                Text = "",
                Dock = DockStyle.Fill
            };
            filePanel.Controls.Add(importPathBox);

            Button importButton = new Button
            {
                Text = L.Import,
                Margin = new Padding(0, 3, 0, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.None
            };
            importButton.FlatAppearance.BorderSize = 1;
            importButton.FlatAppearance.BorderColor = Color.FromArgb(127, 127, 127);
            filePanel.Controls.Add(importButton);

            FileImportArgs importArgs = new FileImportArgs();
            string[] midiStartEnd = importArgs.Midi.GetMidiStartEnd();

            label = new Label
            {
                Margin = new Padding(6, 9, 6, 6),
                Text = L.ImportStart,
                AutoSize = true
            };
            filePanel.Controls.Add(label);

            ComboBox importRangeMidiStartBox = new ComboBox
            {
                AutoSize = true,
                Margin = new Padding(9, 6, 11, 6),
                DataSource = L.ImportRangeMidiStartType,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill
            };
            filePanel.Controls.Add(importRangeMidiStartBox);

            TextBox startBox = new TextBox
            {
                AutoSize = true,
                Margin = new Padding(9, 6, 11, 6),
                Text = midiStartEnd[0],
                Dock = DockStyle.Fill
            };
            filePanel.Controls.Add(startBox);

            label = new Label
            {
                Margin = new Padding(6, 9, 6, 6),
                Text = L.ImportEnd,
                AutoSize = true
            };
            filePanel.Controls.Add(label);

            ComboBox importRangeMidiEndBox = new ComboBox
            {
                AutoSize = true,
                Margin = new Padding(9, 6, 11, 6),
                DataSource = L.ImportRangeMidiEndType,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill
            };
            filePanel.Controls.Add(importRangeMidiEndBox);

            TextBox endBox = new TextBox
            {
                AutoSize = true,
                Margin = new Padding(9, 6, 11, 6),
                Text = midiStartEnd[1],
                Dock = DockStyle.Fill
            };
            filePanel.Controls.Add(endBox);

            importPathBox.Click += delegate (object o, EventArgs e)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = L.FileDialogFilters[0],
                    InitialDirectory = string.IsNullOrEmpty(importPathBox.Text) ? "" : Path.GetDirectoryName(importPathBox.Text),
                    FileName = Path.GetFileName(importPathBox.Text) ?? ""
                };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    importPathBox.Text = openFileDialog.FileName;

                    importArgs.FilePath = importPathBox.Text;
                    midiStartEnd = importArgs.Midi.GetMidiStartEnd();
                    if (importRangeMidiStartBox.SelectedIndex < 2)
                    {
                        startBox.Text = midiStartEnd[importRangeMidiStartBox.SelectedIndex * 2];
                    }
                    if (importRangeMidiEndBox.SelectedIndex < 2)
                    {
                        endBox.Text = midiStartEnd[importRangeMidiEndBox.SelectedIndex * 2 + 1];
                    }
                }
            };

            int[] importMidiRangeChoices = VariableBpmCommon.Settings.ImportMidiRangeChoices;
            startBox.Enabled = importMidiRangeChoices[0] > 1;
            endBox.Enabled = importMidiRangeChoices[1] > 1;

            importRangeMidiStartBox.SelectedIndexChanged += delegate (object o, EventArgs e)
            {
                VariableBpmCommon.Settings.ImportMidiRangeChoices = new int[] { importRangeMidiStartBox.SelectedIndex, importRangeMidiEndBox.SelectedIndex };
                startBox.Enabled = importRangeMidiStartBox.SelectedIndex > 1;
                if (importRangeMidiStartBox.SelectedIndex < 2)
                {
                    startBox.Text = midiStartEnd[importRangeMidiStartBox.SelectedIndex * 2];
                }
            };

            importRangeMidiEndBox.SelectedIndexChanged += delegate (object o, EventArgs e)
            {
                VariableBpmCommon.Settings.ImportMidiRangeChoices = new int[] { importRangeMidiStartBox.SelectedIndex, importRangeMidiEndBox.SelectedIndex };
                endBox.Enabled = importRangeMidiEndBox.SelectedIndex > 1;
                if (importRangeMidiEndBox.SelectedIndex < 2)
                {
                    endBox.Text = midiStartEnd[importRangeMidiEndBox.SelectedIndex * 2 + 1];
                }
            };

            this.Load += delegate (object o, EventArgs e)
            {
                importRangeMidiStartBox.SelectedIndex = importMidiRangeChoices[0];
                importRangeMidiEndBox.SelectedIndex = importMidiRangeChoices[1];
            };

            label = new Label
            {
                Margin = new Padding(6, 9, 6, 6),
                Text = L.ToProjectPosition,
                AutoSize = true
            };
            filePanel.Controls.Add(label);

            ComboBox toProjectBox = new ComboBox
            {
                AutoSize = true,
                Margin = new Padding(9, 6, 11, 6),
                DataSource = L.ToProjectType,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill
            };
            filePanel.Controls.Add(toProjectBox);

            toProjectBox.SelectedIndexChanged += delegate (object o, EventArgs e)
            {
                VariableBpmCommon.Settings.ToProjectChoice = toProjectBox.SelectedIndex;
            };

            ComboBox clearRangeBox = new ComboBox
            {
                AutoSize = true,
                Margin = new Padding(9, 6, 11, 6),
                DataSource = L.ClearRangeType,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill
            };
            filePanel.Controls.Add(clearRangeBox);

            clearRangeBox.SelectedIndexChanged += delegate (object o, EventArgs e)
            {
                VariableBpmCommon.Settings.ClearRangeChoice = clearRangeBox.SelectedIndex;
            };

            this.Load += delegate (object o, EventArgs e)
            {
                toProjectBox.SelectedIndex = VariableBpmCommon.Settings.ToProjectChoice;
                clearRangeBox.SelectedIndex = VariableBpmCommon.Settings.ClearRangeChoice;
            };

            importButton.Click += delegate (object o, EventArgs e)
            {
                importArgs.FilePath = importPathBox.Text;
                importArgs.GetParameters(toProjectBox.SelectedIndex == 1 ? myVegas.Transport.PlayCursorPosition : new Timecode(0), clearRangeBox.SelectedIndex, startBox.Text, endBox.Text);

                bool enableSave = VariableBpmCommon.Enable;
                VariableBpmCommon.Enable = false;

                myVegas.ImportMarkersFrom(importArgs);
                VariableBpmCommon.Enable = enableSave;
                SetFocusToMainTrackView();
            };

            label = new Label
            {
                Margin = new Padding(6, 9, 6, 6),
                Text = L.ExportToFile,
                AutoSize = true
            };
            filePanel.Controls.Add(label);

            TextBox exportPathBox = new TextBox
            {
                AutoSize = true,
                Margin = new Padding(9, 6, 11, 6),
                Text = "",
                Dock = DockStyle.Fill
            };
            filePanel.Controls.Add(exportPathBox);

            exportPathBox.Click += delegate (object o, EventArgs e)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = L.FileDialogFilters[1],
                    InitialDirectory = string.IsNullOrEmpty(exportPathBox.Text) ? "" : Path.GetDirectoryName(exportPathBox.Text),
                    FileName = string.IsNullOrEmpty(exportPathBox.Text) ? Path.GetFileNameWithoutExtension(myVegas.Project.FilePath) ?? "Untitled" : Path.GetFileName(exportPathBox.Text) ?? ""
                };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    exportPathBox.Text = saveFileDialog.FileName;
                }
            };

            Button exportButton = new Button
            {
                Text = L.Export,
                Margin = new Padding(0, 3, 0, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.None
            };
            exportButton.FlatAppearance.BorderSize = 1;
            exportButton.FlatAppearance.BorderColor = Color.FromArgb(127, 127, 127);
            filePanel.Controls.Add(exportButton);

            exportButton.Click += delegate (object o, EventArgs e)
            {
                string path = exportPathBox.Text;
                FileType type = FileType.NotSupported;

                if (!string.IsNullOrEmpty(path))
                {
                    switch (Path.GetExtension(path).ToLower())
                    {
                        case ".mid":
                        case ".midi":
                            type = FileType.Midi;
                            break;

                        case ".json":
                            type = FileType.MarkerInfoList;
                            break;

                        default:
                            break;
                    }
                }

                if (myVegas.ExportMarkersTo(path, type))
                {
                    if (MessageBox.Show(L.FileExportMessages[0], "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Common.ExplorerFile(path);
                    }
                }
                else
                {
                    MessageBox.Show(L.FileExportMessages[1]);
                }
                SetFocusToMainTrackView();
            };

            GroupBox settingsGroup = new GroupBox
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill,
                Text = L.Settings,
                ForeColor = colors[1]
            };
            l.Controls.Add(settingsGroup);
            l.SetColumnSpan(settingsGroup, 2);

            TableLayoutPanel settingsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                Anchor = (AnchorStyles.Top | AnchorStyles.Left),
                GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                ColumnCount = 2
            };
            settingsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            settingsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            settingsGroup.Controls.Add(settingsPanel);

            label = new Label
            {
                Margin = new Padding(6, 9, 6, 6),
                Text = L.Interval,
                AutoSize = true
            };
            settingsPanel.Controls.Add(label);

            int interval = VariableBpmCommon.Settings.Interval;
            TextBox intervalBox = new TextBox
            {
                AutoSize = true,
                Margin = new Padding(9, 6, 6, 6),
                Text = interval.ToString()
            };
            settingsPanel.Controls.Add(intervalBox);

            intervalBox.TextChanged += delegate (object o, EventArgs e)
            {
                if (int.TryParse(intervalBox.Text, out int tmp) && tmp > 0)
                {
                    VariableBpmCommon.Settings.Interval = tmp;
                    if (VariableBpmCommon.BpmTimer != null)
                    {
                        VariableBpmCommon.BpmTimer.Interval = tmp;
                    }
                }
            };

            CheckBox autoStartBox = new CheckBox
            {
                Text = L.AutoStart,
                Margin = new Padding(6, 3, 0, 3),
                AutoSize = true,
                Checked = VariableBpmCommon.Settings.AutoStart
            };

            settingsPanel.Controls.Add(autoStartBox);
            settingsPanel.SetColumnSpan(autoStartBox, 2);

            autoStartBox.CheckedChanged += delegate (object o, EventArgs e)
            {
                VariableBpmCommon.Settings.AutoStart = autoStartBox.Checked;
                SetFocusToMainTrackView();
            };

            CheckBox autoRippleBox = new CheckBox
            {
                Text = L.RippleForMarkers,
                Margin = new Padding(6, 3, 0, 3),
                AutoSize = true,
                Checked = VariableBpmCommon.Settings.RippleForMarkers
            };
            
            settingsPanel.Controls.Add(autoRippleBox);
            settingsPanel.SetColumnSpan(autoRippleBox, 2);

            autoRippleBox.CheckedChanged += delegate (object o, EventArgs e)
            {
                VariableBpmCommon.Settings.RippleForMarkers = autoRippleBox.Checked;
                SetFocusToMainTrackView();
            };

            this.Closed += delegate (object o, EventArgs e)
            {
                VariableBpmCommon.Settings.SaveToFile();
            };

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}