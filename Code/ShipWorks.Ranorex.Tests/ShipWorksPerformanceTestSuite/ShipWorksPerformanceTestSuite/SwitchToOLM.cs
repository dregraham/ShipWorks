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

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using SikuliModule;

namespace ShipWorksPerformanceTestSuite
{
	[TestModule("EC58D36E-FF45-4564-9346-918D685EA8FD", ModuleType.UserCode, 1)]
	public class SwitchToOLM : ITestModule
	{
		
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		public SwitchToOLM()
		{
			// Do not delete - a parameterless constructor is required!
		}

		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.Application' at 2;25.", repo.MainForm.ApplicationInfo, new RecordItemIndex(0));
            repo.MainForm.Application.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorks.ViewMode' at 19;12.", repo.ShipWorks.ViewModeInfo, new RecordItemIndex(1));
            repo.ShipWorks.ViewMode.Click();
            Delay.Milliseconds(0);
            
            SikuliAction.Click(@"..\..\Sikuli_Images\OrderLookupModeButton.png");
		}
	}
}
