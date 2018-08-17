/*
 * Created by Ranorex
 * User: jeman
 * Date: 5/26/2017
 * Time: 1:43 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using System.IO;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace SmokeTest
{
    /// <summary>
    /// Description of CopyStagingHosts.
    /// </summary>
    [TestModule("3709321C-0F10-40DE-8FDD-5AA927C7F545", ModuleType.UserCode, 1)]
    public class CopyStagingHosts : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CopyStagingHosts()
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
            File.Copy(@"\\intfs01\Development\Testing\hostsstaging","C:\\Windows\\System32\\drivers\\etc\\hosts",true);
        }
    }
}
