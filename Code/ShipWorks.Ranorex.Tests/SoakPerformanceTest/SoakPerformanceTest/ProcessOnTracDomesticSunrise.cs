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

namespace SoakPerformanceTest
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The ProcessOnTracDomesticSunrise recording.
    /// </summary>
    [TestModule("3aa7ec41-bd8a-4018-9a9b-dc28c752027f", ModuleType.Recording, 1)]
    public partial class ProcessOnTracDomesticSunrise : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static ProcessOnTracDomesticSunrise instance = new ProcessOnTracDomesticSunrise();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessOnTracDomesticSunrise()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessOnTracDomesticSunrise Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "5.4.5")]
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
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "5.4.5")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;

            Init();

            // Give Focus: Service (Shipment Details Expander section)
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Service (Shipment Details Expander section)\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.Service.Focus();
            Delay.Milliseconds(0);
            
            // Set Service: Sunrise
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: Sunrise\r\nSetting attribute SelectedItemText to 'Sunrise' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "Sunrise");
            Delay.Milliseconds(0);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(2));
            repo.ShipOrders1.SplitContainer.WeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(3));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.WeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Weight: 1.25
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 1.25\r\nKey sequence '1.25' with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(4));
            repo.ShipOrders1.SplitContainer.WeightUSPS.PressKeys("1.25");
            Delay.Milliseconds(0);
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.LengthOnTrac'.", repo.ShipOrders1.SplitContainer.LengthOnTracInfo, new RecordItemIndex(5));
            repo.ShipOrders1.SplitContainer.LengthOnTrac.Focus();
            Delay.Milliseconds(0);
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '7' with focus on 'ShipOrders1.SplitContainer.LengthOnTrac'.", repo.ShipOrders1.SplitContainer.LengthOnTracInfo, new RecordItemIndex(6));
            repo.ShipOrders1.SplitContainer.LengthOnTrac.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WidthOnTrac'.", repo.ShipOrders1.SplitContainer.WidthOnTracInfo, new RecordItemIndex(7));
            repo.ShipOrders1.SplitContainer.WidthOnTrac.Focus();
            Delay.Milliseconds(0);
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '8' with focus on 'ShipOrders1.SplitContainer.WidthOnTrac'.", repo.ShipOrders1.SplitContainer.WidthOnTracInfo, new RecordItemIndex(8));
            repo.ShipOrders1.SplitContainer.WidthOnTrac.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.HeightOnTrac'.", repo.ShipOrders1.SplitContainer.HeightOnTracInfo, new RecordItemIndex(9));
            repo.ShipOrders1.SplitContainer.HeightOnTrac.Focus();
            Delay.Milliseconds(0);
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '9' with focus on 'ShipOrders1.SplitContainer.HeightOnTrac'.", repo.ShipOrders1.SplitContainer.HeightOnTracInfo, new RecordItemIndex(10));
            repo.ShipOrders1.SplitContainer.HeightOnTrac.PressKeys("9");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
