/*
 * Created by Ranorex
 * User: jeman
 * Date: 5/26/2017
 * Time: 1:20 PM
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

namespace PerformanceTesting
{
    /// <summary>
    /// Description of UserCodeModule2.
    /// </summary>
    [TestModule("68AF64BF-AF0A-4670-A327-E6EEF42DC23B", ModuleType.UserCode, 1)]
    public class CopyWiremockHosts : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CopyWiremockHosts()
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
            
            File.Copy("C:\\Windows\\System32\\drivers\\etc\\hosts","C:\\Windows\\System32\\drivers\\etc\\hosts.backup",true);           
            //File.Copy("hostswiremock","C:\\Windows\\System32\\drivers\\etc\\hosts",true);
        }
    }
}
