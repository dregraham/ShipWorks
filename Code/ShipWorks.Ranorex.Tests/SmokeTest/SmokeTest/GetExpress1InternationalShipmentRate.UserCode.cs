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

    }
}
