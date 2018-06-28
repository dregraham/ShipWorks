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
    ///The SetupUPSWorldship recording.
    /// </summary>
    [TestModule("235d4d99-5126-4905-8279-b87daecc98b3", ModuleType.Recording, 1)]
    public partial class SetupUPSWorldship : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static SetupUPSWorldship instance = new SetupUPSWorldship();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupUPSWorldship()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupUPSWorldship Instance
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
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Checked='True') on item 'ShippingSettingsDlg.UPSWorldShip'.", repo.ShippingSettingsDlg.UPSWorldShipInfo, new RecordItemIndex(2));
            Validate.Attribute(repo.ShippingSettingsDlg.UPSWorldShipInfo, "Checked", "True");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingSettingsDlg.UPSWorldShip1' at Center.", repo.ShippingSettingsDlg.UPSWorldShip1Info, new RecordItemIndex(3));
            repo.ShippingSettingsDlg.UPSWorldShip1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingSettingsDlg.Setup1' at Center.", repo.ShippingSettingsDlg.Setup1Info, new RecordItemIndex(4));
            repo.ShippingSettingsDlg.Setup1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber1' at Center.", repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber1Info, new RecordItemIndex(5));
            repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'YA9539' with focus on 'UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber1'.", repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber1Info, new RecordItemIndex(6));
            repo.UpsSetupWizard.MainPanel.EnterYourUPSAccountNumber1.PressKeys("YA9539");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.MainPanel.WorldShipAgree1' at Center.", repo.UpsSetupWizard.MainPanel.WorldShipAgree1Info, new RecordItemIndex(7));
            repo.UpsSetupWizard.MainPanel.WorldShipAgree1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.MainPanel.WorldShipAgree2' at Center.", repo.UpsSetupWizard.MainPanel.WorldShipAgree2Info, new RecordItemIndex(8));
            repo.UpsSetupWizard.MainPanel.WorldShipAgree2.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.Next1' at Center.", repo.UpsSetupWizard.Next1Info, new RecordItemIndex(9));
            repo.UpsSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.Next1' at Center.", repo.UpsSetupWizard.Next1Info, new RecordItemIndex(10));
            repo.UpsSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.MainPanel.LaunchWorldShip' at Center.", repo.UpsSetupWizard.MainPanel.LaunchWorldShipInfo, new RecordItemIndex(11));
            repo.UpsSetupWizard.MainPanel.LaunchWorldShip.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.Next1' at Center.", repo.UpsSetupWizard.Next1Info, new RecordItemIndex(12));
            repo.UpsSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.Next1' at Center.", repo.UpsSetupWizard.Next1Info, new RecordItemIndex(13));
            repo.UpsSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.Next1' at Center.", repo.UpsSetupWizard.Next1Info, new RecordItemIndex(14));
            repo.UpsSetupWizard.Next1.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.Next' at Center.", repo.UpsSetupWizard.NextInfo, new RecordItemIndex(15));
            repo.UpsSetupWizard.Next.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UpsSetupWizard.Finish' at Center.", repo.UpsSetupWizard.FinishInfo, new RecordItemIndex(16));
            repo.UpsSetupWizard.Finish.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingSettingsDlg.Close' at Center.", repo.ShippingSettingsDlg.CloseInfo, new RecordItemIndex(17));
            repo.ShippingSettingsDlg.Close.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
