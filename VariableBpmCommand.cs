#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System;
using System.Collections.Generic;


namespace VariableBpm
{
	internal class VariableBpmCommand
    {
		private Vegas myVegas;
		private readonly CustomCommand VariableBpmCmd = new CustomCommand(CommandCategory.Tools, "VariableBpm");

		internal void VariableBpmInit(Vegas Vegas, ref List<CustomCommand> CustomCommands)
		{
			myVegas = Vegas;
            L.Localize();
            VariableBpmCmd.DisplayName = L.VariableBpm;
            VariableBpmCmd.Invoked += VariableBpm_Invoked;
            VariableBpmCmd.MenuPopup += VariableBpmCommand_MenuPopup;
			CustomCommands.Add(VariableBpmCmd);

            CustomCommand cmdManual = new CustomCommand(CommandCategory.Tools, "VariableBpm_Manual") { DisplayName = L.VariableBpmManualCmd };
            cmdManual.Invoked += delegate (object o, EventArgs e) { myVegas.RefreshBpmList(true); };
            CustomCommands.Add(cmdManual);

            if (VariableBpmCommon.Settings.AutoStart)
            {
                myVegas.ProjectOpened += delegate (object o, EventArgs e)
                {
                    // When Vegas app is closed, strangely enough, ProjectOpened will be called again.
                    // Therefore, it's necessary to use Activated to determine whether Vegas is still active.
                    if (VariableBpmCommon.Activated)
                    {
                        VariableBpmCommon.Enable = true;
                        myVegas.RefreshBpmList();
                    }
                };
            }

            myVegas.ProjectClosed += delegate (object o, EventArgs e)
            {
                if (VariableBpmCommon.BpmTimer != null)
                {
                    VariableBpmCommon.BpmTimer.Stop();
                    VariableBpmCommon.BpmTimer.Dispose();
                    VariableBpmCommon.BpmTimer = null;
                    GC.Collect();
                }
            };

            myVegas.AppActivated += delegate (object o, EventArgs e) { VariableBpmCommon.Activated = true; };
            myVegas.AppDeactivate += delegate (object o, EventArgs e) { VariableBpmCommon.Activated = false; };

            myVegas.MarkersChanged += delegate (object o, EventArgs e)
            {
                if (VariableBpmCommon.Settings.RippleForMarkers)
                {
                    myVegas.RippleForMarkers();
                    VariableBpmCommon.RippleMarkersSave = MarkerInfoList.GetFrom(myVegas.Project.Markers);
                }

                myVegas.RefreshBpmList();
            };
        }

		private void VariableBpmCommand_MenuPopup(object sender, EventArgs e)
		{
            ((CustomCommand)sender).Checked = myVegas.FindDockView("VariableBpm");
		}

		private void VariableBpm_Invoked(object sender, EventArgs e)
		{
			if (!myVegas.ActivateDockView("VariableBpm"))
			{
				myVegas.LoadDockView(new VariableBpm { AutoLoadCommand = VariableBpmCmd });
			}
		}
	}
}