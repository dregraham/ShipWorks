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

namespace PerformanceTesting
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The Process1000Shipments recording.
    /// </summary>
    [TestModule("8872cedf-b887-4cb5-83f2-fd631d2e70e7", ModuleType.Recording, 1)]
    public partial class Process1000Shipments : ITestModule
    {
        /// <summary>
        /// Holds an instance of the PerformanceTestingRepository repository.
        /// </summary>
        public static PerformanceTestingRepository repo = PerformanceTestingRepository.Instance;

        static Process1000Shipments instance = new Process1000Shipments();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public Process1000Shipments()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static Process1000Shipments Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.0")]
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
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.0")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 1000;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            // Cursor moves to the Shipments Grid
            Report.Log(ReportLevel.Info, "Mouse", "Cursor moves to the Shipments Grid\r\nMouse Left Move item 'ShippingDlg.SplitContainer.AduroRot' at Center.", repo.ShippingDlg.SplitContainer.AduroRotInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.AduroRot.MoveTo();
            Delay.Milliseconds(0);
            
            // Click on the 1st Item in the list
            Report.Log(ReportLevel.Info, "Mouse", "Click on the 1st Item in the list\r\nMouse Left Click item 'ShippingDlg.SplitContainer.AduroRot' at Center.", repo.ShippingDlg.SplitContainer.AduroRotInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.AduroRot.Click();
            Delay.Milliseconds(0);
            
            // Cursor moves
            Report.Log(ReportLevel.Info, "Mouse", "Cursor moves\r\nMouse Left Move item 'ShippingDlg.SplitContainer.EntityGrid' at Center.", repo.ShippingDlg.SplitContainer.EntityGridInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.EntityGrid.MoveTo();
            Delay.Milliseconds(0);
            
            // Ctrl+A - All rows are selected
            Report.Log(ReportLevel.Info, "Keyboard", "Ctrl+A - All rows are selected\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.EntityGrid'.", repo.ShippingDlg.SplitContainer.EntityGridInfo, new RecordItemIndex(3));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.EntityGrid);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Preparing Shipments
            Report.Log(ReportLevel.Info, "Wait", "Preparing Shipments\r\nWaiting 5s to exist. Associated repository item: 'ProgressDlg'", repo.ProgressDlg.SelfInfo, new ActionTimeout(5000), new RecordItemIndex(4));
            repo.ProgressDlg.SelfInfo.WaitForExists(5000);
            
            // Preparing Shipments
            Report.Log(ReportLevel.Info, "Wait", "Preparing Shipments\r\nWaiting 15m to not exist. Associated repository item: 'ProgressDlg'", repo.ProgressDlg.SelfInfo, new ActionTimeout(900000), new RecordItemIndex(5));
            repo.ProgressDlg.SelfInfo.WaitForNotExists(900000);
            
            // Cursor moves to Carrier Name Field
            Report.Log(ReportLevel.Info, "Mouse", "Cursor moves to Carrier Name Field\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CarrierName' at Center.", repo.ShippingDlg.SplitContainer.CarrierNameInfo, new RecordItemIndex(6));
            repo.ShippingDlg.SplitContainer.CarrierName.MoveTo();
            Delay.Milliseconds(0);
            
            // Click on Carrier Name Field
            Report.Log(ReportLevel.Info, "Mouse", "Click on Carrier Name Field\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CarrierName' at Center.", repo.ShippingDlg.SplitContainer.CarrierNameInfo, new RecordItemIndex(7));
            repo.ShippingDlg.SplitContainer.CarrierName.Click();
            Delay.Milliseconds(0);
            
            // Key in text
            Report.Log(ReportLevel.Info, "Keyboard", "Key in text\r\nKey sequence 'test' with focus on 'ShippingDlg.SplitContainer.CarrierName'.", repo.ShippingDlg.SplitContainer.CarrierNameInfo, new RecordItemIndex(8));
            repo.ShippingDlg.SplitContainer.CarrierName.PressKeys("test");
            Delay.Milliseconds(0);
            
            // Cursor moves down to Field below Service Field
            Report.Log(ReportLevel.Info, "Mouse", "Cursor moves down to Field below Service Field\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Service' at Center.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(9));
            repo.ShippingDlg.SplitContainer.Service.MoveTo();
            Delay.Milliseconds(0);
            
            // Click on the Service Field
            Report.Log(ReportLevel.Info, "Mouse", "Click on the Service Field\r\nMouse Left Click item 'ShippingDlg.SplitContainer.Service' at Center.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(10));
            repo.ShippingDlg.SplitContainer.Service.Click();
            Delay.Milliseconds(0);
            
            // Key in text
            Report.Log(ReportLevel.Info, "Keyboard", "Key in text\r\nKey sequence 'test' with focus on 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(11));
            repo.ShippingDlg.SplitContainer.Service.PressKeys("test");
            Delay.Milliseconds(0);
            
            // Cursor moves to the Create Label Button
            Report.Log(ReportLevel.Info, "Mouse", "Cursor moves to the Create Label Button\r\nMouse Left Move item 'ShippingDlg.CreateLabel' at Center.", repo.ShippingDlg.CreateLabelInfo, new RecordItemIndex(12));
            repo.ShippingDlg.CreateLabel.MoveTo();
            Delay.Milliseconds(0);
            
            // Click on the Create Label Button
            Report.Log(ReportLevel.Info, "Mouse", "Click on the Create Label Button\r\nMouse Left Click item 'ShippingDlg.CreateLabel' at Center.", repo.ShippingDlg.CreateLabelInfo, new RecordItemIndex(13));
            repo.ShippingDlg.CreateLabel.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", "Start 7/8", new RecordItemIndex(14));
            
            getStartTime();
            Delay.Milliseconds(0);
            
            // Processing Shipments
            Report.Log(ReportLevel.Info, "Wait", "Processing Shipments\r\nWaiting 5s to exist. Associated repository item: 'ProgressDlg'", repo.ProgressDlg.SelfInfo, new ActionTimeout(5000), new RecordItemIndex(16));
            repo.ProgressDlg.SelfInfo.WaitForExists(5000);
            
            // Processing Shipments
            Report.Log(ReportLevel.Info, "Wait", "Processing Shipments\r\nWaiting 15m to not exist. Associated repository item: 'ProgressDlg'", repo.ProgressDlg.SelfInfo, new ActionTimeout(900000), new RecordItemIndex(17));
            repo.ProgressDlg.SelfInfo.WaitForNotExists(900000);
            
            // Validate the Carrier Name
            Report.Log(ReportLevel.Info, "Validation", "Validate the Carrier Name\r\nValidating AttributeEqual (Text='test') on item 'ShippingDlg.SplitContainer.Carrier'.", repo.ShippingDlg.SplitContainer.CarrierInfo, new RecordItemIndex(18));
            Validate.AttributeEqual(repo.ShippingDlg.SplitContainer.CarrierInfo, "Text", "test");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "User", "End 7/8", new RecordItemIndex(19));
            
            getEndTime();
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
