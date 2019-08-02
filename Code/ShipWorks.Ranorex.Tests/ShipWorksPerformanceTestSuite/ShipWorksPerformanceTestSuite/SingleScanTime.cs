/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/25/2019
 * Time: 4:26 PM
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
using System.Diagnostics;

namespace ShipWorksPerformanceTestSuite
{
	
	[TestModule("F8BE65F7-A99B-4462-88DB-7C96270826B7", ModuleType.UserCode, 1)]
	public class SingleScanTime : ITestModule
	{
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		Stopwatch singleScanTime  = new Stopwatch();
		
		public SingleScanTime()
		{
			// Do not delete - a parameterless constructor is required!
		}

		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.PARTContentHost' at Center.", repo.MainForm.PanelDockingArea.PARTContentHostInfo, new RecordItemIndex(2));
			repo.MainForm.PanelDockingArea.PARTContentHost.Click();
			Delay.Milliseconds(0);
			
			Keyboard.Press("{1}");
			singleScanTime.Start();
			repo.MainForm.PanelDockingArea.CreateLabelInfo.WaitForAttributeEqual(30000, "Visible", true);
			singleScanTime.Stop();
			
			Timing.singleScanTime = singleScanTime.ElapsedMilliseconds;
		}
	}
}
