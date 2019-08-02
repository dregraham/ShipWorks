/*
 * Created by Ranorex
 * User: service_builds
 * Date: 8/1/2019
 * Time: 12:30 PM
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
	/// <summary>
	/// Description of DownloadMaster.
	/// </summary>
	[TestModule("FCDACB56-60A8-43FD-9F37-F624231402B0", ModuleType.UserCode, 1)]
	public class DownloadMaster : ITestModule
	{
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		public DownloadMaster()
		{
			// Do not delete - a parameterless constructor is required!
		}

		
		void ITestModule.Run()
		{
			
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			Report.Log(ReportLevel.Info, "Website", "Opening web site 'http://intdev1201:8080/job/master-public-installer/' with browser 'chrome' in normal mode.", new RecordItemIndex(0));
			Host.Current.OpenBrowser("http://intdev1201:8080/job/master-public-installer/", "chrome", "", false, false, false, false, false);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MasterPublicInstallerJenkinsGoog.Pane' at 639;407.", repo.MasterPublicInstallerJenkinsGoog.PaneInfo, new RecordItemIndex(1));
			repo.MasterPublicInstallerJenkinsGoog.Pane.Click("639;407");
		}
	}
}
