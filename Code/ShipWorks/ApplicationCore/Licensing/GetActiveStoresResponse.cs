using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Reads the response from GetActiveStores
    /// </summary>
    public class GetActiveStoresResponse
    {
        List<ActiveStore> activeStores;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetActiveStoresResponse"/> class.
        /// </summary>
        public GetActiveStoresResponse(XmlDocument xmlResponse)
        {
            activeStores = new List<ActiveStore>();

            XPathNamespaceNavigator navigator = new XPathNamespaceNavigator(xmlResponse);

            foreach (XPathNavigator tempXpath in navigator.Select("//ActiveStore"))
            {
                XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(tempXpath, navigator.Namespaces);

                ActiveStore activeStore = new ActiveStore()
                {
                    Name = XPathUtility.Evaluate(xpath, "storeInfo", string.Empty),
                    StoreLicenseKey = XPathUtility.Evaluate(xpath, "license", string.Empty),
                };

                activeStores.Add(activeStore);
            }
        }

        /// <summary>
        /// Gets the active stores.
        /// </summary>
        public List<ActiveStore> ActiveStores => activeStores;
    }
}
