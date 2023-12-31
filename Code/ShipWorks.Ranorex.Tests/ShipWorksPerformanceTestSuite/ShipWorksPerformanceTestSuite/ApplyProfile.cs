﻿/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/11/2019
 * Time: 4:52 PM
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
using Ranorex.Core.Repository;
using SikuliModule;


namespace ShipWorksPerformanceTestSuite
{
	
	[TestModule("649B55A9-8A28-4AE7-92AE-504376E9E041", ModuleType.UserCode, 1)]
	public class ApplyProfile : ITestModule
	{
		
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			
			try {
				
				ApplyProfileMethod();
				
			} catch (Exception) {
				
				RetryAction.RetryOnFailure(2,1,() => {
				ApplyProfileMethod();
		  		});
			}
		}
		
		public void ApplyProfileMethod()
		{
			Random RNG = new Random();
			
			SikuliAction.Click(@"..\..\Sikuli_Images\ApplyButton.png");
			SikuliAction.Click(@"..\..\Sikuli_Images\USPSProfile.png");
			SikuliAction.Click(@"..\..\Sikuli_Images\PostalCodeField.png");
			Keyboard.Press(RNG.Next(10000, 99999).ToString()); // random zipcode generation
		}
	}
}
