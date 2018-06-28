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
    ///The Processi_parcelInternationalSaverDeferred recording.
    /// </summary>
    [TestModule("78216b2a-0630-4ea4-ad3d-82e23b235b7d", ModuleType.Recording, 1)]
    public partial class Processi_parcelInternationalSaverDeferred : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static Processi_parcelInternationalSaverDeferred instance = new Processi_parcelInternationalSaverDeferred();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public Processi_parcelInternationalSaverDeferred()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static Processi_parcelInternationalSaverDeferred Instance
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
            
            // Set Service: Saver Deferred
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: Saver Deferred\r\nSetting attribute SelectedItemText to 'Saver Deferred' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "Saver Deferred");
            Delay.Milliseconds(0);
            
            // Click the SKU and Quantity Field
            Report.Log(ReportLevel.Info, "Mouse", "Click the SKU and Quantity Field\r\nMouse Left Click item 'ShippingDlg.SplitContainer.SKUAndQuantity' at Center.", repo.ShippingDlg.SplitContainer.SKUAndQuantityInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.SKUAndQuantity.Click();
            Delay.Milliseconds(200);
            
            // Select all data in the SKU and Quantity Field
            Report.Log(ReportLevel.Info, "Keyboard", "Select all data in the SKU and Quantity Field\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.SKUAndQuantity'.", repo.ShippingDlg.SplitContainer.SKUAndQuantityInfo, new RecordItemIndex(3));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.SKUAndQuantity);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Enter the SKU and Quantity
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the SKU and Quantity\r\nKey sequence '1,1' with focus on 'ShippingDlg.SplitContainer.SKUAndQuantity'.", repo.ShippingDlg.SplitContainer.SKUAndQuantityInfo, new RecordItemIndex(4));
            repo.ShippingDlg.SplitContainer.SKUAndQuantity.PressKeys("1,1");
            Delay.Milliseconds(0);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(5));
            repo.ShipOrders1.SplitContainer.WeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(6));
            Keyboard.PrepareFocus(repo.ShipOrders1.SplitContainer.WeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Weight: 1.25
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 1.25\r\nKey sequence '1.25' with focus on 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(7));
            repo.ShipOrders1.SplitContainer.WeightUSPS.PressKeys("1.25");
            Delay.Milliseconds(0);
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(8));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '7' with focus on 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(9));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(10));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '8' with focus on 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(11));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(12));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.Focus();
            Delay.Milliseconds(0);
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '9' with focus on 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(13));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.PressKeys("9");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
