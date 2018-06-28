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
    ///The ProcessUPSWorldwideSaver recording.
    /// </summary>
    [TestModule("38222d42-576d-451b-99d7-70b00ef34405", ModuleType.Recording, 1)]
    public partial class ProcessUPSWorldwideSaver : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTest.SmokeTestRepository repository.
        /// </summary>
        public static SmokeTest.SmokeTestRepository repo = SmokeTest.SmokeTestRepository.Instance;

        static ProcessUPSWorldwideSaver instance = new ProcessUPSWorldwideSaver();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessUPSWorldwideSaver()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessUPSWorldwideSaver Instance
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
            Mouse.DefaultMoveTime = 0;
            Keyboard.DefaultKeyPressTime = 20;
            Delay.SpeedFactor = 0.00;

            Init();

            // Give Focus: Service (Shipment Details Expander section)
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Service (Shipment Details Expander section)\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.Service.Focus();
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Service' at Center.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.Service.MoveTo();
            
            // Set Service: UPS Worldwide Saver®
            Report.Log(ReportLevel.Info, "Set Value", "Set Service: UPS Worldwide Saver®\r\nSetting attribute SelectedItemText to 'UPS Worldwide Saver®' on item 'ShippingDlg.SplitContainer.Service'.", repo.ShippingDlg.SplitContainer.ServiceInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Service.Element.SetAttributeValue("SelectedItemText", "UPS Worldwide Saver®");
            
            // Give Focus: Weight
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Weight\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(3));
            repo.ShippingDlg.SplitContainer.Weight.Focus();
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Weight' at Center.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(4));
            repo.ShippingDlg.SplitContainer.Weight.MoveTo();
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(5));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.Weight);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            
            // Set Weight: 1.25
            Report.Log(ReportLevel.Info, "Keyboard", "Set Weight: 1.25\r\nKey sequence '1.25' with focus on 'ShippingDlg.SplitContainer.Weight'.", repo.ShippingDlg.SplitContainer.WeightInfo, new RecordItemIndex(6));
            repo.ShippingDlg.SplitContainer.Weight.PressKeys("1.25");
            
            // Give Focus: Length
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Length\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(7));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.Focus();
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.DimensionsControl.Lengthiparcel' at Center.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(8));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.MoveTo();
            
            // Set Length: 6
            Report.Log(ReportLevel.Info, "Keyboard", "Set Length: 6\r\nKey sequence '6' with focus on 'ShippingDlg.DimensionsControl.Lengthiparcel'.", repo.ShippingDlg.DimensionsControl.LengthiparcelInfo, new RecordItemIndex(9));
            repo.ShippingDlg.DimensionsControl.Lengthiparcel.PressKeys("6");
            
            // Give Focus: Width
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Width\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(10));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.Focus();
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.DimensionsControl.Widthiparcel' at Center.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(11));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.MoveTo();
            
            // Set Width: 7
            Report.Log(ReportLevel.Info, "Keyboard", "Set Width: 7\r\nKey sequence '7' with focus on 'ShippingDlg.DimensionsControl.Widthiparcel'.", repo.ShippingDlg.DimensionsControl.WidthiparcelInfo, new RecordItemIndex(12));
            repo.ShippingDlg.DimensionsControl.Widthiparcel.PressKeys("7");
            
            // Give Focus: Height
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Height\r\nInvoking Focus() on item 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(13));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.Focus();
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.DimensionsControl.Heightiparcel' at Center.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(14));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.MoveTo();
            
            // Set Height: 8
            Report.Log(ReportLevel.Info, "Keyboard", "Set Height: 8\r\nKey sequence '8' with focus on 'ShippingDlg.DimensionsControl.Heightiparcel'.", repo.ShippingDlg.DimensionsControl.HeightiparcelInfo, new RecordItemIndex(15));
            repo.ShippingDlg.DimensionsControl.Heightiparcel.PressKeys("8");
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Customs' at Center.", repo.ShippingDlg.SplitContainer.CustomsInfo, new RecordItemIndex(16));
            repo.ShippingDlg.SplitContainer.Customs.MoveTo();
            
            // Click the Customs Tab
            Report.Log(ReportLevel.Info, "Mouse", "Click the Customs Tab\r\nMouse Left Click item 'ShippingDlg.SplitContainer.Customs' at Center.", repo.ShippingDlg.SplitContainer.CustomsInfo, new RecordItemIndex(17));
            repo.ShippingDlg.SplitContainer.Customs.Click();
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.ButtonAdd' at Center.", repo.ShippingDlg.SplitContainer.ButtonAddInfo, new RecordItemIndex(18));
            repo.ShippingDlg.SplitContainer.ButtonAdd.MoveTo();
            
            // Click the Add button for the Customs Items
            Report.Log(ReportLevel.Info, "Mouse", "Click the Add button for the Customs Items\r\nMouse Left Click item 'ShippingDlg.SplitContainer.ButtonAdd' at Center.", repo.ShippingDlg.SplitContainer.ButtonAddInfo, new RecordItemIndex(19));
            repo.ShippingDlg.SplitContainer.ButtonAdd.Click();
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CustomsWeightUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(20));
            repo.ShippingDlg.SplitContainer.CustomsWeightUSPS.MoveTo();
            
            // Click Weight
            Report.Log(ReportLevel.Info, "Mouse", "Click Weight\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CustomsWeightUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(21));
            repo.ShippingDlg.SplitContainer.CustomsWeightUSPS.Click();
            
            // Select current Weight value
            Report.Log(ReportLevel.Info, "Keyboard", "Select current Weight value\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.CustomsWeightUSPS'.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(22));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.CustomsWeightUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            
            // Enter Weight
            Report.Log(ReportLevel.Info, "Keyboard", "Enter Weight\r\nKey sequence '1.25' with focus on 'ShippingDlg.SplitContainer.CustomsWeightUSPS'.", repo.ShippingDlg.SplitContainer.CustomsWeightUSPSInfo, new RecordItemIndex(23));
            repo.ShippingDlg.SplitContainer.CustomsWeightUSPS.PressKeys("1.25");
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CustomsValueUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(24));
            repo.ShippingDlg.SplitContainer.CustomsValueUSPS.MoveTo();
            
            // Click Value
            Report.Log(ReportLevel.Info, "Mouse", "Click Value\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CustomsValueUSPS' at Center.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(25));
            repo.ShippingDlg.SplitContainer.CustomsValueUSPS.Click();
            
            // Select current Value value
            Report.Log(ReportLevel.Info, "Keyboard", "Select current Value value\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.CustomsValueUSPS'.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(26));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.CustomsValueUSPS);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            
            // Enter Value
            Report.Log(ReportLevel.Info, "Keyboard", "Enter Value\r\nKey sequence '5' with focus on 'ShippingDlg.SplitContainer.CustomsValueUSPS'.", repo.ShippingDlg.SplitContainer.CustomsValueUSPSInfo, new RecordItemIndex(27));
            repo.ShippingDlg.SplitContainer.CustomsValueUSPS.PressKeys("5");
            
            // Move Fix
            Report.Log(ReportLevel.Info, "Mouse", "Move Fix\r\nMouse Left Move item 'ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia' at Center.", repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndiciaInfo, new RecordItemIndex(28));
            repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia.MoveTo();
            
            // Click the Item Value field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Item Value field\r\nMouse Left Click item 'ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia' at Center.", repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndiciaInfo, new RecordItemIndex(29));
            repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia.Click();
            
            // Select current Item Value value
            Report.Log(ReportLevel.Info, "Keyboard", "Select current Item Value value\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia'.", repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndiciaInfo, new RecordItemIndex(30));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
            
            // Enter Item Value
            Report.Log(ReportLevel.Info, "Keyboard", "Enter Item Value\r\nKey sequence '5' with focus on 'ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia'.", repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndiciaInfo, new RecordItemIndex(31));
            repo.ShippingDlg.SplitContainer.CustomsSelectedItemValueUSPSEndicia.PressKeys("5");
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
