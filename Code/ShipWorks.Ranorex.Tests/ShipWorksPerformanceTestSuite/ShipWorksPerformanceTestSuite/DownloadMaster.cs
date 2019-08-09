using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using WinForms = System.Windows.Forms;
using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;
using SikuliSharp;

namespace ShipWorksPerformanceTestSuite
{
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
			string[] fileName = new string[10];
			string Exe = @"..\..\Sikuli_Images\dotExe.PNG";
			int timeOut = 0;
			
			fileName = Directory.GetFiles(@"c:\users\service_builds\downloads\","*.exe");						
			
			if(fileName.Length > 0){ File.Delete(fileName[0]); }
			
			Report.Log(ReportLevel.Info, "Website", "Opening web site 'http://intdev1201:8080/job/master-public-installer/' with browser 'chrome' in normal mode.", new RecordItemIndex(0));
			Host.Current.OpenBrowser("http://intdev1201:8080/job/master-public-installer/", "chrome", "", false, false, false, false, false);
			
			using (var session = Sikuli.CreateSession())
			{
				session.Click(Patterns.FromFile(Exe));
			}
			
			while(timeOut < 30)
			{
				fileName = Directory.GetFiles(@"c:\users\service_builds\downloads\","*.exe");
				
				if(fileName.Length == 0)
				{
					Thread.Sleep(1000);
					timeOut++;
				}
				else
				{
					Host.Current.CloseApplication(repo.MasterPublicInstallerJenkinsGoog.Pane, new Duration(0));
					break;
				}
			}

			File.Copy(fileName[0], @"c:\ShipWorks.exe",true);
			if(fileName.Length > 0){ File.Delete(fileName[0]); }
		}
	}
}
