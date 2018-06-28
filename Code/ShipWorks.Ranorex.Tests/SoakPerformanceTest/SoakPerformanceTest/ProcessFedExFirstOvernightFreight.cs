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
    ///The ProcessFedExFirstOvernightFreight recording.
    /// </summary>
    [TestModule("4c8f1dac-0242-4e9f-9fe0-94af74a8a878", ModuleType.Recording, 1)]
    public partial class ProcessFedExFirstOvernightFreight : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ShippingCarrierRegressionRepository repository.
        /// </summary>
        public static ShippingCarrierRegressionRepository repo = ShippingCarrierRegressionRepository.Instance;

        static ProcessFedExFirstOvernightFreight instance = new ProcessFedExFirstOvernightFreight();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessFedExFirstOvernightFreight()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessFedExFirstOvernightFreight Instance
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

            // Give Focus: Residential \ Commercial dropdown box
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Residential \\ Commercial dropdown box\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.ResidentialDetermination'.", repo.ShipOrders1.SplitContainer.ResidentialDeterminationInfo, new RecordItemIndex(0));
            repo.ShipOrders1.SplitContainer.ResidentialDetermination.Focus();
            Delay.Milliseconds(0);
            
            // Set Residential \ Commercial dropdown: Commercial
            Report.Log(ReportLevel.Info, "Set Value", "Set Residential \\ Commercial dropdown: Commercial\r\nSetting attribute SelectedItemText to 'Commercial' on item 'ShipOrders1.SplitContainer.ResidentialDetermination'.", repo.ShipOrders1.SplitContainer.ResidentialDeterminationInfo, new RecordItemIndex(1));
            repo.ShipOrders1.SplitContainer.ResidentialDetermination.Element.SetAttributeValue("SelectedItemText", "Commercial");
            Delay.Milliseconds(0);
            
            // Give Focus: Service (Shipment Details Expander section)
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Service (Shipment Details Expander section)\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Service.Focus();
            Delay.Milliseconds(0);
            
            // Set Service: FedEx First Overnight® Freight
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: FedEx First Overnight® Freight\r\nSetting attribute SelectedItemText to 'FedEx First Overnight® Freight' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(3));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "FedEx First Overnight® Freight");
            Delay.Milliseconds(0);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(4));
            repo.ShippingDlg.SplitContainer.Weight.Focus();
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(5));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.Weight);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Weight: 200
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 200\r\nKey sequence '200' with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(6));
            repo.ShippingDlg.SplitContainer.Weight.PressKeys("200");
            Delay.Milliseconds(0);
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(7));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Set Length: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 7\r\nKey sequence '7' with focus on 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(8));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(9));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Set Width: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 8\r\nKey sequence '8' with focus on 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(10));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(11));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Set Height: 9
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 9\r\nKey sequence '9' with focus on 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(12));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.PressKeys("9");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
