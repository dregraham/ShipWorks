/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 7/25/2019
 * Time: 3:52 PM
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
	/// <summary>
	/// Description of PrintLabel.
	/// </summary>
	[TestModule("52BC6767-83C3-4550-A54D-959E707A3905", ModuleType.UserCode, 1)]
	public class PrintLabel : ITestModule
	{
		
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		public PrintLabel()
		{
			// Do not delete - a parameterless constructor is required!
		}
		
		public void ChangePDFName()
		{
			//	Gets the current class name
			FileName.pdfname = "Print Label";
			
			// Changes the pdf file name
			PDFName newName = new PDFName();
			newName.ChangeName();
		}
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			
				 try {
            	
            	PrintLabelMethod();
            	
            } catch (Exception) {
            	
            	RetryAction.RetryOnFailure(2,1,() => {
				PrintLabelMethod();
	           	});
            }
		}
		
		public void PrintLabelMethod()
		{
			ChangePDFName();
			SikuliAction.Click(@"..\..\Sikuli_Images\PrintButton.png");
		}
	}
}
