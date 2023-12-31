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

namespace ProcessDHL
{
#pragma warning disable 0436 //(CS0436) The type 'type' in 'assembly' conflicts with the imported type 'type2' in 'assembly'. Using the type defined in 'assembly'.
    /// <summary>
    ///The CheckShipWorksVersion recording.
    /// </summary>
    [TestModule("4dfee956-d61f-4ea3-afd0-8c27f1cdcad2", ModuleType.Recording, 1)]
    public partial class CheckShipWorksVersion : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static CheckShipWorksVersion instance = new CheckShipWorksVersion();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CheckShipWorksVersion()
        {
            ShipWorksVersion = "ShipWorksVersion";
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static CheckShipWorksVersion Instance
        {
            get { return instance; }
        }

#region Variables

        string _ShipWorksVersion;

        /// <summary>
        /// Gets or sets the value of variable ShipWorksVersion.
        /// </summary>
        [TestVariable("74aa69c6-5365-4800-bc09-16ab59ae3ec3")]
        public string ShipWorksVersion
        {
            get { return _ShipWorksVersion; }
            set { _ShipWorksVersion = value; }
        }

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
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.Help' at Center.", repo.ShipWorksSa.HelpInfo, new RecordItemIndex(0));
            repo.ShipWorksSa.Help.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksSa.Help' at Center.", repo.ShipWorksSa.HelpInfo, new RecordItemIndex(1));
            repo.ShipWorksSa.Help.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.AboutShipWorks' at Center.", repo.ShipWorksSa.AboutShipWorksInfo, new RecordItemIndex(2));
            repo.ShipWorksSa.AboutShipWorks.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksSa.AboutShipWorks' at Center.", repo.ShipWorksSa.AboutShipWorksInfo, new RecordItemIndex(3));
            repo.ShipWorksSa.AboutShipWorks.Click();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab}'.", new RecordItemIndex(4));
            Keyboard.Press("{Tab}");
            Delay.Milliseconds(0);
            
            GrabVersion();
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
