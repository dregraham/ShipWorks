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
    ///The SetupUSPSNoPostage recording.
    /// </summary>
    [TestModule("45b82fe3-d640-4772-94cc-4fcd865057fc", ModuleType.Recording, 1)]
    public partial class SetupUSPSNoPostage : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTest.SmokeTestRepository repository.
        /// </summary>
        public static SmokeTest.SmokeTestRepository repo = SmokeTest.SmokeTestRepository.Instance;

        static SetupUSPSNoPostage instance = new SetupUSPSNoPostage();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupUSPSNoPostage()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupUSPSNoPostage Instance
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

            // Click the Shipping Settings button
            Report.Log(ReportLevel.Info, "Mouse", "Click the Shipping Settings button\r\nMouse Left Click item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(0));
            repo.MainForm.Shipping.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(1));
            repo.MainForm.Manage.MoveTo();
            
            // Click the Manage Ribbon
            Report.Log(ReportLevel.Info, "Mouse", "Click the Manage Ribbon\r\nMouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(2));
            repo.MainForm.Manage.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(3));
            repo.MainForm.Shipping.MoveTo();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.USPSWOPostage' at Center.", repo.ShippingSettingsDlg.USPSWOPostageInfo, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.USPSWOPostage.MoveTo();
            
            // Check the USPS (w/o Postage) checkbox on the Shipping Settings Window
            Report.Log(ReportLevel.Info, "Invoke Action", "Check the USPS (w/o Postage) checkbox on the Shipping Settings Window\r\nInvoking Check() on item 'ShippingSettingsDlg.USPSWOPostage'.", repo.ShippingSettingsDlg.USPSWOPostageInfo, new RecordItemIndex(5));
            repo.ShippingSettingsDlg.USPSWOPostage.Check();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.USPSWOPostage1' at Center.", repo.ShippingSettingsDlg.USPSWOPostage1Info, new RecordItemIndex(6));
            repo.ShippingSettingsDlg.USPSWOPostage1.MoveTo();
            
            // Click the USPS (w/o Postage) button on the sidebar of the Shipping Settings Window
            Report.Log(ReportLevel.Info, "Mouse", "Click the USPS (w/o Postage) button on the sidebar of the Shipping Settings Window\r\nMouse Left Click item 'ShippingSettingsDlg.USPSWOPostage1' at Center.", repo.ShippingSettingsDlg.USPSWOPostage1Info, new RecordItemIndex(7));
            repo.ShippingSettingsDlg.USPSWOPostage1.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Setup11' at Center.", repo.ShippingSettingsDlg.Setup11Info, new RecordItemIndex(8));
            repo.ShippingSettingsDlg.Setup11.MoveTo();
            
            // Click the Setup button on the USPS (w/o Postage) section of the Shipping Settings window
            Report.Log(ReportLevel.Info, "Mouse", "Click the Setup button on the USPS (w/o Postage) section of the Shipping Settings window\r\nMouse Left Click item 'ShippingSettingsDlg.Setup11' at Center.", repo.ShippingSettingsDlg.Setup11Info, new RecordItemIndex(9));
            repo.ShippingSettingsDlg.Setup11.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(10));
            repo.PostalWebSetupWizard.Next.MoveTo();
            
            // Click Next on the Setup USPS Shipping Page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Setup USPS Shipping Page\r\nMouse Left Click item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(11));
            repo.PostalWebSetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(12));
            repo.PostalWebSetupWizard.Next.MoveTo();
            
            // Click Next on the Origin Address page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Origin Address page\r\nMouse Left Click item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(13));
            repo.PostalWebSetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(14));
            repo.PostalWebSetupWizard.Next.MoveTo();
            
            // Click Next on the Shipment Defaults page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Shipment Defaults page\r\nMouse Left Click item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(15));
            repo.PostalWebSetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'PostalWebSetupWizard.PrintActionBox' at Center.", repo.PostalWebSetupWizard.PrintActionBoxInfo, new RecordItemIndex(16));
            repo.PostalWebSetupWizard.PrintActionBox.MoveTo();
            
            // Uncheck the Automatically print labels after processing checkbox
            Report.Log(ReportLevel.Info, "Invoke Action", "Uncheck the Automatically print labels after processing checkbox\r\nInvoking Uncheck() on item 'PostalWebSetupWizard.PrintActionBox'.", repo.PostalWebSetupWizard.PrintActionBoxInfo, new RecordItemIndex(17));
            repo.PostalWebSetupWizard.PrintActionBox.Uncheck();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(18));
            repo.PostalWebSetupWizard.Next.MoveTo();
            
            // Click Next on the Printing Setup page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Printing Setup page\r\nMouse Left Click item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(19));
            repo.PostalWebSetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(20));
            repo.PostalWebSetupWizard.Next.MoveTo();
            
            // Click Next on the Processing Setup page
            Report.Log(ReportLevel.Info, "Mouse", "Click Next on the Processing Setup page\r\nMouse Left Click item 'PostalWebSetupWizard.Next' at Center.", repo.PostalWebSetupWizard.NextInfo, new RecordItemIndex(21));
            repo.PostalWebSetupWizard.Next.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'PostalWebSetupWizard.Finish' at Center.", repo.PostalWebSetupWizard.FinishInfo, new RecordItemIndex(22));
            repo.PostalWebSetupWizard.Finish.MoveTo();
            
            // Click Finish on the Setup Complete page
            Report.Log(ReportLevel.Info, "Mouse", "Click Finish on the Setup Complete page\r\nMouse Left Click item 'PostalWebSetupWizard.Finish' at Center.", repo.PostalWebSetupWizard.FinishInfo, new RecordItemIndex(23));
            repo.PostalWebSetupWizard.Finish.Click();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(24));
            repo.ShippingSettingsDlg.Close.MoveTo();
            
            // Click Close on the Shipping Settings window
            Report.Log(ReportLevel.Info, "Mouse", "Click Close on the Shipping Settings window\r\nMouse Left Click item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(25));
            repo.ShippingSettingsDlg.Close.Click();
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
