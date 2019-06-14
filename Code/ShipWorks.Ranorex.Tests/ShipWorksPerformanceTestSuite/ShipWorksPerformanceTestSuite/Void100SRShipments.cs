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
    [TestModule("DA7C4F4F-6DA0-42F2-975F-4A1838CE9682", ModuleType.UserCode, 1)]
    public class Void100SRShipments : ITestModule
    {
    	public static ShipWorksPerformanceTestSuiteRepository repo = ShipWorksPerformanceTestSuiteRepository.Instance;
		Select100Orders selectOrders = new Select100Orders();
		Load100SROrders loadOrders = new Load100SROrders();
		ApplyProfileTo100Orders applyprofile = new ApplyProfileTo100Orders();
		SelectShippingRuleFilter selectfilter = new SelectShippingRuleFilter();
		static int retryMax = 2;
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
            
         	if(repo.ShippingDlg.VoidSelected.Enabled == true)
			{
				VoidShipments();
				ConfirmVoidShipmentStarted();
				ConfirmVoidShipmentEnded();
				ConfirmAllVoided();
				CloseShipOrdersDialog();
			}
			else
			{
				RetryAction.EscapeFromScreen(3);
			}   
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
			RetryAction.EscapeFromScreen(3);
			
			selectfilter.SelectFilter();
			selectOrders.SelectOrders();
			loadOrders.LoadOrders();
			applyprofile.SelectAllShipments();
			VoidShipments();
			ConfirmVoidShipmentStarted();
			ConfirmVoidShipmentEnded();
			ConfirmAllVoided();
		}
		
		void CloseShipOrdersDialog()
		{
			Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'ShippingDlg.Close' at Center.", repo.ShippingDlg.CloseInfo, new RecordItemIndex(7));
			repo.ShippingDlg.Close.Click();
			Delay.Milliseconds(0);
		}
    }
}
