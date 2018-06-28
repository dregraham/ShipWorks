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
    ///The SetupExpress1 recording.
    /// </summary>
    [TestModule("d8d03f29-38a7-4060-bb22-56dfa6e1243f", ModuleType.Recording, 1)]
    public partial class SetupExpress1 : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTest.SmokeTestRepository repository.
        /// </summary>
        public static SmokeTest.SmokeTestRepository repo = SmokeTest.SmokeTestRepository.Instance;

        static SetupExpress1 instance = new SetupExpress1();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupExpress1()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupExpress1 Instance
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

            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(0));
            repo.MainForm.Manage.MoveTo();
            
            // Click the Manage Ribbon
            Report.Log(ReportLevel.Info, "Mouse", "Click the Manage Ribbon\r\nMouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(1));
            repo.MainForm.Manage.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(2));
            repo.MainForm.Shipping.MoveTo();
            
            // Click the Shipping Settings button
            Report.Log(ReportLevel.Info, "Mouse", "Click the Shipping Settings button\r\nMouse Left Click item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(3));
            repo.MainForm.Shipping.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.USPSExpress1' at Center.", repo.ShippingSettingsDlg.USPSExpress1Info, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.USPSExpress1.MoveTo();
            
            // Check the USPS (Express 1) checkbox on the Shipping Settings Window
            Report.Log(ReportLevel.Info, "Invoke Action", "Check the USPS (Express 1) checkbox on the Shipping Settings Window\r\nInvoking Check() on item 'ShippingSettingsDlg.USPSExpress1'.", repo.ShippingSettingsDlg.USPSExpress1Info, new RecordItemIndex(5));
            repo.ShippingSettingsDlg.USPSExpress1.Check();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.USPSExpress11' at Center.", repo.ShippingSettingsDlg.USPSExpress11Info, new RecordItemIndex(6));
            repo.ShippingSettingsDlg.USPSExpress11.MoveTo();
            
            // Click the USPS (Express 1) button on the sidebar of the Shipping Settings Window
            Report.Log(ReportLevel.Info, "Mouse", "Click the USPS (Express 1) button on the sidebar of the Shipping Settings Window\r\nMouse Left Click item 'ShippingSettingsDlg.USPSExpress11' at Center.", repo.ShippingSettingsDlg.USPSExpress11Info, new RecordItemIndex(7));
            repo.ShippingSettingsDlg.USPSExpress11.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Setup4' at Center.", repo.ShippingSettingsDlg.Setup4Info, new RecordItemIndex(8));
            repo.ShippingSettingsDlg.Setup4.MoveTo();
            
            // Click the Setup button on the Express 1 section of the Shipping Settings window
            Report.Log(ReportLevel.Info, "Mouse", "Click the Setup button on the Express 1 section of the Shipping Settings window\r\nMouse Left Click item 'ShippingSettingsDlg.Setup4' at Center.", repo.ShippingSettingsDlg.Setup4Info, new RecordItemIndex(9));
            repo.ShippingSettingsDlg.Setup4.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.RadioExistingAccount' at Center.", repo.Express1SetupWizard.MainPanel.RadioExistingAccountInfo, new RecordItemIndex(10));
            repo.Express1SetupWizard.MainPanel.RadioExistingAccount.MoveTo();
            
            // Select the Use an existings Express 1 account
            Report.Log(ReportLevel.Info, "Mouse", "Select the Use an existings Express 1 account\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.RadioExistingAccount' at Center.", repo.Express1SetupWizard.MainPanel.RadioExistingAccountInfo, new RecordItemIndex(11));
            repo.Express1SetupWizard.MainPanel.RadioExistingAccount.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(12));
            repo.Express1SetupWizard.Next.MoveTo();
            
            // Click the next button on the Setup Express 1 Shipping page
            Report.Log(ReportLevel.Info, "Mouse", "Click the next button on the Setup Express 1 Shipping page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(13));
            repo.Express1SetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.FullName' at Center.", repo.Express1SetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(14));
            repo.Express1SetupWizard.MainPanel.FullName.MoveTo();
            
            // Click the Full Name field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Full Name field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.FullName' at Center.", repo.Express1SetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(15));
            repo.Express1SetupWizard.MainPanel.FullName.Click();
            
            // Enter the Full Name
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Full Name\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Wes Clayton' with focus on 'Express1SetupWizard.MainPanel.FullName'.", repo.Express1SetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(16));
            repo.Express1SetupWizard.MainPanel.FullName.PressKeys("{LControlKey down}{Akey}{LControlKey up}Wes Clayton");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Company' at Center.", repo.Express1SetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(17));
            repo.Express1SetupWizard.MainPanel.Company.MoveTo();
            
            // Click the Company field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Company field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Company' at Center.", repo.Express1SetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(18));
            repo.Express1SetupWizard.MainPanel.Company.Click();
            
            // Enter the Company
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Company\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Interapptive' with focus on 'Express1SetupWizard.MainPanel.Company'.", repo.Express1SetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(19));
            repo.Express1SetupWizard.MainPanel.Company.PressKeys("{LControlKey down}{Akey}{LControlKey up}Interapptive");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Street' at Center.", repo.Express1SetupWizard.MainPanel.StreetInfo, new RecordItemIndex(20));
            repo.Express1SetupWizard.MainPanel.Street.MoveTo();
            
            // Click the Street field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Street field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Street' at Center.", repo.Express1SetupWizard.MainPanel.StreetInfo, new RecordItemIndex(21));
            repo.Express1SetupWizard.MainPanel.Street.Click();
            
            // Enter the Street
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Street\r\nKey sequence '{End}{LShiftKey 6}{LShiftKey down}{Home}{LShiftKey up}1 Memorial Dr #2000' with focus on 'Express1SetupWizard.MainPanel.Street'.", repo.Express1SetupWizard.MainPanel.StreetInfo, new RecordItemIndex(22));
            repo.Express1SetupWizard.MainPanel.Street.PressKeys("{End}{LShiftKey 6}{LShiftKey down}{Home}{LShiftKey up}1 Memorial Dr #2000");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.City' at Center.", repo.Express1SetupWizard.MainPanel.CityInfo, new RecordItemIndex(23));
            repo.Express1SetupWizard.MainPanel.City.MoveTo();
            
            // Click the City field
            Report.Log(ReportLevel.Info, "Mouse", "Click the City field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.City' at Center.", repo.Express1SetupWizard.MainPanel.CityInfo, new RecordItemIndex(24));
            repo.Express1SetupWizard.MainPanel.City.Click();
            
            // Enter the City
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the City\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Saint Louis' with focus on 'Express1SetupWizard.MainPanel.City'.", repo.Express1SetupWizard.MainPanel.CityInfo, new RecordItemIndex(25));
            repo.Express1SetupWizard.MainPanel.City.PressKeys("{LControlKey down}{Akey}{LControlKey up}Saint Louis");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.StateExpress1' at Center.", repo.Express1SetupWizard.MainPanel.StateExpress1Info, new RecordItemIndex(26));
            repo.Express1SetupWizard.MainPanel.StateExpress1.MoveTo();
            
            // Click the State
            Report.Log(ReportLevel.Info, "Mouse", "Click the State\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.StateExpress1' at Center.", repo.Express1SetupWizard.MainPanel.StateExpress1Info, new RecordItemIndex(27));
            repo.Express1SetupWizard.MainPanel.StateExpress1.Click();
            
            // Enter the State
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the State\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}MO' with focus on 'Express1SetupWizard.MainPanel.StateExpress1'.", repo.Express1SetupWizard.MainPanel.StateExpress1Info, new RecordItemIndex(28));
            repo.Express1SetupWizard.MainPanel.StateExpress1.PressKeys("{LControlKey down}{Akey}{LControlKey up}MO");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.PostalCodeExpress1' at Center.", repo.Express1SetupWizard.MainPanel.PostalCodeExpress1Info, new RecordItemIndex(29));
            repo.Express1SetupWizard.MainPanel.PostalCodeExpress1.MoveTo();
            
            // Click the Postal Code
            Report.Log(ReportLevel.Info, "Mouse", "Click the Postal Code\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.PostalCodeExpress1' at Center.", repo.Express1SetupWizard.MainPanel.PostalCodeExpress1Info, new RecordItemIndex(30));
            repo.Express1SetupWizard.MainPanel.PostalCodeExpress1.Click();
            
            // Enter the Postal Code
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Postal Code\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}63102' with focus on 'Express1SetupWizard.MainPanel.PostalCodeExpress1'.", repo.Express1SetupWizard.MainPanel.PostalCodeExpress1Info, new RecordItemIndex(31));
            repo.Express1SetupWizard.MainPanel.PostalCodeExpress1.PressKeys("{LControlKey down}{Akey}{LControlKey up}63102");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Email' at Center.", repo.Express1SetupWizard.MainPanel.EmailInfo, new RecordItemIndex(32));
            repo.Express1SetupWizard.MainPanel.Email.MoveTo();
            
            // Click the Email
            Report.Log(ReportLevel.Info, "Mouse", "Click the Email\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Email' at Center.", repo.Express1SetupWizard.MainPanel.EmailInfo, new RecordItemIndex(33));
            repo.Express1SetupWizard.MainPanel.Email.Click();
            
            // Enter the Email
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Email\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}sales@interapptive.com' with focus on 'Express1SetupWizard.MainPanel.Email'.", repo.Express1SetupWizard.MainPanel.EmailInfo, new RecordItemIndex(34));
            repo.Express1SetupWizard.MainPanel.Email.PressKeys("{LControlKey down}{Akey}{LControlKey up}sales@interapptive.com");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Phone' at Center.", repo.Express1SetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(35));
            repo.Express1SetupWizard.MainPanel.Phone.MoveTo();
            
            // Click the Phone
            Report.Log(ReportLevel.Info, "Mouse", "Click the Phone\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Phone' at Center.", repo.Express1SetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(36));
            repo.Express1SetupWizard.MainPanel.Phone.Click();
            
            // Enter the Phone
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Phone\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}800-952-7784' with focus on 'Express1SetupWizard.MainPanel.Phone'.", repo.Express1SetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(37));
            repo.Express1SetupWizard.MainPanel.Phone.PressKeys("{LControlKey down}{Akey}{LControlKey up}800-952-7784");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Fax' at Center.", repo.Express1SetupWizard.MainPanel.FaxInfo, new RecordItemIndex(38));
            repo.Express1SetupWizard.MainPanel.Fax.MoveTo();
            
            // Click the Fax
            Report.Log(ReportLevel.Info, "Mouse", "Click the Fax\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Fax' at Center.", repo.Express1SetupWizard.MainPanel.FaxInfo, new RecordItemIndex(39));
            repo.Express1SetupWizard.MainPanel.Fax.Click();
            
            // Enter the Fax
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Fax\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}800-952-7784' with focus on 'Express1SetupWizard.MainPanel.Fax'.", repo.Express1SetupWizard.MainPanel.FaxInfo, new RecordItemIndex(40));
            repo.Express1SetupWizard.MainPanel.Fax.PressKeys("{LControlKey down}{Akey}{LControlKey up}800-952-7784");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(41));
            repo.Express1SetupWizard.Next.MoveTo();
            
            // Click Next on the Account Registration Enter the Address for your Express1 Account page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Account Registration Enter the Address for your Express1 Account page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(42));
            repo.Express1SetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Express1AccountNumber' at Center.", repo.Express1SetupWizard.MainPanel.Express1AccountNumberInfo, new RecordItemIndex(43));
            repo.Express1SetupWizard.MainPanel.Express1AccountNumber.MoveTo();
            
            // Click the Express1 account number field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Express1 account number field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Express1AccountNumber' at Center.", repo.Express1SetupWizard.MainPanel.Express1AccountNumberInfo, new RecordItemIndex(44));
            repo.Express1SetupWizard.MainPanel.Express1AccountNumber.Click();
            
            // Enter the Express1 account number
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Express1 account number\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}759cc789-25ab-4701-b791-b0c7d4b47701' with focus on 'Express1SetupWizard.MainPanel.Express1AccountNumber'.", repo.Express1SetupWizard.MainPanel.Express1AccountNumberInfo, new RecordItemIndex(45));
            repo.Express1SetupWizard.MainPanel.Express1AccountNumber.PressKeys("{LControlKey down}{Akey}{LControlKey up}759cc789-25ab-4701-b791-b0c7d4b47701");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Password' at Center.", repo.Express1SetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(46));
            repo.Express1SetupWizard.MainPanel.Password.MoveTo();
            
            // Click the Password field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Password field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Password' at Center.", repo.Express1SetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(47));
            repo.Express1SetupWizard.MainPanel.Password.Click();
            
            // Enter the Password
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Password\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}75496d77-0b3e-4d18-b272-9399197ccd90' with focus on 'Express1SetupWizard.MainPanel.Password'.", repo.Express1SetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(48));
            repo.Express1SetupWizard.MainPanel.Password.PressKeys("{LControlKey down}{Akey}{LControlKey up}75496d77-0b3e-4d18-b272-9399197ccd90");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(49));
            repo.Express1SetupWizard.Next.MoveTo();
            
            // Click Next on the Express1 Account Enter your Existing Account Information page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Express1 Account Enter your Existing Account Information page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(50));
            repo.Express1SetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(51));
            repo.Express1SetupWizard.Next.MoveTo();
            
            // Click next on the Express1 Settings Configure Express1 settings page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Express1 Settings Configure Express1 settings page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(52));
            repo.Express1SetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(53));
            repo.Express1SetupWizard.Next.MoveTo();
            
            // Click next on the Origin Address page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Origin Address page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(54));
            repo.Express1SetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(55));
            repo.Express1SetupWizard.Next.MoveTo();
            
            // Click next on the Shipping Defaults page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Shipping Defaults page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(56));
            repo.Express1SetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.PrintActionBox' at Center.", repo.Express1SetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(57));
            repo.Express1SetupWizard.MainPanel.PrintActionBox.MoveTo();
            
            // Uncheck the Automatically print labels after processing checkbox
            Report.Log(ReportLevel.Info, "Invoke Action", "Uncheck the Automatically print labels after processing checkbox\r\nInvoking Uncheck() on item 'Express1SetupWizard.MainPanel.PrintActionBox'.", repo.Express1SetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(58));
            repo.Express1SetupWizard.MainPanel.PrintActionBox.Uncheck();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(59));
            repo.Express1SetupWizard.Next.MoveTo();
            
            // Click next on the Printing Setup page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Printing Setup page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(60));
            repo.Express1SetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(61));
            repo.Express1SetupWizard.Next.MoveTo();
            
            // Click the next button on the Processing Setup page
            Report.Log(ReportLevel.Info, "Mouse", "Click the next button on the Processing Setup page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(62));
            repo.Express1SetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.BuyPostage' at Center.", repo.Express1SetupWizard.MainPanel.BuyPostageInfo, new RecordItemIndex(63));
            repo.Express1SetupWizard.MainPanel.BuyPostage.MoveTo();
            
            // Click: Buy Postage...
            Report.Log(ReportLevel.Info, "Mouse", "Click: Buy Postage...\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.BuyPostage' at Center.", repo.Express1SetupWizard.MainPanel.BuyPostageInfo, new RecordItemIndex(64));
            repo.Express1SetupWizard.MainPanel.BuyPostage.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UspsPurchasePostageDlg.Postage' at Center.", repo.UspsPurchasePostageDlg.PostageInfo, new RecordItemIndex(65));
            repo.UspsPurchasePostageDlg.Postage.MoveTo();
            
            // Give Focus: Purchase amount
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Purchase amount\r\nInvoking Focus() on item 'UspsPurchasePostageDlg.Postage'.", repo.UspsPurchasePostageDlg.PostageInfo, new RecordItemIndex(66));
            repo.UspsPurchasePostageDlg.Postage.Focus();
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'UspsPurchasePostageDlg.Postage'.", repo.UspsPurchasePostageDlg.PostageInfo, new RecordItemIndex(67));
            Keyboard.PrepareFocus(repo.UspsPurchasePostageDlg.Postage);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            
            // Set Purchase amount: 300
            Report.Log(ReportLevel.Info, "Keyboard", "Set Purchase amount: 300\r\nKey sequence '300' with focus on 'UspsPurchasePostageDlg.Postage'.", repo.UspsPurchasePostageDlg.PostageInfo, new RecordItemIndex(68));
            repo.UspsPurchasePostageDlg.Postage.PressKeys("300");
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UspsPurchasePostageDlg.Purchase' at Center.", repo.UspsPurchasePostageDlg.PurchaseInfo, new RecordItemIndex(69));
            repo.UspsPurchasePostageDlg.Purchase.MoveTo();
            
            // Click: Purchase
            Report.Log(ReportLevel.Info, "Mouse", "Click: Purchase\r\nMouse Left Click item 'UspsPurchasePostageDlg.Purchase' at Center.", repo.UspsPurchasePostageDlg.PurchaseInfo, new RecordItemIndex(70));
            repo.UspsPurchasePostageDlg.Purchase.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShipWorks.ButtonOK' at Center.", repo.ShipWorks.ButtonOKInfo, new RecordItemIndex(71));
            repo.ShipWorks.ButtonOK.MoveTo();
            
            // Click: OK (purchase request has been submitted to Express1)
            Report.Log(ReportLevel.Info, "Mouse", "Click: OK (purchase request has been submitted to Express1)\r\nMouse Left Click item 'ShipWorks.ButtonOK' at Center.", repo.ShipWorks.ButtonOKInfo, new RecordItemIndex(72));
            repo.ShipWorks.ButtonOK.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Finish' at Center.", repo.Express1SetupWizard.FinishInfo, new RecordItemIndex(73));
            repo.Express1SetupWizard.Finish.MoveTo();
            
            // Click Finish on the Express1 Account Registration Complete page
            Report.Log(ReportLevel.Info, "Mouse", "Click Finish on the Express1 Account Registration Complete page\r\nMouse Left Click item 'Express1SetupWizard.Finish' at Center.", repo.Express1SetupWizard.FinishInfo, new RecordItemIndex(74));
            repo.Express1SetupWizard.Finish.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(75));
            repo.ShippingSettingsDlg.Close.MoveTo();
            
            // Click Close on the Shipping Setting window
            Report.Log(ReportLevel.Info, "Mouse", "Click Close on the Shipping Setting window\r\nMouse Left Click item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(76));
            repo.ShippingSettingsDlg.Close.Click();
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
