/*
 * Created by Ranorex
 * User: jeman
 * Date: 4/7/2016
 * Time: 2:02 PM
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

namespace ShippingCarrierRegression
{
    /// <summary>
    /// Description of TestingRecordingCall.
    /// </summary>
    [TestModule("7A29A995-2A07-469F-B159-A451EDC354C8", ModuleType.UserCode, 1)]
    public class TestingRecordingCall : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public TestingRecordingCall()
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
            int x = 1;
            if (x==1)
            {
            CreateAnOrder.Start();
        	}
            
  	    }
	}
}