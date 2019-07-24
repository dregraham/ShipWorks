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
    
    [TestModule("F1962406-94AE-46AC-BECF-F04035C3BD15", ModuleType.UserCode, 1)]
    public class Select500OrdersFilter : ITestModule
    {
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
        
        public Select500OrdersFilter()
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
        	Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.RawText500Orders' at Center.", repo.MainForm.PanelDockingArea.RawText500OrdersInfo, new RecordItemIndex(0));
			repo.MainForm.PanelDockingArea.RawText500Orders.Click();			
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.Text500Orders' at 36;8.", repo.MainForm.PanelDockingArea.Text500OrdersInfo, new RecordItemIndex(2));
			repo.MainForm.PanelDockingArea.Text500Orders.MoveTo();
            
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Visible='True') on item 'MainForm.PanelDockingArea.Text500Orders'.", repo.MainForm.PanelDockingArea.Text500OrdersInfo, new RecordItemIndex(4));
			Validate.AttributeEqual(repo.MainForm.PanelDockingArea.Text500OrdersInfo, "Visible", "True");
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='500Orders') on item 'MainForm.PanelDockingArea.Text500Orders'.", repo.MainForm.PanelDockingArea.Text500OrdersInfo, new RecordItemIndex(5));
			Validate.AttributeEqual(repo.MainForm.PanelDockingArea.Text500OrdersInfo, "Text", "500Orders");			
        }
    }
}
