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
    ///The ProcessUPSMailInnovations recording.
    /// </summary>
    [TestModule("272ad607-fc62-407f-9400-aee78ee18c3a", ModuleType.Recording, 1)]
    public partial class ProcessUPSMailInnovations : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTest.SmokeTestRepository repository.
        /// </summary>
        public static SmokeTest.SmokeTestRepository repo = SmokeTest.SmokeTestRepository.Instance;

        static ProcessUPSMailInnovations instance = new ProcessUPSMailInnovations();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessUPSMailInnovations()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessUPSMailInnovations Instance
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
            
            // Set Service: UPS Ground
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: UPS Ground\r\nSetting attribute SelectedItemText to 'UPS Expedited Mail Innovations®' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "UPS Expedited Mail Innovations®");
            Delay.Milliseconds(0);
            
            // Give Focus: Packaging
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Packaging\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.PackagingType'.", repo.ShippingDlg.SplitContainer.PackagingTypeInfo, new RecordItemIndex(3));
            repo.ShippingDlg.SplitContainer.PackagingType.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.PackagingType' at Center.", repo.ShippingDlg.SplitContainer.PackagingTypeInfo, new RecordItemIndex(4));
            repo.ShippingDlg.SplitContainer.PackagingType.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set the Packaging dropdown value to "Ireggulars"
            Report.Log(ReportLevel.Info, "Set Value", "Set the Packaging dropdown value to \"Ireggulars\"\r\nSetting attribute SelectedItemText to 'Irregulars' on item 'ShippingDlg.SplitContainer.PackagingType'.", repo.ShippingDlg.SplitContainer.PackagingTypeInfo, new RecordItemIndex(5));
            repo.ShippingDlg.SplitContainer.PackagingType.Element.SetAttributeValue("SelectedItemText", "Irregulars");
            Delay.Milliseconds(0);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(6));
            repo.ShippingDlg.SplitContainer.Weight.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Weight' at Center.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(7));
            repo.ShippingDlg.SplitContainer.Weight.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(8));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.Weight);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Weight: 1.25
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 1.25\r\nKey sequence '0.25' with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(9));
            repo.ShippingDlg.SplitContainer.Weight.PressKeys("0.25");
            Delay.Milliseconds(0);
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(10));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.DimensionsControl.Lengthiparcel' at Center.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(11));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '7' with focus on 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(12));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(13));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.DimensionsControl.Widthiparcel' at Center.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(14));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '8' with focus on 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(15));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(16));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.DimensionsControl.Heightiparcel' at Center.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(17));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '9' with focus on 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(18));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.PressKeys("9");
            Delay.Milliseconds(0);
            
            // Give Focus: Confirmation
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Confirmation\r\nInvoking Focus() on item 'ShipOrders1.Confirmation'.", repo.ShipOrders1.ConfirmationInfo, new RecordItemIndex(19));
            repo.ShipOrders1.Confirmation.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.Confirmation' at Center.", repo.ShipOrders1.ConfirmationInfo, new RecordItemIndex(20));
            repo.ShipOrders1.Confirmation.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Confirmation: USPS Delivery Confimration
            Report.Log(ReportLevel.Info, "Set Value", "Set Confirmation: USPS Delivery Confimration\r\nSetting attribute SelectedItemText to 'USPS Delivery Confirmation' on item 'ShipOrders1.Confirmation'.", repo.ShipOrders1.ConfirmationInfo, new RecordItemIndex(21));
            repo.ShipOrders1.Confirmation.Element.SetAttributeValue("SelectedItemText", "USPS Delivery Confirmation");
            Delay.Milliseconds(0);
            
            // Give Focus: Endorsement
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Endorsement\r\nInvoking Focus() on item 'ShipOrders1.UspsEndorsement'.", repo.ShipOrders1.UspsEndorsementInfo, new RecordItemIndex(22));
            repo.ShipOrders1.UspsEndorsement.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShipOrders1.UspsEndorsement' at Center.", repo.ShipOrders1.UspsEndorsementInfo, new RecordItemIndex(23));
            repo.ShipOrders1.UspsEndorsement.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Set Endorsement: Forwarding Service Requested
            Report.Log(ReportLevel.Info, "Set Value", "Set Endorsement: Forwarding Service Requested\r\nSetting attribute SelectedItemText to 'Forwarding Service Requested' on item 'ShipOrders1.UspsEndorsement'.", repo.ShipOrders1.UspsEndorsementInfo, new RecordItemIndex(24));
            repo.ShipOrders1.UspsEndorsement.Element.SetAttributeValue("SelectedItemText", "Forwarding Service Requested");
            Delay.Milliseconds(0);
            
            // Give Focus: Cost Center text box
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Cost Center text box\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.CostCenter'.", repo.ShippingDlg.SplitContainer.CostCenterInfo, new RecordItemIndex(25));
            repo.ShippingDlg.SplitContainer.CostCenter.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CostCenter' at Center.", repo.ShippingDlg.SplitContainer.CostCenterInfo, new RecordItemIndex(26));
            repo.ShippingDlg.SplitContainer.CostCenter.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.CostCenter'.", repo.ShippingDlg.SplitContainer.CostCenterInfo, new RecordItemIndex(27));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.CostCenter);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Cost Center: ShipWorksTesting
            Report.Log(ReportLevel.Info, "Keyboard", "Set Cost Center: ShipWorksTesting\r\nKey sequence 'ShipWorksTesting' with focus on 'ShippingDlg.SplitContainer.CostCenter'.", repo.ShippingDlg.SplitContainer.CostCenterInfo, new RecordItemIndex(28));
            repo.ShippingDlg.SplitContainer.CostCenter.PressKeys("ShipWorksTesting");
            Delay.Milliseconds(0);
            
            // Give Focus: Package ID
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Package ID\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.PackageID'.", repo.ShippingDlg.SplitContainer.PackageIDInfo, new RecordItemIndex(29));
            repo.ShippingDlg.SplitContainer.PackageID.Focus();
            Delay.Milliseconds(0);
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.PackageID' at Center.", repo.ShippingDlg.SplitContainer.PackageIDInfo, new RecordItemIndex(30));
            repo.ShippingDlg.SplitContainer.PackageID.MoveTo(1000);
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.PackageID'.", repo.ShippingDlg.SplitContainer.PackageIDInfo, new RecordItemIndex(31));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.PackageID);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Package ID: ShipWorkssTesting{//Order/Number}
            Report.Log(ReportLevel.Info, "Keyboard", "Set Package ID: ShipWorkssTesting{//Order/Number}\r\nKey sequence 'ShipWorksTesting{{//Order/Number}' with focus on 'ShippingDlg.SplitContainer.PackageID'.", repo.ShippingDlg.SplitContainer.PackageIDInfo, new RecordItemIndex(32));
            repo.ShippingDlg.SplitContainer.PackageID.PressKeys("ShipWorksTesting{{//Order/Number}");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
