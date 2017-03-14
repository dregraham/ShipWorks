using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Interapptive.Shared.Net;
using Rebex.Mail;

namespace ShipWorks.ApplicationCore.Logging
{
    class NullApiLogEntry:IApiLogEntry
    {
        public ApiLogEncryption Encryption { get; set; }
        public void LogRequest(string xml)
        {

        }

        public void LogRequest(XmlDocument xmlDocument)
        {

        }

        public void LogRequest(string text, string fileExtension)
        {

        }

        public void LogRequest(HttpRequestSubmitter request)
        {

        }

        public void LogRequest(IHttpRequestSubmitter request)
        {

        }

        public void LogRequest(MailMessage mailMessage)
        {

        }

        public void LogResponse(string xml)
        {

        }

        public void LogResponse(string text, string fileExtension)
        {

        }

        public void LogResponse(XmlDocument xmlDocument)
        {

        }

        public void LogResponse(Exception ex)
        {

        }

        public void LogRequestSupplement(string xml, string supplementName)
        {

        }

        public void LogRequestSupplement(FileInfo fileInfo, string supplementName)
        {

        }

        public void LogRequestSupplement(byte[] supplementData, string supplementName, string extension)
        {

        }

        public void LogResponseSupplement(string xml, string supplementName)
        {

        }
    }
}
