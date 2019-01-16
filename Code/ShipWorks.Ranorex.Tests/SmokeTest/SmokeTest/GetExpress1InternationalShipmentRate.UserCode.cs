﻿///////////////////////////////////////////////////////////////////////////////
//
// This file was automatically generated by RANOREX.
// Your custom recording code should go in this file.
// The designer will only add methods to this file, so your custom code won't be overwritten.
// http://www.ranorex.com
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace SmokeTest
{
    public partial class GetExpress1InternationalShipmentRate
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void CheckPostageBalance()
        {
        	
        	float ExpOneShipmentRateFloat = float.Parse(ExpOneShipmentRate.Remove(0,1));
        	float ExpOneAvailablePostageFloat = float.Parse(ExpOneAvailablePostage.Remove(0,1));
        	
        	if (ExpOneShipmentRateFloat>ExpOneAvailablePostageFloat)
        	{        		  
	            repo.ShippingDlg.Close.MoveTo();
	            Delay.Milliseconds(200);	            
	            
	            repo.ShippingDlg.Close.Click();
	            Delay.Milliseconds(200);
	            
	            TestSuite.Current.GetTestContainer("ProcessVoidExpress1InternationalShipment").Checked = false;
        	}
        }

        public void CheckRates()
        {
        	if (repo.ShipOrders1.SplitContainer.InternationalShipmentRateInfo.Exists(30000) == false)
        	{
	            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}'.", new RecordItemIndex(15));
	            Keyboard.Press("{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}{Escape}");
	            Delay.Milliseconds(0);        		
        	}
        	else
        	{
	    		// Move mouse to get the shipment rate
	            Report.Log(ReportLevel.Info, "Mouse", "Move mouse to get the shipment rate\r\nMouse Left Move item 'ShipOrders1.SplitContainer.InternationalShipmentRate' at Center.", repo.ShipOrders1.SplitContainer.InternationalShipmentRateInfo, new RecordItemIndex(10));
	            repo.ShipOrders1.SplitContainer.InternationalShipmentRate.MoveTo();
	            Delay.Milliseconds(200);
	            
	            // Get the rate and store it in a variable
	            Report.Log(ReportLevel.Info, "Get Value", "Get the rate and store it in a variable\r\nGetting attribute 'RawText' from item 'ShipOrders1.SplitContainer.InternationalShipmentRate' and assigning its value to variable 'ExpOneShipmentRate'.", repo.ShipOrders1.SplitContainer.InternationalShipmentRateInfo, new RecordItemIndex(11));
	            ExpOneShipmentRate = repo.ShipOrders1.SplitContainer.InternationalShipmentRate.Element.GetAttributeValueText("RawText");
	            Delay.Milliseconds(0);
	            
	            // Compare the available postage with the shipment rate and, if there is enough postage available, process the shipment.
	            CheckPostageBalance();
	            Delay.Milliseconds(0);
        	}
        }
    }
}
