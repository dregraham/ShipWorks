/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/12/2019
 * Time: 11:15 AM
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
	
	[TestModule("CDFF00AF-93C5-4627-AC76-CC066DEA4B0A", ModuleType.UserCode, 1)]
	public class VoidLabel : ITestModule
	{
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		public VoidLabel()
		{
			// Do not delete - a parameterless constructor is required!
		}
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;					
						
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.Void' at Center.", repo.MainForm.VoidInfo, new RecordItemIndex(0));
			repo.MainForm.Void.Click();
			Delay.Milliseconds(0);			
			
			SikuliAction.Click(@"..\..\Sikuli_Images\OkButton.png");
			SikuliAction.Click(@"..\..\Sikuli_Images\ClearButton.png");
		}
	}
}
