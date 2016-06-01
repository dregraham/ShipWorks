using System;
using System.IO;
using System.Xml;
using Rebex.Mail;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Interface for logging request\response from the various API's that ShipWorks
    /// works with.
    /// </summary>
    public interface IApiLogEntry
    {
        /// <summary>
        /// Contols how the log output will be encrypted
        /// </summary>
        ApiLogEncryption Encryption { get; set; }

        /// <summary>
        /// Log the ShipWorks request content.
        /// </summary>
        void LogRequest(string xml);

        /// <summary>
        /// Log the request XmlDocument content
        /// </summary>
        void LogRequest(XmlDocument xmlDocument);

        /// <summary>
        /// Log the text content request
        /// </summary>
        void LogRequest(string text, string fileExtension);

        /// <summary>
        /// Log the contents of the given HttpRequestSubmitter
        /// </summary>
        void LogRequest(HttpRequestSubmitter request);

        /// <summary>
        /// Log the request as the given mail message
        /// </summary>
        void LogRequest(MailMessage mailMessage);

        /// <summary>
        /// Log the API's response to ShipWorks
        /// </summary>
        void LogResponse(string xml);

        /// <summary>
        /// Log the API's response to ShipWorks
        /// </summary>
        void LogResponse(string text, string fileExtension);

        /// <summary>
        /// Log the response XmlDocument
        /// </summary>
        void LogResponse(XmlDocument xmlDocument);

        /// <summary>
        /// Log the response exception
        /// </summary>
        void LogResponse(Exception ex);

        /// <summary>
        /// Log supplemental request request data
        /// </summary>
        void LogRequestSupplement(string xml, string supplementName);

        /// <summary>
        /// Log supplemental request request data
        /// </summary>
        void LogRequestSupplement(FileInfo fileInfo, string supplementName);

        /// <summary>
        /// Log supplement binary request data
        /// </summary>
        void LogRequestSupplement(byte[] supplementData, string supplementName, string extension);

        /// <summary>
        /// Log supplemental request request data
        /// </summary>
        void LogResponseSupplement(string xml, string supplementName);
    }
}