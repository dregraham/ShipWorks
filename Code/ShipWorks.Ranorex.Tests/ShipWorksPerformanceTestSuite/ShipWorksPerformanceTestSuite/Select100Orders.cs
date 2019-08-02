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
    [TestModule("440AB339-88DE-4864-8957-4752D24B3890", ModuleType.UserCode, 1)]
    public class Select100Orders : ITestModule
    {
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
    	
        public Select100Orders()
        {
            // Do not delete - a parameterless constructor is required!
        }

        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            SelectOrders();
            ValidateOrdersSelected();
        }
        
        public void SelectOrders()
        {
        	Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.FedExGround' at Center.", repo.MainForm.PanelDockingArea.FedExGroundInfo, new RecordItemIndex(0));
            repo.MainForm.PanelDockingArea.FedExGround.Click();
            			
			Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{LControlKey down}{Akey}{LControlKey up}'.", new RecordItemIndex(3));
			Keyboard.Press("{LControlKey down}{Akey}{LControlKey up}");			
		}
		
		void ValidateOrdersSelected(){
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Visible='True') on item 'MainForm.Selected500'.", repo.MainForm.Selected500Info, new RecordItemIndex(6));
			Validate.AttributeEqual(repo.MainForm.Selected100Info , "Visible", "True");						
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (RawText='Selected: 500') on item 'MainForm.Selected500'.", repo.MainForm.Selected500Info, new RecordItemIndex(7));
			Validate.AttributeEqual(repo.MainForm.Selected100Info, "RawText", "Selected: 100");			
        }
    }
}
