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

namespace SmokeTest
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The OLMProcessUSPSInternationalPriority recording.
    /// </summary>
    [TestModule("c1543ca6-d719-45d4-a387-5b6ef72adf86", ModuleType.Recording, 1)]
    public partial class OLMProcessUSPSInternationalPriority : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static OLMProcessUSPSInternationalPriority instance = new OLMProcessUSPSInternationalPriority();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public OLMProcessUSPSInternationalPriority()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static OLMProcessUSPSInternationalPriority Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
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
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.1")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            // Give Focus: Service (Shipment Details Expander section)
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Service (Shipment Details Expander section)\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.Service.Focus();
            Delay.Milliseconds(0);
            
            // Move to Service
            Report.Log(ReportLevel.Info, "Mouse", "Move to Service\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Service' at Center.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.Service.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Service: UPS Ground
            Report.Log(ReportLevel.Info, "Set value", "Set Service: UPS Ground\r\nSetting attribute SelectedItemText to 'International Priority' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "International Priority");
            Delay.Milliseconds(0);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(3));
            repo.ShipOrders1.SplitContainer.WeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move to Weight
            Report.Log(ReportLevel.Info, "Mouse", "Move to Weight\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(4));
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
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Length\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(7));
            repo.ShipOrders1.SplitContainer.LengthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move to Length
            Report.Log(ReportLevel.Info, "Mouse", "Move to Length\r\nMouse Left Move item 'ShipOrders1.SplitContainer.LengthUSPS' at Center.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(8));
            repo.ShipOrders1.SplitContainer.LengthUSPS.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '7' with focus on 'ShipOrders1.SplitContainer.LengthUSPS'.", repo.ShipOrders1.SplitContainer.LengthUSPSInfo, new RecordItemIndex(9));
            repo.ShipOrders1.SplitContainer.LengthUSPS.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Width\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(10));
            repo.ShipOrders1.SplitContainer.WidthUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move to Width
            Report.Log(ReportLevel.Info, "Mouse", "Move to Width\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WidthUSPS' at Center.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(11));
            repo.ShipOrders1.SplitContainer.WidthUSPS.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '8' with focus on 'ShipOrders1.SplitContainer.WidthUSPS'.", repo.ShipOrders1.SplitContainer.WidthUSPSInfo, new RecordItemIndex(12));
            repo.ShipOrders1.SplitContainer.WidthUSPS.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Height\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(13));
            repo.ShipOrders1.SplitContainer.HeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move to Height
            Report.Log(ReportLevel.Info, "Mouse", "Move to Height\r\nMouse Left Move item 'ShipOrders1.SplitContainer.HeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(14));
            repo.ShipOrders1.SplitContainer.HeightUSPS.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '9' with focus on 'ShipOrders1.SplitContainer.HeightUSPS'.", repo.ShipOrders1.SplitContainer.HeightUSPSInfo, new RecordItemIndex(15));
            repo.ShipOrders1.SplitContainer.HeightUSPS.PressKeys("9");
            Delay.Milliseconds(0);
            
            // Move to Customs Tab
            Report.Log(ReportLevel.Info, "Mouse", "Move to Customs Tab\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Customs' at Center.", repo.ShippingDlg.SplitContainer.CustomsInfo, new RecordItemIndex(16));
            repo.ShippingDlg.SplitContainer.Customs.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Customs Tab
            Report.Log(ReportLevel.Info, "Mouse", "Click the Customs Tab\r\nMouse Left Click item 'ShippingDlg.SplitContainer.Customs' at Center.", repo.ShippingDlg.SplitContainer.CustomsInfo, new RecordItemIndex(17));
            repo.ShippingDlg.SplitContainer.Customs.Click();
            Delay.Milliseconds(200);
            
            // Move to Add button for the Customs Items
            Report.Log(ReportLevel.Info, "Mouse", "Move to Add button for the Customs Items\r\nMouse Left Move item 'ShippingDlg.SplitContainer.ButtonAdd' at Center.", repo.ShippingDlg.SplitContainer.ButtonAddInfo, new RecordItemIndex(18));
            repo.ShippingDlg.SplitContainer.ButtonAdd.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Add button for the Customs Items
            Report.Log(ReportLevel.Info, "Mouse", "Click the Add button for the Customs Items\r\nMouse Left Click item 'ShippingDlg.SplitContainer.ButtonAdd' at Center.", repo.ShippingDlg.SplitContainer.ButtonAddInfo, new RecordItemIndex(19));
            repo.ShippingDlg.SplitContainer.ButtonAdd.Click();
            Delay.Milliseconds(200);
            
            // Move to Customs Weight
            Report.Log(ReportLevel.Info, "Mouse", "Move to Customs Weight\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CustomsWeightUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(20));
            repo.ShippingDlg.SplitContainer.CustomsWeightUSPS.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Weight
            Report.Log(ReportLevel.Info, "Mouse", "Click Weight\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CustomsWeightUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(21));
            repo.ShippingDlg.SplitContainer.CustomsWeightUSPS.Click();
            Delay.Milliseconds(200);
            
            // Select current Weight value
            Report.Log(ReportLevel.Info, "Keyboard", "Select current Weight value\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.CustomsWeightUSPS'.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(22));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.CustomsWeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Enter Weight
            Report.Log(ReportLevel.Info, "Keyboard", "Enter Weight\r\nKey sequence '5' with focus on 'ShippingDlg.SplitContainer.CustomsWeightUSPS'.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(23));
            repo.ShippingDlg.SplitContainer.CustomsWeightUSPS.PressKeys("5");
            Delay.Milliseconds(0);
            
            // Move to Customs Value
            Report.Log(ReportLevel.Info, "Mouse", "Move to Customs Value\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CustomsValueUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(24));
            repo.ShippingDlg.SplitContainer.CustomsValueUSPS.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Value
            Report.Log(ReportLevel.Info, "Mouse", "Click Value\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CustomsValueUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(25));
            repo.ShippingDlg.SplitContainer.CustomsValueUSPS.Click();
            Delay.Milliseconds(200);
            
            // Select current Value value
            Report.Log(ReportLevel.Info, "Keyboard", "Select current Value value\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.CustomsValueUSPS'.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(26));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.CustomsValueUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Enter Value
            Report.Log(ReportLevel.Info, "Keyboard", "Enter Value\r\nKey sequence '5' with focus on 'ShippingDlg.SplitContainer.CustomsValueUSPS'.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(27));
            repo.ShippingDlg.SplitContainer.CustomsValueUSPS.PressKeys("5");
            Delay.Milliseconds(0);
            
            // Changes the pdf file name to current carrier service
            ChangePDFName();
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
