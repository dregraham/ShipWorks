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
        public ActivationResponse(XmlDocument response)
        {
            MethodConditions.EnsureArgumentIsNotNull(response, nameof(response));

            XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(response);
            xpath.Namespaces.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
            xpath.Namespaces.AddNamespace("a", "http://schemas.datacontract.org/2004/07/Sdc.Server.ShipWorksNet.Protocol.CustomerLicenseInfo");
            xpath.Namespaces.AddNamespace("i", "http://www.w3.org/2001/XMLSchema-instance");
            xpath.Namespaces.AddNamespace("", "http://stamps.com/xml/namespace/2015/09/shipworks/activationv1");

            Key = XPathUtility.Evaluate(xpath, "//a:CustomerLicenseKey", string.Empty);
            StampsUsername = XPathUtility.Evaluate(xpath, "//a:StampsUserName", string.Empty);
            AssociatedStampsUserName = XPathUtility.Evaluate(xpath, "//a:AssociatedStampsUserName", string.Empty);
        }

        /// <summary>
        /// The customer Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The Stamps.com username.
        /// </summary>
        public string StampsUsername { get; set; }

        /// <summary>
        /// The associated stamps username. If empty, do not create new Stamps account in ShipWorks
        /// </summary>
        public string AssociatedStampsUserName { get; set; }
    }
}