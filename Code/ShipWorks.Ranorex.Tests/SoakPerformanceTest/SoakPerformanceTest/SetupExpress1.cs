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
    ///The SetupExpress1 recording.
    /// </summary>
    [TestModule("90c88945-c670-4e2d-8eb7-1cd590bf516c", ModuleType.Recording, 1)]
    public partial class SetupExpress1 : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

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

            // click manage
            Report.Log(ReportLevel.Info, "Mouse", "click manage\r\nMouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(0));
            repo.MainForm.Manage.Click();
            Delay.Milliseconds(200);
            
            // click: shipping settings
            Report.Log(ReportLevel.Info, "Mouse", "click: shipping settings\r\nMouse Left Click item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(1));
            repo.MainForm.Shipping.Click();
            Delay.Milliseconds(200);
            
            // check: USPS (Express1)
            Report.Log(ReportLevel.Info, "Invoke Action", "check: USPS (Express1)\r\nInvoking Check() on item 'ShippingSettingsDlg.USPSExpress1'.", repo.ShippingSettingsDlg.USPSExpress1Info, new RecordItemIndex(2));
            repo.ShippingSettingsDlg.USPSExpress1.Check();
            Delay.Milliseconds(0);
            
            // click: USPS (Express1)
            Report.Log(ReportLevel.Info, "Mouse", "click: USPS (Express1)\r\nMouse Left Click item 'ShippingSettingsDlg.USPSExpress11' at Center.", repo.ShippingSettingsDlg.USPSExpress11Info, new RecordItemIndex(3));
            repo.ShippingSettingsDlg.USPSExpress11.Click();
            Delay.Milliseconds(200);
            
            // click setup... button
            Report.Log(ReportLevel.Info, "Mouse", "click setup... button\r\nMouse Left Click item 'ShippingSettingsDlg.Setup4' at Center.", repo.ShippingSettingsDlg.Setup4Info, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.Setup4.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.RadioExistingAccount' at Center.", repo.Express1SetupWizard.MainPanel.RadioExistingAccountInfo, new RecordItemIndex(5));
            repo.Express1SetupWizard.MainPanel.RadioExistingAccount.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(6));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(7));
            repo.Express1SetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.FullName' at Center.", repo.Express1SetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(8));
            repo.Express1SetupWizard.MainPanel.FullName.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'Wes Clayton' with focus on 'Express1SetupWizard.MainPanel.FullName'.", repo.Express1SetupWizard.MainPanel.FullNameInfo, new RecordItemIndex(9));
            repo.Express1SetupWizard.MainPanel.FullName.PressKeys("Wes Clayton");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.Company' at Center.", repo.Express1SetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(10));
            repo.Express1SetupWizard.MainPanel.Company.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'Interapptive' with focus on 'Express1SetupWizard.MainPanel.Company'.", repo.Express1SetupWizard.MainPanel.CompanyInfo, new RecordItemIndex(11));
            repo.Express1SetupWizard.MainPanel.Company.PressKeys("Interapptive");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.Street' at Center.", repo.Express1SetupWizard.MainPanel.StreetInfo, new RecordItemIndex(12));
            repo.Express1SetupWizard.MainPanel.Street.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '1 Memorial Dr #2000' with focus on 'Express1SetupWizard.MainPanel.Street'.", repo.Express1SetupWizard.MainPanel.StreetInfo, new RecordItemIndex(13));
            repo.Express1SetupWizard.MainPanel.Street.PressKeys("1 Memorial Dr #2000");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.City' at Center.", repo.Express1SetupWizard.MainPanel.CityInfo, new RecordItemIndex(14));
            repo.Express1SetupWizard.MainPanel.City.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'Saint Louis' with focus on 'Express1SetupWizard.MainPanel.City'.", repo.Express1SetupWizard.MainPanel.CityInfo, new RecordItemIndex(15));
            repo.Express1SetupWizard.MainPanel.City.PressKeys("Saint Louis");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.StateExpress1' at Center.", repo.Express1SetupWizard.MainPanel.StateExpress1Info, new RecordItemIndex(16));
            repo.Express1SetupWizard.MainPanel.StateExpress1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'MO' with focus on 'Express1SetupWizard.MainPanel.StateExpress1'.", repo.Express1SetupWizard.MainPanel.StateExpress1Info, new RecordItemIndex(17));
            repo.Express1SetupWizard.MainPanel.StateExpress1.PressKeys("MO");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.PostalCodeExpress1' at Center.", repo.Express1SetupWizard.MainPanel.PostalCodeExpress1Info, new RecordItemIndex(18));
            repo.Express1SetupWizard.MainPanel.PostalCodeExpress1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '63102' with focus on 'Express1SetupWizard.MainPanel.PostalCodeExpress1'.", repo.Express1SetupWizard.MainPanel.PostalCodeExpress1Info, new RecordItemIndex(19));
            repo.Express1SetupWizard.MainPanel.PostalCodeExpress1.PressKeys("63102");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.Email' at Center.", repo.Express1SetupWizard.MainPanel.EmailInfo, new RecordItemIndex(20));
            repo.Express1SetupWizard.MainPanel.Email.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'sales@interapptive.com' with focus on 'Express1SetupWizard.MainPanel.Email'.", repo.Express1SetupWizard.MainPanel.EmailInfo, new RecordItemIndex(21));
            repo.Express1SetupWizard.MainPanel.Email.PressKeys("sales@interapptive.com");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.Phone' at Center.", repo.Express1SetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(22));
            repo.Express1SetupWizard.MainPanel.Phone.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '800-952-7784' with focus on 'Express1SetupWizard.MainPanel.Phone'.", repo.Express1SetupWizard.MainPanel.PhoneInfo, new RecordItemIndex(23));
            repo.Express1SetupWizard.MainPanel.Phone.PressKeys("800-952-7784");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.Fax' at Center.", repo.Express1SetupWizard.MainPanel.FaxInfo, new RecordItemIndex(24));
            repo.Express1SetupWizard.MainPanel.Fax.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '800-952-7784' with focus on 'Express1SetupWizard.MainPanel.Fax'.", repo.Express1SetupWizard.MainPanel.FaxInfo, new RecordItemIndex(25));
            repo.Express1SetupWizard.MainPanel.Fax.PressKeys("800-952-7784");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(26));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(27));
            repo.Express1SetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.Express1AccountNumber' at Center.", repo.Express1SetupWizard.MainPanel.Express1AccountNumberInfo, new RecordItemIndex(28));
            repo.Express1SetupWizard.MainPanel.Express1AccountNumber.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '759cc789-25ab-4701-b791-b0c7d4b47701' with focus on 'Express1SetupWizard.MainPanel.Express1AccountNumber'.", repo.Express1SetupWizard.MainPanel.Express1AccountNumberInfo, new RecordItemIndex(29));
            repo.Express1SetupWizard.MainPanel.Express1AccountNumber.PressKeys("759cc789-25ab-4701-b791-b0c7d4b47701");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.Password' at Center.", repo.Express1SetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(30));
            repo.Express1SetupWizard.MainPanel.Password.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.MainPanel.Password' at Center.", repo.Express1SetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(31));
            repo.Express1SetupWizard.MainPanel.Password.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '75496d77-0b3e-4d18-b272-9399197ccd90' with focus on 'Express1SetupWizard.MainPanel.Password'.", repo.Express1SetupWizard.MainPanel.PasswordInfo, new RecordItemIndex(32));
            repo.Express1SetupWizard.MainPanel.Password.PressKeys("75496d77-0b3e-4d18-b272-9399197ccd90");
            Delay.Milliseconds(0);
            
            //Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(33));
            //repo.Express1SetupWizard.Next.Click();
            //Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Enter}'.", new RecordItemIndex(34));
            Keyboard.Press("{Enter}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 1000ms.", new RecordItemIndex(35));
            Delay.Duration(1000, false);
            
            //Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(36));
            //repo.Express1SetupWizard.Next.Click();
            //Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Enter}'.", new RecordItemIndex(37));
            Keyboard.Press("{Enter}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 1000ms.", new RecordItemIndex(38));
            Delay.Duration(1000, false);
            
            //Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(39));
            //repo.Express1SetupWizard.Next.Click();
            //Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab}{Tab}{Enter}'.", new RecordItemIndex(40));
            Keyboard.Press("{Tab}{Tab}{Enter}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 1000ms.", new RecordItemIndex(41));
            Delay.Duration(1000, false);
            
            //Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(42));
            //repo.Express1SetupWizard.Next.Click();
            //Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Enter}'.", new RecordItemIndex(43));
            Keyboard.Press("{Enter}");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(44));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Invoke Action", "Invoking Uncheck() on item 'Express1SetupWizard.MainPanel.PrintActionBox'.", repo.Express1SetupWizard.MainPanel.PrintActionBoxInfo, new RecordItemIndex(45));
            repo.Express1SetupWizard.MainPanel.PrintActionBox.Uncheck();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(46));
            repo.Express1SetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(47));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.Next' at Center.", repo.Express1SetupWizard.NextInfo, new RecordItemIndex(48));
            repo.Express1SetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            // Click: Buy Postage...
            Report.Log(ReportLevel.Info, "Mouse", "Click: Buy Postage...\r\nMouse Left Click item 'Express1SetupWizard.MainPanel.BuyPostage' at Center.", repo.Express1SetupWizard.MainPanel.BuyPostageInfo, new RecordItemIndex(49));
            repo.Express1SetupWizard.MainPanel.BuyPostage.Click();
            Delay.Milliseconds(200);
            
            // Give Focus: Purchase amount
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus: Purchase amount\r\nInvoking Focus() on item 'UspsPurchasePostageDlg.Postage'.", repo.UspsPurchasePostageDlg.PostageInfo, new RecordItemIndex(50));
            repo.UspsPurchasePostageDlg.Postage.Focus();
            Delay.Milliseconds(0);
            
            // Select All (CTRL+A)
            Report.Log(ReportLevel.Info, "Keyboard", "Select All (CTRL+A)\r\nKey 'Ctrl+A' Press with focus on 'UspsPurchasePostageDlg.Postage'.", repo.UspsPurchasePostageDlg.PostageInfo, new RecordItemIndex(51));
            Keyboard.PrepareFocus(repo.UspsPurchasePostageDlg.Postage);
            Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, Keyboard.DefaultScanCode, Keyboard.DefaultKeyPressTime, 1, true);
            Delay.Milliseconds(0);
            
            // Set Purchase amount: 300
            Report.Log(ReportLevel.Info, "Keyboard", "Set Purchase amount: 300\r\nKey sequence '300' with focus on 'UspsPurchasePostageDlg.Postage'.", repo.UspsPurchasePostageDlg.PostageInfo, new RecordItemIndex(52));
            repo.UspsPurchasePostageDlg.Postage.PressKeys("300");
            Delay.Milliseconds(0);
            
            // Click: Purchase
            Report.Log(ReportLevel.Info, "Mouse", "Click: Purchase\r\nMouse Left Click item 'UspsPurchasePostageDlg.Purchase' at Center.", repo.UspsPurchasePostageDlg.PurchaseInfo, new RecordItemIndex(53));
            repo.UspsPurchasePostageDlg.Purchase.Click();
            Delay.Milliseconds(200);
            
            // Delay: 500ms
            Report.Log(ReportLevel.Info, "Delay", "Delay: 500ms\r\nWaiting for 500ms.", new RecordItemIndex(54));
            Delay.Duration(500, false);
            
            // Click: OK (purchase request has been submitted to Express1)
            Report.Log(ReportLevel.Info, "Mouse", "Click: OK (purchase request has been submitted to Express1)\r\nMouse Left Click item 'ShipWorks.ButtonOK' at Center.", repo.ShipWorks.ButtonOKInfo, new RecordItemIndex(55));
            repo.ShipWorks.ButtonOK.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(56));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'Express1SetupWizard.Finish' at Center.", repo.Express1SetupWizard.FinishInfo, new RecordItemIndex(57));
            repo.Express1SetupWizard.Finish.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Delay", "Waiting for 500ms.", new RecordItemIndex(58));
            Delay.Duration(500, false);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(59));
            repo.ShippingSettingsDlg.Close.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
