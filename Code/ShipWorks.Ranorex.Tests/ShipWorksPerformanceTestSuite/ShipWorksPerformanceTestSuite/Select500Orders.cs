/*
 * Created by Ranorex
 * User: SMadke
 * Date: 6/11/2019
 * Time: 10:48 AM
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
	
	[TestModule("15FEB502-A74D-4E42-A249-2DBA9B26F18B", ModuleType.UserCode, 1)]
	public class Select500Orders : ITestModule
	{
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		public Select500Orders()
		{
			// Do not delete - a parameterless constructor is required!
		}
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.RawText500Orders' at Center.", repo.MainForm.PanelDockingArea.RawText500OrdersInfo, new RecordItemIndex(0));
			repo.MainForm.PanelDockingArea.RawText500Orders.Click();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Visible='True') on item 'MainForm.PanelDockingArea.Text500Orders'.", repo.MainForm.PanelDockingArea.Text500OrdersInfo, new RecordItemIndex(4));
			Validate.AttributeEqual(repo.MainForm.PanelDockingArea.Text500OrdersInfo, "Visible", "True");
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='500Orders') on item 'MainForm.PanelDockingArea.Text500Orders'.", repo.MainForm.PanelDockingArea.Text500OrdersInfo, new RecordItemIndex(5));
			Validate.AttributeEqual(repo.MainForm.PanelDockingArea.Text500OrdersInfo, "Text", "500Orders");
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.RawText1' at Center.", repo.MainForm.PanelDockingArea.RawText1Info, new RecordItemIndex(2));
			repo.MainForm.PanelDockingArea.RawText1.Click();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{LControlKey down}{Akey}{LControlKey up}'.", new RecordItemIndex(3));
			Keyboard.Press("{LControlKey down}{Akey}{LControlKey up}");
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Visible='True') on item 'MainForm.Selected500'.", repo.MainForm.Selected500Info, new RecordItemIndex(6));
			Validate.AttributeEqual(repo.MainForm.Selected500Info, "Visible", "True");
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (RawText='Selected: 500') on item 'MainForm.Selected500'.", repo.MainForm.Selected500Info, new RecordItemIndex(7));
			Validate.AttributeEqual(repo.MainForm.Selected500Info, "RawText", "Selected: 500");
			Delay.Milliseconds(0);
		}
	}
}
