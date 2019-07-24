using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [TestModule("08421A6A-1BF3-42DD-A978-3C096F1B150C", ModuleType.UserCode, 1)]
    public class Process100Orders : ITestModule
    {
    	Stopwatch process100Time = new Stopwatch();
    	public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
    	
        public Process100Orders()
        {
            // Do not delete - a parameterless constructor is required!
        }

        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
        	
            ApplyProfileOther();
			ProcessOrder();
			
			Timing.totalProcess100Time = process100Time.ElapsedMilliseconds;
		}
		
		void ApplyProfileOther()
		{
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.ApplyProfile' at Center.", repo.ShippingDlg.ApplyProfileInfo, new RecordItemIndex(4));
			repo.ShippingDlg.ApplyProfile.Click();
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ContextMenuPrint.OtherProfile' at Center.", repo.ContextMenuPrint.OtherProfileInfo, new RecordItemIndex(1));
			repo.ContextMenuPrint.OtherProfile.Click();
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.SplitContainer.CarrierName1' at Center.", repo.ShippingDlg.SplitContainer.CarrierName1Info, new RecordItemIndex(2));
			repo.ShippingDlg.SplitContainer.CarrierNameInfo.WaitForExists(120000);			
		}
		
		void ProcessOrder()
		{
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.ProcessDropDownButton' at Center.", repo.ShippingDlg.ProcessDropDownButtonInfo, new RecordItemIndex(3));
			repo.ShippingDlg.ProcessDropDownButton.Click();
			
			process100Time.Start();
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Enabled='False') on item 'ShippingDlg.SplitContainer.ComboShipmentType'.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(0));
			repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo.WaitForAttributeEqual(120000, "Enabled", "False");
			
			process100Time.Stop();
		}
    }
}
