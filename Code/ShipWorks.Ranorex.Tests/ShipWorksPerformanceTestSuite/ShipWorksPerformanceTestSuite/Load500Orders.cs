/*
 * Created by Ranorex
 * User: SMadke
 * Date: 6/11/2019
 * Time: 11:17 AM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
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
	
	[TestModule("73A28C83-6DD2-4863-9261-44853DA1AC68", ModuleType.UserCode, 1)]
	public class Load500Orders : ITestModule
	{
		Stopwatch loadTime  = new Stopwatch();
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		SelectAllOrders selectOrders = new SelectAllOrders();
		Select500OrdersFilter select500filter = new Select500OrdersFilter();
		
		public Load500Orders()
		{
			// Do not delete - a parameterless constructor is required!
		}
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			try {
				
				loadTime.Start();
				
				LoadOrders();
               	Validate500OrdersLoaded();
               	
               	loadTime.Stop();
               	
			} 
			catch (Exception) {
				
				RetryAction.RetryOnFailure(2,1,() => {
				       
				    loadTime.Stop();
				    
                   	select500filter.SelectFilter();
	               	selectOrders.SelectOrders();
	               	
	               	loadTime.Start();
	               	
	               	LoadOrders();
	               	Validate500OrdersLoaded();
	               	
	               	loadTime.Stop();
	           	});	
			}
			
			Timing.totalLoad500Time = loadTime.ElapsedMilliseconds;
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
		
		public void Validate500OrdersLoaded(){
			
			repo.ShippingDlg.SplitContainer.Shipments500Info.WaitForExists(5000);
		}
	}
}




