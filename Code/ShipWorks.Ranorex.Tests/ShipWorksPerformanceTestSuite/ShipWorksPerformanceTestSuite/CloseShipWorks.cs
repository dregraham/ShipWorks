/*
 * Created by Ranorex
 * User: SMadke
 * Date: 6/10/2019
 * Time: 10:14 AM
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
    
    [TestModule("82D40B1A-C4FC-4BA1-962A-81CC793C1F7E", ModuleType.UserCode, 1)]
    public class CloseShipWorks : ITestModule
    {        
        public CloseShipWorks()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            ExtraMethods em = new ExtraMethods();            
            em.KillBackgroundShipWorksProcesses();            
        }
    }
}
