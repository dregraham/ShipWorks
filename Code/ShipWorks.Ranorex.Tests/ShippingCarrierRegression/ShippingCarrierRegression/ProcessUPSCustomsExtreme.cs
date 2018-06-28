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
    ///The ProcessUPSCustomsExtreme recording.
    /// </summary>
    [TestModule("d82eee9b-1f6c-4aff-8a2e-940465457285", ModuleType.Recording, 1)]
    public partial class ProcessUPSCustomsExtreme : ITestModule
    {
        /// <summary>
        /// Holds an instance of the ShippingCarrierRegressionRepository repository.
        /// </summary>
        public static ShippingCarrierRegressionRepository repo = ShippingCarrierRegressionRepository.Instance;

        static ProcessUPSCustomsExtreme instance = new ProcessUPSCustomsExtreme();

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public ProcessUPSCustomsExtreme()
        {
        }

        /// <summary>
        /// Gets a static instance of this recording.
        /// </summary>
        public static ProcessUPSCustomsExtreme Instance
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
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.00;

            Init();

        }

#region Image Feature Data
#endregion
    }
#pragma warning restore 0436
}
