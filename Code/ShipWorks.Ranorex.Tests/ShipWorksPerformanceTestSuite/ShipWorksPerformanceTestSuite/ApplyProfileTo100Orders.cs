﻿using System;
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

    [TestModule("4833ECEE-B4AC-4EAF-9FA5-25DDF1D28F38", ModuleType.UserCode, 1)]
    public class ApplyProfileTo100Orders : ITestModule
    {
    	Stopwatch applyProfile100Time = new Stopwatch();
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
        public ApplyProfileTo100Orders()
        {
            // Do not delete - a parameterless constructor is required!
        }

        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            SelectAllShipments();            
            ApplyBestRateProfile();
            
            applyProfile100Time.Stop();
            
            Timing.totalApplyProfile100Time = applyProfile100Time.ElapsedMilliseconds;
        }
        
        public void SelectAllShipments()
		{		
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.SplitContainer.RawText1Of1' at Center.", repo.ShippingDlg.SplitContainer.RawText1Of1Info, new RecordItemIndex(5));
            repo.ShippingDlg.SplitContainer.RawText1Of1.Click();            
			
			Report.Log(ReportLevel.Info, "Keyboard", "Key 'Ctrl+A' Press with focus on 'ShippingDlg.SplitContainer.EntityGrid'.", repo.ShippingDlg.SplitContainer.EntityGridInfo, new RecordItemIndex(1));
			Keyboard.PrepareFocus(repo.ShippingDlg.SplitContainer.EntityGrid);
			Keyboard.Press(System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control, 30, Keyboard.DefaultKeyPressTime, 1, true);
						
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Visible='True') on item 'ShippingDlg.SplitContainer.Selected500'.", repo.ShippingDlg.SplitContainer.Selected500Info, new RecordItemIndex(2));
			Validate.AttributeEqual(repo.ShippingDlg.SplitContainer.Selected100Info, "Visible", "True");
						
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (RawText='Selected: 100') on item 'ShippingDlg.SplitContainer.Selected500'.", repo.ShippingDlg.SplitContainer.Selected500Info, new RecordItemIndex(3));
			Validate.AttributeEqual(repo.ShippingDlg.SplitContainer.Selected100Info, "RawText", "Selected: 100");			
		}
		
		void ApplyBestRateProfile()
		{	
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.SplitContainer.ComboShipmentType' at Center.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(0));
			repo.ShippingDlg.SplitContainer.ComboShipmentType.MoveTo();
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.ApplyProfile' at Center.", repo.ShippingDlg.ApplyProfileInfo, new RecordItemIndex(4));
			repo.ShippingDlg.ApplyProfile.Click();
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ContextMenuPrint.BestRateProfile' at Center.", repo.ContextMenuPrint.BestRateProfileInfo, new RecordItemIndex(0));
			repo.ContextMenuPrint.BestRateProfile.Click();
			
			applyProfile100Time.Start();
			
			Report.Log(ReportLevel.Info, "Wait", "Waiting 2m to not exist. Associated repository item: 'ProgressDlg.PreparingShipments'", repo.ProgressDlg.PreparingShipmentsInfo, new ActionTimeout(120000), new RecordItemIndex(1));
			repo.ProgressDlg.PreparingShipmentsInfo.WaitForExists(120000);
			
			Report.Log(ReportLevel.Info, "Wait", "Waiting 2m to not exist. Associated repository item: 'ProgressDlg.PreparingShipments'", repo.ProgressDlg.PreparingShipmentsInfo, new ActionTimeout(120000), new RecordItemIndex(1));
			repo.ProgressDlg.PreparingShipmentsInfo.WaitForNotExists(120000);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.SplitContainer.ShipDate' at Center.", repo.ShippingDlg.SplitContainer.ShipDateInfo, new RecordItemIndex(6));
			repo.ShippingDlg.SplitContainer.ShipDateInfo.WaitForExists(120000);			
		}
    }
}
