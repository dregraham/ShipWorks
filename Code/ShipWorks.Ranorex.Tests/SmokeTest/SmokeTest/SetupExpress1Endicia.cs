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
    ///The SetupExpress1Endicia recording.
    /// </summary>
    [TestModule("3283f0c2-5d91-44d1-b2d7-e466d7d93c3e", ModuleType.Recording, 1)]
    public partial class SetupExpress1Endicia : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static SetupExpress1Endicia instance = new SetupExpress1Endicia();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupExpress1Endicia()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupExpress1Endicia Instance
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

            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(0));
            repo.MainForm.Manage.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Manage Ribbon
            Report.Log(ReportLevel.Info, "Mouse", "Click the Manage Ribbon\r\nMouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(1));
            repo.MainForm.Manage.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(2));
            repo.MainForm.Shipping.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Shipping Settings button
            Report.Log(ReportLevel.Info, "Mouse", "Click the Shipping Settings button\r\nMouse Left Click item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(3));
            repo.MainForm.Shipping.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.USPSExpress1ForEndicia' at Center.", repo.ShippingSettingsDlg.USPSExpress1ForEndiciaInfo, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.USPSExpress1ForEndicia.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Check the USPS (Express 1 for Endicia) checkbox on the Shipping Settings Window
            Report.Log(ReportLevel.Info, "Invoke action", "Check the USPS (Express 1 for Endicia) checkbox on the Shipping Settings Window\r\nInvoking Check() on item 'ShippingSettingsDlg.USPSExpress1ForEndicia'.", repo.ShippingSettingsDlg.USPSExpress1ForEndiciaInfo, new RecordItemIndex(5));
            repo.ShippingSettingsDlg.USPSExpress1ForEndicia.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.USPSExpress1ForEndicia1' at Center.", repo.ShippingSettingsDlg.USPSExpress1ForEndicia1Info, new RecordItemIndex(6));
            repo.ShippingSettingsDlg.USPSExpress1ForEndicia1.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the USPS (Express 1 for Endicia) button on the sidebar of the Shipping Settings Window
            Report.Log(ReportLevel.Info, "Mouse", "Click the USPS (Express 1 for Endicia) button on the sidebar of the Shipping Settings Window\r\nMouse Left Click item 'ShippingSettingsDlg.USPSExpress1ForEndicia1' at Center.", repo.ShippingSettingsDlg.USPSExpress1ForEndicia1Info, new RecordItemIndex(7));
            repo.ShippingSettingsDlg.USPSExpress1ForEndicia1.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Setup5' at Center.", repo.ShippingSettingsDlg.Setup5Info, new RecordItemIndex(8));
            repo.ShippingSettingsDlg.Setup5.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Setup button on the Express 1 for Endicia section of the Shipping Settings window
            Report.Log(ReportLevel.Info, "Mouse", "Click the Setup button on the Express 1 for Endicia section of the Shipping Settings window\r\nMouse Left Click item 'ShippingSettingsDlg.Setup5' at Center.", repo.ShippingSettingsDlg.Setup5Info, new RecordItemIndex(9));
            repo.ShippingSettingsDlg.Setup5.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.UseAnExistingExpress1Account' at Center.", repo.Express1SetupWizard.MainPanel.UseAnExistingExpress1AccountInfo, new RecordItemIndex(10));
            repo.Express1SetupWizard.MainPanel.UseAnExistingExpress1Account.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Select the Use an existings Express 1 account
            Report.Log(ReportLevel.Info, "Mouse", "Select the Use an existings Express 1 account\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.UseAnExistingExpress1Account' at Center.", repo.Express1SetupWizard.MainPanel.UseAnExistingExpress1AccountInfo, new RecordItemIndex(11));
            repo.Express1SetupWizard.MainPanel.UseAnExistingExpress1Account.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(12));
            repo.Express1SetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the next button on the Setup Express 1 for Endicia Shipping page
            Report.Log(ReportLevel.Info, "Mouse", "Click the next button on the Setup Express 1 for Endicia Shipping page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(13));
            repo.Express1SetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.FullName' at Center.", repo.Express1SetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(14));
            repo.Express1SetupWizard.MainPanel.FullName.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Full Name field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Full Name field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.FullName' at Center.", repo.Express1SetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(15));
            repo.Express1SetupWizard.MainPanel.FullName.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Full Name
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Full Name\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Wes Clayton' with focus on 'Express1SetupWizard.MainPanel.FullName'.", repo.Express1SetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(16));
            repo.Express1SetupWizard.MainPanel.FullName.PressKeys("{LControlKey down}{Akey}{LControlKey up}Wes Clayton");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Company' at Center.", repo.Express1SetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(17));
            repo.Express1SetupWizard.MainPanel.Company.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Company field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Company field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Company' at Center.", repo.Express1SetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(18));
            repo.Express1SetupWizard.MainPanel.Company.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Company
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Company\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Interapptive' with focus on 'Express1SetupWizard.MainPanel.Company'.", repo.Express1SetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(19));
            repo.Express1SetupWizard.MainPanel.Company.PressKeys("{LControlKey down}{Akey}{LControlKey up}Interapptive");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Street' at Center.", repo.Express1SetupWizard.MainPanel.StreetInfo, new RecordItemIndex(20));
            repo.Express1SetupWizard.MainPanel.Street.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Street field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Street field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Street' at Center.", repo.Express1SetupWizard.MainPanel.StreetInfo, new RecordItemIndex(21));
            repo.Express1SetupWizard.MainPanel.Street.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Street
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Street\r\nKey sequence '{End}{LShiftKey 6}{LShiftKey down}{Home}{LShiftKey up}1 Memorial Dr #2000' with focus on 'Express1SetupWizard.MainPanel.Street'.", repo.Express1SetupWizard.MainPanel.StreetInfo, new RecordItemIndex(22));
            repo.Express1SetupWizard.MainPanel.Street.PressKeys("{End}{LShiftKey 6}{LShiftKey down}{Home}{LShiftKey up}1 Memorial Dr #2000");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.City' at Center.", repo.Express1SetupWizard.MainPanel.CityInfo, new RecordItemIndex(23));
            repo.Express1SetupWizard.MainPanel.City.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the City field
            Report.Log(ReportLevel.Info, "Mouse", "Click the City field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.City' at Center.", repo.Express1SetupWizard.MainPanel.CityInfo, new RecordItemIndex(24));
            repo.Express1SetupWizard.MainPanel.City.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the City
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the City\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Saint Louis' with focus on 'Express1SetupWizard.MainPanel.City'.", repo.Express1SetupWizard.MainPanel.CityInfo, new RecordItemIndex(25));
            repo.Express1SetupWizard.MainPanel.City.PressKeys("{LControlKey down}{Akey}{LControlKey up}Saint Louis");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.State' at Center.", repo.Express1SetupWizard.MainPanel.StateInfo, new RecordItemIndex(26));
            repo.Express1SetupWizard.MainPanel.State.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the State
            Report.Log(ReportLevel.Info, "Mouse", "Click the State\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.State' at Center.", repo.Express1SetupWizard.MainPanel.StateInfo, new RecordItemIndex(27));
            repo.Express1SetupWizard.MainPanel.State.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the State
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the State\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}MO' with focus on 'Express1SetupWizard.MainPanel.State'.", repo.Express1SetupWizard.MainPanel.StateInfo, new RecordItemIndex(28));
            repo.Express1SetupWizard.MainPanel.State.PressKeys("{LControlKey down}{Akey}{LControlKey up}MO");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.PostalCodeExpress1' at Center.", repo.Express1SetupWizard.MainPanel.PostalCodeExpress1Info, new RecordItemIndex(29));
            repo.Express1SetupWizard.MainPanel.PostalCodeExpress1.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Postal Code
            Report.Log(ReportLevel.Info, "Mouse", "Click the Postal Code\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.PostalCodeExpress1' at Center.", repo.Express1SetupWizard.MainPanel.PostalCodeExpress1Info, new RecordItemIndex(30));
            repo.Express1SetupWizard.MainPanel.PostalCodeExpress1.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Postal Code
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Postal Code\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}63102' with focus on 'Express1SetupWizard.MainPanel.PostalCodeExpress1'.", repo.Express1SetupWizard.MainPanel.PostalCodeExpress1Info, new RecordItemIndex(31));
            repo.Express1SetupWizard.MainPanel.PostalCodeExpress1.PressKeys("{LControlKey down}{Akey}{LControlKey up}63102");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Email' at Center.", repo.Express1SetupWizard.MainPanel.EmailInfo, new RecordItemIndex(32));
            repo.Express1SetupWizard.MainPanel.Email.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Email
            Report.Log(ReportLevel.Info, "Mouse", "Click the Email\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Email' at Center.", repo.Express1SetupWizard.MainPanel.EmailInfo, new RecordItemIndex(33));
            repo.Express1SetupWizard.MainPanel.Email.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Email
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Email\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}sales@interapptive.com' with focus on 'Express1SetupWizard.MainPanel.Email'.", repo.Express1SetupWizard.MainPanel.EmailInfo, new RecordItemIndex(34));
            repo.Express1SetupWizard.MainPanel.Email.PressKeys("{LControlKey down}{Akey}{LControlKey up}sales@interapptive.com");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Phone' at Center.", repo.Express1SetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(35));
            repo.Express1SetupWizard.MainPanel.Phone.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Phone
            Report.Log(ReportLevel.Info, "Mouse", "Click the Phone\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Phone' at Center.", repo.Express1SetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(36));
            repo.Express1SetupWizard.MainPanel.Phone.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Phone
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Phone\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}800-952-7784' with focus on 'Express1SetupWizard.MainPanel.Phone'.", repo.Express1SetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(37));
            repo.Express1SetupWizard.MainPanel.Phone.PressKeys("{LControlKey down}{Akey}{LControlKey up}800-952-7784");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Fax' at Center.", repo.Express1SetupWizard.MainPanel.FaxInfo, new RecordItemIndex(38));
            repo.Express1SetupWizard.MainPanel.Fax.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Fax
            Report.Log(ReportLevel.Info, "Mouse", "Click the Fax\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Fax' at Center.", repo.Express1SetupWizard.MainPanel.FaxInfo, new RecordItemIndex(39));
            repo.Express1SetupWizard.MainPanel.Fax.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Fax
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Fax\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}800-952-7784' with focus on 'Express1SetupWizard.MainPanel.Fax'.", repo.Express1SetupWizard.MainPanel.FaxInfo, new RecordItemIndex(40));
            repo.Express1SetupWizard.MainPanel.Fax.PressKeys("{LControlKey down}{Akey}{LControlKey up}800-952-7784");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next1' at Center.", repo.Express1SetupWizard.Next1Info, new RecordItemIndex(41));
            repo.Express1SetupWizard.Next1.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Account Registration Enter the Address for your Express1 for Endicia Account page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Account Registration Enter the Address for your Express1 for Endicia Account page\r\nMouse Left Click item 'Express1SetupWizard.Next1' at Center.", repo.Express1SetupWizard.Next1Info, new RecordItemIndex(42));
            repo.Express1SetupWizard.Next1.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Express1AccountNumber' at Center.", repo.Express1SetupWizard.MainPanel.Express1AccountNumberInfo, new RecordItemIndex(43));
            repo.Express1SetupWizard.MainPanel.Express1AccountNumber.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Express1 account number field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Express1 account number field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Express1AccountNumber' at Center.", repo.Express1SetupWizard.MainPanel.Express1AccountNumberInfo, new RecordItemIndex(44));
            repo.Express1SetupWizard.MainPanel.Express1AccountNumber.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Express1 account number
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Express1 account number\r\nKey sequence 'af6438cd-634d-4ad3-bd80-2076933be08b' with focus on 'Express1SetupWizard.MainPanel.Express1AccountNumber'.", repo.Express1SetupWizard.MainPanel.Express1AccountNumberInfo, new RecordItemIndex(45));
            repo.Express1SetupWizard.MainPanel.Express1AccountNumber.PressKeys("af6438cd-634d-4ad3-bd80-2076933be08b");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.Password' at Center.", repo.Express1SetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(46));
            repo.Express1SetupWizard.MainPanel.Password.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Password field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Password field\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.Password' at Center.", repo.Express1SetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(47));
            repo.Express1SetupWizard.MainPanel.Password.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Password
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Password\r\nKey sequence 'be7be995-a927-4730-8159-b0dbb4ba7653' with focus on 'Express1SetupWizard.MainPanel.Password'.", repo.Express1SetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(48));
            repo.Express1SetupWizard.MainPanel.Password.PressKeys("be7be995-a927-4730-8159-b0dbb4ba7653");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(49));
            repo.Express1SetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Express1 Account Enter your Existing Account Information page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Express1 Account Enter your Existing Account Information page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(50));
            repo.Express1SetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(51));
            repo.Express1SetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click next on the Express1 Settings Configure Express1 for Endicia settings page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Express1 Settings Configure Express1 for Endicia settings page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(52));
            repo.Express1SetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(53));
            repo.Express1SetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click next on the Origin Address page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Origin Address page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(54));
            repo.Express1SetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(55));
            repo.Express1SetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click next on the Shipping Defaults page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Shipping Defaults page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(56));
            repo.Express1SetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.MainPanel.PrintActionBox' at Center.", repo.Express1SetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(57));
            repo.Express1SetupWizard.MainPanel.PrintActionBox.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Check the Automatically print labels after processing checkbox
            Report.Log(ReportLevel.Info, "Invoke action", "Check the Automatically print labels after processing checkbox\r\nInvoking Check() on item 'Express1SetupWizard.MainPanel.PrintActionBox'.", repo.Express1SetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(58));
            repo.Express1SetupWizard.MainPanel.PrintActionBox.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(59));
            repo.Express1SetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click next on the Printing Setup page
            Report.Log(ReportLevel.Info, "Mouse", "Click next on the Printing Setup page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(60));
            repo.Express1SetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(61));
            repo.Express1SetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the next button on the Processing Setup page
            Report.Log(ReportLevel.Info, "Mouse", "Click the next button on the Processing Setup page\r\nMouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(62));
            repo.Express1SetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'Express1SetupWizard.Finish' at Center.", repo.Express1SetupWizard.FinishInfo, new RecordItemIndex(63));
            repo.Express1SetupWizard.Finish.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Finish on the Express1 for Endicia Account Registration Complete page
            Report.Log(ReportLevel.Info, "Mouse", "Click Finish on the Express1 for Endicia Account Registration Complete page\r\nMouse Left Click item 'Express1SetupWizard.Finish' at Center.", repo.Express1SetupWizard.FinishInfo, new RecordItemIndex(64));
            repo.Express1SetupWizard.Finish.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(65));
            repo.ShippingSettingsDlg.Close.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Close on the Shipping Setting window
            Report.Log(ReportLevel.Info, "Mouse", "Click Close on the Shipping Setting window\r\nMouse Left Click item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(66));
            repo.ShippingSettingsDlg.Close.Click(300);
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
