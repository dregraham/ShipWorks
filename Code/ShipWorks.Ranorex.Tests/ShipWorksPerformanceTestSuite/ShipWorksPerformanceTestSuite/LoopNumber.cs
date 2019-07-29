/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/13/2019
 * Time: 3:39 PM
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
	
	[TestModule("4D233ECA-93E5-4260-9280-BC5314C223FB", ModuleType.UserCode, 1)]
	public class LoopNumber : ITestModule
	{
		public static int counter;
		public LoopNumber()
		{
			// Do not delete - a parameterless constructor is required!
		}

		
		void ITestModule.Run()
		{
			var enterOrder = new EnterOrder();
			var applyProfile = new ApplyProfile();
			var createLabel = new CreateLabel();
			var printLabel = new PrintLabel();
			var voidLabel = new VoidLabel();

			
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			for (counter = 8; counter < 10; counter++)
			{
				TestModuleRunner.Run(enterOrder);
				TestModuleRunner.Run(applyProfile);
				TestModuleRunner.Run(createLabel);
				TestModuleRunner.Run(printLabel);
				TestModuleRunner.Run(voidLabel);
			}
		}
	}
}
