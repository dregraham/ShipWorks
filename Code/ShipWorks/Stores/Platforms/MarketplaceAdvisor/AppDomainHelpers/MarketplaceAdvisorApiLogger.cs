using System;
using System.IO;
using System.Xml;
using Interapptive.Shared.Net;
using Rebex.Mail;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers
{
    /// <summary>
    /// Wrap the ApiLogger in a marshalable class so logging can happen accross Appdomains
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the client tried to use the ApiLogEntry natively, the ShipWorks initialization process would start again
    /// since ShipWorks hasn't actually been initialized in the secondary app domain.
    /// </para>
    /// <para>
    /// This is only partially implemented because I wanted to get logging functionality to the MarketplaceAdvisor web service
    /// without having to modify the service itself.
    /// </para>
    /// </remarks>
    public class MarketplaceAdvisorApiLogger : MarshalByRefObject, IApiLogEntry
    {
        private ApiLogEntry logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action"></param>
        public MarketplaceAdvisorApiLogger(string action)
        {
            logger = new ApiLogEntry(ApiLogSource.MarketplaceAdvisor, action);
        }

        public ApiLogEncryption Encryption { get; set; }

        /// <summary>
        /// Log the request
        /// </summary>
        public void LogRequest(string xml)
        {
            logger.LogRequest(xml);
        }

        public void LogRequest(XmlDocument xmlDocument)
        {
            throw new NotImplementedException();
        }

        public void LogRequest(string text, string fileExtension)
        {
            logger.LogRequest(text, fileExtension);
        }

        public void LogRequest(HttpRequestSubmitter request)
        {
            throw new NotImplementedException();
        }

        public void LogRequest(IHttpRequestSubmitter request)
        {
            throw new NotImplementedException();
        }

        public void LogRequest(MailMessage mailMessage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Log the response
        /// </summary>
        public void LogResponse(string xml)
        {
            logger.LogResponse(xml);
        }

        public void LogResponse(string text, string fileExtension)
        {
            throw new NotImplementedException();
        }

        public void LogResponse(XmlDocument xmlDocument)
        {
            throw new NotImplementedException();
        }

        public void LogResponse(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void LogRequestSupplement(string xml, string supplementName)
        {
            throw new NotImplementedException();
        }

        public void LogRequestSupplement(FileInfo fileInfo, string supplementName)
        {
            throw new NotImplementedException();
        }

        public void LogRequestSupplement(byte[] supplementData, string supplementName, string extension)
        {
            throw new NotImplementedException();
        }

        public void LogResponseSupplement(string xml, string supplementName)
        {
            throw new NotImplementedException();
        }
    }
}