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
    ///The SetupUPS recording.
    /// </summary>
    [TestModule("91d6fb70-cdca-4014-aecc-badff845481a", ModuleType.Recording, 1)]
    public partial class SetupUPS : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static SetupUPS instance = new SetupUPS();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupUPS()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupUPS Instance
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
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.CheckBoxUPS' at Center.", repo.ShippingSettingsDlg.CheckBoxUPSInfo, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.CheckBoxUPS.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Check the UPS box on the Shipping Setting General page, Providers tab
            Report.Log(ReportLevel.Info, "Invoke action", "Check the UPS box on the Shipping Setting General page, Providers tab\r\nInvoking Check() on item 'ShippingSettingsDlg.CheckBoxUPS'.", repo.ShippingSettingsDlg.CheckBoxUPSInfo, new RecordItemIndex(5));
            repo.ShippingSettingsDlg.CheckBoxUPS.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.ListItemUPS' at Center.", repo.ShippingSettingsDlg.ListItemUPSInfo, new RecordItemIndex(6));
            repo.ShippingSettingsDlg.ListItemUPS.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the UPS button on the Shipping Setting sidebar
            Report.Log(ReportLevel.Info, "Mouse", "Click the UPS button on the Shipping Setting sidebar\r\nMouse Left Click item 'ShippingSettingsDlg.ListItemUPS' at Center.", repo.ShippingSettingsDlg.ListItemUPSInfo, new RecordItemIndex(7));
            repo.ShippingSettingsDlg.ListItemUPS.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Setup' at Center.", repo.ShippingSettingsDlg.SetupInfo, new RecordItemIndex(8));
            repo.ShippingSettingsDlg.Setup.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Setup button on the Shipping Settings UPS page
            Report.Log(ReportLevel.Info, "Mouse", "Click the Setup button on the Shipping Settings UPS page\r\nMouse Left Click item 'ShippingSettingsDlg.Setup' at Center.", repo.ShippingSettingsDlg.SetupInfo, new RecordItemIndex(9));
            repo.ShippingSettingsDlg.Setup.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.ExistingAccount' at Center.", repo.UpsSetupWizard.MainPanel.ExistingAccountInfo, new RecordItemIndex(10));
            repo.UpsSetupWizard.MainPanel.ExistingAccount.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Select the Use an existing UPS account radio button
            Report.Log(ReportLevel.Info, "Mouse", "Select the Use an existing UPS account radio button\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.ExistingAccount' at Center.", repo.UpsSetupWizard.MainPanel.ExistingAccountInfo, new RecordItemIndex(11));
            repo.UpsSetupWizard.MainPanel.ExistingAccount.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber' at Center.", repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumberInfo, new RecordItemIndex(12));
            repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Enter Your UPS account number field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Enter Your UPS account number field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber' at Center.", repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumberInfo, new RecordItemIndex(13));
            repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the UPS account number
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the UPS account number\r\nKey sequence 'TT9723' with focus on 'UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber'.", repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumberInfo, new RecordItemIndex(14));
            repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber.PressKeys("TT9723");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(15));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Account Registration - Setup ShipWorks to work with your UPS account page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Account Registration - Setup ShipWorks to work with your UPS account page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(16));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.RadioAcceptAgreement' at Center.", repo.UpsSetupWizard.MainPanel.RadioAcceptAgreementInfo, new RecordItemIndex(17));
            repo.UpsSetupWizard.MainPanel.RadioAcceptAgreement.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Select the Yes, I accept the agreement radio button
            Report.Log(ReportLevel.Info, "Mouse", "Select the Yes, I accept the agreement radio button\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.RadioAcceptAgreement' at Center.", repo.UpsSetupWizard.MainPanel.RadioAcceptAgreementInfo, new RecordItemIndex(18));
            repo.UpsSetupWizard.MainPanel.RadioAcceptAgreement.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(19));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Next button on the Account Registration - Please Read the following important information before continuing page
            Report.Log(ReportLevel.Info, "Mouse", "Click the Next button on the Account Registration - Please Read the following important information before continuing page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(20));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.FullName' at Center.", repo.UpsSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(21));
            repo.UpsSetupWizard.MainPanel.FullName.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Full Name field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Full Name field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.FullName' at Center.", repo.UpsSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(22));
            repo.UpsSetupWizard.MainPanel.FullName.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Full Name
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Full Name\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Interapptive ShipWorks' with focus on 'UpsSetupWizard.MainPanel.FullName'.", repo.UpsSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(23));
            repo.UpsSetupWizard.MainPanel.FullName.PressKeys("{LControlKey down}{Akey}{LControlKey up}Interapptive ShipWorks");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.CompanyName' at Center.", repo.UpsSetupWizard.MainPanel.CompanyNameInfo, new RecordItemIndex(24));
            repo.UpsSetupWizard.MainPanel.CompanyName.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Company field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Company field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.CompanyName' at Center.", repo.UpsSetupWizard.MainPanel.CompanyNameInfo, new RecordItemIndex(25));
            repo.UpsSetupWizard.MainPanel.CompanyName.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Company
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Company\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Interapptive Inc.' with focus on 'UpsSetupWizard.MainPanel.CompanyName'.", repo.UpsSetupWizard.MainPanel.CompanyNameInfo, new RecordItemIndex(26));
            repo.UpsSetupWizard.MainPanel.CompanyName.PressKeys("{LControlKey down}{Akey}{LControlKey up}Interapptive Inc.");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.Street' at Center.", repo.UpsSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(27));
            repo.UpsSetupWizard.MainPanel.Street.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Street field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Street field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.Street' at Center.", repo.UpsSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(28));
            repo.UpsSetupWizard.MainPanel.Street.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Street
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Street\r\nKey sequence '{End}{LShiftKey 6}{LShiftKey down}{Home}{LShiftKey up}1 S Memorial Dr., Ste 2000' with focus on 'UpsSetupWizard.MainPanel.Street'.", repo.UpsSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(29));
            repo.UpsSetupWizard.MainPanel.Street.PressKeys("{End}{LShiftKey 6}{LShiftKey down}{Home}{LShiftKey up}1 S Memorial Dr., Ste 2000");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.City' at Center.", repo.UpsSetupWizard.MainPanel.CityInfo, new RecordItemIndex(30));
            repo.UpsSetupWizard.MainPanel.City.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the City field
            Report.Log(ReportLevel.Info, "Mouse", "Click the City field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.City' at Center.", repo.UpsSetupWizard.MainPanel.CityInfo, new RecordItemIndex(31));
            repo.UpsSetupWizard.MainPanel.City.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the City
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the City\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Saint Louis' with focus on 'UpsSetupWizard.MainPanel.City'.", repo.UpsSetupWizard.MainPanel.CityInfo, new RecordItemIndex(32));
            repo.UpsSetupWizard.MainPanel.City.PressKeys("{LControlKey down}{Akey}{LControlKey up}Saint Louis");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.State' at Center.", repo.UpsSetupWizard.MainPanel.StateInfo, new RecordItemIndex(33));
            repo.UpsSetupWizard.MainPanel.State.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the State field
            Report.Log(ReportLevel.Info, "Mouse", "Click the State field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.State' at Center.", repo.UpsSetupWizard.MainPanel.StateInfo, new RecordItemIndex(34));
            repo.UpsSetupWizard.MainPanel.State.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the State
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the State\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}Missouri' with focus on 'UpsSetupWizard.MainPanel.State'.", repo.UpsSetupWizard.MainPanel.StateInfo, new RecordItemIndex(35));
            repo.UpsSetupWizard.MainPanel.State.PressKeys("{LControlKey down}{Akey}{LControlKey up}Missouri");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.PostalCode' at Center.", repo.UpsSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(36));
            repo.UpsSetupWizard.MainPanel.PostalCode.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Postal Code field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Postal Code field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.PostalCode' at Center.", repo.UpsSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(37));
            repo.UpsSetupWizard.MainPanel.PostalCode.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Postal Code
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Postal Code\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}63102' with focus on 'UpsSetupWizard.MainPanel.PostalCode'.", repo.UpsSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(38));
            repo.UpsSetupWizard.MainPanel.PostalCode.PressKeys("{LControlKey down}{Akey}{LControlKey up}63102");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.Email' at Center.", repo.UpsSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(39));
            repo.UpsSetupWizard.MainPanel.Email.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Email field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Email field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.Email' at Center.", repo.UpsSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(40));
            repo.UpsSetupWizard.MainPanel.Email.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Email
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Email\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}sales@interapptive.com' with focus on 'UpsSetupWizard.MainPanel.Email'.", repo.UpsSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(41));
            repo.UpsSetupWizard.MainPanel.Email.PressKeys("{LControlKey down}{Akey}{LControlKey up}sales@interapptive.com");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.Phone' at Center.", repo.UpsSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(42));
            repo.UpsSetupWizard.MainPanel.Phone.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Phone field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Phone field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.Phone' at Center.", repo.UpsSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(43));
            repo.UpsSetupWizard.MainPanel.Phone.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Phone
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Phone\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}1-800-952-7784' with focus on 'UpsSetupWizard.MainPanel.Phone'.", repo.UpsSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(44));
            repo.UpsSetupWizard.MainPanel.Phone.PressKeys("{LControlKey down}{Akey}{LControlKey up}1-800-952-7784");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.Website' at Center.", repo.UpsSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(45));
            repo.UpsSetupWizard.MainPanel.Website.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click the Website field
            Report.Log(ReportLevel.Info, "Mouse", "Click the Website field\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.Website' at Center.", repo.UpsSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(46));
            repo.UpsSetupWizard.MainPanel.Website.Click(300);
            Delay.Milliseconds(200);
            
            // Enter the Website
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Website\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}interapptive.com' with focus on 'UpsSetupWizard.MainPanel.Website'.", repo.UpsSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(47));
            repo.UpsSetupWizard.MainPanel.Website.PressKeys("{LControlKey down}{Akey}{LControlKey up}interapptive.com");
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(48));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Account Registration Enter your UPS account information page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Account Registration Enter your UPS account information page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(49));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(50));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Account Registration Configure how ShipWorks displays shipping rates page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Account Registration Configure how ShipWorks displays shipping rates page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(51));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(52));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Account Registration Configure UPS setting page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Account Registration Configure UPS setting page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(53));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(54));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Account Registration ShipWorks exclusive promotion from UPS page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Account Registration ShipWorks exclusive promotion from UPS page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(55));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(56));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Origin Address Enter the origin addresses for your shipments page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Origin Address Enter the origin addresses for your shipments page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(57));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(58));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Shipping Defaults Configure defaults settings for shipments page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Shipping Defaults Configure defaults settings for shipments page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(59));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.MainPanel.PrintActionBox' at Center.", repo.UpsSetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(60));
            repo.UpsSetupWizard.MainPanel.PrintActionBox.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Select the Automatically print labels after processing checkbox
            Report.Log(ReportLevel.Info, "Invoke action", "Select the Automatically print labels after processing checkbox\r\nInvoking Check() on item 'UpsSetupWizard.MainPanel.PrintActionBox'.", repo.UpsSetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(61));
            repo.UpsSetupWizard.MainPanel.PrintActionBox.Check();
            Delay.Milliseconds(0);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(62));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Printing Setup Configure what to print when printing shipments page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Printing Setup Configure what to print when printing shipments page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(63));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'TemplatePrinterSelectionDlg.UseDefault1' at Center.", repo.TemplatePrinterSelectionDlg.UseDefault1Info, new RecordItemIndex(64));
            repo.TemplatePrinterSelectionDlg.UseDefault1.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click: Use Default - Labels\Thermal
            Report.Log(ReportLevel.Info, "Mouse", "Click: Use Default - Labels\\Thermal\r\nMouse Left Click item 'TemplatePrinterSelectionDlg.UseDefault1' at Center.", repo.TemplatePrinterSelectionDlg.UseDefault1Info, new RecordItemIndex(65));
            repo.TemplatePrinterSelectionDlg.UseDefault1.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'TemplatePrinterSelectionDlg.UseDefault2' at Center.", repo.TemplatePrinterSelectionDlg.UseDefault2Info, new RecordItemIndex(66));
            repo.TemplatePrinterSelectionDlg.UseDefault2.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click: Use Default - Labels\Standard
            Report.Log(ReportLevel.Info, "Mouse", "Click: Use Default - Labels\\Standard\r\nMouse Left Click item 'TemplatePrinterSelectionDlg.UseDefault2' at Center.", repo.TemplatePrinterSelectionDlg.UseDefault2Info, new RecordItemIndex(67));
            repo.TemplatePrinterSelectionDlg.UseDefault2.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'TemplatePrinterSelectionDlg.UseDefault' at Center.", repo.TemplatePrinterSelectionDlg.UseDefaultInfo, new RecordItemIndex(68));
            repo.TemplatePrinterSelectionDlg.UseDefault.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click: Use Default - Labels\Commercial
            Report.Log(ReportLevel.Info, "Mouse", "Click: Use Default - Labels\\Commercial\r\nMouse Left Click item 'TemplatePrinterSelectionDlg.UseDefault' at Center.", repo.TemplatePrinterSelectionDlg.UseDefaultInfo, new RecordItemIndex(69));
            repo.TemplatePrinterSelectionDlg.UseDefault.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'TemplatePrinterSelectionDlg.ButtonOk' at Center.", repo.TemplatePrinterSelectionDlg.ButtonOkInfo, new RecordItemIndex(70));
            repo.TemplatePrinterSelectionDlg.ButtonOk.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click OK on the Printer Selection popup
            Report.Log(ReportLevel.Info, "Mouse", "Click OK on the Printer Selection popup\r\nMouse Left Click item 'TemplatePrinterSelectionDlg.ButtonOk' at Center.", repo.TemplatePrinterSelectionDlg.ButtonOkInfo, new RecordItemIndex(71));
            repo.TemplatePrinterSelectionDlg.ButtonOk.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(72));
            repo.UpsSetupWizard.Next.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Next on the Processing Setup Configure automated processing tasks page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Processing Setup Configure automated processing tasks page\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(73));
            repo.UpsSetupWizard.Next.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'UpsSetupWizard.Finish' at Center.", repo.UpsSetupWizard.FinishInfo, new RecordItemIndex(74));
            repo.UpsSetupWizard.Finish.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Finish on the Account Registration You are now ready to use UPS with ShipWorks page
            Report.Log(ReportLevel.Info, "Mouse", "Click Finish on the Account Registration You are now ready to use UPS with ShipWorks page\r\nMouse Left Click item 'UpsSetupWizard.Finish' at Center.", repo.UpsSetupWizard.FinishInfo, new RecordItemIndex(75));
            repo.UpsSetupWizard.Finish.Click(300);
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(76));
            repo.ShippingSettingsDlg.Close.MoveTo(300);
            Delay.Milliseconds(200);
            
            // Click Close on the Shipping Settings window
            Report.Log(ReportLevel.Info, "Mouse", "Click Close on the Shipping Settings window\r\nMouse Left Click item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(77));
            repo.ShippingSettingsDlg.Close.Click(300);
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
