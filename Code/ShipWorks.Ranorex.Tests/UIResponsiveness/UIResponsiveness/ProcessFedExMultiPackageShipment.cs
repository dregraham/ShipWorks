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

namespace UIResponsiveness
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The ProcessFedExMultiPackageShipment recording.
    /// </summary>
    [TestModule("914020ba-8a4b-41e6-bb9f-52503052428e", ModuleType.Recording, 1)]
    public partial class ProcessFedExMultiPackageShipment : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static ProcessFedExMultiPackageShipment instance = new ProcessFedExMultiPackageShipment();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessFedExMultiPackageShipment()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessFedExMultiPackageShipment Instance
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
            Mouse.DefaultMoveTime = 0;
            Keyboard.DefaultKeyPressTime = 20;
            Delay.SpeedFactor = 0.0;

            Init();

            // Click: Apply Profile
            Report.Log(ReportLevel.Info, "Mouse", "Click: Apply Profile\r\nMouse Left Click item 'ShipOrders1.ApplyProfile' at Center.", repo.ShipOrders1.ApplyProfileInfo, new RecordItemIndex(0));
            repo.ShipOrders1.ApplyProfile.Click();
            
            // Click: Defaults - FedEx
            Report.Log(ReportLevel.Info, "Mouse", "Click: Defaults - FedEx\r\nMouse Left Click item 'ContextMenuPrint.DefaultsFedEx' at Center.", repo.ContextMenuPrint.DefaultsFedExInfo, new RecordItemIndex(1));
            repo.ContextMenuPrint.DefaultsFedEx.Click();
            
            // Give Focus: Service (Shipment Details Expander section)
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Service (Shipment Details Expander section)\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Service.Focus();
            
            // Set Service: FedEx Standard Overnight®
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: FedEx Standard Overnight®\r\nSetting attribute SelectedItemText to 'FedEx Standard Overnight®' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(3));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "FedEx Standard Overnight®");
            
            // Give Focus: Packages (count) dropdown box
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Packages (count) dropdown box\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.PackageCountCombo'.", repo.ShipOrders1.SplitContainer.PackageCountComboInfo, new RecordItemIndex(4));
            repo.ShipOrders1.SplitContainer.PackageCountCombo.Focus();
            
            // Set Packages: 2
            Report.Log(ReportLevel.Info, "Set Value", "Set Packages: 2\r\nSetting attribute SelectedItemText to '2' on item 'ShipOrders1.SplitContainer.PackageCountCombo'.", repo.ShipOrders1.SplitContainer.PackageCountComboInfo, new RecordItemIndex(5));
            repo.ShipOrders1.SplitContainer.PackageCountCombo.Element.SetAttributeValue("SelectedItemText", "2");
            
            // Give Focus: Package 1
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Package 1\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.Package1'.", repo.ShipOrders1.SplitContainer.Package1Info, new RecordItemIndex(6));
            repo.ShipOrders1.SplitContainer.Package1.Focus();
            
            // Click: Package 1
            Report.Log(ReportLevel.Info, "Mouse", "Click: Package 1\r\nMouse Left Click item 'ShipOrders1.SplitContainer.Package1' at Center.", repo.ShipOrders1.SplitContainer.Package1Info, new RecordItemIndex(7));
            repo.ShipOrders1.SplitContainer.Package1.Click();
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.Package1'.", repo.ShipOrders1.SplitContainer.Package1Info, new RecordItemIndex(8));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.Package1);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(9));
            repo.ShippingDlg.SplitContainer.Weight.Focus();
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(10));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.Weight);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            
            // Set Weight: 1
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 1\r\nKey sequence '1' with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(11));
            repo.ShippingDlg.SplitContainer.Weight.PressKeys("1");
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(12));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.Focus();
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '6' with focus on 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(13));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.PressKeys("6");
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(14));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.Focus();
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '7' with focus on 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(15));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.PressKeys("7");
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(16));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.Focus();
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '8' with focus on 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(17));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.PressKeys("8");
            
            // Give Focus: Package 2
            //Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Package 2\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.Package21'.", repo.ShipOrders1.SplitContainer.Package21Info, new RecordItemIndex(18));
            //repo.ShipOrders1.SplitContainer.Package21.Focus();
            
            // Click: Package 2
            //Report.Log(ReportLevel.Info, "Mouse", "Click: Package 2\r\nMouse Left Click item 'ShipOrders1.SplitContainer.Package21' at Center.", repo.ShipOrders1.SplitContainer.Package21Info, new RecordItemIndex(19));
            //repo.ShipOrders1.SplitContainer.Package21.Click();
            
            // Give Focus: Package 2
            //Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Package 2\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.Package2'.", repo.ShipOrders1.SplitContainer.Package2Info, new RecordItemIndex(20));
            //repo.ShipOrders1.SplitContainer.Package2.Focus();
            
            // Click: Package 2
            //Report.Log(ReportLevel.Info, "Mouse", "Click: Package 2\r\nMouse Left Click item 'ShipOrders1.SplitContainer.Package2' at Center.", repo.ShipOrders1.SplitContainer.Package2Info, new RecordItemIndex(21));
            //repo.ShipOrders1.SplitContainer.Package2.Click();
            
            // Give Focus: Weight
            //Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(22));
            //repo.ShippingDlg.SplitContainer.Weight.Focus();
            
            // Select All (CTRL+A)
            //Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(23));
            //Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.Weight);
            //Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            
            // Set Weight: 2
            //Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 2\r\nKey sequence '2' with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(24));
            //repo.ShippingDlg.SplitContainer.Weight.PressKeys("2");
            
            // Give Focus: Length
            //Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(25));
            //repo.ShippingDlg.DimensionsControl.Lengthiparcel.Focus();
            
            // Set Length: 7
            //Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 7\r\nKey sequence '7' with focus on 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(26));
            //repo.ShippingDlg.DimensionsControl.Lengthiparcel.PressKeys("7");
            
            // Give Focus: Width
            //Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(27));
            //repo.ShippingDlg.DimensionsControl.Widthiparcel.Focus();
            
            // Set Width: 8
            //Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 8\r\nKey sequence '8' with focus on 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(28));
            //repo.ShippingDlg.DimensionsControl.Widthiparcel.PressKeys("8");
            
            // Give Focus: Height
            //Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(29));
            //repo.ShippingDlg.DimensionsControl.Heightiparcel.Focus();
            
            // Set Height: 9
            //Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 9\r\nKey sequence '9' with focus on 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(30));
            //repo.ShippingDlg.DimensionsControl.Heightiparcel.PressKeys("9");
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
