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
	[TestModule("DA7C4F4F-6DA0-42F2-975F-4A1838CE9682", ModuleType.UserCode, 1)]
	public class Void100SRShipments : ITestModule
	{
		Stopwatch void100SRTime = new Stopwatch();
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		Select100Orders selectOrders = new Select100Orders();
		Load100SROrders loadOrders = new Load100SROrders();
		ApplyProfileTo100Orders applyprofile = new ApplyProfileTo100Orders();
		SelectShippingRuleFilter selectfilter = new SelectShippingRuleFilter();		
		static int retryCount = 0;
		
		public Void100SRShipments()
		{
			// Do not delete - a parameterless constructor is required!
		}

		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			try {

				if(CheckIfVoid())
				{
					CloseShipOrdersDialog();	
				}				
				else
				{
					VoidShipments();
					void100SRTime.Start();
					
					ConfirmVoidShipmentStarted();
					ConfirmVoidShipmentEnded();
					ConfirmAllVoided();
					
					void100SRTime.Stop();
					
					CloseShipOrdersDialog();
				}
				
			}
			catch (Exception) {
				
				RetryAction.RetryOnFailure(2,1,() => {
				       
               		RetryVoidShipment();
	           	});					
			}
			
			Timing.totalVoid100SRTime = void100SRTime.ElapsedMilliseconds;
		}
		
		bool CheckIfVoid()
		{
			if (repo.ShippingDlg.VoidSelected.Enabled == false)
			{
				return true;
			}
			else
			{
				return false;				
			}
		}
		
		void VoidShipments()
		{
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.VoidSelected' at Center.", repo.ShippingDlg.VoidSelectedInfo, new RecordItemIndex(0));
			repo.ShippingDlg.VoidSelected.Click();
			
			repo.ShipmentVoidConfirmDlg.ButtonOk.MoveTo();
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipmentVoidConfirmDlg.ButtonOk' at Center.", repo.ShipmentVoidConfirmDlg.ButtonOkInfo, new RecordItemIndex(1));
			repo.ShipmentVoidConfirmDlg.ButtonOk.Click();			
		}
		
		void ConfirmVoidShipmentStarted()
		{
			repo.ProgressDlg.VoidingShipmentsInfo.WaitForAttributeEqual(5000, "Visible", "True");
		}
		
		void ConfirmVoidShipmentEnded()
		{
			repo.ProgressDlg.VoidingShipmentsInfo.WaitForNotExists(600000);
		}
		
		void ConfirmAllVoided()
		{
			Report.Log(ReportLevel.Info, "Validation", "RetryCount: " + retryCount, repo.ShippingDlg.VoidSelectedInfo, new RecordItemIndex(2));
			repo.ShippingDlg.VoidSelectedInfo.WaitForAttributeEqual(5000, "Enabled", "False");			
		}
		
		void RetryVoidShipment()
		{
			void100SRTime.Stop();
			
			RetryAction.EscapeFromScreen(3);			
			selectfilter.SelectFilter();
			selectOrders.SelectOrders();
			loadOrders.LoadOrders();
			applyprofile.SelectAllShipments();
			
			if (CheckIfVoid())
			{
				CloseShipOrdersDialog();
			}
			else
			{
				VoidShipments();
				void100SRTime.Start();
				
				ConfirmVoidShipmentStarted();
				ConfirmVoidShipmentEnded();
				ConfirmAllVoided();
				
				void100SRTime.Stop();
				
				CloseShipOrdersDialog();
			}
		}
		
		void CloseShipOrdersDialog()
		{
			repo.ShippingDlg.Close.MoveTo(1000);
			
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.Close' at Center.", repo.ShippingDlg.CloseInfo, new RecordItemIndex(0));
            repo.ShippingDlg.Close.Click(1000);
		}
	}
}
