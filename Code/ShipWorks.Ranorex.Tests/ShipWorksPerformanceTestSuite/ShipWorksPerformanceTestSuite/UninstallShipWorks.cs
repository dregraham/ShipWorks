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
    [TestModule("AAD56913-9930-4392-AB92-F706699E4BA4", ModuleType.UserCode, 1)]
    public class UninstallShipWorks : ITestModule
    {
    	public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
    	ExtraMethods extra = new ExtraMethods();
    	
        public UninstallShipWorks()
        {
            // Do not delete - a parameterless constructor is required!
        }

        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            LaunchUninstaller();
            
            ShipWorksUninstall();
        }
        
        void LaunchUninstaller()
        {
        	
        	Host.Local.RunApplication($@"{RetryAction.CurrentInstallDir}\Uninstall\unins000.exe");
        }
        
        void ShipWorksUninstall()
        {
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksRUninstall.ShipWorksRUninstall' at Center.", repo.ShipWorksRUninstall.ShipWorksRUninstallInfo, new RecordItemIndex(0));
            repo.ShipWorksRUninstall.ShipWorksRUninstall.Click();            
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksRUninstall.ButtonYes' at Center.", repo.ShipWorksRUninstall.ButtonYesInfo, new RecordItemIndex(1));
            repo.ShipWorksRUninstall.ButtonYes.Click();            
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'FormIu14D2N.TNewNotebook' at Center.", repo.FormIu14D2N.TNewNotebookInfo, new RecordItemIndex(2));
            repo.FormIu14D2N.TNewNotebook.Click();            
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksRUninstall.ShipWorksRUninstall' at Center.", repo.ShipWorksRUninstall.ShipWorksRUninstallInfo, new RecordItemIndex(3));
            repo.ShipWorksRUninstall.ShipWorksRUninstall.Click();            
            
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipWorksRUninstall.ButtonOK' at Center.", repo.ShipWorksRUninstall.ButtonOKInfo, new RecordItemIndex(4));
            repo.ShipWorksRUninstall.ButtonOK.Click();
        }
    }
}
