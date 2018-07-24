/*
 * Created by Ranorex
 * User: jeman
 * Date: 5/25/2017
 * Time: 12:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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

namespace PerformanceTesting
{
    /// <summary>
    /// Description of UnzipGenericFileStuff.
    /// </summary>
    [TestModule("9BE29F57-D231-49FB-9A67-7478758C73FB", ModuleType.UserCode, 1)]
    public class UnzipGenericFileStuff : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public UnzipGenericFileStuff()
        {
            // Do not delete - a parameterless constructor is required!
        }

        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            DurationResults.ExtractFiles();
        }
    }
}
