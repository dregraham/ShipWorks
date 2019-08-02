/*
 * Created by Ranorex
 * User: service_builds
 * Date: 8/1/2019
 * Time: 12:39 PM
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
    /// Description of SetBullZipDefaultPrinter.
    /// </summary>
    [TestModule("BD475CA5-3E9D-4F65-A3E4-31551917EBCE", ModuleType.UserCode, 1)]
    public class SetBullZipDefaultPrinter : ITestModule
    {
    	public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
    	
        public SetBullZipDefaultPrinter()
        {
            // Do not delete - a parameterless constructor is required!
        }

        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
              // launches bullzip pdf printer options dialog
            LaunchBullZipPDFPrinterOptions();
            Delay.Milliseconds(0);
            
            // move mouse to the options dialog
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to the options dialog\r\nMouse Left Move item 'BullzipPDFPrinterOptions.BullzipPDFPrinterOptions' at Center.", repo.BullzipPDFPrinterOptions.BullzipPDFPrinterOptionsInfo, new RecordItemIndex(1));
            repo.BullzipPDFPrinterOptions.BullzipPDFPrinterOptions.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click the options dialog
            Report.Log(ReportLevel.Info, "Mouse", "click the options dialog\r\nMouse Left Click item 'BullzipPDFPrinterOptions.BullzipPDFPrinterOptions' at Center.", repo.BullzipPDFPrinterOptions.BullzipPDFPrinterOptionsInfo, new RecordItemIndex(2));
            repo.BullzipPDFPrinterOptions.BullzipPDFPrinterOptions.Click(300);
            Delay.Milliseconds(200);
            
            // tab to select option set drop down
            Report.Log(ReportLevel.Info, "Keyboard", "tab to select option set drop down\r\nKey sequence '{Tab down}'.", new RecordItemIndex(3));
            Keyboard.Press("{Tab down}");
            Delay.Milliseconds(0);
            
            // type 'a' to select auto_print
            Report.Log(ReportLevel.Info, "Keyboard", "type 'a' to select auto_print\r\nKey sequence 'a'.", new RecordItemIndex(4));
            Keyboard.Press("a");
            Delay.Milliseconds(0);
            
            // move mouse to apply button
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to apply button\r\nMouse Left Move item 'BullzipPDFPrinterOptions.Apply' at Center.", repo.BullzipPDFPrinterOptions.ApplyInfo, new RecordItemIndex(5));
            repo.BullzipPDFPrinterOptions.Apply.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click on apply button
            Report.Log(ReportLevel.Info, "Mouse", "click on apply button\r\nMouse Left Click item 'BullzipPDFPrinterOptions.Apply' at Center.", repo.BullzipPDFPrinterOptions.ApplyInfo, new RecordItemIndex(6));
            repo.BullzipPDFPrinterOptions.Apply.Click(300);
            Delay.Milliseconds(200);
            
            // move mouse to ok button
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to ok button\r\nMouse Left Move item 'PDFPrinter.ButtonOK' at Center.", repo.PDFPrinter.ButtonOKInfo, new RecordItemIndex(7));
            repo.PDFPrinter.ButtonOK.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click on ok button
            Report.Log(ReportLevel.Info, "Mouse", "click on ok button\r\nMouse Left Click item 'PDFPrinter.ButtonOK' at Center.", repo.PDFPrinter.ButtonOKInfo, new RecordItemIndex(8));
            repo.PDFPrinter.ButtonOK.Click(300);
            Delay.Milliseconds(200);
            
            // move mouse to ok button
            Report.Log(ReportLevel.Info, "Mouse", "move mouse to ok button\r\nMouse Left Move item 'BullzipPDFPrinterOptions.ButtonOK' at Center.", repo.BullzipPDFPrinterOptions.ButtonOKInfo, new RecordItemIndex(9));
            repo.BullzipPDFPrinterOptions.ButtonOK.MoveTo(300);
            Delay.Milliseconds(200);
            
            // click on ok button
            Report.Log(ReportLevel.Info, "Mouse", "click on ok button\r\nMouse Left Click item 'BullzipPDFPrinterOptions.ButtonOK' at Center.", repo.BullzipPDFPrinterOptions.ButtonOKInfo, new RecordItemIndex(10));
            repo.BullzipPDFPrinterOptions.ButtonOK.Click(300);
            Delay.Milliseconds(200);
            
            // Sets default printer to 'BullZip'
            SetDefaultPrinter();
            Delay.Milliseconds(0);
        }
        
         public void SetDefaultPrinter()
        {
            string setDefaultPrinterBATPath = @"\BatchFile\SetDefaultPrinterBullZip.bat";
        	string currentDir = System.IO.Directory.GetCurrentDirectory();
        	string smokeTestPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(currentDir,@"..\..\"));
        	string BullZipConfigPath = smokeTestPath + @"\BatchFile\auto_print.ini";
        	string BullZipLocalUserPath = @"\AppData\Roaming\PDF Writer\Bullzip PDF Printer\Option Sets\auto_print.ini";
        	
        	//	Gets current user name
        	string currentUser = Environment.UserName;
        	
        	//	Copy config file to local BullZip settings folder
        	System.IO.Directory.CreateDirectory(@"C:\Users\" + currentUser + @"\AppData\Roaming\PDF Writer\Bullzip PDF Printer\Option Sets");
        	System.IO.File.Copy(BullZipConfigPath, @"C:\Users\" + currentUser + BullZipLocalUserPath, true);
        	
        	//	Creates a directory to save all the pdf labels after printing
        	System.IO.Directory.CreateDirectory(@"C:\printpdflabels\"+ FileName.pdfFolder);
        	
        	//	Runs the batch file in the folder BatchFiles. This batch file set the default printer to 'BullZip'.
        	System.Diagnostics.Process.Start(string.Concat(smokeTestPath,setDefaultPrinterBATPath));
        }

        public void LaunchBullZipPDFPrinterOptions()
        {
            System.Diagnostics.Process.Start(@"C:\Program Files\Bullzip\PDF Printer\gui.exe");
        }
        
    }
}
