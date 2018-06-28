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

namespace UIResponsiveness
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The SetupUPS recording.
    /// </summary>
    [TestModule("559cab08-94c4-4a06-b319-8a978f08d8cf", ModuleType.Recording, 1)]
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

            // Click: Manage
            Report.Log(ReportLevel.Info, "Mouse", "Click: Manage\r\nMouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(0));
            repo.MainForm.Manage.Click();
            Delay.Milliseconds(200);
            
            // Click: Shipping (Settings)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Shipping (Settings)\r\nMouse Left Click item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(1));
            repo.MainForm.Shipping.Click();
            Delay.Milliseconds(200);
            
            // Check: UPS to enable UPS carrier
            Report.Log(ReportLevel.Info, "Validation", "Check: UPS to enable UPS carrier\r\nValidating AttributeEqual (Checked='True') on item 'ShippingSettingsDlg.CheckBoxUPS'.", repo.ShippingSettingsDlg.CheckBoxUPSInfo, new RecordItemIndex(2));
            Validate.Attribute(repo.ShippingSettingsDlg.CheckBoxUPSInfo, "Checked", "True");
            Delay.Milliseconds(100);
            
            // Click: UPS carrier
            Report.Log(ReportLevel.Info, "Mouse", "Click: UPS carrier\r\nMouse Left Click item 'ShippingSettingsDlg.ListItemUPS' at Center.", repo.ShippingSettingsDlg.ListItemUPSInfo, new RecordItemIndex(3));
            repo.ShippingSettingsDlg.ListItemUPS.Click();
            Delay.Milliseconds(200);
            
            // Click: Setup
            Report.Log(ReportLevel.Info, "Mouse", "Click: Setup\r\nMouse Left Click item 'ShippingSettingsDlg.Setup' at Center.", repo.ShippingSettingsDlg.SetupInfo, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.Setup.Click();
            Delay.Milliseconds(200);
            
            // Click: Use an existing UPS account.
            Report.Log(ReportLevel.Info, "Mouse", "Click: Use an existing UPS account.\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.ExistingAccount' at Center.", repo.UpsSetupWizard.MainPanel.ExistingAccountInfo, new RecordItemIndex(5));
            repo.UpsSetupWizard.MainPanel.ExistingAccount.Click();
            Delay.Milliseconds(200);
            
            // Click: Enter Your UPS account number
            Report.Log(ReportLevel.Info, "Mouse", "Click: Enter Your UPS account number\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber' at Center.", repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumberInfo, new RecordItemIndex(6));
            repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber.Click();
            Delay.Milliseconds(200);
            
            // Input UPS Account number: TT9723
            Report.Log(ReportLevel.Info, "Keyboard", "Input UPS Account number: TT9723\r\nKey sequence 'TT9723' with focus on 'UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber'.", repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumberInfo, new RecordItemIndex(7));
            repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber.PressKeys("TT9723");
            Delay.Milliseconds(0);
            
            // Click: Next (Input UPS Account number page)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Next (Input UPS Account number page)\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(8));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Click: Yes, I accept the agreement.
            Report.Log(ReportLevel.Info, "Mouse", "Click: Yes, I accept the agreement.\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.RadioAcceptAgreement' at Center.", repo.UpsSetupWizard.MainPanel.RadioAcceptAgreementInfo, new RecordItemIndex(9));
            repo.UpsSetupWizard.MainPanel.RadioAcceptAgreement.Click();
            Delay.Milliseconds(200);
            
            // Click: Next (UPS agreement page)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Next (UPS agreement page)\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(10));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Click: Full Name
            Report.Log(ReportLevel.Info, "Mouse", "Click: Full Name\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.FullName' at Center.", repo.UpsSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(11));
            repo.UpsSetupWizard.MainPanel.FullName.Click();
            Delay.Milliseconds(200);
            
            // Input Full Name
            Report.Log(ReportLevel.Info, "Keyboard", "Input Full Name\r\nKey sequence 'Wes Clayton' with focus on 'UpsSetupWizard.MainPanel.FullName'.", repo.UpsSetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(12));
            repo.UpsSetupWizard.MainPanel.FullName.PressKeys("Wes Clayton");
            Delay.Milliseconds(0);
            
            // Click: Company Name
            Report.Log(ReportLevel.Info, "Mouse", "Click: Company Name\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.CompanyName' at Center.", repo.UpsSetupWizard.MainPanel.CompanyNameInfo, new RecordItemIndex(13));
            repo.UpsSetupWizard.MainPanel.CompanyName.Click();
            Delay.Milliseconds(200);
            
            // Input: Company Name
            Report.Log(ReportLevel.Info, "Keyboard", "Input: Company Name\r\nKey sequence 'Interapptive Inc.' with focus on 'UpsSetupWizard.MainPanel.CompanyName'.", repo.UpsSetupWizard.MainPanel.CompanyNameInfo, new RecordItemIndex(14));
            repo.UpsSetupWizard.MainPanel.CompanyName.PressKeys("Interapptive Inc.");
            Delay.Milliseconds(0);
            
            // Click: Street
            Report.Log(ReportLevel.Info, "Mouse", "Click: Street\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.Street' at Center.", repo.UpsSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(15));
            repo.UpsSetupWizard.MainPanel.Street.Click();
            Delay.Milliseconds(200);
            
            // Input Street1 Address
            Report.Log(ReportLevel.Info, "Keyboard", "Input Street1 Address\r\nKey sequence '1 S Memorial Dr., Ste 2000' with focus on 'UpsSetupWizard.MainPanel.Street'.", repo.UpsSetupWizard.MainPanel.StreetInfo, new RecordItemIndex(16));
            repo.UpsSetupWizard.MainPanel.Street.PressKeys("1 S Memorial Dr., Ste 2000");
            Delay.Milliseconds(0);
            
            // Click City
            Report.Log(ReportLevel.Info, "Mouse", "Click City\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.City' at Center.", repo.UpsSetupWizard.MainPanel.CityInfo, new RecordItemIndex(17));
            repo.UpsSetupWizard.MainPanel.City.Click();
            Delay.Milliseconds(200);
            
            // Input City
            Report.Log(ReportLevel.Info, "Keyboard", "Input City\r\nKey sequence 'Saint Louis' with focus on 'UpsSetupWizard.MainPanel.City'.", repo.UpsSetupWizard.MainPanel.CityInfo, new RecordItemIndex(18));
            repo.UpsSetupWizard.MainPanel.City.PressKeys("Saint Louis");
            Delay.Milliseconds(0);
            
            // Click State
            Report.Log(ReportLevel.Info, "Mouse", "Click State\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.State' at Center.", repo.UpsSetupWizard.MainPanel.StateInfo, new RecordItemIndex(19));
            repo.UpsSetupWizard.MainPanel.State.Click();
            Delay.Milliseconds(200);
            
            // Input State
            Report.Log(ReportLevel.Info, "Keyboard", "Input State\r\nKey sequence 'Missouri' with focus on 'UpsSetupWizard.MainPanel.State'.", repo.UpsSetupWizard.MainPanel.StateInfo, new RecordItemIndex(20));
            repo.UpsSetupWizard.MainPanel.State.PressKeys("Missouri");
            Delay.Milliseconds(0);
            
            // Click Postal Code
            Report.Log(ReportLevel.Info, "Mouse", "Click Postal Code\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.PostalCode' at Center.", repo.UpsSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(21));
            repo.UpsSetupWizard.MainPanel.PostalCode.Click();
            Delay.Milliseconds(200);
            
            // Input Postal Code
            Report.Log(ReportLevel.Info, "Keyboard", "Input Postal Code\r\nKey sequence '63102' with focus on 'UpsSetupWizard.MainPanel.PostalCode'.", repo.UpsSetupWizard.MainPanel.PostalCodeInfo, new RecordItemIndex(22));
            repo.UpsSetupWizard.MainPanel.PostalCode.PressKeys("63102");
            Delay.Milliseconds(0);
            
            // Click Email
            Report.Log(ReportLevel.Info, "Mouse", "Click Email\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.Email' at Center.", repo.UpsSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(23));
            repo.UpsSetupWizard.MainPanel.Email.Click();
            Delay.Milliseconds(200);
            
            // Input Email
            Report.Log(ReportLevel.Info, "Keyboard", "Input Email\r\nKey sequence 'sales@interapptive.com' with focus on 'UpsSetupWizard.MainPanel.Email'.", repo.UpsSetupWizard.MainPanel.EmailInfo, new RecordItemIndex(24));
            repo.UpsSetupWizard.MainPanel.Email.PressKeys("sales@interapptive.com");
            Delay.Milliseconds(0);
            
            // Click Phone
            Report.Log(ReportLevel.Info, "Mouse", "Click Phone\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.Phone' at Center.", repo.UpsSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(25));
            repo.UpsSetupWizard.MainPanel.Phone.Click();
            Delay.Milliseconds(200);
            
            // Input Phone
            Report.Log(ReportLevel.Info, "Keyboard", "Input Phone\r\nKey sequence '1-800-952-7784' with focus on 'UpsSetupWizard.MainPanel.Phone'.", repo.UpsSetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(26));
            repo.UpsSetupWizard.MainPanel.Phone.PressKeys("1-800-952-7784");
            Delay.Milliseconds(0);
            
            // Click Website
            Report.Log(ReportLevel.Info, "Mouse", "Click Website\r\nMouse Left Click item 'UpsSetupWizard.MainPanel.Website' at Center.", repo.UpsSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(27));
            repo.UpsSetupWizard.MainPanel.Website.Click();
            Delay.Milliseconds(200);
            
            // Input Website
            Report.Log(ReportLevel.Info, "Keyboard", "Input Website\r\nKey sequence 'interapptive.com' with focus on 'UpsSetupWizard.MainPanel.Website'.", repo.UpsSetupWizard.MainPanel.WebsiteInfo, new RecordItemIndex(28));
            repo.UpsSetupWizard.MainPanel.Website.PressKeys("interapptive.com");
            Delay.Milliseconds(0);
            
            // Click: Next (Input Address information page)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Next (Input Address information page)\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(29));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Click: Next (Rate Type, Daily Pickup page)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Next (Rate Type, Daily Pickup page)\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(30));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Click: Next (Requested label format page)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Next (Requested label format page)\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(31));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Click: Next (Origin address page)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Next (Origin address page)\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(32));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Click: Next (Shipment Defaults and Rules page)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Next (Shipment Defaults and Rules page)\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(33));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Click: Next (Printing setup page)
            //Report.Log(ReportLevel.Info, "Mouse", "Click: Next (Printing setup page)\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(34));
            //repo.UpsSetupWizard.Next.Click();
            //Delay.Milliseconds(200);
            
            // Uncheck: Automatically print labels after processing
            Report.Log(ReportLevel.Info, "Invoke Action", "Uncheck: Automatically print labels after processing\r\nInvoking Uncheck() on item 'UpsSetupWizard.MainPanel.PrintActionBox'.", repo.UpsSetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(35));
            repo.UpsSetupWizard.MainPanel.PrintActionBox.Uncheck();
            Delay.Milliseconds(0);
            
            // Click: Next (Printing setup page)
            Report.Log(ReportLevel.Info, "Mouse", "Click: Next (Printing setup page)\r\nMouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(36));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Click: Use Default - Labels\Thermal
            Report.Log(ReportLevel.Info, "Mouse", "Click: Use Default - Labels\\Thermal\r\nMouse Left Click item 'TemplatePrinterSelectionDlg.UseDefault1' at Center.", repo.TemplatePrinterSelectionDlg.UseDefault1Info, new RecordItemIndex(37));
            repo.TemplatePrinterSelectionDlg.UseDefault1.Click();
            Delay.Milliseconds(200);
            
            // Click: Use Default - Labels\Standard
            Report.Log(ReportLevel.Info, "Mouse", "Click: Use Default - Labels\\Standard\r\nMouse Left Click item 'TemplatePrinterSelectionDlg.UseDefault2' at Center.", repo.TemplatePrinterSelectionDlg.UseDefault2Info, new RecordItemIndex(38));
            repo.TemplatePrinterSelectionDlg.UseDefault2.Click();
            Delay.Milliseconds(200);
            
            // DISABLED
            //Report.Log(ReportLevel.Info, "Mouse", "DISABLED\r\nMouse Left Click item 'TemplatePrinterSelectionDlg.Choose' at Center.", repo.TemplatePrinterSelectionDlg.ChooseInfo, new RecordItemIndex(39));
            //repo.TemplatePrinterSelectionDlg.Choose.Click();
            //Delay.Milliseconds(200);
            
            // DISABLED
            //Report.Log(ReportLevel.Info, "Mouse", "DISABLED\r\nMouse Left Click item 'PrinterSelectionDlg.PrinterDropdown' at Center.", repo.PrinterSelectionDlg.PrinterDropdownInfo, new RecordItemIndex(40));
            //repo.PrinterSelectionDlg.PrinterDropdown.Click();
            //Delay.Milliseconds(200);
            
            // DISABLED
            //Report.Log(ReportLevel.Info, "Mouse", "DISABLED\r\nMouse Left Click item 'List1000.ListItemBergerPcZebraZP450CTP' at Center.", repo.List1000.ListItemBergerPcZebraZP450CTPInfo, new RecordItemIndex(41));
            //repo.List1000.ListItemBergerPcZebraZP450CTP.Click();
            //Delay.Milliseconds(200);
            
            // DISABLED
            //Report.Log(ReportLevel.Info, "Mouse", "DISABLED\r\nMouse Left Click item 'PrinterSelectionDlg.ButtonOk' at Center.", repo.PrinterSelectionDlg.ButtonOkInfo, new RecordItemIndex(42));
            //repo.PrinterSelectionDlg.ButtonOk.Click();
            //Delay.Milliseconds(200);
            
            // DISABLED
            //Report.Log(ReportLevel.Info, "Mouse", "DISABLED\r\nMouse Left Click item 'TemplatePrinterSelectionDlg.UseDefault2' at Center.", repo.TemplatePrinterSelectionDlg.UseDefault2Info, new RecordItemIndex(43));
            //repo.TemplatePrinterSelectionDlg.UseDefault2.Click();
            //Delay.Milliseconds(200);
            
            // DISABLED
            //Report.Log(ReportLevel.Info, "Mouse", "DISABLED\r\nMouse Left Click item 'TemplatePrinterSelectionDlg.Choose1' at Center.", repo.TemplatePrinterSelectionDlg.Choose1Info, new RecordItemIndex(44));
            //repo.TemplatePrinterSelectionDlg.Choose1.Click();
            //Delay.Milliseconds(200);
            
            // DISABLED
            //Report.Log(ReportLevel.Info, "Mouse", "DISABLED\r\nMouse Left Click item 'PrinterSelectionDlg.PrinterDropdown' at Center.", repo.PrinterSelectionDlg.PrinterDropdownInfo, new RecordItemIndex(45));
            //repo.PrinterSelectionDlg.PrinterDropdown.Click();
            //Delay.Milliseconds(200);
            
            // DISABLED
            //Report.Log(ReportLevel.Info, "Mouse", "DISABLED\r\nMouse Left Click item 'List1000.ListItemEpsonWF_40' at Center.", repo.List1000.ListItemEpsonWF_40Info, new RecordItemIndex(46));
            //repo.List1000.ListItemEpsonWF_40.Click();
            //Delay.Milliseconds(200);
            
            // DISABLED
            //Report.Log(ReportLevel.Info, "Mouse", "DISABLED\r\nMouse Left Click item 'PrinterSelectionDlg.ButtonOk' at Center.", repo.PrinterSelectionDlg.ButtonOkInfo, new RecordItemIndex(47));
            //repo.PrinterSelectionDlg.ButtonOk.Click();
            //Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'TemplatePrinterSelectionDlg.UseDefault' at Center.", repo.TemplatePrinterSelectionDlg.UseDefaultInfo, new RecordItemIndex(48));
            repo.TemplatePrinterSelectionDlg.UseDefault.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'TemplatePrinterSelectionDlg.ButtonOk' at Center.", repo.TemplatePrinterSelectionDlg.ButtonOkInfo, new RecordItemIndex(49));
            repo.TemplatePrinterSelectionDlg.ButtonOk.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(50));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.Finish' at Center.", repo.UpsSetupWizard.FinishInfo, new RecordItemIndex(51));
            repo.UpsSetupWizard.Finish.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(52));
            repo.ShippingSettingsDlg.Close.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
