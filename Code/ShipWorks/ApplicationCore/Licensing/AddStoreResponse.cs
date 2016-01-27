using System.Xml;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Response from TangoWebClient.AddStore 
    /// </summary>
    public class AddStoreResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddStoreResponse(XmlDocument xmlResponse)
        {
            XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(xmlResponse);
            Error = XPathUtility.Evaluate(xpath, "//Error", "");
            Key = XPathUtility.Evaluate(xpath, "//LicenseKey", "");

            Success = string.IsNullOrEmpty(Error);
        }

        /// <summary>
        /// License Key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Success!
        /// </summary>
        public bool Success { get; }
    }
}