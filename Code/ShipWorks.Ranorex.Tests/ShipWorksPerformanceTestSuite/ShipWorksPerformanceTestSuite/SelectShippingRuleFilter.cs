/*
 * Created by Ranorex
 * User: SMadke
 * Date: 6/13/2019
 * Time: 1:56 PM
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
    
    [TestModule("201A8525-CA1D-476E-BB44-0D6959897FAD", ModuleType.UserCode, 1)]
    public class SelectShippingRuleFilter : ITestModule
    {
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
        
        public SelectShippingRuleFilter()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            try {
            	
            	SelectFilter();
            	
            } catch (Exception) {
            	
            	RetryAction.RetryOnFailure(2,1,() => {
				       
               		SelectFilter();
	           	});
            }
        }
        
        public void SelectFilter()
        {
        	Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.RawText100OrdersSR' at Center.", repo.MainForm.PanelDockingArea.RawText100OrdersSRInfo, new RecordItemIndex(4));
            repo.MainForm.PanelDockingArea.RawText100OrdersSR.Click();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.Text100OrdersSR' at 30;11.", repo.MainForm.PanelDockingArea.Text100OrdersSRInfo, new RecordItemIndex(1));
            repo.MainForm.PanelDockingArea.Text100OrdersSR.MoveTo();            
        }
    }
}
