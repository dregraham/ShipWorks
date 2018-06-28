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
    ///The SetupAmazonShipping recording.
    /// </summary>
    [TestModule("e88b1c66-1cb1-4755-84d3-578bc94fdc09", ModuleType.Recording, 1)]
    public partial class SetupAmazonShipping : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static SetupAmazonShipping instance = new SetupAmazonShipping();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupAmazonShipping()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupAmazonShipping Instance
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

            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(0));
            repo.MainForm.Manage.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(1));
            repo.MainForm.Shipping.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Invoke Action", "Invoking Check() on item 'ShippingSettingsDlg.Amazon'.", repo.ShippingSettingsDlg.AmazonInfo, new RecordItemIndex(2));
            repo.ShippingSettingsDlg.Amazon.Check();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingSettingsDlg.Amazon1' at Center.", repo.ShippingSettingsDlg.Amazon1Info, new RecordItemIndex(3));
            repo.ShippingSettingsDlg.Amazon1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingSettingsDlg.Setup9' at Center.", repo.ShippingSettingsDlg.Setup9Info, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.Setup9.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.Next' at Center.", repo.AmazonShipmentSetupWizard.NextInfo, new RecordItemIndex(5));
            repo.AmazonShipmentSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.MainPanel.FullName' at Center.", repo.AmazonShipmentSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(6));
            repo.AmazonShipmentSetupWizard.MainPanel.FullName.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '1{Space down}{LShiftKey down}{Space up}M{LShiftKey up}emorial{Space down}{LShiftKey down}{Space up}D{LShiftKey up}r{Space}{LShiftKey down}#{LShiftKey up}2' with focus on 'AmazonShipmentSetupWizard.MainPanel.FullName'.", repo.AmazonShipmentSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(7));
            repo.AmazonShipmentSetupWizard.MainPanel.FullName.PressKeys("1{Space down}{LShiftKey down}{Space up}M{LShiftKey up}emorial{Space down}{LShiftKey down}{Space up}D{LShiftKey up}r{Space}{LShiftKey down}#{LShiftKey up}2");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '000' with focus on 'AmazonShipmentSetupWizard.MainPanel.FullName'.", repo.AmazonShipmentSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(8));
            repo.AmazonShipmentSetupWizard.MainPanel.FullName.PressKeys("000");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Back 20}{LShiftKey down}W{LShiftKey up}{LShiftKey}es{Space down}{LShiftKey down}{Space up}C{LShiftKey up}layton' with focus on 'AmazonShipmentSetupWizard.MainPanel.FullName'.", repo.AmazonShipmentSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(9));
            repo.AmazonShipmentSetupWizard.MainPanel.FullName.PressKeys("{Back 20}{LShiftKey down}W{LShiftKey up}{LShiftKey}es{Space down}{LShiftKey down}{Space up}C{LShiftKey up}layton");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.MainPanel.Company' at Center.", repo.AmazonShipmentSetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(10));
            repo.AmazonShipmentSetupWizard.MainPanel.Company.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{LShiftKey down}i{LShiftKey up}Nterapptive' with focus on 'AmazonShipmentSetupWizard.MainPanel.Company'.", repo.AmazonShipmentSetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(11));
            repo.AmazonShipmentSetupWizard.MainPanel.Company.PressKeys("{LShiftKey down}i{LShiftKey up}Nterapptive");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.MainPanel.Street' at Center.", repo.AmazonShipmentSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(12));
            repo.AmazonShipmentSetupWizard.MainPanel.Street.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '1 Memorial Dr Suite #2000' with focus on 'AmazonShipmentSetupWizard.MainPanel.Street'.", repo.AmazonShipmentSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(13));
            repo.AmazonShipmentSetupWizard.MainPanel.Street.PressKeys("1 Memorial Dr Suite #2000");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.MainPanel.City' at Center.", repo.AmazonShipmentSetupWizard.MainPanel.CityInfo, new RecordItemIndex(14));
            repo.AmazonShipmentSetupWizard.MainPanel.City.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'Saint Louis' with focus on 'AmazonShipmentSetupWizard.MainPanel.City'.", repo.AmazonShipmentSetupWizard.MainPanel.CityInfo, new RecordItemIndex(15));
            repo.AmazonShipmentSetupWizard.MainPanel.City.PressKeys("Saint Louis");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.MainPanel.State' at Center.", repo.AmazonShipmentSetupWizard.MainPanel.StateInfo, new RecordItemIndex(16));
            repo.AmazonShipmentSetupWizard.MainPanel.State.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'MO' with focus on 'AmazonShipmentSetupWizard.MainPanel.State'.", repo.AmazonShipmentSetupWizard.MainPanel.StateInfo, new RecordItemIndex(17));
            repo.AmazonShipmentSetupWizard.MainPanel.State.PressKeys("MO");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.MainPanel.PostalCode' at Center.", repo.AmazonShipmentSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(18));
            repo.AmazonShipmentSetupWizard.MainPanel.PostalCode.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '63102' with focus on 'AmazonShipmentSetupWizard.MainPanel.PostalCode'.", repo.AmazonShipmentSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(19));
            repo.AmazonShipmentSetupWizard.MainPanel.PostalCode.PressKeys("63102");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.MainPanel.Email' at Center.", repo.AmazonShipmentSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(20));
            repo.AmazonShipmentSetupWizard.MainPanel.Email.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'sales@interapptive.com' with focus on 'AmazonShipmentSetupWizard.MainPanel.Email'.", repo.AmazonShipmentSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(21));
            repo.AmazonShipmentSetupWizard.MainPanel.Email.PressKeys("sales@interapptive.com");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.MainPanel.Phone' at Center.", repo.AmazonShipmentSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(22));
            repo.AmazonShipmentSetupWizard.MainPanel.Phone.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '314-875-9780' with focus on 'AmazonShipmentSetupWizard.MainPanel.Phone'.", repo.AmazonShipmentSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(23));
            repo.AmazonShipmentSetupWizard.MainPanel.Phone.PressKeys("314-875-9780");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.MainPanel.Website' at Center.", repo.AmazonShipmentSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(24));
            repo.AmazonShipmentSetupWizard.MainPanel.Website.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'interapptive.com' with focus on 'AmazonShipmentSetupWizard.MainPanel.Website'.", repo.AmazonShipmentSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(25));
            repo.AmazonShipmentSetupWizard.MainPanel.Website.PressKeys("interapptive.com");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.Next' at Center.", repo.AmazonShipmentSetupWizard.NextInfo, new RecordItemIndex(26));
            repo.AmazonShipmentSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(27));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.Next' at Center.", repo.AmazonShipmentSetupWizard.NextInfo, new RecordItemIndex(28));
            repo.AmazonShipmentSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(29));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Invoke Action", "Invoking Uncheck() on item 'AmazonShipmentSetupWizard.MainPanel.PrintActionBox'.", repo.AmazonShipmentSetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(30));
            repo.AmazonShipmentSetupWizard.MainPanel.PrintActionBox.Uncheck();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(31));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.Next1' at Center.", repo.AmazonShipmentSetupWizard.Next1Info, new RecordItemIndex(32));
            repo.AmazonShipmentSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(33));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.Next1' at Center.", repo.AmazonShipmentSetupWizard.Next1Info, new RecordItemIndex(34));
            repo.AmazonShipmentSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(35));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'AmazonShipmentSetupWizard.Next1' at Center.", repo.AmazonShipmentSetupWizard.Next1Info, new RecordItemIndex(36));
            repo.AmazonShipmentSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(37));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(38));
            repo.ShippingSettingsDlg.Close.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
