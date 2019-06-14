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
    [TestModule("AB495B19-C723-47F7-8D39-651B9B20A704", ModuleType.UserCode, 1)]
    public class Select100OrdersFilter : ITestModule
    {
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
    	
        public Select100OrdersFilter()
        {
            // Do not delete - a parameterless constructor is required!
        }

        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            SelectFilter();
        }
        
        public void SelectFilter()
        {
        	Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.RawText100Orders' at Center.", repo.MainForm.PanelDockingArea.RawText100OrdersInfo, new RecordItemIndex(8));
            repo.MainForm.PanelDockingArea.RawText100Orders.Click();
            Delay.Milliseconds(0);        	
        }
    }
}
