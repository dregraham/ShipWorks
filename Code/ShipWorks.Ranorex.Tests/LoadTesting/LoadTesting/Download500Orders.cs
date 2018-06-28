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

namespace LoadTesting
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The Download500Orders recording.
    /// </summary>
    [TestModule("6af9c7ae-3925-4317-8bde-4e9ef9f0dac7", ModuleType.Recording, 1)]
    public partial class Download500Orders : ITestModule
    {
        /// <summary>
        /// Holds an instance of the LoadTestingRepository repository.
        /// </summary>
        public static LoadTestingRepository repo = LoadTestingRepository.Instance;

        static Download500Orders instance = new Download500Orders();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public Download500Orders()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static Download500Orders Instance
        {
            get { return instance; }
        }

#region Variables

#endregion

        /// <summary>
        /// Starts the replay of the static recording <see cref="Instance"/>.
        /// </summary>
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.0")]
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
        [System.CodeDom.Compiler.GeneratedCode("Ranorex", "8.0")]
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 1000;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            try {
                //Report.Log(ReportLevel.Info, "Mouse", "(Optional Action)\r\nMouse Left Move item 'MainForm.Home' at Center.", repo.MainForm.HomeInfo, new RecordItemIndex(0));
                //repo.MainForm.Home.MoveTo();
                //Delay.Milliseconds(0);
            } catch(Exception ex) { Report.Log(ReportLevel.Warn, "Module", "(Optional Action) " + ex.Message, new RecordItemIndex(0)); }
            
            try {
                //Report.Log(ReportLevel.Info, "Mouse", "(Optional Action)\r\nMouse Left Click item 'MainForm.Home' at Center.", repo.MainForm.HomeInfo, new RecordItemIndex(1));
                //repo.MainForm.Home.Click();
                //Delay.Milliseconds(0);
            } catch(Exception ex) { Report.Log(ReportLevel.Warn, "Module", "(Optional Action) " + ex.Message, new RecordItemIndex(1)); }
            
            try {
                //Report.Log(ReportLevel.Info, "Mouse", "(Optional Action)\r\nMouse Left Move item 'MainForm.DownloadAsterisk' at Center.", repo.MainForm.DownloadAsteriskInfo, new RecordItemIndex(2));
                //repo.MainForm.DownloadAsterisk.MoveTo();
                //Delay.Milliseconds(0);
            } catch(Exception ex) { Report.Log(ReportLevel.Warn, "Module", "(Optional Action) " + ex.Message, new RecordItemIndex(2)); }
            
            try {
                //Report.Log(ReportLevel.Info, "Mouse", "(Optional Action)\r\nMouse Left Click item 'MainForm.DownloadAsterisk' at Center.", repo.MainForm.DownloadAsteriskInfo, new RecordItemIndex(3));
                //repo.MainForm.DownloadAsterisk.Click();
                //Delay.Milliseconds(0);
            } catch(Exception ex) { Report.Log(ReportLevel.Warn, "Module", "(Optional Action) " + ex.Message, new RecordItemIndex(3)); }
            
            try {
                //Report.Log(ReportLevel.Info, "Mouse", "(Optional Action)\r\nMouse Left Move item 'ShipWorks.AsdfGenericFilePerformanceStore' at Center.", repo.ShipWorks.AsdfGenericFilePerformanceStoreInfo, new RecordItemIndex(4));
                //repo.ShipWorks.AsdfGenericFilePerformanceStore.MoveTo();
                //Delay.Milliseconds(0);
            } catch(Exception ex) { Report.Log(ReportLevel.Warn, "Module", "(Optional Action) " + ex.Message, new RecordItemIndex(4)); }
            
            try {
                //Report.Log(ReportLevel.Info, "Mouse", "(Optional Action)\r\nMouse Left Click item 'ShipWorks.AsdfGenericFilePerformanceStore' at Center.", repo.ShipWorks.AsdfGenericFilePerformanceStoreInfo, new RecordItemIndex(5));
                //repo.ShipWorks.AsdfGenericFilePerformanceStore.Click();
                //Delay.Milliseconds(0);
            } catch(Exception ex) { Report.Log(ReportLevel.Warn, "Module", "(Optional Action) " + ex.Message, new RecordItemIndex(5)); }
            
            Report.Log(ReportLevel.Info, "User", "Start 1", new RecordItemIndex(6));
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 5s to exist. Associated repository item: 'ProgressDlg'", repo.ProgressDlg.SelfInfo, new ActionTimeout(5000), new RecordItemIndex(7));
            repo.ProgressDlg.SelfInfo.WaitForExists(5000);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 15m to not exist. Associated repository item: 'ProgressDlg'", repo.ProgressDlg.SelfInfo, new ActionTimeout(900000), new RecordItemIndex(8));
            repo.ProgressDlg.SelfInfo.WaitForNotExists(900000);
            
            Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (RawText='(1,000)') on item 'MainForm.RawText1000'.", repo.MainForm.RawText1000Info, new RecordItemIndex(9));
            Validate.AttributeEqual(repo.MainForm.RawText1000Info, "RawText", "(1,000)");
            Delay.Milliseconds(100);
            
            Report.Log(ReportLevel.Info, "User", "End 1", new RecordItemIndex(10));
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
