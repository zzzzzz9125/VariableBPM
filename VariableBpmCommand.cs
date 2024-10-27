#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System;
using System.Collections.Generic;
using System.Linq;

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
            VariableBpmCmd.SetIconFile("VariableBpm.png");
            CustomCommands.Add(VariableBpmCmd);

            CustomCommand cmdManual = new CustomCommand(CommandCategory.Tools, "VariableBpm_Manual") { DisplayName = L.VariableBpmManualCmd, CanAddToMenu = false, CanAddToKeybindings = true };
            cmdManual.Invoked += delegate (object o, EventArgs e) { myVegas.RefreshBpmList(true); };
            cmdManual.SetIconFile("VariableBpm.png");
            CustomCommands.Add(cmdManual);

#if TEST
            CustomCommand cmdTest = new CustomCommand(CommandCategory.Tools, "VariableBpm_Test");
            cmdTest.Invoked += delegate (object o, EventArgs e)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    myVegas.ShowError(ex);
                }
            };
            cmdTest.SetIconFile("VariableBpm.png");
            CustomCommands.Add(cmdTest);
#endif

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
                if (VariableBpmCommon.Settings.RippleForMarkersChoice != 2)
                {
                    List<Marker> list = new List<Marker>();
                    if (VariableBpmCommon.Settings.RippleForMarkersChoice == 1)
                    {
                        list.AddRange(myVegas.Project.Markers);
                    }
                    else
                    {
                        list = VariableBpmCommon.CurrentBpmPointList.Markers;
                    }
                    VariableBpmCommon.RippleForMarkers(list);
                    VariableBpmCommon.RippleMarkersSave = MarkerInfoList.GetFrom(list);
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