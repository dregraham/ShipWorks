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
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace SmokeTest
{
    public partial class SetupUPS
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void VerifyLastInvoice()
        {
        	if (repo.UpsSetupWizard.MainPanel.InvoiceNumberInfo.Exists(30000))
        	{
        		string[] invoiceDetails = File.ReadAllLines("\\\\intfs01\\Development\\Testing\\UPSInvoice\\Invoice.txt");       		
        	
        		Report.Log(ReportLevel.Info, "Keyboard", "Key sequence with focus on 'UpsSetupWizard.MainPanel.InvoiceNumber'.", repo.UpsSetupWizard.MainPanel.InvoiceNumberInfo, new RecordItemIndex(80));
        		repo.UpsSetupWizard.MainPanel.InvoiceNumber.PressKeys(invoiceDetails[0] + "{Tab down}");
            	Delay.Milliseconds(1000);
            	
	            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence with focus on 'UpsSetupWizard.MainPanel.List172019'.", repo.UpsSetupWizard.MainPanel.List172019Info, new RecordItemIndex(82));
	            repo.UpsSetupWizard.MainPanel.List172019.PressKeys(invoiceDetails[1] + "{Right}" + invoiceDetails[2] + "{Right}" + invoiceDetails[3] + "{Tab down}");
	            Delay.Milliseconds(1000);
	            
                Report.Log(ReportLevel.Info, "Keyboard", "Key sequence with focus on 'UpsSetupWizard.MainPanel.InvoiceAmount'.", repo.UpsSetupWizard.MainPanel.InvoiceAmountInfo, new RecordItemIndex(84));
                repo.UpsSetupWizard.MainPanel.InvoiceAmount.PressKeys("{LControlKey down}{Akey}{LControlKey up}" + invoiceDetails[4] + "{Tab down}");
	            Delay.Milliseconds(1000);
	            
                Report.Log(ReportLevel.Info, "Keyboard", "Key sequence 'asdf{Tab down}' with focus on 'UpsSetupWizard.MainPanel.ControlID'.", repo.UpsSetupWizard.MainPanel.ControlIDInfo, new RecordItemIndex(86));
                repo.UpsSetupWizard.MainPanel.ControlID.PressKeys(invoiceDetails[5] + "{Tab down}{Return}");
	            Delay.Milliseconds(1000);	
        	}
        	else{}       		            
        }
    }
}