﻿using System.Xml;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Get InsureShip credentials from Tango
    /// </summary>
    [Component]
    public class TangoGetInsureShipCredentialsRequest : ITangoGetInsureShipCredentialsRequest
    {
        private readonly ITangoWebRequestClient webRequestClient;
        private readonly IHttpRequestSubmitterFactory requestSubmitterFactory;
        private readonly IInsureShipSettings settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoGetInsureShipCredentialsRequest(
            IHttpRequestSubmitterFactory requestSubmitterFactory,
            ITangoWebRequestClient webRequestClient,
            IInsureShipSettings settings)
        {
            this.settings = settings;
            this.requestSubmitterFactory = requestSubmitterFactory;
            this.webRequestClient = webRequestClient;
        }

        /// <summary>
        /// Populate the given store with InsureShip credentials
        /// </summary>
        public void PopulateCredentials(StoreEntity store)
        {
            var request = requestSubmitterFactory.GetHttpVariableRequestSubmitter();
            request.Variables.Add("action", "getinsureshipcredentials");
            request.Variables.Add("license", store.License);

            if (settings.UseTestServer)
            {
                request.AddVariable("insureshiptestcreds", "true");
            }

            webRequestClient.ProcessXmlRequest(request, "GetInsureShipCredentials", false)
                .Do(x => SetCredentialsOnStore(x, store));
        }

        /// <summary>
        /// Set the credentials on the store
        /// </summary>
        private void SetCredentialsOnStore(XmlDocument xmlDocument, StoreEntity store)
        {
            Functional.ParseLong(xmlDocument.SelectSingleNode("//ClientID")?.InnerText)
                .Do(x => store.InsureShipClientID = x);
            store.InsureShipApiKey = xmlDocument.SelectSingleNode("//ApiKey")?.InnerText;
        }
    }
}
