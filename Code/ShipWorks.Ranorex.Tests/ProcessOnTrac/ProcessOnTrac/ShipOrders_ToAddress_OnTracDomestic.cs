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

namespace ProcessOnTrac
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The ShipOrders_ToAddress_OnTracDomestic recording.
    /// </summary>
    [TestModule("1a176cb5-4bd4-4ba6-8a65-5b96f17af82f", ModuleType.Recording, 1)]
    public partial class ShipOrders_ToAddress_OnTracDomestic : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static ShipOrders_ToAddress_OnTracDomestic instance = new ShipOrders_ToAddress_OnTracDomestic();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ShipOrders_ToAddress_OnTracDomestic()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ShipOrders_ToAddress_OnTracDomestic Instance
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

            // Give Focus: Country
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Country\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Country'.", repo.ShippingDlg.SplitContainer.CountryInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.Country.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Country' at Center.", repo.ShippingDlg.SplitContainer.CountryInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.Country.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Country: United States
            Report.Log(ReportLevel.Info, "Set value", "Set Country: United States\r\nSetting attribute SelectedItemText to 'United States' on item 'ShippingDlg.SplitContainer.Country'.", repo.ShippingDlg.SplitContainer.CountryInfo, new RecordItemIndex(2));
            repo.ShippingDlg.SplitContainer.Country.Element.SetAttributeValue("SelectedItemText", "United States");
            Delay.Milliseconds(0);
            
            // Give Focus: Full Name
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Full Name\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.FullName'.", repo.ShippingDlg.SplitContainer.FullNameInfo, new RecordItemIndex(3));
            repo.ShippingDlg.SplitContainer.FullName.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.FullName' at Center.", repo.ShippingDlg.SplitContainer.FullNameInfo, new RecordItemIndex(4));
            repo.ShippingDlg.SplitContainer.FullName.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Full Name: Full Name
            Report.Log(ReportLevel.Info, "Keyboard", "Set Full Name: Full Name\r\nKey sequence 'Full Name' with focus on 'ShippingDlg.SplitContainer.FullName'.", repo.ShippingDlg.SplitContainer.FullNameInfo, new RecordItemIndex(5));
            repo.ShippingDlg.SplitContainer.FullName.PressKeys("Full Name");
            Delay.Milliseconds(0);
            
            // Give Focus: Company
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Company\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Company'.", repo.ShippingDlg.SplitContainer.CompanyInfo, new RecordItemIndex(6));
            repo.ShippingDlg.SplitContainer.Company.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Company' at Center.", repo.ShippingDlg.SplitContainer.CompanyInfo, new RecordItemIndex(7));
            repo.ShippingDlg.SplitContainer.Company.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Company: Company Name
            Report.Log(ReportLevel.Info, "Keyboard", "Set Company: Company Name\r\nKey sequence 'Company Name' with focus on 'ShippingDlg.SplitContainer.Company'.", repo.ShippingDlg.SplitContainer.CompanyInfo, new RecordItemIndex(8));
            repo.ShippingDlg.SplitContainer.Company.PressKeys("Company Name");
            Delay.Milliseconds(0);
            
            // Give Focus: Street
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: Street\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.Street'.", repo.ShippingDlg.SplitContainer.StreetInfo, new RecordItemIndex(9));
            repo.ShippingDlg.SplitContainer.Street.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.Street' at Center.", repo.ShippingDlg.SplitContainer.StreetInfo, new RecordItemIndex(10));
            repo.ShippingDlg.SplitContainer.Street.MoveTo();
            Delay.Milliseconds(200);
            
            // Set Street: 1 Memorial Dr{Return}Suite 2000
            Report.Log(ReportLevel.Info, "Keyboard", "Set Street: 1 Memorial Dr{Return}Suite 2000\r\nKey sequence '98 Shaw Ave' with focus on 'ShippingDlg.SplitContainer.Street'.", repo.ShippingDlg.SplitContainer.StreetInfo, new RecordItemIndex(11));
            repo.ShippingDlg.SplitContainer.Street.PressKeys("98 Shaw Ave");
            Delay.Milliseconds(0);
            
            // Give Focus: City
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: City\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.City'.", repo.ShippingDlg.SplitContainer.CityInfo, new RecordItemIndex(12));
            repo.ShippingDlg.SplitContainer.City.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.City' at Center.", repo.ShippingDlg.SplitContainer.CityInfo, new RecordItemIndex(13));
            repo.ShippingDlg.SplitContainer.City.MoveTo();
            Delay.Milliseconds(200);
            
            // Set City: Saint Louis
            Report.Log(ReportLevel.Info, "Keyboard", "Set City: Saint Louis\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Clovis' with focus on 'ShippingDlg.SplitContainer.City'.", repo.ShippingDlg.SplitContainer.CityInfo, new RecordItemIndex(14));
            repo.ShippingDlg.SplitContainer.City.PressKeys("{LControlKey down}{Akey}{LControlKey up}Clovis");
            Delay.Milliseconds(0);
            
            // Give Focus: State
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: State\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.State'.", repo.ShippingDlg.SplitContainer.StateInfo, new RecordItemIndex(15));
            repo.ShippingDlg.SplitContainer.State.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.State' at Center.", repo.ShippingDlg.SplitContainer.StateInfo, new RecordItemIndex(16));
            repo.ShippingDlg.SplitContainer.State.MoveTo();
            Delay.Milliseconds(200);
            
            // Select All: State/Province
            Report.Log(ReportLevel.Info, "Keyboard", "Select All: State/Province\r\nKey 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.State'.", repo.ShippingDlg.SplitContainer.StateInfo, new RecordItemIndex(17));
            Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.State);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set State: Missouri
            Report.Log(ReportLevel.Info, "Keyboard", "Set State: Missouri\r\nKey sequence 'CA' with focus on 'ShippingDlg.SplitContainer.State'.", repo.ShippingDlg.SplitContainer.StateInfo, new RecordItemIndex(18));
            repo.ShippingDlg.SplitContainer.State.PressKeys("CA");
            Delay.Milliseconds(0);
            
            // Give Focus: PostalCode
            Report.Log(ReportLevel.Info, "Invoke action", "Give Focus: PostalCode\r\nInvoking Focus() on item 'ShippingDlg.SplitContainer.PostalCode'.", repo.ShippingDlg.SplitContainer.PostalCodeInfo, new RecordItemIndex(19));
            repo.ShippingDlg.SplitContainer.PostalCode.Focus();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.PostalCode' at Center.", repo.ShippingDlg.SplitContainer.PostalCodeInfo, new RecordItemIndex(20));
            repo.ShippingDlg.SplitContainer.PostalCode.MoveTo();
            Delay.Milliseconds(200);
            
            // Set PostalCode: 63102
            Report.Log(ReportLevel.Info, "Keyboard", "Set PostalCode: 63102\r\nKey sequence '93612' with focus on 'ShippingDlg.SplitContainer.PostalCode'.", repo.ShippingDlg.SplitContainer.PostalCodeInfo, new RecordItemIndex(21));
            repo.ShippingDlg.SplitContainer.PostalCode.PressKeys("93612");
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
