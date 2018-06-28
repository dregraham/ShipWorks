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
using System.Net.Mail;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Repository;
using Ranorex.Core.Testing;

namespace SmokeTest
{
    public partial class SendInfoEmail
    {
        /// <summary>
        /// This method gets called right after the recording has been started.
        /// It can be used to execute recording specific initialization code.
        /// </summary>
        private void Init()
        {
            // Your recording specific initialization code goes here.
        }

        public void SendEmail()
        {
            // TODO: Replace the following line with your code implementation.
            GatherCurrentDateandTime gdt = new GatherCurrentDateandTime();
            GetComparisonReportDirectory gcrd = new GetComparisonReportDirectory();
            
            
            
            
            if (Environment.MachineName != "V-QA-AVG-CUSTOM" && Environment.MachineName != "V-QA-BEEFCAKE1" && Environment.MachineName != "KGICONA-4WX3JH2" && Environment.MachineName != "BERGER-PC" && Environment.MachineName != "MADKE-PC" && Environment.MachineName != "JEMAN-PC")
                {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ShipWorksQA@gmail.com");
                mail.To.Add("j.eman@shipworks.com");
                mail.To.Add("k.gicona@shipworks.com");
                mail.To.Add("b.berger@shipworks.com");
                mail.To.Add("s.madke@shipworks.com");
                mail.Subject = "Test Mail";
                mail.Body = "The smoketest was ran on a PC other than the average customer VM or the beefcake VM. It was ran on " + gdt.localDate 
                			+ ". The version of ShipWorks it was ran on was: " + SWVersion.SWVersionNumber 
                			+ ". The PDF label comparison reports are located: " + gcrd.ComparsionDirectory
                			+ ". The pass/fail report is on " + Environment.MachineName
                			+ ", and the file is located here: \"" + Ranorex.Core.Reporting.TestReport.ReportEnvironment.ReportViewFilePath + "\"";
   
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ShipWorksQA@gmail.com", "katieisanerd");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
               Report.Log(ReportLevel.Info, "Get Value", "Something bad happened and the email didn't send" + ex.ToString(), repo.ShipWorksSa.VersionInfo, new RecordItemIndex(6));
            }
  }
        }

    }
}
