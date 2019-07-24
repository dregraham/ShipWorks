using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [TestModule("A2144E13-7E55-455D-9FA5-CFC2CD6D585E", ModuleType.UserCode, 1)]
    public class Load100Orders : ITestModule
    {
    	Stopwatch load100Time = new Stopwatch();
    	public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		Select100Orders selectOrders = new Select100Orders();
		SelectShippingRuleFilter selectfilter = new SelectShippingRuleFilter();
		
        public Load100Orders()
        {
            // Do not delete - a parameterless constructor is required!
        }

        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            try {
				
            	load100Time.Start();
            	
				LoadOrders();
               	Validate100OrdersLoaded();
               	
               	load100Time.Stop();
               	
			} 
			catch (Exception) {
				
				RetryAction.RetryOnFailure(2,1,() => {
				       
                   	load100Time.Stop();
            	                           	
                   	selectfilter.SelectFilter();
                   	selectOrders.SelectOrders();
	               	selectOrders.SelectOrders();
	               	
	               	load100Time.Start();
	               	
	               	LoadOrders();
	               	Validate100OrdersLoaded();
	               	
               		load100Time.Stop();
	           	});	
			}
            
            Timing.totalLoad100Time = load100Time.ElapsedMilliseconds;
        }
        
        public void LoadOrders()
		{
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.FedExGround' at Center.", repo.MainForm.PanelDockingArea.FedExGroundInfo, new RecordItemIndex(0));
            repo.MainForm.PanelDockingArea.FedExGround.MoveTo();
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.FedExGround' at Center.", repo.MainForm.PanelDockingArea.FedExGroundInfo, new RecordItemIndex(0));
            repo.MainForm.PanelDockingArea.FedExGround.Click(System.Windows.Forms.MouseButtons.Right);
            
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ContextMenuOrderGrid.ShipOrders' at Center.", repo.ContextMenuOrderGrid.ShipOrdersInfo, new RecordItemIndex(9));
			repo.ContextMenuOrderGrid.ShipOrders.Click();
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Enabled='True') on item 'ShippingDlg.ApplyProfile'.", repo.ShippingDlg.ApplyProfileInfo, new RecordItemIndex(11));
			Validate.AttributeEqual(repo.ShippingDlg.ShippingServicesInfo, "Enabled", "True");
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Visible='True') on item 'ShippingDlg.ApplyProfile'.", repo.ShippingDlg.ApplyProfileInfo, new RecordItemIndex(12));
			Validate.AttributeEqual(repo.ShippingDlg.ShippingServicesInfo, "Visible", "True");			
		}
		
		public void Validate100OrdersLoaded(){
			
			repo.ShippingDlg.SplitContainer.Shipments100Info.WaitForExists(5000);
		}
    }
}
