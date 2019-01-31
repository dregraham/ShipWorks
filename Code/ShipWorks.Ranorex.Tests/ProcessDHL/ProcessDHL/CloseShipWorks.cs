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
    ///The CloseShipWorks recording.
    /// </summary>
    [TestModule("c7353f28-1189-41dd-b286-2f3b7d351400", ModuleType.Recording, 1)]
    public partial class CloseShipWorks : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static CloseShipWorks instance = new CloseShipWorks();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CloseShipWorks()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static CloseShipWorks Instance
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
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'ShipWorksSa.Application' at Center.", repo.ShipWorksSa.ApplicationInfo, new RecordItemIndex(1));
            repo.ShipWorksSa.Application.MoveTo();
            Delay.Milliseconds(200);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left DoubleClick item 'ShipWorksSa.Application' at Center.", repo.ShipWorksSa.ApplicationInfo, new RecordItemIndex(2));
            repo.ShipWorksSa.Application.DoubleClick();
            Delay.Milliseconds(200);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
