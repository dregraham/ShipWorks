/*
 * Created by Ranorex
 * User: jeman
 * Date: 5/25/2017
 * Time: 1:25 PM
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
    /// Description of WriteDataToExcel.
    /// </summary>
    [TestModule("AEC11A41-9059-48CF-AD2C-E5E3A43AA2E4", ModuleType.UserCode, 1)]
    public class WriteDataToExcel : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public WriteDataToExcel()
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
            DurationResults.WriteData();
        }
    }
}
