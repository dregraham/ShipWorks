/*
 * Created by Ranorex
 * User: gdeblois
 * Date: 6/4/2019
 * Time: 1:46 PM
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

namespace ShipWorksPerformanceTestSuite
{
    /// <summary>
    /// Description of InstallShipWorks.
    /// </summary>
    [TestModule("48CCCFFA-1273-486F-9F8C-AD0E793F6D83", ModuleType.UserCode, 1)]
    public class InstallShipWorks : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// 
        public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
        ExtraMethods extra = new ExtraMethods();
        
        public InstallShipWorks()
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
            
            try
            {
            Host.Local.RunApplication(@"C:\ShipWorks.exe");
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.IAcceptTheAgreement1' at Center.", repo.SomeForm.IAcceptTheAgreement1Info, new RecordItemIndex(0));
            repo.SomeForm.IAcceptTheAgreement1.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.IAcceptTheAgreement1' at Center.", repo.SomeForm.IAcceptTheAgreement1Info, new RecordItemIndex(1));
            repo.SomeForm.IAcceptTheAgreement1.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.Next' at Center.", repo.SomeForm.NextInfo, new RecordItemIndex(2));
            repo.SomeForm.Next.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.Next' at Center.", repo.SomeForm.NextInfo, new RecordItemIndex(3));
            repo.SomeForm.Next.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.DirEdit' at Center.", repo.SomeForm.DirEditInfo, new RecordItemIndex(4));
            repo.SomeForm.DirEdit.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.DirEdit' at Center.", repo.SomeForm.DirEditInfo, new RecordItemIndex(5));
            repo.SomeForm.DirEdit.Click();
            Delay.Milliseconds(0);      
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'C Drive' with focus on 'SomeForm.DirEdit'.", repo.SomeForm.DirEditInfo, new RecordItemIndex(5));
            repo.SomeForm.DirEdit.PressKeys("{LControlKey down}{Akey}{LControlKey up}C:\\TestFolderSW");
            Delay.Milliseconds(0);           
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.Next' at Center.", repo.SomeForm.NextInfo, new RecordItemIndex(7));
            repo.SomeForm.Next.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.Next' at Center.", repo.SomeForm.NextInfo, new RecordItemIndex(8));
            repo.SomeForm.Next.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.Start_Menu' at Center.", repo.SomeForm.Start_MenuInfo, new RecordItemIndex(9));
            repo.SomeForm.Start_Menu.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.Start_Menu' at Center.", repo.SomeForm.Start_MenuInfo, new RecordItemIndex(10));
            repo.SomeForm.Start_Menu.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{LControlKey down}{Akey}{LControlKey up}Smoke Test' with focus on 'SomeForm.Start_Menu'.", repo.SomeForm.Start_MenuInfo, new RecordItemIndex(11));
            repo.SomeForm.Start_Menu.PressKeys("{LControlKey down}{Akey}{LControlKey up}Smoke Test");
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.Next' at Center.", repo.SomeForm.NextInfo, new RecordItemIndex(12));
            repo.SomeForm.Next.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.Next' at Center.", repo.SomeForm.NextInfo, new RecordItemIndex(13));
            repo.SomeForm.Next.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.CreateADesktopIcon' at Center.", repo.SomeForm.CreateADesktopIconInfo, new RecordItemIndex(14));
            repo.SomeForm.CreateADesktopIcon.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.CreateADesktopIcon' at Center.", repo.SomeForm.CreateADesktopIconInfo, new RecordItemIndex(15));
            repo.SomeForm.CreateADesktopIcon.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.Next' at Center.", repo.SomeForm.NextInfo, new RecordItemIndex(16));
            repo.SomeForm.Next.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.Next' at Center.", repo.SomeForm.NextInfo, new RecordItemIndex(17));
            repo.SomeForm.Next.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.Install' at Center.", repo.SomeForm.InstallInfo, new RecordItemIndex(18));
            repo.SomeForm.Install.MoveTo();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.Install' at Center.", repo.SomeForm.InstallInfo, new RecordItemIndex(19));
            repo.SomeForm.Install.Click();
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Wait", "Waiting 20m to exist. Associated repository item: 'SomeForm.Finish'", repo.SomeForm.FinishInfo, new ActionTimeout(1200000), new RecordItemIndex(25));
            repo.SomeForm.FinishInfo.WaitForExists(1200000);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Move item 'SomeForm.Finish' at Center.", repo.SomeForm.FinishInfo, new RecordItemIndex(26));
            repo.SomeForm.Finish.MoveTo(0);
            Delay.Milliseconds(0);
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.Finish' at Center.", repo.SomeForm.FinishInfo, new RecordItemIndex(27));
            repo.SomeForm.Finish.Click();
            Delay.Milliseconds(0);
            }
            
            catch(Exception e)
            {           	
            	Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'SomeForm.Close2' at Center.", repo.SomeForm.Close2Info, new RecordItemIndex(6));
           		repo.SomeForm.Close2.Click();
            	Delay.Milliseconds(0);
            
           		Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ExitSetup.ButtonYes' at Center.", repo.ExitSetup.ButtonYesInfo, new RecordItemIndex(9));
           		repo.ExitSetup.ButtonYes.Click();
          		Delay.Milliseconds(0);  

          		extra.KillBackgroundShipWorksProcesses();
				throw e;    
           	}             	      	
       }         
   }        
}