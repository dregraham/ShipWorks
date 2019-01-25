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
    ///The LookupAnOrder recording.
    /// </summary>
    [TestModule("7bb9e37a-0c6b-4a4d-98f7-d2aa040ae9d9", ModuleType.Recording, 1)]
    public partial class LookupAnOrder : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static LookupAnOrder instance = new LookupAnOrder();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public LookupAnOrder()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static LookupAnOrder Instance
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

            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.PanelDockingArea.PARTContentHost' at Center.", repo.ShipWorksSa.PanelDockingArea.PARTContentHostInfo, new RecordItemIndex(0));
            repo.ShipWorksSa.PanelDockingArea.PARTContentHost.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksSa.PanelDockingArea.PARTContentHost' at Center.", repo.ShipWorksSa.PanelDockingArea.PARTContentHostInfo, new RecordItemIndex(1));
            repo.ShipWorksSa.PanelDockingArea.PARTContentHost.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '3'.", new RecordItemIndex(2));
            Keyboard.Press("3");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.PanelDockingArea.Button' at Center.", repo.ShipWorksSa.PanelDockingArea.ButtonInfo, new RecordItemIndex(3));
            repo.ShipWorksSa.PanelDockingArea.Button.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksSa.PanelDockingArea.Button' at Center.", repo.ShipWorksSa.PanelDockingArea.ButtonInfo, new RecordItemIndex(4));
            repo.ShipWorksSa.PanelDockingArea.Button.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='First Last') on item 'ShipWorksSa.PanelDockingArea.FullNameTextBox'.", repo.ShipWorksSa.PanelDockingArea.FullNameTextBoxInfo, new RecordItemIndex(5));
            Validate.AttributeEqual(repo.ShipWorksSa.PanelDockingArea.FullNameTextBoxInfo, "Text", "First Last");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='Orderlookup Company') on item 'ShipWorksSa.PanelDockingArea.CompanyTextBox'.", repo.ShipWorksSa.PanelDockingArea.CompanyTextBoxInfo, new RecordItemIndex(6));
            Validate.AttributeEqual(repo.ShipWorksSa.PanelDockingArea.CompanyTextBoxInfo, "Text", "Orderlookup Company");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='Orderlookup Address') on item 'MainForm.ToFirstLastDomestic.StreetTextBox'.", repo.MainForm.ToFirstLastDomestic.StreetTextBoxInfo, new RecordItemIndex(7));
            Validate.AttributeEqual(repo.MainForm.ToFirstLastDomestic.StreetTextBoxInfo, "Text", "Orderlookup Address");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='Orderlookup City') on item 'MainForm.ToFirstLastDomestic.CityTextBox'.", repo.MainForm.ToFirstLastDomestic.CityTextBoxInfo, new RecordItemIndex(8));
            Validate.AttributeEqual(repo.MainForm.ToFirstLastDomestic.CityTextBoxInfo, "Text", "Orderlookup City");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='OrderLookup State') on item 'ShipWorksSa.PanelDockingArea.StateComboBox'.", repo.ShipWorksSa.PanelDockingArea.StateComboBoxInfo, new RecordItemIndex(9));
            Validate.AttributeEqual(repo.ShipWorksSa.PanelDockingArea.StateComboBoxInfo, "Text", "OrderLookup State");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='12345') on item 'MainForm.ToFirstLastDomestic.PostalTextBox'.", repo.MainForm.ToFirstLastDomestic.PostalTextBoxInfo, new RecordItemIndex(10));
            Validate.AttributeEqual(repo.MainForm.ToFirstLastDomestic.PostalTextBoxInfo, "Text", "12345");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='[United States, US]') on item 'ShipWorksSa.PanelDockingArea.CountryComboBox'.", repo.ShipWorksSa.PanelDockingArea.CountryComboBoxInfo, new RecordItemIndex(11));
            Validate.AttributeEqual(repo.ShipWorksSa.PanelDockingArea.CountryComboBoxInfo, "Text", "[United States, US]");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='OrderLookup@ShipWorks.com') on item 'ShipWorksSa.PanelDockingArea.EmailTextBox'.", repo.ShipWorksSa.PanelDockingArea.EmailTextBoxInfo, new RecordItemIndex(12));
            Validate.AttributeEqual(repo.ShipWorksSa.PanelDockingArea.EmailTextBoxInfo, "Text", "OrderLookup@ShipWorks.com");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='800-952-7784') on item 'ShipWorksSa.PanelDockingArea.PhoneTextBox'.", repo.ShipWorksSa.PanelDockingArea.PhoneTextBoxInfo, new RecordItemIndex(13));
            Validate.AttributeEqual(repo.ShipWorksSa.PanelDockingArea.PhoneTextBoxInfo, "Text", "800-952-7784");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.PanelDockingArea.Clear' at Center.", repo.ShipWorksSa.PanelDockingArea.ClearInfo, new RecordItemIndex(14));
            repo.ShipWorksSa.PanelDockingArea.Clear.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksSa.PanelDockingArea.Clear' at Center.", repo.ShipWorksSa.PanelDockingArea.ClearInfo, new RecordItemIndex(15));
            repo.ShipWorksSa.PanelDockingArea.Clear.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
