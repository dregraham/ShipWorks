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
    ///The ProcessFedExOneRatePriorityOvernight recording.
    /// </summary>
    [TestModule("6a68054c-95ab-4746-9f6e-eb2cb3ad4afc", ModuleType.Recording, 1)]
    public partial class ProcessFedExOneRatePriorityOvernight : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ShippingCarrierRegressionRepository repository.
        /// </summary>
        public static ShippingCarrierRegressionRepository repo = ShippingCarrierRegressionRepository.Instance;

        static ProcessFedExOneRatePriorityOvernight instance = new ProcessFedExOneRatePriorityOvernight();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessFedExOneRatePriorityOvernight()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessFedExOneRatePriorityOvernight Instance
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

            // Move fix
            Report.Log(ReportLevel.Info, "Mouse", "Move fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.ResidentialDetermination' at Center.", repo.ShipOrders1.SplitContainer.ResidentialDeterminationInfo, new RecordItemIndex(0));
            repo.ShipOrders1.SplitContainer.ResidentialDetermination.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Give Focus: Residential \ Commercial dropdown box
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Residential \\ Commercial dropdown box\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.ResidentialDetermination'.", repo.ShipOrders1.SplitContainer.ResidentialDeterminationInfo, new RecordItemIndex(1));
            repo.ShipOrders1.SplitContainer.ResidentialDetermination.Focus();
            Delay.Milliseconds(0);
            
            // Set Residential \ Commercial dropdown: Commercial
            Report.Log(ReportLevel.Info, "Set Value", "Set Residential \\ Commercial dropdown: Commercial\r\nSetting attribute SelectedItemText to 'Commercial' on item 'ShipOrders1.SplitContainer.ResidentialDetermination'.", repo.ShipOrders1.SplitContainer.ResidentialDeterminationInfo, new RecordItemIndex(2));
            repo.ShipOrders1.SplitContainer.ResidentialDetermination.Element.SetAttributeValue("SelectedItemText", "Commercial");
            Delay.Milliseconds(0);
            
            // Move fix
            Report.Log(ReportLevel.Info, "Mouse", "Move fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Service' at Center.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(3));
            repo.ShippingDlg.SplitContainer.Service.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Give Focus: Service (Shipment Details Expander section)
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Service (Shipment Details Expander section)\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(4));
            repo.ShippingDlg.SplitContainer.Service.Focus();
            Delay.Milliseconds(0);
            
            // Set Service: FedEx One Rate® (Priority Overnight)
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: FedEx One Rate® (Priority Overnight)\r\nSetting attribute SelectedItemText to 'FedEx One Rate® (Priority Overnight)' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(5));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "FedEx One Rate® (Priority Overnight)");
            Delay.Milliseconds(0);
            
            // Move fix
            Report.Log(ReportLevel.Info, "Mouse", "Move fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Weight' at Center.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(6));
            repo.ShippingDlg.SplitContainer.Weight.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(7));
            repo.ShippingDlg.SplitContainer.Weight.Focus();
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(8));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.Weight);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Weight: 5
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 5\r\nKey sequence '5' with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(9));
            repo.ShippingDlg.SplitContainer.Weight.PressKeys("5");
            Delay.Milliseconds(0);
            
            // Move fix
            Report.Log(ReportLevel.Info, "Mouse", "Move fix\r\nMouse Left Move item 'ShippingDlg.DimensionsControl.Lengthiparcel' at Center.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(10));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(11));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Set Length: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 7\r\nKey sequence '7' with focus on 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(12));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Move fix
            Report.Log(ReportLevel.Info, "Mouse", "Move fix\r\nMouse Left Move item 'ShippingDlg.DimensionsControl.Widthiparcel' at Center.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(13));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(14));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Set Width: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 8\r\nKey sequence '8' with focus on 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(15));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Move fix
            Report.Log(ReportLevel.Info, "Mouse", "Move fix\r\nMouse Left Move item 'ShippingDlg.DimensionsControl.Heightiparcel' at Center.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(16));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(17));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Set Height: 9
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 9\r\nKey sequence '9' with focus on 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(18));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.PressKeys("9");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
