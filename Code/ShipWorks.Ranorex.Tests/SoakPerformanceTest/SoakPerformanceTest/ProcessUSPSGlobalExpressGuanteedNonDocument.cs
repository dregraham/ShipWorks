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
    ///The ProcessUSPSGlobalExpressGuanteedNonDocument recording.
    /// </summary>
    [TestModule("edb93e6d-f44f-497c-bd36-432e80edb703", ModuleType.Recording, 1)]
    public partial class ProcessUSPSGlobalExpressGuanteedNonDocument : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ShippingCarrierRegressionRepository repository.
        /// </summary>
        public static ShippingCarrierRegressionRepository repo = ShippingCarrierRegressionRepository.Instance;

        static ProcessUSPSGlobalExpressGuanteedNonDocument instance = new ProcessUSPSGlobalExpressGuanteedNonDocument();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessUSPSGlobalExpressGuanteedNonDocument()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessUSPSGlobalExpressGuanteedNonDocument Instance
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
            
            // Set Service: Global Express Guaranteed Non-Document
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: Global Express Guaranteed Non-Document\r\nSetting attribute SelectedItemText to 'Global Express Guaranteed Non-Document' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "Global Express Guaranteed Non-Document");
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
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(5));
            repo.ShipOrders1.SplitContainer.LengthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '7' with focus on 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(6));
            repo.ShipOrders1.SplitContainer.LengthUSPS.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(7));
            repo.ShipOrders1.SplitContainer.WidthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '8' with focus on 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(8));
            repo.ShipOrders1.SplitContainer.WidthUSPS.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(9));
            repo.ShipOrders1.SplitContainer.HeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '9' with focus on 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(10));
            repo.ShipOrders1.SplitContainer.HeightUSPS.PressKeys("9");
            Delay.Milliseconds(0);
            
            // Click the Customs Tab
            Report.Log(ReportLevel.Info, "Mouse", "Click the Customs Tab\r\nMouse Left Click item 'ShippingDlg.SplitContainer.Customs' at Center.", repo.ShippingDlg.SplitContainer.CustomsInfo, new RecordItemIndex(11));
            repo.ShippingDlg.SplitContainer.Customs.Click();
            Delay.Milliseconds(200);
            
            // Click the Add button for the Customs Items
            Report.Log(ReportLevel.Info, "Mouse", "Click the Add button for the Customs Items\r\nMouse Left Click item 'ShippingDlg.SplitContainer.ButtonAdd' at Center.", repo.ShippingDlg.SplitContainer.ButtonAddInfo, new RecordItemIndex(12));
            repo.ShippingDlg.SplitContainer.ButtonAdd.Click();
            Delay.Milliseconds(200);
            
            // Click Weight
            Report.Log(ReportLevel.Info, "Mouse", "Click Weight\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CustomsWeightUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(13));
            repo.ShippingDlg.SplitContainer.CustomsWeightUSPS.Click();
            Delay.Milliseconds(200);
            
            // Select current Weight value
            Report.Log(ReportLevel.Info, "Keyboard", "Select current Weight value\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.CustomsWeightUSPS'.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(14));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.CustomsWeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Enter Weight
            Report.Log(ReportLevel.Info, "Keyboard", "Enter Weight\r\nKey sequence '5' with focus on 'ShippingDlg.SplitContainer.CustomsWeightUSPS'.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(15));
            repo.ShippingDlg.SplitContainer.CustomsWeightUSPS.PressKeys("5");
            Delay.Milliseconds(0);
            
            // Click Value
            Report.Log(ReportLevel.Info, "Mouse", "Click Value\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CustomsValueUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(16));
            repo.ShippingDlg.SplitContainer.CustomsValueUSPS.Click();
            Delay.Milliseconds(200);
            
            // Select current Value value
            Report.Log(ReportLevel.Info, "Keyboard", "Select current Value value\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.CustomsValueUSPS'.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(17));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.CustomsValueUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Enter Value
            Report.Log(ReportLevel.Info, "Keyboard", "Enter Value\r\nKey sequence '5' with focus on 'ShippingDlg.SplitContainer.CustomsValueUSPS'.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(18));
            repo.ShippingDlg.SplitContainer.CustomsValueUSPS.PressKeys("5");
            Delay.Milliseconds(0);
            
            // Click Value
            Report.Log(ReportLevel.Info, "Mouse", "Click Value\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia' at Center.", repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndiciaInfo, new RecordItemIndex(19));
            repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia.Click();
            Delay.Milliseconds(200);
            
            // Select current Value value
            Report.Log(ReportLevel.Info, "Keyboard", "Select current Value value\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia'.", repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndiciaInfo, new RecordItemIndex(20));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Enter Value
            Report.Log(ReportLevel.Info, "Keyboard", "Enter Value\r\nKey sequence '5' with focus on 'ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia'.", repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndiciaInfo, new RecordItemIndex(21));
            repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia.PressKeys("5");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
