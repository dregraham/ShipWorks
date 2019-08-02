/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/4/2019
 * Time: 1:03 PM
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
using System.Linq;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

using System.IO;

namespace ShipWorksPerformanceTestSuite
{
	/// <summary>
	/// Description of CopyStagingHosts.
	/// </summary>
	[TestModule("A008879E-2CDE-4625-83DB-DF20BE2F6541", ModuleType.UserCode, 1)]
	public class CopyStagingHosts : ITestModule
	{
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public CopyStagingHosts()
		{
			// Do not delete - a parameterless constructor is required!
		}

		/// <summary>
		/// Performs the playback of actions in this module.
		/// </summary>
		/// <remarks>You should not call this method directly, instead pass the module
		/// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
		/// that will in turn invoke this method.</remarks>
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			string newPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\"));
			
			string env = File.ReadLines(newPath + "env.txt").First().Trim();
			
			if(env=="QASC")
			{
				File.Copy(newPath+@"references\qasc\hosts", @"C:\Windows\System32\drivers\etc\hosts", true);
			}
			else
			{
				File.Copy(newPath+@"references\staging\hosts", @"C:\Windows\System32\drivers\etc\hosts", true);
			}
		}
	}
}
