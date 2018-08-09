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
using System.IO;

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
            	string labelDirectory = "C:\\printpdflabels\\" + FileName.pdfFolder + "\\";
            	string reportDirectory = "C:\\printpdflabels\\" + FileName.pdfFolder + "\\ComparisonReport\\";
            	using(Ionic.Zip.ZipFile labelszip = new Ionic.Zip.ZipFile())
        			{
            		labelszip.AddDirectory(labelDirectory);
            		labelszip.Save("labels.zip");
        			}
            	using(Ionic.Zip.ZipFile reportszip = new Ionic.Zip.ZipFile())
        			{
            		reportszip.AddDirectory(reportDirectory);
            		reportszip.Save("reports.zip");
        			}
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
                mail.Body = "The smoketest was ran on: " + Environment.MachineName + ". + System.Environment.NewLine"
                			+ "It was ran on: " + gdt.localDate  + ". + System.Environment.NewLine";
                if(SWVersion.SWVersionNumber.Length > 0)
                {
					mail.Body = mail.Body 
							+ "The version of ShipWorks it was ran on was: " + SWVersion.SWVersionNumber  + ". + System.Environment.NewLine";
                }
                else
                {
                	mail.Body = mail.Body 
                			+ "The smoketest did not return the version of ShipWorks it was ran on. This means that the CheckShipWorksVersion module did not run. This is generally not a good thing, and means something went terribly wrong. This should be investigated.";
                }
                mail.Body = mail.Body 
		                	+ "The version of ShipWorks it was ran on was: " + SWVersion.SWVersionNumber  + ". + System.Environment.NewLine"
                			+ "The PDF label comparison reports are located: " + gcrd.ComparsionDirectory  + ". + System.Environment.NewLine"
                			+ "The pass/fail report is on " + Environment.MachineName + ", + System.Environment.NewLine"
                			+ ", and the file is located here: " + "\"" + Ranorex.Core.Reporting.TestReport.ReportEnvironment.ReportViewFilePath +  "\"" + ". + System.Environment.NewLine";
              
                
                if(Directory.Exists(labelDirectory))
                {
                	if(!Directory.EnumerateFileSystemEntries(labelDirectory).IsEmpty())
                   	{
                		if(File.Exists(@"C:\Program Files (x86)\diffpdfc\diffpdfc.exe"))
                		{
                		   	if(Directory.Exists(reportDirectory))
                		   	{
                		   		if(!Directory.EnumerateFileSystemEntries(reportDirectory).IsEmpty())
                		   		{
                		   			if(File.Exists("diffpdfcoutput.txt"))
                		   			   {
                		   			   	mail.Attachments.Add(new Attachment("labels.zip"));
                		   			   	File.Delete("labels.zip");
										mail.Attachments.Add(new Attachment("reports.zip"));
										File.Delete("reports.zip");
										mail.Attachments.Add(new Attachment("diffpdfcoutput.txt"));
										File.Delete("diffpdfcoutput.txt");
										mail.Body = mail.Body + "Attached are the labels that the smoketest produced, the comparison reports that show the difference between those labels and the known good labels. If there are any issues, please review the attached diffpdfcoutput.txt for errors that were produced during the automated comparison process.";
                		   			   	Report.Log(ReportLevel.Info, "Email sent with labels.zip, reports.zip, and diffpdfcoutput.txt.", "\n");
                		   				}
                		   			   
                		   			else
                		   			{
                		   				mail.Attachments.Add(new Attachment("labels.zip"));
                		   				File.Delete("labels.zip");
										mail.Attachments.Add(new Attachment("reports.zip"));
										File.Delete("reports.zip");
										mail.Body = mail.Body + "Attached are the labels that the smoketest produced, the comparison reports that show the difference between those labels and the known good labels. There was no output from the automated comparison process.";
                		   			   	Report.Log(ReportLevel.Info, "Email sent with labels.zip and reports.zip", "\n");
                		   			}
                		   		}
                		   	}
                		   	if((!Directory.Exists(reportDirectory)) | (Directory.EnumerateFileSystemEntries(reportDirectory).IsEmpty()))
                		   	{
                		 	  	if(File.Exists("diffpdfcoutput.txt"))
                		 	  	{
                		 	  		mail.Attachments.Add(new Attachment("labels.zip"));
                		 	  		File.Delete("labels.zip");
									mail.Attachments.Add(new Attachment("diffpdfcoutput.txt"));
									File.Delete("diffpdfcoutput.txt");
									mail.Body = mail.Body + "Attached are the labels that the smoketest produced. The comparison reports (pdfs that show the difference between those labels and the known good labels) were not produced. Please review the attached diffpdfcoutput.txt for errors that were produced during the automated comparison process.";
                		 	  	     Report.Log(ReportLevel.Info, "Email sent with labels.zip and diffpdfcoutput.txt.", "\n");
                		 	  	}
                		 	  	else
                		 	  	{
                		 	  		mail.Attachments.Add(new Attachment("labels.zip"));
                		 	  		File.Delete("labels.zip");
									mail.Body = mail.Body + "Attached are the labels that the smoketest produced. The comparison reports (pdfs that show the difference between those labels and the known good labels) were not produced. Diffpdfc (which was installed on the computer the smoketest was ran on) did not produce any output. Please review the ranorex logs to see what happened.";
									Report.Log(ReportLevel.Info, "Email sent with labels.zip", "\n");
                		 	  	}
                			}
                		}
                		else
                		{
							mail.Attachments.Add(new Attachment("labels.zip"));
							File.Delete("labels.zip");
							mail.Body = mail.Body + "Attached are the labels that the smoketest produced. Diffpdfc was not installed on the computer the smoketest was ran on, please manually verify that the labels printed correctly.";
	               			Report.Log(ReportLevel.Info, "Email sent with labels.zip.", "\n");
                		}
                   	}
                 }
                if((!Directory.Exists(labelDirectory)) | (Directory.EnumerateFileSystemEntries(labelDirectory).IsEmpty()))
                {
              	 	 mail.Body = mail.Body + "No shipping labels were created during this run of the smoketest. If this was not expected, please check all ranorex and shipworks logs to understand why this occurred.";
	            }
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
