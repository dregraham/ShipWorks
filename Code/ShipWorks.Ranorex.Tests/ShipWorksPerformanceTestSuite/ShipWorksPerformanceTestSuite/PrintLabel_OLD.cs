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
using System.IO;

namespace ShipWorksPerformanceTestSuite
{
	
	[TestModule("805F6BA8-7A3D-4130-A711-BA21BF1703E6", ModuleType.UserCode, 1)]
	public class PrintLabel_OLD : ITestModule
	{
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		private string labelFileName;
		private int findFileTimeoutCounter = 0;
		
		
		public PrintLabel_OLD()
		{
			// Do not delete - a parameterless constructor is required!
		}
		public string GetPrintName()
		{
			string PrintName = System.DateTime.Now.ToString("MMddyyyyHHmmss");
			string PrintPrefix = "LabelPrint_";
			labelFileName = PrintPrefix + PrintName;
			return(PrintPrefix + PrintName);
		}
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			repo.MainForm.PanelDockingArea.CreateLabelInfo.WaitForAttributeEqual(30000, "Visible", false);
			SikuliAction.Click(@"..\..\Sikuli_Images\PrintButton.png");
			
			// Cursor moves to Desktop
			Report.Log(ReportLevel.Info, "Mouse", "Cursor moves to Desktop\r\nMouse Left Move item 'SavePrintOutputAs.Desktop' at Center.", repo.SavePrintOutputAs.DesktopInfo, new RecordItemIndex(32));
			repo.SavePrintOutputAs.Desktop.MoveTo();
			Delay.Milliseconds(0);
			
			// Click Desktop
			Report.Log(ReportLevel.Info, "Mouse", "Click Desktop\r\nMouse Left Click item 'SavePrintOutputAs.Desktop' at Center.", repo.SavePrintOutputAs.DesktopInfo, new RecordItemIndex(33));
			repo.SavePrintOutputAs.Desktop.Click();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{LMenu down}{Dkey}{LMenu up}'.", new RecordItemIndex(34));
			Keyboard.Press("{LMenu down}{Dkey}{LMenu up}");
			Delay.Milliseconds(0);
			
			// Key in path Desktop\Generic Performance\Labels
			Report.Log(ReportLevel.Info, "Keyboard", "Key in path Desktop\\Generic Performance\r\nKey sequence 'Desktop\\Generic Performance\\'.", new RecordItemIndex(35));
			Keyboard.Press("Desktop\\Generic Performance\\Labels\\");
			Delay.Milliseconds(0);
			
			// Enter on the keyboard
			Report.Log(ReportLevel.Info, "Keyboard", "Enter on the keyboard\r\nKey sequence '{Return}'.", new RecordItemIndex(36));
			Keyboard.Press("{Return}");
			Delay.Milliseconds(0);
			
//			// Cursor moves to the File Name Field
//			Report.Log(ReportLevel.Info, "Mouse", "Cursor moves to the File Name Field\r\nMouse Left Move item 'SavePrintOutputAs.ComboBox1148' at Center.", repo.SavePrintOutputAs.ComboBox1148Info, new RecordItemIndex(37));
//			repo.SavePrintOutputAs.ComboBox1148.MoveTo();
//			Delay.Milliseconds(0);
			
			// Click File Name Field
			Report.Log(ReportLevel.Info, "Mouse", "Click File Name Field\r\nMouse Left Click item 'SavePrintOutputAs.ComboBox1148' at Center.", repo.SavePrintOutputAs.ComboBox1148Info, new RecordItemIndex(38));
			repo.SavePrintOutputAs.ComboBox1148.Click();
			Delay.Milliseconds(0);
			
			// Key in text
			Report.Log(ReportLevel.Info, "Keyboard", "Key in text\r\nKey sequence 'Empty.swb'.", new RecordItemIndex(39));
			Keyboard.Press(GetPrintName());
			Delay.Milliseconds(0);
			
			// Press Enter on the keyboard > Window closes
			Report.Log(ReportLevel.Info, "Keyboard", "Press Enter on the keyboard > Window closes\r\nKey sequence '{Return}'.", new RecordItemIndex(40));
			Keyboard.Press("{Return}");
			Delay.Milliseconds(0);
			
			while (!File.Exists("C:\\Users\\service_builds\\Desktop\\Generic Performance\\Labels\\" + labelFileName + ".pdf"))
			       {
			       	Delay.Milliseconds(100);
			       	if(findFileTimeoutCounter > 300)
			       	{
			       		throw new FileNotFoundException("Ranorex could not locate the label. Check the PrintLabel class and make sure the label is printed.");
			       	}
			       	findFileTimeoutCounter++;
			       }
			}
	}
}
