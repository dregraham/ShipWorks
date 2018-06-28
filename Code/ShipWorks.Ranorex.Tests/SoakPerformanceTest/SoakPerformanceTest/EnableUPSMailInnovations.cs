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
    ///The EnableUPSMailInnovations recording.
    /// </summary>
    [TestModule("821e5d87-02ff-4866-8889-69102ea19058", ModuleType.Recording, 1)]
    public partial class EnableUPSMailInnovations : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static EnableUPSMailInnovations instance = new EnableUPSMailInnovations();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public EnableUPSMailInnovations()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static EnableUPSMailInnovations Instance
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
            Mouse.DefaultMoveTime = 0;
            Keyboard.DefaultKeyPressTime = 20;
            Delay.SpeedFactor = 0.0;

            Init();

            // Click: Manage ribbon buttons
            Report.Log(ReportLevel.Info, "Mouse", "Click: Manage ribbon buttons\r\nMouse Left Click item 'MainForm.Manage' at Center.", repo.MainForm.ManageInfo, new RecordItemIndex(0));
            repo.MainForm.Manage.Click();
            
            // Click: Shipping Settings button
            Report.Log(ReportLevel.Info, "Mouse", "Click: Shipping Settings button\r\nMouse Left Click item 'MainForm.Shipping' at Center.", repo.MainForm.ShippingInfo, new RecordItemIndex(1));
            repo.MainForm.Shipping.Click();
            
            // Click: UPS (Provider)
            Report.Log(ReportLevel.Info, "Mouse", "Click: UPS (Provider)\r\nMouse Left Click item 'ShippingSettingsDlg.ListItemUPS' at Center.", repo.ShippingSettingsDlg.ListItemUPSInfo, new RecordItemIndex(2));
            repo.ShippingSettingsDlg.ListItemUPS.Click();
            
            // Give Focus
            Report.Log(ReportLevel.Info, "Invoke Action", "Give Focus\r\nInvoking Focus() on item 'ShippingSettingsDlg.TabPageSettings.MailInnovations'.", repo.ShippingSettingsDlg.TabPageSettings.MailInnovationsInfo, new RecordItemIndex(3));
            repo.ShippingSettingsDlg.TabPageSettings.MailInnovations.Focus();
            
            // Set Checked: UPS Mail Innovations checkbox
            Report.Log(ReportLevel.Info, "Set Value", "Set Checked: UPS Mail Innovations checkbox\r\nSetting attribute Checked to 'true' on item 'ShippingSettingsDlg.TabPageSettings.MailInnovations'.", repo.ShippingSettingsDlg.TabPageSettings.MailInnovationsInfo, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.TabPageSettings.MailInnovations.Element.SetAttributeValue("Checked", "true");
            
            // Delay 500ms
            Report.Log(ReportLevel.Info, "Delay", "Delay 500ms\r\nWaiting for 500ms.", new RecordItemIndex(5));
            Delay.Duration(500, false);
            
            // Click: Close Shipping Settings UI
            Report.Log(ReportLevel.Info, "Mouse", "Click: Close Shipping Settings UI\r\nMouse Left Click item 'ShippingSettingsDlg.Close1' at Center.", repo.ShippingSettingsDlg.Close1Info, new RecordItemIndex(6));
            repo.ShippingSettingsDlg.Close1.Click();
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
