/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/11/2019
 * Time: 3:42 PM
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
using System.Diagnostics;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace ShipWorksPerformanceTestSuite
{
	
	[TestModule("E739EE77-CA31-4989-93FC-194882657B8C", ModuleType.UserCode, 1)]
	public class EnterOrder : ITestModule
	{
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		public EnterOrder()
		{
			// Do not delete - a parameterless constructor is required!
		}

		
		void ITestModule.Run()
		{
//			Mouse.DefaultMoveTime = 300;
//			Keyboard.DefaultKeyPressTime = 100;
//			Delay.SpeedFactor = 1.0;
			
				 try {
            	
            	EnterOrderMethod();
            	
            } catch (Exception) {
            	
            	RetryAction.RetryOnFailure(2,1,() => {
				       
	           	});
            }
		}
		
		public void EnterOrderMethod()
		{
			Stopwatch orderLoadTime = new Stopwatch();
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.PARTContentHost' at Center.", repo.MainForm.PanelDockingArea.PARTContentHostInfo, new RecordItemIndex(2));
			repo.MainForm.PanelDockingArea.PARTContentHost.Click();
			
			Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '1'.", new RecordItemIndex(0));
			Keyboard.Press(LoopNumber.counter.ToString());
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.Button' at Center.", repo.MainForm.PanelDockingArea.ButtonInfo, new RecordItemIndex(1));
			repo.MainForm.PanelDockingArea.Button.Click();
			
			orderLoadTime.Start();			
			repo.MainForm.PanelDockingArea.ShipmentDetailsInfo.WaitForAttributeEqual(30000, "Visible", "True");
			orderLoadTime.Stop();
			long orderLoadTimeValue = orderLoadTime.ElapsedMilliseconds;
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Visible='True') on item 'MainForm.PanelDockingArea.ShipmentDetails'.", repo.MainForm.PanelDockingArea.ShipmentDetailsInfo, new RecordItemIndex(1));
			Validate.AttributeEqual(repo.MainForm.PanelDockingArea.ShipmentDetailsInfo, "Visible", "True");
						
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Enabled='True') on item 'MainForm.PanelDockingArea.ShipmentDetails'.", repo.MainForm.PanelDockingArea.ShipmentDetailsInfo, new RecordItemIndex(0));
			Validate.AttributeEqual(repo.MainForm.PanelDockingArea.ShipmentDetailsInfo, "Enabled", "True");
					
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='Shipment Details') on item 'MainForm.PanelDockingArea.ShipmentDetails'.", repo.MainForm.PanelDockingArea.ShipmentDetailsInfo, new RecordItemIndex(2));
			Validate.AttributeEqual(repo.MainForm.PanelDockingArea.ShipmentDetailsInfo, "Text", "Shipment Details");
		}
	}
}
