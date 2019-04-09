/*
 * Created by Ranorex
 * User: SMadke
 * Date: 4/9/2019
 * Time: 11:31 AM
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

namespace OrderCreation
{
    /// <summary>
    /// Description of CreateShopifyOrder.
    /// </summary>
    [TestModule("D1430E56-BCDD-49BC-BE78-BF03251CD60C", ModuleType.UserCode, 1)]
    public class CreateShopifyOrder : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CreateShopifyOrder()
        {
            Report.Log(ReportLevel.Info, "Website", "Opening web site 'https://joanie-loves-tchotchke.myshopify.com/admin/orders' with browser 'firefox' in normal mode.", new RecordItemIndex(0));
            Host.Current.OpenBrowser("https://joanie-loves-tchotchke.myshopify.com/admin/orders", "firefox", "", false, false, false, false, false);
            Delay.Milliseconds(0);
        }
        
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
        }
    }
}
