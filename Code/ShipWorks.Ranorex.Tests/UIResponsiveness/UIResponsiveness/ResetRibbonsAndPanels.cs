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
    ///The ResetRibbonsAndPanels recording.
    /// </summary>
    [TestModule("33bb36fd-4193-4bb7-a11c-00c109a537ce", ModuleType.Recording, 1)]
    public partial class ResetRibbonsAndPanels : ITestModule
    {
        /// <summary>
        /// Holds an instance of the UIResponsivenessRepository repository.
        /// </summary>
        public static UIResponsivenessRepository repo = UIResponsivenessRepository.Instance;

        static ResetRibbonsAndPanels instance = new ResetRibbonsAndPanels();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ResetRibbonsAndPanels()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ResetRibbonsAndPanels Instance
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

            // Click: View
            Report.Log(ReportLevel.Info, "Mouse", "Click: View\r\nMouse Left Click item 'MainForm.View' at Center.", repo.MainForm.ViewInfo, new RecordItemIndex(0));
            repo.MainForm.View.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorks2.ResetView' at Center.", repo.ShipWorks2.ResetViewInfo, new RecordItemIndex(1));
            repo.ShipWorks2.ResetView.Click();
            Delay.Milliseconds(200);
            
            // Click: Ribbons and Panels
            Report.Log(ReportLevel.Info, "Mouse", "Click: Ribbons and Panels\r\nMouse Left Click item 'ShipWorks2.RibbonAndPanels' at Center.", repo.ShipWorks2.RibbonAndPanelsInfo, new RecordItemIndex(2));
            repo.ShipWorks2.RibbonAndPanels.Click();
            Delay.Milliseconds(200);
            
            // Click: Reset
            Report.Log(ReportLevel.Info, "Mouse", "Click: Reset\r\nMouse Left Click item 'ShipWorks2.Reset' at Center.", repo.ShipWorks2.ResetInfo, new RecordItemIndex(3));
            repo.ShipWorks2.Reset.Click();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
