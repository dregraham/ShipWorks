/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/12/2019
 * Time: 11:14 AM
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
	
	[TestModule("4C843EE3-6E4F-4B5E-9656-8ABE6789A336", ModuleType.UserCode, 1)]
	public class CreateLabel : ITestModule
	{
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		public CreateLabel()
		{
			// Do not delete - a parameterless constructor is required!
		}

		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.CreateLabel' at Center.", repo.MainForm.PanelDockingArea.CreateLabelInfo, new RecordItemIndex(1));
			repo.MainForm.PanelDockingArea.CreateLabel.Click();
			Delay.Milliseconds(0);
		}
	}
}
