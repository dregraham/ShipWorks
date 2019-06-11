/*
 * Created by Ranorex
 * User: SMadke
 * Date: 6/11/2019
 * Time: 11:17 AM
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
	
	[TestModule("73A28C83-6DD2-4863-9261-44853DA1AC68", ModuleType.UserCode, 1)]
	public class Load500Orders : ITestModule
	{
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;		
		
		public Load500Orders()
		{
			// Do not delete - a parameterless constructor is required!
		}
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'MainForm.PanelDockingArea.RawText1' at Center.", repo.MainForm.PanelDockingArea.RawText1Info, new RecordItemIndex(2));
			repo.MainForm.PanelDockingArea.RawText1.MoveTo();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Right Click item 'MainForm.PanelDockingArea.RawText1' at Center.", repo.MainForm.PanelDockingArea.RawText1Info, new RecordItemIndex(8));
			repo.MainForm.PanelDockingArea.RawText1.Click(System.Windows.Forms.MouseButtons.Right);
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ContextMenuOrderGrid.ShipOrders' at Center.", repo.ContextMenuOrderGrid.ShipOrdersInfo, new RecordItemIndex(9));
			repo.ContextMenuOrderGrid.ShipOrders.Click();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ProgressDlg.LoadShipments' at Center.", repo.ProgressDlg.LoadShipmentsInfo, new RecordItemIndex(10));
			repo.ProgressDlg.LoadShipments.Click();
			Delay.Milliseconds(0);
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Enabled='True') on item 'ShippingDlg.ApplyProfile'.", repo.ShippingDlg.ApplyProfileInfo, new RecordItemIndex(11));
			Validate.AttributeEqual(repo.ShippingDlg.ApplyProfileInfo, "Enabled", "True");
			Delay.Milliseconds(100);
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Visible='True') on item 'ShippingDlg.ApplyProfile'.", repo.ShippingDlg.ApplyProfileInfo, new RecordItemIndex(12));
			Validate.AttributeEqual(repo.ShippingDlg.ApplyProfileInfo, "Visible", "True");
			Delay.Milliseconds(100);
			
			Report.Log(ReportLevel.Info, "Validation", "Validating AttributeEqual (Text='Apply Profile') on item 'ShippingDlg.ApplyProfile'.", repo.ShippingDlg.ApplyProfileInfo, new RecordItemIndex(13));
			Validate.AttributeEqual(repo.ShippingDlg.ApplyProfileInfo, "Text", "Apply Profile");
			Delay.Milliseconds(100);

		}
	}
}
