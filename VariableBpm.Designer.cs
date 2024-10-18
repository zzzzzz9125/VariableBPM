#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System.Drawing;
using System.Windows.Forms;
using System;
using System.IO;
using System.Runtime;

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
            this.DisplayName = L.VariableBpm;
            this.Font = new Font(L.Font, 9);

            TableLayoutPanel l = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                ColumnCount = 2,
                Dock = DockStyle.Fill
            };
            l.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            l.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.Controls.Add(l);

            CheckBox autoBox = new CheckBox
            {
                Text =  L.VariableBpmAuto,
                Margin = new Padding(6, 3, 0, 3),
                AutoSize = true,
                Checked = VariableBpmCommon.Enable
            };

            l.Controls.Add(autoBox);
            l.SetColumnSpan(autoBox, 2);

            autoBox.CheckedChanged += delegate (object o, EventArgs e)
            {
                VariableBpmCommon.Enable = autoBox.Checked;
                myVegas.RefreshBpmList();
                SetFocusToMainTrackView();
            };

            Button manualButton = new Button
            {
                Text = L.VariableBpmManual,
                Margin = new Padding(0, 3, 0, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat
            };
            manualButton.FlatAppearance.BorderSize = 0;
            l.Controls.Add(manualButton);
            l.SetColumnSpan(manualButton, 2);

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
                ColumnCount = 2
            };
            filePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            filePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fileGroup.Controls.Add(filePanel);

            Button importButton = new Button
            {
                Text = L.ImportFrom,
                Margin = new Padding(0, 3, 0, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat
            };
            importButton.FlatAppearance.BorderSize = 0;
            filePanel.Controls.Add(importButton);

            importButton.Click += delegate (object o, EventArgs e)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog { Filter = L.FileDialogFilters[0] };
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(openFileDialog.FileName))
                    {
                        MessageBox.Show(L.FileImportErrors[0]);
                        return;
                    }

                    bool enableSave = VariableBpmCommon.Enable;
                    VariableBpmCommon.Enable = false;
                    FileType type = FileType.NotSupported;
                    switch (openFileDialog.FilterIndex)
                    {
                        case 1:
                            type = FileType.Midi;
                            break;

                        case 2:
                            type = FileType.MarkerInfoList;
                            break;

                        default:
                            switch (Path.GetExtension(openFileDialog.FileName).ToLower())
                            {
                                case ".mid":
                                case ".midi":
                                    type = FileType.Midi;
                                    break;

                                case ".json":
                                    type = FileType.MarkerInfoList;
                                    break;

                                default:
                                    MessageBox.Show(L.FileImportErrors[1]);
                                    break;
                            }
                            break;
                    }
                    myVegas.ImportMarkersFrom(openFileDialog.FileName, type);
                    VariableBpmCommon.Enable = enableSave;
                    SetFocusToMainTrackView();
                }
            };

            Button exportButton = new Button
            {
                Text = L.ExportTo,
                Margin = new Padding(0, 3, 0, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat
            };
            exportButton.FlatAppearance.BorderSize = 0;
            filePanel.Controls.Add(exportButton);

            exportButton.Click += delegate (object o, EventArgs e)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = L.FileDialogFilters[1], FileName = Path.GetFileNameWithoutExtension(myVegas.Project.FilePath) ?? "Untitled" };
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileType type = FileType.NotSupported;
                    switch (saveFileDialog.FilterIndex)
                    {
                        case 1:
                            type = FileType.Midi;
                            break;

                        case 2:
                            type = FileType.MarkerInfoList;
                            break;

                        default:
                            break;
                    }
                    VariableBpmCommon.ExportMarkersTo(saveFileDialog.FileName, type);
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

            Label label = new Label
            {
                Margin = new Padding(6, 10, 0, 6),
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