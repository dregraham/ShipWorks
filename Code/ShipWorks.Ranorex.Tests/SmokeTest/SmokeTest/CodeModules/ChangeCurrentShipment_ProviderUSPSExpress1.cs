/*
 * Created by Ranorex
 * User: SMadke
 * Date: 4/18/2019
 * Time: 1:57 PM
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
    
    [TestModule("377DD385-85E0-4FDB-96F9-3E907087CDEA", ModuleType.UserCode, 1)]
    public class ChangeCurrentShipment_ProviderUSPSExpress1 : ITestModule
    {
        
        public ChangeCurrentShipment_ProviderUSPSExpress1(){}
        
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            SmokeTestRepository repo = new SmokeTestRepository();
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(0));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.MoveTo();
            Delay.Milliseconds(200);
            
            // Click Provider Dropdown
            Report.Log(ReportLevel.Info, "Mouse", "Click Provider Dropdown\r\nMouse Left Click item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(1));
            repo.ShippingDlg.SplitContainer.ComboShipmentType.Click();
            Delay.Milliseconds(200);
            
            // Mouse Move to avoid click inconsistency
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Move to avoid click inconsistency\r\nMouse Left Move item 'List1000.USPSExpress1' at Center.", repo.List1000.USPSExpress1Info, new RecordItemIndex(2));
            repo.List1000.USPSExpress1.MoveTo();
            Delay.Milliseconds(200);
            
            // Click USPS (Express 1) in provider dropdown
            Report.Log(ReportLevel.Info, "Mouse", "Click USPS (Express 1) in provider dropdown\r\nMouse Left Click item 'List1000.USPSExpress1' at Center.", repo.List1000.USPSExpress1Info, new RecordItemIndex(3));
            repo.List1000.USPSExpress1.Click();
            Delay.Milliseconds(200);
        }
    }
}
