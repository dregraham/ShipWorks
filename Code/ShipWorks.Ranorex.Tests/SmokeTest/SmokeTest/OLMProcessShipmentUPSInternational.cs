﻿///////////////////////////////////////////////////////////////////////////////
//
// This file was automatically generated by RANOREX.
// DO NOT MODIFY THIS FILE! It is regenerated by the designer.
// All your modifications will be lost!
// http://www.ranorex.com
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using Ranorex.Core.Repository;

namespace SmokeTest
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The OLMProcessShipmentUPSInternational recording.
    /// </summary>
    [TestModule("c083640a-c0cb-4f7f-bfcb-bb41a1cf6a41", ModuleType.Recording, 1)]
    public partial class OLMProcessShipmentUPSInternational : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static OLMProcessShipmentUPSInternational instance = new OLMProcessShipmentUPSInternational();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public OLMProcessShipmentUPSInternational()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static OLMProcessShipmentUPSInternational Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
        public static void Start()
        {
            TestModuleRunner.Run(Instance);
        }

        /// <summary>
        /// Performs the playback of actions in this recording.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.PanelDockingArea.PARTContentHost' at Center.", repo.ShipWorksSa.PanelDockingArea.PARTContentHostInfo, new RecordItemIndex(0));
            repo.ShipWorksSa.PanelDockingArea.PARTContentHost.MoveTo();
            Delay.Milliseconds(200);
            
            // Clicks the search field
            Report.Log(ReportLevel.Info, "Mouse", "Clicks the search field\r\nMouse Left Click item 'ShipWorksSa.PanelDockingArea.PARTContentHost' at Center.", repo.ShipWorksSa.PanelDockingArea.PARTContentHostInfo, new RecordItemIndex(1));
            repo.ShipWorksSa.PanelDockingArea.PARTContentHost.Click();
            Delay.Milliseconds(200);
            
            // Types the order number
            Report.Log(ReportLevel.Info, "Keyboard", "Types the order number\r\nKey sequence '9-M'.", new RecordItemIndex(2));
            Keyboard.Press("9-M");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.PanelDockingArea.Button' at Center.", repo.ShipWorksSa.PanelDockingArea.ButtonInfo, new RecordItemIndex(3));
            repo.ShipWorksSa.PanelDockingArea.Button.MoveTo();
            Delay.Milliseconds(0);
            
            // Clicks the search button
            Report.Log(ReportLevel.Info, "Mouse", "Clicks the search button\r\nMouse Left Click item 'ShipWorksSa.PanelDockingArea.Button' at Center.", repo.ShipWorksSa.PanelDockingArea.ButtonInfo, new RecordItemIndex(4));
            repo.ShipWorksSa.PanelDockingArea.Button.Click();
            Delay.Milliseconds(0);
            
            // Checks if the UPS Ground rate is displayed
            Report.Log(ReportLevel.Info, "Wait", "Checks if the UPS Ground rate is displayed\r\nWaiting 30s to exist. Associated repository item: 'ShipWorksSa.PanelDockingArea.UPSWorldwideExpressR'", repo.ShipWorksSa.PanelDockingArea.UPSWorldwideExpressRInfo, new ActionTimeout(30000), new RecordItemIndex(5));
            repo.ShipWorksSa.PanelDockingArea.UPSWorldwideExpressRInfo.WaitForExists(30000);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.PanelDockingArea.UPSWorldwideExpressR' at Center.", repo.ShipWorksSa.PanelDockingArea.UPSWorldwideExpressRInfo, new RecordItemIndex(6));
            repo.ShipWorksSa.PanelDockingArea.UPSWorldwideExpressR.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'MainForm.PanelDockingArea.OLMCreateLabel' at Center.", repo.MainForm.PanelDockingArea.OLMCreateLabelInfo, new RecordItemIndex(7));
            repo.MainForm.PanelDockingArea.OLMCreateLabel.MoveTo();
            Delay.Milliseconds(200);
            
            // Clicks create label
            Report.Log(ReportLevel.Info, "Mouse", "Clicks create label\r\nMouse Left Click item 'MainForm.PanelDockingArea.OLMCreateLabel' at Center.", repo.MainForm.PanelDockingArea.OLMCreateLabelInfo, new RecordItemIndex(8));
            repo.MainForm.PanelDockingArea.OLMCreateLabel.Click();
            Delay.Milliseconds(200);
            
            // Checks if the message is displayed
            Report.Log(ReportLevel.Info, "Wait", "Checks if the message is displayed\r\nWaiting 30s to exist. Associated repository item: 'MainForm.PanelDockingArea.ThisOrdersShipmentHasBeenProcessed'", repo.MainForm.PanelDockingArea.ThisOrdersShipmentHasBeenProcessedInfo, new ActionTimeout(30000), new RecordItemIndex(9));
            repo.MainForm.PanelDockingArea.ThisOrdersShipmentHasBeenProcessedInfo.WaitForExists(30000);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'MainForm.PanelDockingArea.ThisOrdersShipmentHasBeenProcessed' at CenterLeft.", repo.MainForm.PanelDockingArea.ThisOrdersShipmentHasBeenProcessedInfo, new RecordItemIndex(10));
            repo.MainForm.PanelDockingArea.ThisOrdersShipmentHasBeenProcessed.MoveTo(Location.CenterLeft);
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.ThisOrdersShipmentHasBeenProcessed' at CenterLeft.", repo.MainForm.PanelDockingArea.ThisOrdersShipmentHasBeenProcessedInfo, new RecordItemIndex(11));
            repo.MainForm.PanelDockingArea.ThisOrdersShipmentHasBeenProcessed.Click(Location.CenterLeft);
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
