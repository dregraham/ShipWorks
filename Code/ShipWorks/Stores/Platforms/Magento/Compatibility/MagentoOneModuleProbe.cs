using System;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Magento.Compatibility
{
    /// <summary>
    /// Check to see if a specific store is compatible with Magento One Module
    /// </summary>
    public class MagentoOneModuleProbe : IMagentoProbe
    {
        /// <summary>
        /// Check to see if the store is compatible with Magento 1
        /// </summary>
        public GenericResult<Uri> FindCompatibleUrl(MagentoStoreEntity store)
        {
            // First try just the given url
            GenericResult<Uri> result = ProbeUrl(store.ModuleUrl);

            if (!result.Success)
            {
                // If the given url doesnt work try adding shipworks3.php
                result = ProbeUrl($"{store.ModuleUrl.TrimEnd('/')}/shipworks3.php");
            }

            return result;
        }

        /// <summary>
        /// Probe the given url to see if it is a ShipWorks Magento Module
        /// </summary>
        private GenericResult<Uri> ProbeUrl(string url)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter {Verb = HttpVerb.Get};

            try
            {
                request.Uri = new Uri(url);
                using (IHttpResponseReader response = request.GetResponse())
                {
                    string resultXml = response.ReadResult(Encoding.UTF8);
                    resultXml = XmlUtility.StripInvalidXmlCharacters(resultXml);
                    GenericModuleResponse genericResponse = new GenericModuleResponse(resultXml);

                    string result = XPathUtility.Evaluate(genericResponse.XPath, "//ShipWorks", string.Empty);

                    if (result == string.Empty)
                    {
                        return GenericResult.FromError("ShipWorks could not find a compatible module at the given url.", request.Uri);
                    }
                }
            }
            catch (Exception)
            {
                return GenericResult.FromError("Exception occurred while attempting to connect to ShipWorks Module.", request.Uri);
            }

            return GenericResult.FromSuccess(request.Uri);
        }
    }
}