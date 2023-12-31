﻿using System;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client to the ShipEngine Partner API
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipEnginePartnerWebClient : IShipEnginePartnerWebClient
    {
        private const string CreateAccountUrl = "https://platform.shipengine.com/v1/partners/accounts";
        private const string ProxyEndpoint = "shipEngine";

        private readonly IHttpRequestSubmitterFactory requestFactory;
        private readonly WebClientEnvironmentFactory webClientEnvironmentFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly ITangoWebClient tangoWebClient;
        private readonly IDatabaseIdentifier databaseIdentifier;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public ShipEnginePartnerWebClient(IHttpRequestSubmitterFactory requestFactory,
                                          WebClientEnvironmentFactory webClientEnvironmentFactory,
                                          Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
                                          ITangoWebClient tangoWebClient,
                                          IDatabaseIdentifier databaseIdentifier,
                                          Func<Type, ILog> logFactory)
        {
            this.requestFactory = requestFactory;
            this.webClientEnvironmentFactory = webClientEnvironmentFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.tangoWebClient = tangoWebClient;
            this.databaseIdentifier = databaseIdentifier;
            log = logFactory(typeof(ShipEnginePartnerWebClient));
        }

        /// <summary>
        /// Creates a new ShipEngine account and returns the account ID
        /// </summary>
        public async Task<string> CreateNewAccount()
        {
            string customerId = tangoWebClient.GetTangoCustomerId();
            var createShipEngineAccount = new CreateShipEngineAccount();

            if (!string.IsNullOrEmpty(customerId))
            {
                createShipEngineAccount.ExternalAccountID = $"{customerId}-{databaseIdentifier.Get()}";
            }

            IHttpRequestSubmitter request = requestFactory.GetHttpTextPostRequestSubmitter(
                JsonConvert.SerializeObject(createShipEngineAccount,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), "application/json");
            request.Headers.Add("SW-originalRequestUrl", CreateAccountUrl);
            request.Uri = new Uri(webClientEnvironmentFactory.SelectedEnvironment.ProxyUrl + ProxyEndpoint);

            JToken responseToken = null;

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ShipEngine, "CreateNewAccount");
            apiLogEntry.LogRequest(request);

            try
            {
                IHttpResponseReader response = await request.GetResponseAsync().ConfigureAwait(false);
                string result = response.ReadResult();

                apiLogEntry.LogResponse(result);
                responseToken = JObject.Parse(result)["api_key"]["encrypted_api_key"];
            }
            catch (JsonReaderException ex)
            {
                log.Error(ex);
            }
            catch (WebException ex)
            {
                log.Error(ex);
                apiLogEntry.LogResponse(ex);
            }

            if (responseToken == null)
            {
                log.Error("Unable to get api key from ShipEngine");
                throw new ShipEngineException("Unable to get api key from ShipEngine.");
            }

            return responseToken.ToString();
        }
    }
}
