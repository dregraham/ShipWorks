/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/5/2019
 * Time: 4:00 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;

namespace ShipWorksPerformanceTestSuite
{
	/// <summary>
	/// Description of ExtraMethods.
	/// </summary>
	public class ExtraMethods
	{
		public ExtraMethods()
		{				
		}
		
		public void KillBackgroundShipWorksProcesses()
			{
				System.Diagnostics.Process.Start("CMD.exe", "/C taskkill -im shipworks* -f");
				System.Diagnostics.Process.Start("CMD.exe", "/C taskkill -im ShipWorks.Escalator* -f");
			}
		
		public string GetFolderName()
		{
			string folderName = DateTime.Now.ToString("MMddyyyyHHmmss");
			string folderPrefix = "SWPerformanceTest_";
			return(folderPrefix + folderName);
		}
	}
}
