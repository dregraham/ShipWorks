/*
 * Created by Ranorex
 * User: SMadke
 * Date: 6/11/2019
 * Time: 1:01 PM
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
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
	
	[TestModule("C7FEAAD9-B5ED-4097-9A55-E8ED71D09BBE", ModuleType.UserCode, 1)]
	public class ProcessOrders : ITestModule
	{
		Stopwatch processTime = new Stopwatch();
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		public ProcessOrders()
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
			
			Timing.totalProcess500Time = processTime.ElapsedMilliseconds;
		}
		
		void ApplyProfileOther()
		{
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.ApplyProfile' at Center.", repo.ShippingDlg.ApplyProfileInfo, new RecordItemIndex(4));
			repo.ShippingDlg.ApplyProfile.Click();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ContextMenuPrint.OtherProfile' at Center.", repo.ContextMenuPrint.OtherProfileInfo, new RecordItemIndex(1));
			repo.ContextMenuPrint.OtherProfile.Click();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.SplitContainer.CarrierName1' at Center.", repo.ShippingDlg.SplitContainer.CarrierName1Info, new RecordItemIndex(2));
			repo.ShippingDlg.SplitContainer.CarrierNameInfo.WaitForExists(120000);
			Delay.Milliseconds(0);
		}
		
		void ProcessOrder()
		{
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.ProcessDropDownButton' at Center.", repo.ShippingDlg.ProcessDropDownButtonInfo, new RecordItemIndex(3));
			repo.ShippingDlg.ProcessDropDownButton.Click();
			Delay.Milliseconds(0);
			
			processTime.Start();
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Enabled='False') on item 'ShippingDlg.SplitContainer.ComboShipmentType'.", repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo, new RecordItemIndex(0));
			repo.ShippingDlg.SplitContainer.ComboShipmentTypeInfo.WaitForAttributeEqual(120000, "Enabled", "False");
			Delay.Milliseconds(100);
			
			processTime.Stop();
		}
	}
}
