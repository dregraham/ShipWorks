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
    ///The ProcessUSPSExpress1EndiciaPriority recording.
    /// </summary>
    [TestModule("8b588592-1ef6-4d13-992c-4d11d181ba54", ModuleType.Recording, 1)]
    public partial class ProcessUSPSExpress1EndiciaPriority : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static ProcessUSPSExpress1EndiciaPriority instance = new ProcessUSPSExpress1EndiciaPriority();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessUSPSExpress1EndiciaPriority()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessUSPSExpress1EndiciaPriority Instance
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
            Mouse.DefaultMoveTime = 1000;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            // Give Focus: Service (Shipment Details Expander section)
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Service (Shipment Details Expander section)\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.Service.Focus();
            Delay.Milliseconds(0);
            
            // Move mouse to avoid click inconsistency.
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to avoid click inconsistency.\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Service' at Center.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.Service.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Set Service: Priority
            Report.Log(ReportLevel.Info, "Set value", "Set Service: Priority\r\nSetting attribute SelectedItemText to 'Priority' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "Priority");
            Delay.Milliseconds(0);
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WeightUSPS'.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(3));
            repo.ShipOrders1.SplitContainer.WeightUSPS.Focus();
            Delay.Milliseconds(0);
            
            // Move mouse to avoid click inconsistency.
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to avoid click inconsistency.\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WeightUSPS' at Center.", repo.ShipOrders1.SplitContainer.WeightUSPSInfo, new RecordItemIndex(4));
            repo.ShipOrders1.SplitContainer.WeightUSPS.MoveTo(300);
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
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Length\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.LengthExpress1Endicia'.", repo.ShipOrders1.SplitContainer.LengthExpress1EndiciaInfo, new RecordItemIndex(7));
            repo.ShipOrders1.SplitContainer.LengthExpress1Endicia.Focus();
            Delay.Milliseconds(0);
            
            // Move mouse to avoid click inconsistency.
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to avoid click inconsistency.\r\nMouse Left Move item 'ShipOrders1.SplitContainer.LengthExpress1Endicia' at Center.", repo.ShipOrders1.SplitContainer.LengthExpress1EndiciaInfo, new RecordItemIndex(8));
            repo.ShipOrders1.SplitContainer.LengthExpress1Endicia.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '7' with focus on 'ShipOrders1.SplitContainer.LengthExpress1Endicia'.", repo.ShipOrders1.SplitContainer.LengthExpress1EndiciaInfo, new RecordItemIndex(9));
            repo.ShipOrders1.SplitContainer.LengthExpress1Endicia.PressKeys("7");
            Delay.Milliseconds(0);
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Width\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.WidthExpress1Endicia'.", repo.ShipOrders1.SplitContainer.WidthExpress1EndiciaInfo, new RecordItemIndex(10));
            repo.ShipOrders1.SplitContainer.WidthExpress1Endicia.Focus();
            Delay.Milliseconds(0);
            
            // Move mouse to avoid click inconsistency.
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to avoid click inconsistency.\r\nMouse Left Move item 'ShipOrders1.SplitContainer.WidthExpress1Endicia' at Center.", repo.ShipOrders1.SplitContainer.WidthExpress1EndiciaInfo, new RecordItemIndex(11));
            repo.ShipOrders1.SplitContainer.WidthExpress1Endicia.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '8' with focus on 'ShipOrders1.SplitContainer.WidthExpress1Endicia'.", repo.ShipOrders1.SplitContainer.WidthExpress1EndiciaInfo, new RecordItemIndex(12));
            repo.ShipOrders1.SplitContainer.WidthExpress1Endicia.PressKeys("8");
            Delay.Milliseconds(0);
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Height\r\nInvoking Focus() on item 'ShipOrders1.SplitContainer.HeightExpress1Endicia'.", repo.ShipOrders1.SplitContainer.HeightExpress1EndiciaInfo, new RecordItemIndex(13));
            repo.ShipOrders1.SplitContainer.HeightExpress1Endicia.Focus();
            Delay.Milliseconds(0);
            
            // Move mouse to avoid click inconsistency.
            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to avoid click inconsistency.\r\nMouse Left Move item 'ShipOrders1.SplitContainer.HeightExpress1Endicia' at Center.", repo.ShipOrders1.SplitContainer.HeightExpress1EndiciaInfo, new RecordItemIndex(14));
            repo.ShipOrders1.SplitContainer.HeightExpress1Endicia.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '9' with focus on 'ShipOrders1.SplitContainer.HeightExpress1Endicia'.", repo.ShipOrders1.SplitContainer.HeightExpress1EndiciaInfo, new RecordItemIndex(15));
            repo.ShipOrders1.SplitContainer.HeightExpress1Endicia.PressKeys("9");
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
