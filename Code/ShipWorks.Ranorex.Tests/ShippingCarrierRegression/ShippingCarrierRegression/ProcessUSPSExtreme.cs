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
    ///The ProcessUSPSExtreme recording.
    /// </summary>
    [TestModule("3d2d8fe1-2c77-4544-87c4-5d7754987c0c", ModuleType.Recording, 1)]
    public partial class ProcessUSPSExtreme : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ShippingCarrierRegressionRepository repository.
        /// </summary>
        public static ShippingCarrierRegressionRepository repo = ShippingCarrierRegressionRepository.Instance;

        static ProcessUSPSExtreme instance = new ProcessUSPSExtreme();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessUSPSExtreme()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessUSPSExtreme Instance
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
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Service' at Center.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.Service.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Service: Priority
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: Priority\r\nSetting attribute SelectedItemText to 'Priority' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "Priority");
            Delay.Milliseconds(0);
            
            // Focus on the confirmation dropdown
            Report.Log(ReportLevel.Info, "Invoke Action", "Focus on the confirmation dropdown\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.Confirmation'.", repo.ShipOrders1.SplitContainer.ConfirmationInfo, new RecordItemIndex(3));
            repo.ShipOrders1.SplitContainer.Confirmation.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.Confirmation' at Center.", repo.ShipOrders1.SplitContainer.ConfirmationInfo, new RecordItemIndex(4));
            repo.ShipOrders1.SplitContainer.Confirmation.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Confirmation: Delivery Confirmation
            Report.Log(ReportLevel.Info, "Set Value", "Set Confirmation: Delivery Confirmation\r\nSetting attribute SelectedItemText to 'Delivery Confirmation' on item 'ShipOrders1.SplitContainer.Confirmation'.", repo.ShipOrders1.SplitContainer.ConfirmationInfo, new RecordItemIndex(5));
            repo.ShipOrders1.SplitContainer.Confirmation.Element.SetAttributeValue("SelectedItemText", "Delivery Confirmation");
            Delay.Milliseconds(0);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(6));
            repo.ShipOrders1.SplitContainer.WeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(7));
            repo.ShipOrders1.SplitContainer.WeightUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(8));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.WeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Weight: 70
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 70\r\nKey sequence '70' with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(9));
            repo.ShipOrders1.SplitContainer.WeightUSPS.PressKeys("70");
            Delay.Milliseconds(0);
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(10));
            repo.ShipOrders1.SplitContainer.LengthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.LengthUSPS' at Center.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(11));
            repo.ShipOrders1.SplitContainer.LengthUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '7' with focus on 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(12));
            repo.ShipOrders1.SplitContainer.LengthUSPS.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(13));
            repo.ShipOrders1.SplitContainer.WidthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WidthUSPS' at Center.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(14));
            repo.ShipOrders1.SplitContainer.WidthUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '8' with focus on 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(15));
            repo.ShipOrders1.SplitContainer.WidthUSPS.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(16));
            repo.ShipOrders1.SplitContainer.HeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.HeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(17));
            repo.ShipOrders1.SplitContainer.HeightUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '9' with focus on 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(18));
            repo.ShipOrders1.SplitContainer.HeightUSPS.PressKeys("9");
            Delay.Milliseconds(0);
            
            // Verify that "Priority" shows up in the rates area of the ship orders dialog
            Report.Log(ReportLevel.Info, "Validation", "Verify that \"Priority\" shows up in the rates area of the ship orders dialog\r\nValidating AttributeEqual (RawText='Priority') on item 'ShippingDlg.SplitContainer.Priority'.", repo.ShippingDlg.SplitContainer.PriorityInfo, new RecordItemIndex(19));
            Validate.Attribute(repo.ShippingDlg.SplitContainer.PriorityInfo, "RawText", "Priority");
            Delay.Milliseconds(100);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(20));
            repo.ShipOrders1.SplitContainer.WeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(21));
            repo.ShipOrders1.SplitContainer.WeightUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(22));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.WeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Weight: 71
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 71\r\nKey sequence '71' with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(23));
            repo.ShipOrders1.SplitContainer.WeightUSPS.PressKeys("71");
            Delay.Milliseconds(0);
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(24));
            repo.ShipOrders1.SplitContainer.LengthUSPS.Focus();
            Delay.Milliseconds(100);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.LengthUSPS' at Center.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(25));
            repo.ShipOrders1.SplitContainer.LengthUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Verify that "No rates are available for the shipment displays in the rates area of the Shipping dialog
            Report.Log(ReportLevel.Info, "Validation", "Verify that \"No rates are available for the shipment displays in the rates area of the Shipping dialog\r\nValidating AttributeEqual (RawText='No rates are available for the shipment.') on item 'ShippingDlg.SplitContainer.NoRatesAreAvailableForTheShipment'.", repo.ShippingDlg.SplitContainer.NoRatesAreAvailableForTheShipmentInfo, new RecordItemIndex(26));
            Validate.Attribute(repo.ShippingDlg.SplitContainer.NoRatesAreAvailableForTheShipmentInfo, "RawText", "No rates are available for the shipment.");
            Delay.Milliseconds(100);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(27));
            repo.ShipOrders1.SplitContainer.WeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(28));
            repo.ShipOrders1.SplitContainer.WeightUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(29));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.WeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Weight: 70
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 70\r\nKey sequence '70' with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(30));
            repo.ShipOrders1.SplitContainer.WeightUSPS.PressKeys("70");
            Delay.Milliseconds(0);
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(31));
            repo.ShipOrders1.SplitContainer.LengthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.LengthUSPS' at Center.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(32));
            repo.ShipOrders1.SplitContainer.LengthUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Length: 27
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 27\r\nKey sequence '27' with focus on 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(33));
            repo.ShipOrders1.SplitContainer.LengthUSPS.PressKeys("27");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(34));
            repo.ShipOrders1.SplitContainer.WidthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WidthUSPS' at Center.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(35));
            repo.ShipOrders1.SplitContainer.WidthUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Width: 27
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 27\r\nKey sequence '27' with focus on 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(36));
            repo.ShipOrders1.SplitContainer.WidthUSPS.PressKeys("27");
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(37));
            repo.ShipOrders1.SplitContainer.HeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.HeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(38));
            repo.ShipOrders1.SplitContainer.HeightUSPS.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Height: 27
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 27\r\nKey sequence '27' with focus on 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(39));
            repo.ShipOrders1.SplitContainer.HeightUSPS.PressKeys("27");
            Delay.Milliseconds(0);
            
            // Verify that "No rates are available for the shipment displays in the rates area of the Shipping dialog
            Report.Log(ReportLevel.Info, "Validation", "Verify that \"No rates are available for the shipment displays in the rates area of the Shipping dialog\r\nValidating AttributeEqual (RawText='No rates are available for the shipment.') on item 'ShippingDlg.SplitContainer.NoRatesAreAvailableForTheShipment'.", repo.ShippingDlg.SplitContainer.NoRatesAreAvailableForTheShipmentInfo, new RecordItemIndex(40));
            Validate.Attribute(repo.ShippingDlg.SplitContainer.NoRatesAreAvailableForTheShipmentInfo, "RawText", "No rates are available for the shipment.");
            Delay.Milliseconds(100);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.Close' at Center.", repo.ShippingDlg.CloseInfo, new RecordItemIndex(41));
            repo.ShippingDlg.Close.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Close out of the Ship Orders Dialog
            Report.Log(ReportLevel.Info, "Mouse", "Close out of the Ship Orders Dialog\r\nMouse Left Click item 'ShippingDlg.Close' at Center.", repo.ShippingDlg.CloseInfo, new RecordItemIndex(42));
            repo.ShippingDlg.Close.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
