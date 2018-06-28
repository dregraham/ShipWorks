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

namespace ShippingCarrierRegression
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The ProcessUSPSPriority recording.
    /// </summary>
    [TestModule("25b5cda0-246f-4966-9e14-1397d1be51c8", ModuleType.Recording, 1)]
    public partial class ProcessUSPSPriority : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTest.SmokeTestRepository repository.
        /// </summary>
        public static SmokeTest.SmokeTestRepository repo = SmokeTest.SmokeTestRepository.Instance;

        static ProcessUSPSPriority instance = new ProcessUSPSPriority();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessUSPSPriority()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessUSPSPriority Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "7.0")]
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
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "7.0")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            // Give Focus: Service (Shipment Details Expander section)
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Service (Shipment Details Expander section)\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.Service.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Service' at Center.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.Service.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Service: Priority
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: Priority\r\nSetting attribute SelectedItemText to 'Priority' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "Priority");
            Delay.Milliseconds(0);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(3));
            repo.ShipOrders1.SplitContainer.WeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(4));
            repo.ShipOrders1.SplitContainer.WeightUSPS.MoveTo();
            Delay.Milliseconds(200);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(5));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.WeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Weight: 1.25
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 1.25\r\nKey sequence '1.25' with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(6));
            repo.ShipOrders1.SplitContainer.WeightUSPS.PressKeys("1.25");
            Delay.Milliseconds(0);
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(7));
            repo.ShipOrders1.SplitContainer.LengthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipOrders1.SplitContainer.LengthUSPS' at Center.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(8));
            repo.ShipOrders1.SplitContainer.LengthUSPS.MoveTo();
            Delay.Milliseconds(200);
            
            // Select All
            Report.Log(ReportLevel.Info, "Keyboard", "Select All\r\nKey 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(9));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.LengthUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '7' with focus on 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(10));
            repo.ShipOrders1.SplitContainer.LengthUSPS.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(11));
            repo.ShipOrders1.SplitContainer.WidthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WidthUSPS' at Center.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(12));
            repo.ShipOrders1.SplitContainer.WidthUSPS.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '8' with focus on 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(13));
            repo.ShipOrders1.SplitContainer.WidthUSPS.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Select All
            Report.Log(ReportLevel.Info, "Keyboard", "Select All\r\nKey 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(14));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.WidthUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(15));
            repo.ShipOrders1.SplitContainer.HeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipOrders1.SplitContainer.HeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(16));
            repo.ShipOrders1.SplitContainer.HeightUSPS.MoveTo();
            Delay.Milliseconds(200);
            
            // Select All
            Report.Log(ReportLevel.Info, "Keyboard", "Select All\r\nKey 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(17));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.HeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '9' with focus on 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(18));
            repo.ShipOrders1.SplitContainer.HeightUSPS.PressKeys("9");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
