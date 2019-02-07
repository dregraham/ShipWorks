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
    ///The SetupOnTrac recording.
    /// </summary>
    [TestModule("6d1e6009-c806-406b-8d57-5d73cec0da04", ModuleType.Recording, 1)]
    public partial class SetupOnTrac : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static SetupOnTrac instance = new SetupOnTrac();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupOnTrac()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupOnTrac Instance
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

            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(0));
            repo.MainForm.Manage.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Manage Ribbon
            Report.Log(ReportLevel.Info, "Mouse", "Click the Manage Ribbon\r\nMouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(1));
            repo.MainForm.Manage.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(2));
            repo.MainForm.Shipping.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Shipping Settings button
            Report.Log(ReportLevel.Info, "Mouse", "Click the Shipping Settings button\r\nMouse Left Click item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(3));
            repo.MainForm.Shipping.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.OnTrac' at Center.", repo.ShippingSettingsDlg.OnTracInfo, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.OnTrac.MoveTo();
            Delay.Milliseconds(200);
            
            // Check the OnTrac checkbox on the Shipping Settings Window
            Report.Log(ReportLevel.Info, "Invoke action", "Check the OnTrac checkbox on the Shipping Settings Window\r\nInvoking Check() on item 'ShippingSettingsDlg.OnTrac'.", repo.ShippingSettingsDlg.OnTracInfo, new RecordItemIndex(5));
            repo.ShippingSettingsDlg.OnTrac.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.OnTrac1' at Center.", repo.ShippingSettingsDlg.OnTrac1Info, new RecordItemIndex(6));
            repo.ShippingSettingsDlg.OnTrac1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the OnTrac button on the sidebar of the Shipping Settings Window
            Report.Log(ReportLevel.Info, "Mouse", "Click the OnTrac button on the sidebar of the Shipping Settings Window\r\nMouse Left Click item 'ShippingSettingsDlg.OnTrac1' at Center.", repo.ShippingSettingsDlg.OnTrac1Info, new RecordItemIndex(7));
            repo.ShippingSettingsDlg.OnTrac1.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Setup7' at Center.", repo.ShippingSettingsDlg.Setup7Info, new RecordItemIndex(8));
            repo.ShippingSettingsDlg.Setup7.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Setup button on the OnTrac section of the Shipping Settings window
            Report.Log(ReportLevel.Info, "Mouse", "Click the Setup button on the OnTrac section of the Shipping Settings window\r\nMouse Left Click item 'ShippingSettingsDlg.Setup7' at Center.", repo.ShippingSettingsDlg.Setup7Info, new RecordItemIndex(9));
            repo.ShippingSettingsDlg.Setup7.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.Next' at Center.", repo.OnTracSetupWizard.NextInfo, new RecordItemIndex(10));
            repo.OnTracSetupWizard.Next.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Next on the Setup OnTrac Shipping Page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Setup OnTrac Shipping Page\r\nMouse Left Click item 'OnTracSetupWizard.Next' at Center.", repo.OnTracSetupWizard.NextInfo, new RecordItemIndex(11));
            repo.OnTracSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.EnterTheAccountNumberAndAPIPasswor' at Center.", repo.OnTracSetupWizard.MainPanel.EnterTheAccountNumberAndAPIPassworInfo, new RecordItemIndex(12));
            repo.OnTracSetupWizard.MainPanel.EnterTheAccountNumberAndAPIPasswor.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Account field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Account field\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.EnterTheAccountNumberAndAPIPasswor' at Center.", repo.OnTracSetupWizard.MainPanel.EnterTheAccountNumberAndAPIPassworInfo, new RecordItemIndex(13));
            repo.OnTracSetupWizard.MainPanel.EnterTheAccountNumberAndAPIPasswor.Click();
            Delay.Milliseconds(200);
            
            // Enter the Account
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Account\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}166489' with focus on 'OnTracSetupWizard.MainPanel.EnterTheAccountNumberAndAPIPasswor'.", repo.OnTracSetupWizard.MainPanel.EnterTheAccountNumberAndAPIPassworInfo, new RecordItemIndex(14));
            repo.OnTracSetupWizard.MainPanel.EnterTheAccountNumberAndAPIPasswor.PressKeys("{LControlKey down}{Akey}{LControlKey up}166489");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.Password' at Center.", repo.OnTracSetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(15));
            repo.OnTracSetupWizard.MainPanel.Password.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Password field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Password field\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.Password' at Center.", repo.OnTracSetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(16));
            repo.OnTracSetupWizard.MainPanel.Password.Click();
            Delay.Milliseconds(200);
            
            // Enter the Password field
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Password field\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}testshipping1' with focus on 'OnTracSetupWizard.MainPanel.Password'.", repo.OnTracSetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(17));
            repo.OnTracSetupWizard.MainPanel.Password.PressKeys("{LControlKey down}{Akey}{LControlKey up}testshipping1");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(18));
            repo.OnTracSetupWizard.Next1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Next on the OnTrac Credentials page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the OnTrac Credentials page\r\nMouse Left Click item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(19));
            repo.OnTracSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.FullName' at Center.", repo.OnTracSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(20));
            repo.OnTracSetupWizard.MainPanel.FullName.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Full Name field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Full Name field\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.FullName' at Center.", repo.OnTracSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(21));
            repo.OnTracSetupWizard.MainPanel.FullName.Click();
            Delay.Milliseconds(200);
            
            // Enter the Full Name
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Full Name\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Wes Clayton' with focus on 'OnTracSetupWizard.MainPanel.FullName'.", repo.OnTracSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(22));
            repo.OnTracSetupWizard.MainPanel.FullName.PressKeys("{LControlKey down}{Akey}{LControlKey up}Wes Clayton");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.Company' at Center.", repo.OnTracSetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(23));
            repo.OnTracSetupWizard.MainPanel.Company.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Company field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Company field\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.Company' at Center.", repo.OnTracSetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(24));
            repo.OnTracSetupWizard.MainPanel.Company.Click();
            Delay.Milliseconds(200);
            
            // Enter the Company
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Company\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Interapptive' with focus on 'OnTracSetupWizard.MainPanel.Company'.", repo.OnTracSetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(25));
            repo.OnTracSetupWizard.MainPanel.Company.PressKeys("{LControlKey down}{Akey}{LControlKey up}Interapptive");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.Street' at Center.", repo.OnTracSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(26));
            repo.OnTracSetupWizard.MainPanel.Street.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Street field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Street field\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.Street' at Center.", repo.OnTracSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(27));
            repo.OnTracSetupWizard.MainPanel.Street.Click();
            Delay.Milliseconds(200);
            
            // Enter the Street
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Street\r\nKey sequence '{End}{LShiftKey 6}{LShiftKey down}{Home}{LShiftKey up}1 Memorial Dr #2000' with focus on 'OnTracSetupWizard.MainPanel.Street'.", repo.OnTracSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(28));
            repo.OnTracSetupWizard.MainPanel.Street.PressKeys("{End}{LShiftKey 6}{LShiftKey down}{Home}{LShiftKey up}1 Memorial Dr #2000");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.City' at Center.", repo.OnTracSetupWizard.MainPanel.CityInfo, new RecordItemIndex(29));
            repo.OnTracSetupWizard.MainPanel.City.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the City field
            Report.Log(ReportLevel.Info, "Mouse", "Click the City field\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.City' at Center.", repo.OnTracSetupWizard.MainPanel.CityInfo, new RecordItemIndex(30));
            repo.OnTracSetupWizard.MainPanel.City.Click();
            Delay.Milliseconds(200);
            
            // Enter the City
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the City\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Saint Louis' with focus on 'OnTracSetupWizard.MainPanel.City'.", repo.OnTracSetupWizard.MainPanel.CityInfo, new RecordItemIndex(31));
            repo.OnTracSetupWizard.MainPanel.City.PressKeys("{LControlKey down}{Akey}{LControlKey up}Saint Louis");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.State' at Center.", repo.OnTracSetupWizard.MainPanel.StateInfo, new RecordItemIndex(32));
            repo.OnTracSetupWizard.MainPanel.State.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the State
            Report.Log(ReportLevel.Info, "Mouse", "Click the State\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.State' at Center.", repo.OnTracSetupWizard.MainPanel.StateInfo, new RecordItemIndex(33));
            repo.OnTracSetupWizard.MainPanel.State.Click();
            Delay.Milliseconds(200);
            
            // Enter the State
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the State\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Missouri' with focus on 'OnTracSetupWizard.MainPanel.State'.", repo.OnTracSetupWizard.MainPanel.StateInfo, new RecordItemIndex(34));
            repo.OnTracSetupWizard.MainPanel.State.PressKeys("{LControlKey down}{Akey}{LControlKey up}Missouri");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.PostalCode' at Center.", repo.OnTracSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(35));
            repo.OnTracSetupWizard.MainPanel.PostalCode.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Postal Code
            Report.Log(ReportLevel.Info, "Mouse", "Click the Postal Code\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.PostalCode' at Center.", repo.OnTracSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(36));
            repo.OnTracSetupWizard.MainPanel.PostalCode.Click();
            Delay.Milliseconds(200);
            
            // Enter the Postal Code
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Postal Code\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}63102' with focus on 'OnTracSetupWizard.MainPanel.PostalCode'.", repo.OnTracSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(37));
            repo.OnTracSetupWizard.MainPanel.PostalCode.PressKeys("{LControlKey down}{Akey}{LControlKey up}63102");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.Email' at Center.", repo.OnTracSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(38));
            repo.OnTracSetupWizard.MainPanel.Email.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Email
            Report.Log(ReportLevel.Info, "Mouse", "Click the Email\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.Email' at Center.", repo.OnTracSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(39));
            repo.OnTracSetupWizard.MainPanel.Email.Click();
            Delay.Milliseconds(200);
            
            // Enter the Email
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Email\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}sales@interapptive.com' with focus on 'OnTracSetupWizard.MainPanel.Email'.", repo.OnTracSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(40));
            repo.OnTracSetupWizard.MainPanel.Email.PressKeys("{LControlKey down}{Akey}{LControlKey up}sales@interapptive.com");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.Phone' at Center.", repo.OnTracSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(41));
            repo.OnTracSetupWizard.MainPanel.Phone.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Phone
            Report.Log(ReportLevel.Info, "Mouse", "Click the Phone\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.Phone' at Center.", repo.OnTracSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(42));
            repo.OnTracSetupWizard.MainPanel.Phone.Click();
            Delay.Milliseconds(200);
            
            // Enter the Phone
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Phone\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}314-875-9780' with focus on 'OnTracSetupWizard.MainPanel.Phone'.", repo.OnTracSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(43));
            repo.OnTracSetupWizard.MainPanel.Phone.PressKeys("{LControlKey down}{Akey}{LControlKey up}314-875-9780");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.Website' at Center.", repo.OnTracSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(44));
            repo.OnTracSetupWizard.MainPanel.Website.MoveTo();
            Delay.Milliseconds(200);
            
            // Click the Website
            Report.Log(ReportLevel.Info, "Mouse", "Click the Website\r\nMouse Left Click item 'OnTracSetupWizard.MainPanel.Website' at Center.", repo.OnTracSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(45));
            repo.OnTracSetupWizard.MainPanel.Website.Click();
            Delay.Milliseconds(200);
            
            // Enter the Website
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Website\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}interapptive.com' with focus on 'OnTracSetupWizard.MainPanel.Website'.", repo.OnTracSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(46));
            repo.OnTracSetupWizard.MainPanel.Website.PressKeys("{LControlKey down}{Akey}{LControlKey up}interapptive.com");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(47));
            repo.OnTracSetupWizard.Next1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Next on the Contact Information page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Contact Information page\r\nMouse Left Click item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(48));
            repo.OnTracSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(49));
            repo.OnTracSetupWizard.Next1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Next on the OnTrac Settings Page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the OnTrac Settings Page\r\nMouse Left Click item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(50));
            repo.OnTracSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(51));
            repo.OnTracSetupWizard.Next1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Next on the Shipment Defaults Page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Shipment Defaults Page\r\nMouse Left Click item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(52));
            repo.OnTracSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.MainPanel.PrintActionBox' at Center.", repo.OnTracSetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(53));
            repo.OnTracSetupWizard.MainPanel.PrintActionBox.MoveTo();
            Delay.Milliseconds(200);
            
            // Check the Automatically print labels afer processing checkbox
            Report.Log(ReportLevel.Info, "Invoke action", "Check the Automatically print labels afer processing checkbox\r\nInvoking Check() on item 'OnTracSetupWizard.MainPanel.PrintActionBox'.", repo.OnTracSetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(54));
            repo.OnTracSetupWizard.MainPanel.PrintActionBox.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(55));
            repo.OnTracSetupWizard.Next1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Next on the Printing Setup page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Printing Setup page\r\nMouse Left Click item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(56));
            repo.OnTracSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(57));
            repo.OnTracSetupWizard.Next1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Next on the Processing Setup page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Processing Setup page\r\nMouse Left Click item 'OnTracSetupWizard.Next1' at Center.", repo.OnTracSetupWizard.Next1Info, new RecordItemIndex(58));
            repo.OnTracSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'OnTracSetupWizard.Finish' at Center.", repo.OnTracSetupWizard.FinishInfo, new RecordItemIndex(59));
            repo.OnTracSetupWizard.Finish.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Finish on the Setup Complete page
            Report.Log(ReportLevel.Info, "Mouse", "Click Finish on the Setup Complete page\r\nMouse Left Click item 'OnTracSetupWizard.Finish' at Center.", repo.OnTracSetupWizard.FinishInfo, new RecordItemIndex(60));
            repo.OnTracSetupWizard.Finish.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(61));
            repo.ShippingSettingsDlg.Close.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Close on the Shipping Settings window
            Report.Log(ReportLevel.Info, "Mouse", "Click Close on the Shipping Settings window\r\nMouse Left Click item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(62));
            repo.ShippingSettingsDlg.Close.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
