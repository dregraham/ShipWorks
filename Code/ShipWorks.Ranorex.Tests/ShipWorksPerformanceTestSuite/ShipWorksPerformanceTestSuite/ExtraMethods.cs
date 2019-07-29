/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/5/2019
 * Time: 4:00 PM
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
			string folderName = System.DateTime.Now.ToString("MMddyyyyHHmmss");
			string folderPrefix = "SWPerformanceTest_";
			return(folderPrefix + folderName);
		}
	}
	
	public static class RetryAction
	{
		
		public static void RetryOnFailure(int maxAttempts, int pressEscape, Action action)
		{
			var attempts = 0;
			
			do
			{
				try
				{
					attempts++;
					
					EscapeFromScreen(pressEscape);
					
					action();
					break;
				}
				catch(Exception e)
				{
					if(attempts == maxAttempts)
					{
						throw e;
					}
				}
			}
			while(true);
		}
		
		public static void EscapeFromScreen(int pressEscape)
		{
			for (int keyPress = 0; keyPress < pressEscape; keyPress++)
			{						
				Keyboard.Press("{Escape}");
				Delay.Seconds(5);
			}			
		}
	}
	
	public static class Timing
	{
		public static long totalLoad500Time = 0;
		public static long totalApplyProfile500Time = 0;
		public static long totalProcessTime = 0;
		public static long totalVoid500Time = 0;		
		
		public static long totalLoad100SRTime = 0;
		public static long totalApplyProfile100SRTime = 0;		
		public static long totalProcess100SRTime = 0;
		public static long totalVoid100SRTime = 0;		
		
		public static long totalLoad100Time = 0;
		public static long totalApplyProfile100Time = 0;		
		public static long totalProcess100Time = 0;
		public static long totalVoid100Time = 0;		
	}
}
