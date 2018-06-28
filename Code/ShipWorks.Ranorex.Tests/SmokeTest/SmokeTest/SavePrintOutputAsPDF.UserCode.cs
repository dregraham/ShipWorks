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
    public partial class SavePrintOutputAsPDF
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }        

        public void SetPDFFileDirectory()
        {
        	System.IO.Directory.CreateDirectory(FileName.pdfFolderPath + FileName.pdfFolder);
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence '{LControlKey down}{Akey}{LControlKey up}C:\\Label_PDFs{Return}'.");
            Keyboard.Press("{LControlKey down}{Akey}{LControlKey up}" + FileName.pdfFolderPath + FileName.pdfFolder + "{Return}");
        }

        public void SetPDFFileName()
        {
            Report.Log(ReportLevel.Info, "Keyboard", "Key sequence ''.");
            Keyboard.Press(FileName.pdfname + System.DateTime.Now.ToString("_MMdd_HHmmss"));
            
            switch (FileName.pdfname)
            {
            	case "ProcessFedExInternational":
            		{
            			FileName.pdfname = FileName.pdfname + "CommercialInvoice";
            			break;
            		}
            	case "ProcessUPSInternational":
            		{
            			FileName.pdfname = FileName.pdfname + "CommercialInvoice";
            			break;
            		}            		
            }
        }

        public void Mouse_Click_Close(RepoItemInfo buttonInfo)
        {
            Report.Log(ReportLevel.Info, "Mouse", "Mouse Left Click item 'buttonInfo'", buttonInfo);

            //	Clicks the Close button if it exists.
            if (buttonInfo.Exists(5000))
            {
            	repo.ShippingDlg.Close.Click();
            }
        }                     
    }
}
