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
    ///The ProcessVoidShipment_Validate_CloseShipOrders2 recording.
    /// </summary>
    [TestModule("993e5102-d2f4-4841-85b3-22aff3d709a8", ModuleType.Recording, 1)]
    public partial class ProcessVoidShipment_Validate_CloseShipOrders2 : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static ProcessVoidShipment_Validate_CloseShipOrders2 instance = new ProcessVoidShipment_Validate_CloseShipOrders2();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessVoidShipment_Validate_CloseShipOrders2()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessVoidShipment_Validate_CloseShipOrders2 Instance
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
            Mouse.DefaultMoveTime = 1000;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            // Move to the Create Label Button
            Report.Log(ReportLevel.Info, "Mouse", "Move to the Create Label Button\r\nMouse Left Move item 'ShippingDlg.ProcessDropDownButton' at Center.", repo.ShippingDlg.ProcessDropDownButtonInfo, new RecordItemIndex(0));
            repo.ShippingDlg.ProcessDropDownButton.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Create Label button
            Report.Log(ReportLevel.Info, "Mouse", "Click the Create Label button\r\nMouse Left Click item 'ShippingDlg.ProcessDropDownButton' at Center.", repo.ShippingDlg.ProcessDropDownButtonInfo, new RecordItemIndex(1));
            repo.ShippingDlg.ProcessDropDownButton.Click(300);
            Delay.Milliseconds(200);
            
            // Wait until the processing Shipments dialog does not exist
            Report.Log(ReportLevel.Info, "Wait", "Wait until the processing Shipments dialog does not exist\r\nWaiting 30s to not exist. Associated repository item: 'ProcessingShipments.ProcessingShipments'", repo.ProcessingShipments.ProcessingShipmentsInfo, new ActionTimeout(30000), new RecordItemIndex(2));
            repo.ProcessingShipments.ProcessingShipmentsInfo.WaitForNotExists(30000);
            
            // Wait 5s
            Report.Log(ReportLevel.Info, "Delay", "Wait 5s\r\nWaiting for 10s.", new RecordItemIndex(3));
            Delay.Duration(10000, false);
            
            // Move to Close button (Ship Orders UI)
            Report.Log(ReportLevel.Info, "Mouse", "Move to Close button (Ship Orders UI)\r\nMouse Left Move item 'ShippingDlg.Close' at Center.", repo.ShippingDlg.CloseInfo, new RecordItemIndex(4));
            repo.ShippingDlg.Close.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click close button (Ship Orders UI)
            Report.Log(ReportLevel.Info, "Mouse", "Click close button (Ship Orders UI)\r\nMouse Left Click item 'ShippingDlg.Close' at Center.", repo.ShippingDlg.CloseInfo, new RecordItemIndex(5));
            repo.ShippingDlg.Close.Click(300);
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
