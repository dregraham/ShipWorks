﻿///////////////////////////////////////////////////////////////////////////////
//
// This file was automatically generated by RANOREX.
// Your custom recording code should go in this file.
// The designer will only add methods to this file, so your custom code won't be overwritten.
// http://www.ranorex.com
// 
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WinForms = System.Windows.Forms;
using System.Diagnostics;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace SmokeTest
{
    public partial class SetupUSPSStamps
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void USPSPostageBalanceTest()
            {
        		float USPSPostagefloat = float.Parse(USPSPostageBalance.Remove(0,1));
        	

        		if (USPSPostagefloat<9000)
	      			{
			            // Move to the Purchase button
			            Report.Log(ReportLevel.Info, "Mouse", "Move to the Purchase button\r\nMouse Left Move item 'UspsPurchasePostageDlg.Purchase' at Center.", repo.UspsPurchasePostageDlg.PurchaseInfo, new RecordItemIndex(84));
			            repo.UspsPurchasePostageDlg.Purchase.MoveTo();
			            Delay.Milliseconds(200);
			            
			            // Click: Purchase
			            Report.Log(ReportLevel.Info, "Mouse", "Click: Purchase\r\nMouse Left Click item 'UspsPurchasePostageDlg.Purchase' at Center.", repo.UspsPurchasePostageDlg.PurchaseInfo, new RecordItemIndex(85));
			            repo.UspsPurchasePostageDlg.Purchase.Click();
			            Delay.Milliseconds(200);
			            
			            // Delay for the OK button to appear
			            Report.Log(ReportLevel.Info, "Delay", "Delay for the OK button to appear\r\nWaiting for 4s.", new RecordItemIndex(86));
			            Delay.Duration(4000, false);
			            
			            // Click: OK (purchase request has been submitted to USPS)
			            Report.Log(ReportLevel.Info, "Mouse", "Click: OK (purchase request has been submitted to USPS)\r\nMouse Left Click item 'ShipWorks.ButtonOK' at Center.", repo.ShipWorks.ButtonOKInfo, new RecordItemIndex(89));
			            repo.ShipWorks.ButtonOK.Click();
			            Delay.Milliseconds(200);
	        	      	Report.Log(ReportLevel.Info, "Get Value", "The USPS Postage is currently below $9,000");
        			}

        	      
        		else
        			{
			        	// Move to the Cancel button
			            Report.Log(ReportLevel.Info, "Mouse", "Move to the OK button\r\nMouse Left Move item 'UspsPurchasePostageDlg.Cancel' at Center.", repo.UspsPurchasePostageDlg.CancelInfo, new RecordItemIndex(87));
			            repo.UspsPurchasePostageDlg.Cancel.MoveTo();
			            Delay.Milliseconds(200);
			            
			            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'UspsPurchasePostageDlg.Cancel' at Center.", repo.UspsPurchasePostageDlg.CancelInfo, new RecordItemIndex(88));
			            repo.UspsPurchasePostageDlg.Cancel.Click();
			            Delay.Milliseconds(200);
        		   		Report.Log(ReportLevel.Info, "Get Value", "The USPS Postage is currently greater than $9,000");
        			}
        	}

        public void SetupUSPSStamps_Run_application()
        {
        	
        	string smokeTestPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),@"..\..\"));
        	string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        	if(Environ == "Production")
        	{
        		;
        	}
            Report.Log(ReportLevel.Info, "Application", "Run TestServers2.cmd to change test server settings in registry");
            Host.Local.RunApplication(smokeTestPath + @"\ZipFiles\TestServers2.cmd", "", "", false);
        }

        public void Key_sequence_Username(RepoItemInfo textInfo)
        {
        	
        	if(Environ == "Production")
        	{            
        	Report.Log(ReportLevel.Info, "Keyboard", "Enter the Username\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}apptiveBrian' with focus on 'textInfo'.", textInfo);
            textInfo.FindAdapter<Text>().PressKeys("{LControlKey down}{Akey}{LControlKey up}apptiveBrian");
        	}
        	

            else
            {
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Username\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}interapptive' with focus on 'textInfo'.", textInfo);
            textInfo.FindAdapter<Text>().PressKeys("{LControlKey down}{Akey}{LControlKey up}interapptive");
            }
        }

        public void Key_sequence_Password(RepoItemInfo textInfo)
        {
        	
        	if(Environ == "Production")
        	{            
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Password\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}password1' with focus on 'textInfo'.", textInfo);
            textInfo.FindAdapter<Text>().PressKeys("{LControlKey down}{Akey}{LControlKey up}stamps7458");
        	}
        	

            else
            {
            Report.Log(ReportLevel.Info, "Keyboard", "Enter the Password\r\nKey sequence '{LControlKey down}{Akey}{LControlKey up}password1' with focus on 'textInfo'.", textInfo);
            textInfo.FindAdapter<Text>().PressKeys("{LControlKey down}{Akey}{LControlKey up}password1");
            }

        }

        public void SelectTestServer()
        {
        	
        	string smokeTestPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(),@"..\..\"));
        	string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        	if(Environ == "Production")
        	{
            Report.Log(ReportLevel.Info, "Application", "Run command to change USPS test server settings in registry to production");
            Process regeditProcess = Process.Start("regedit.exe", "/s " + smokeTestPath + @"ZipFiles\TestServers.reg");
			regeditProcess.WaitForExit();
        	}
            
            else
            {
            Report.Log(ReportLevel.Info, "Application", "Run command to change USPS test server settings in registry to testing");
            Process regeditProcess = Process.Start("regedit.exe", "/s " + smokeTestPath + @"ZipFiles\TestServers2.reg");
			regeditProcess.WaitForExit();
            }
        }

        public void RemoveWebRegUSPS()
        {
        	if(EnvironmentUSPS=="Production")
        	{
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab}{Tab}{Tab}{Tab}{Down}'.");
            Keyboard.Press("{Tab}{Tab}{Tab}{Tab}{Down}");
        	}
        	else
        	{
        	Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Tab}{Tab}{Tab}{Tab}'.");
            Keyboard.Press("{Tab}{Tab}{Tab}{Tab}");	
        	}
        }

        public void UncheckUSPSStampsSetupCleanup()
        {
            TestSuite.Current.GetTestContainer("USPSStampsSetupCleanUp").Checked = false;
        }       
    } 
}