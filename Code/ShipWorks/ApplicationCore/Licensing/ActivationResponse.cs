using System.Xml;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Contains customer activation response
    /// </summary>
    public class ActivationResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="response"></param>
        public ActivationResponse(XmlDocument response)
        {

            XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(response);
            xpath.Namespaces.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
            xpath.Namespaces.AddNamespace("a", "http://schemas.datacontract.org/2004/07/Sdc.Server.ShipWorksNet.Protocol.CustomerLicenseInfo");
            xpath.Namespaces.AddNamespace("i", "http://www.w3.org/2001/XMLSchema-instance");
            xpath.Namespaces.AddNamespace("", "http://stamps.com/xml/namespace/2015/09/shipworks/activationv1");

            Key = XPathUtility.Evaluate(xpath, "//a:CustomerLicenseKey", "");
        }

        /// <summary>
        /// The customer Key
        /// </summary>
        public string Key { get; set; }
    }
}