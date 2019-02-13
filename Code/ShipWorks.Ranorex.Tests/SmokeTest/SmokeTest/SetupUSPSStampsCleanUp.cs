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
    ///The SetupUSPSStampsCleanUp recording.
    /// </summary>
    [TestModule("3d822982-ba90-42d1-b376-da20de22caf4", ModuleType.Recording, 1)]
    public partial class SetupUSPSStampsCleanUp : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static SetupUSPSStampsCleanUp instance = new SetupUSPSStampsCleanUp();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SetupUSPSStampsCleanUp()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static SetupUSPSStampsCleanUp Instance
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
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}'.", new RecordItemIndex(0));
            Keyboard.Press("{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}");
            Delay.Milliseconds(0);
            
            SkipAllUSPSStampsTestCases();
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
