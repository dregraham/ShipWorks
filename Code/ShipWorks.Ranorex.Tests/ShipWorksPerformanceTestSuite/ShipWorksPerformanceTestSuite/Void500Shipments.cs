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
	
	[TestModule("CF731CEB-4065-4AFA-85C0-A45FC3AA463A", ModuleType.UserCode, 1)]
	public class Void500Shipments : ITestModule
	{
		Stopwatch voidTime = new Stopwatch();
		public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		SelectAllOrders selectOrders = new SelectAllOrders();
		Load500Orders loadOrders = new Load500Orders();
		ApplyProfileTo500Orders applyprofile = new ApplyProfileTo500Orders();
		Select500OrdersFilter select500filter = new Select500OrdersFilter();
		static int retryMax = 2;
		static int retryCount = 0;
		
		public Void500Shipments()
		{
			// Do not delete - a parameterless constructor is required!
		}
		
		void ITestModule.Run()
		{
			Mouse.DefaultMoveTime = 300;
			Keyboard.DefaultKeyPressTime = 100;
			Delay.SpeedFactor = 1.0;
			
			try {
				
				VoidShipments();
				voidTime.Start();

				ConfirmVoidShipmentStarted();
				ConfirmVoidShipmentEnded();
				ConfirmAllVoided();
				
				voidTime.Stop();
				
				CloseShipOrdersDialog();
			}
			catch (Exception) {
				
				RetryAction.RetryOnFailure(3,1,() => {
				       
               		RetryVoidShipment();
	           	});					
			}
			
			Timing.totalVoid500Time = voidTime.ElapsedMilliseconds;
		}
		
		void VoidShipments()
		{
			
			try {
				
				Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.VoidSelected' at Center.", repo.ShippingDlg.VoidSelectedInfo, new RecordItemIndex(0));
				repo.ShippingDlg.VoidSelected.Click();
				Delay.Milliseconds(0);
				
				repo.ShipmentVoidConfirmDlg.ButtonOkInfo.WaitForExists(5000);
				
				Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipmentVoidConfirmDlg.ButtonOk' at Center.", repo.ShipmentVoidConfirmDlg.ButtonOkInfo, new RecordItemIndex(1));
				repo.ShipmentVoidConfirmDlg.ButtonOk.Click();
				Delay.Milliseconds(0);
				
			}
			catch (Exception) {
				
				RetryAction.RetryOnFailure(3,0,() => {
				                           	
                   	repo.ShippingDlg.VoidSelected.Click();
                   	
                   	repo.ShipmentVoidConfirmDlg.ButtonOkInfo.WaitForExists(5000);
                   	
                   	Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShipmentVoidConfirmDlg.ButtonOk' at Center.", repo.ShipmentVoidConfirmDlg.ButtonOkInfo, new RecordItemIndex(1));
                   	repo.ShipmentVoidConfirmDlg.ButtonOk.Click();
                   	Delay.Milliseconds(0);
               	});
			}
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
				Delay.Milliseconds(0);
				
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
			voidTime.Stop();
			
			RetryAction.EscapeFromScreen(3);
			
			select500filter.SelectFilter();
			selectOrders.SelectOrders();
			loadOrders.LoadOrders();
			applyprofile.SelectAllShipments();
			
			VoidShipments();			
			voidTime.Start();
			
			ConfirmVoidShipmentStarted();
			ConfirmVoidShipmentEnded();
			ConfirmAllVoided();
			
			voidTime.Stop();
		}
		
		void CloseShipOrdersDialog()
		{
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.Close' at Center.", repo.ShippingDlg.CloseInfo, new RecordItemIndex(7));
			repo.ShippingDlg.Close.Click();
			Delay.Milliseconds(0);
		}
	}
}
