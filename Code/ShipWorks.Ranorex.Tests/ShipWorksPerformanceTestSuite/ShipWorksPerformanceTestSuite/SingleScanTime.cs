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
using System.Diagnostics;
using System.Drawing;
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
			string search = @"..\..\Sikuli_Images\Search.PNG";
			string searchfield = @"..\..\Sikuli_Images\SearchField.PNG";
			string shipmentdetails = @"..\..\Sikuli_Images\ShipmentDetails.PNG";
			string clear = @"..\..\Sikuli_Images\ClearButton.PNG";
			int[] average90 = new int[20];
			Random r = new Random();
			Stopwatch sw = new Stopwatch();

			using (var session = Sikuli.CreateSession())
			{
				for(int i = 0; i < 20; i++)
				{
					session.Click(Patterns.FromFile(searchfield));
					session.Type(r.Next(50,71).ToString());
					session.Click(Patterns.FromFile(search));
										
					sw.Start();
					session.Hover(Patterns.FromFile(clear));
					session.Exists(Patterns.FromFile(shipmentdetails));
					sw.Stop();
					
					session.Click(Patterns.FromFile(clear));
					average90[i] = (int)sw.ElapsedMilliseconds;
					sw.Reset();					
				}
			}
			
			Array.Sort(average90);
			int averageSingleScanTime = 0;
			
			for(int i = 0; i < 18; i++)
			{
				averageSingleScanTime += average90[i];
			}
			
			Timing.singleScanTime = (averageSingleScanTime/18);		
		}
	}
}
