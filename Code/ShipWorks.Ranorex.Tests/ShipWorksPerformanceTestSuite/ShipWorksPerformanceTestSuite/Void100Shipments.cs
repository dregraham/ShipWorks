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
	[TestModule("89CC9D0A-4E47-454A-A813-9C9CE320E774", ModuleType.UserCode, 1)]
	public class Void100Shipments : ITestModule
	{
		Stopwatch void100Time = new Stopwatch();
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		Select100Orders selectOrders = new Select100Orders();
		Load100Orders loadOrders = new Load100Orders();
		ApplyProfileTo100Orders applyprofile = new ApplyProfileTo100Orders();
		Select100OrdersFilter selectfilter = new Select100OrdersFilter();
		static int retryMax = 2;
		static int retryCount = 0;
		
		public Void100Shipments()
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
					void100Time.Start();
					
					ConfirmVoidShipmentStarted();
					ConfirmVoidShipmentEnded();
					ConfirmAllVoided();
					
					void100Time.Stop();
					
					CloseShipOrdersDialog();
				}
			}
			catch (Exception)
			{
				RetryAction.RetryOnFailure(2,1,() => {
				                           	
               		RetryVoidShipment();
               	});
			}
			
			Timing.totalVoid100Time = void100Time.ElapsedMilliseconds;
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
			try {
				
				repo.ProgressDlg.VoidingShipmentsInfo.WaitForAttributeEqual(5000, "Visible", "True");
				
			} catch (Exception) {
				
				VoidShipments();
			}
		}
		
		void ConfirmVoidShipmentEnded()
		{
			try {
				
				repo.ProgressDlg.VoidingShipmentsInfo.WaitForNotExists(600000);
				
			} catch (Exception) {
				
				if(retryCount > retryMax){
					
					retryCount = 1;
					throw;
				}
				
				retryCount++;
				RetryVoidShipment();
			}
			
		}
		
		void ConfirmAllVoided()
		{
			try {
				
				Report.Log(ReportLevel.Info, "Validation", "RetryCount: " + retryCount, repo.ShippingDlg.VoidSelectedInfo, new RecordItemIndex(2));
				repo.ShippingDlg.VoidSelectedInfo.WaitForAttributeEqual(5000, "Enabled", "False");				
				
			} catch (Exception) {
				
				if(retryCount > retryMax){
					
					retryCount = 1;
					throw;
				}
				
				retryCount++;
				RetryVoidShipment();
			}
		}
		
		void RetryVoidShipment()
		{
			void100Time.Stop();
			
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
				void100Time.Start();
				
				ConfirmVoidShipmentStarted();
				ConfirmVoidShipmentEnded();
				ConfirmAllVoided();
				
				void100Time.Stop();
				
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
