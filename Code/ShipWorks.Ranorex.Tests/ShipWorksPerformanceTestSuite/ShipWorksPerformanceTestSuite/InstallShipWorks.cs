/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/4/2019
 * Time: 1:46 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
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

namespace ShipWorksPerformanceTestSuite
{
    /// <summary>
    /// Description of InstallShipWorks.
    /// </summary>
    [TestModule("48CCCFFA-1273-486F-9F8C-AD0E793F6D83", ModuleType.UserCode, 1)]
    public class InstallShipWorks : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public InstallShipWorks()
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
            
            Host.Local.RunApplication(@"C:\ShipWorks.exe");
        }
    }
}
