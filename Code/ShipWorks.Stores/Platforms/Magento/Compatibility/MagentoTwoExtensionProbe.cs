﻿using System;
using System.Net;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Stores.Platforms.Magento.Compatibility
{
    /// <summary>
    /// Check to see if a specific store is compatible with Magento Two Extension
    /// </summary>
    [KeyedComponent(typeof(IMagentoProbe), MagentoVersion.MagentoTwo)]
    public class MagentoTwoExtensionProbe : IMagentoProbe
    {
        private readonly ILicenseService licenseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoTwoExtensionProbe(ILicenseService licenseService)
        {
            this.licenseService = licenseService;
        }

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

                // the hub is not compatible with the Magento 2 Module, force these users to use the REST API
                if (licenseService.IsHub)
                {
                    return GenericResult.FromError("ShipWorks could not find a compatible module at the given url.", request.Uri);
                }

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
            catch (WebException)
            {
                return GenericResult.FromError("Exception occurred while attempting to connect to ShipWorks Module.", request.Uri);
            }

            return GenericResult.FromSuccess(new Uri(url.TrimEnd('/')));
        }
    }
}