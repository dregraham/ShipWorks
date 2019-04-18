/*
 * Created by Ranorex
 * User: SMadke
 * Date: 4/18/2019
 * Time: 2:01 PM
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

namespace SmokeTest.CodeModules
{
    /// <summary>
    /// Description of ShipOrders_ToAddress_Domestic2.
    /// </summary>
    [TestModule("E97D9A81-21C8-496A-BBAA-203069B2B61C", ModuleType.UserCode, 1)]
    public class ShipOrders_ToAddress_Domestic2 : ITestModule
    {
        
        public ShipOrders_ToAddress_Domestic2(){}
        
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
        }
    }
}
