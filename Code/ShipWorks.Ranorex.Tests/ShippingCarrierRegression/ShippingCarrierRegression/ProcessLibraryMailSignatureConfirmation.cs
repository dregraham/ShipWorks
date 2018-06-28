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
    ///The ProcessLibraryMailSignatureConfirmation recording.
    /// </summary>
    [TestModule("514d9149-0971-4847-9e20-634540b38b18", ModuleType.Recording, 1)]
    public partial class ProcessLibraryMailSignatureConfirmation : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ShippingCarrierRegressionRepository repository.
        /// </summary>
        public static ShippingCarrierRegressionRepository repo = ShippingCarrierRegressionRepository.Instance;

        static ProcessLibraryMailSignatureConfirmation instance = new ProcessLibraryMailSignatureConfirmation();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessLibraryMailSignatureConfirmation()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessLibraryMailSignatureConfirmation Instance
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
            
            // Set Service: Library Mail
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: Library Mail\r\nSetting attribute SelectedItemText to 'Library Mail' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "Library Mail");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Invoke Action", "Invoking Focus() on item 'ShipOrders1.SplitContainer.Confirmation'.", repo.ShipOrders1.SplitContainer.ConfirmationInfo, new RecordItemIndex(3));
            repo.ShipOrders1.SplitContainer.Confirmation.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.SplitContainer.Confirmation' at Center.", repo.ShipOrders1.SplitContainer.ConfirmationInfo, new RecordItemIndex(4));
            repo.ShipOrders1.SplitContainer.Confirmation.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Confirmation: Signature Confirmation
            Report.Log(ReportLevel.Info, "Set Value", "Set Confirmation: Signature Confirmation\r\nSetting attribute SelectedItemText to 'Signature Confirmation' on item 'ShipOrders1.SplitContainer.Confirmation'.", repo.ShipOrders1.SplitContainer.ConfirmationInfo, new RecordItemIndex(5));
            repo.ShipOrders1.SplitContainer.Confirmation.Element.SetAttributeValue("SelectedItemText", "Signature Confirmation");
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
            
            // Set Weight: 1.25
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 1.25\r\nKey sequence '0.5' with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(9));
            repo.ShipOrders1.SplitContainer.WeightUSPS.PressKeys("0.5");
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
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
