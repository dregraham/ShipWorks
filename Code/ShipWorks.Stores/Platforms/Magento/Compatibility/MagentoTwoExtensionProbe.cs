using System;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.Compatibility
{
    /// <summary>
    /// Check to see if a specific store is compatible with Magento Two Extension
    /// </summary>
    [KeyedComponent(typeof(IMagentoProbe), MagentoVersion.MagentoTwo)]
    public class MagentoTwoExtensionProbe : IMagentoProbe
    {
        /// <summary>
        /// Check to see if the store is compatible with Magento 1
        /// </summary>
        public GenericResult<Uri> FindCompatibleUrl(MagentoStoreEntity store) => 
            ProbeUrl(store.ModuleUrl);

        /// <summary>
        /// Probe the given url to see if it is a ShipWorks Magento Two Extension
        /// </summary>
        private GenericResult<Uri> ProbeUrl(string url)
        {
            MagentoHttpRequestSubmitter request = new MagentoHttpRequestSubmitter() {Verb = HttpVerb.Get};

            try
            {
                request.Uri = new Uri($"{url.TrimEnd('/')}/rest/V1/shipworks");
                request.AllowAutoRedirect = false;
                using (IHttpResponseReader response = request.GetResponse())
                {
                    string resultXml = response.ReadResult(Encoding.UTF8);
                    string result = XmlUtility.StripInvalidXmlCharacters(resultXml);

                    if (result != "\"This is your ShipWorks module URL\"")
                    {
                        return GenericResult.FromError("ShipWorks could not find a compatible module at the given url.", request.Uri);
                    }
                }
            }
            catch (Exception)
            {
                return GenericResult.FromError("Exception occurred while attempting to connect to ShipWorks Module.", request.Uri);
            }

            return GenericResult.FromSuccess(new Uri(url.TrimEnd('/')));
        }
    }
}