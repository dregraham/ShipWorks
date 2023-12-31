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
    ///The ModuleChecker recording.
    /// </summary>
    [TestModule("bbb58594-dd05-44a3-be75-ac0a924900c4", ModuleType.Recording, 1)]
    public partial class ModuleChecker : ITestModule
    {
        /// <summary>
        /// Holds an instance of the SmokeTestRepository repository.
        /// </summary>
        public static SmokeTestRepository repo = SmokeTestRepository.Instance;

        static ModuleChecker instance = new ModuleChecker();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ModuleChecker()
        {
            Environ = "";
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ModuleChecker Instance
        {
            get { return instance; }
        }

#region Variables

        string _Environ;

        /// <summary>
        /// Gets or sets the value of variable Environ.
        /// </summary>
        [TestVariable("ea3c58a2-7d9b-4bbb-8c13-69d12840837c")]
        public string Environ
        {
            get { return _Environ; }
            set { _Environ = value; }
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
            Mouse.DefaultMoveTime = 1000;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

            TurnOffFirefoxPlugin();
            Delay.Milliseconds(0);
            
            CompareModuleChecker();
            Delay.Milliseconds(0);
            
            CopyHosts();
            Delay.Milliseconds(0);
            
        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
