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
    ///The SelectandUnselect1000Shipments recording.
    /// </summary>
    [TestModule("8a898a59-502b-42cf-8292-19c1d648e5e2", ModuleType.Recording, 1)]
    public partial class SelectandUnselect1000Shipments : ITestModule
    {
        /// <summary>
        /// Holds an instance of the PerformanceTestingRepository repository.
        /// </summary>
        public static PerformanceTestingRepository repo = PerformanceTestingRepository.Instance;

        static SelectandUnselect1000Shipments instance = new SelectandUnselect1000Shipments();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SelectandUnselect1000Shipments()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SelectandUnselect1000Shipments Instance
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

            // Cursor moves on Ship Order Panel to the 1st row in the Shipments Panel on the left
            Report.Log(ReportLevel.Info, "Mouse", "Cursor moves on Ship Order Panel to the 1st row in the Shipments Panel on the left\r\nMouse Left Move item 'ShippingDlg.SplitContainer.RawText1Of1' at Center.", repo.ShippingDlg.SplitContainer.RawText1Of1Info, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.RawText1Of1.MoveTo();
            Delay.Milliseconds(0);
            
            // Click on the 1st row in the Shipments Panel
            Report.Log(ReportLevel.Info, "Mouse", "Click on the 1st row in the Shipments Panel\r\nMouse Left Click item 'ShippingDlg.SplitContainer.RawText1Of1' at Center.", repo.ShippingDlg.SplitContainer.RawText1Of1Info, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.RawText1Of1.Click();
            Delay.Milliseconds(0);
            
            // Ctrl+A to select All
            Report.Log(ReportLevel.Info, "Keyboard", "Ctrl+A to select All\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.EntityGrid'.", repo.ShippingDlg.SplitContainer.EntityGridInfo, new RecordItemIndex(2));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.EntityGrid);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", "Start 3", new RecordItemIndex(3));
            
            getStartTime();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 5s to exist. Associated repository item: 'ProgressDlg'", repo.ProgressDlg.SelfInfo, new ActionTimeout(5000), new RecordItemIndex(5));
            repo.ProgressDlg.SelfInfo.WaitForExists(5000);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 15m to not exist. Associated repository item: 'ProgressDlg'", repo.ProgressDlg.SelfInfo, new ActionTimeout(900000), new RecordItemIndex(6));
            repo.ProgressDlg.SelfInfo.WaitForNotExists(900000);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='') on item 'ShippingDlg.SplitContainer.CarrierName'.", repo.ShippingDlg.SplitContainer.CarrierNameInfo, new RecordItemIndex(7));
            Validate.AttributeEqual(repo.ShippingDlg.SplitContainer.CarrierNameInfo, "Text", "");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "User", "End 3", new RecordItemIndex(8));
            
            getEndTime();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.SplitContainer.RawText1Of1' at 11;5.", repo.ShippingDlg.SplitContainer.RawText1Of1Info, new RecordItemIndex(10));
            repo.ShippingDlg.SplitContainer.RawText1Of1.Click("11;5");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "User", "Start 4", new RecordItemIndex(11));
            
            getStartTime2();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='') on item 'ShippingDlg.SplitContainer.CarrierName'.", repo.ShippingDlg.SplitContainer.CarrierNameInfo, new RecordItemIndex(13));
            Validate.AttributeEqual(repo.ShippingDlg.SplitContainer.CarrierNameInfo, "Text", "");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "User", "End 4", new RecordItemIndex(14));
            
            getEndTime2();
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
