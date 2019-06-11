/*
 * Created by Ranorex
 * User: SMadke
 * Date: 6/11/2019
 * Time: 1:49 PM
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
	
	[TestModule("CF731CEB-4065-4AFA-85C0-A45FC3AA463A", ModuleType.UserCode, 1)]
	public class Void500Shipments : ITestModule
	{
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		
		public Void500Shipments()
		{
			// Do not delete - a parameterless constructor is required!
		}
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.VoidSelected' at Center.", repo.ShippingDlg.VoidSelectedInfo, new RecordItemIndex(0));
			repo.ShippingDlg.VoidSelected.Click();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipmentVoidConfirmDlg.ButtonOk' at Center.", repo.ShipmentVoidConfirmDlg.ButtonOkInfo, new RecordItemIndex(1));
			repo.ShipmentVoidConfirmDlg.ButtonOk.Click();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Enabled='False') on item 'ShippingDlg.VoidSelected'.", repo.ShippingDlg.VoidSelectedInfo, new RecordItemIndex(2));
			repo.ShippingDlg.VoidSelectedInfo.WaitForAttributeEqual(600000, "Enabled", "False");
			Delay.Milliseconds(0);
		}
	}
}
