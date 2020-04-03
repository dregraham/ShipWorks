using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client to the ShipEngine Partner API
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipEnginePartnerWebClient : IShipEnginePartnerWebClient
    {
        private const string CreateAccountUrl = "https://api.shipengine.com/v1/partners/accounts";
        private const string ProxyEndpoint = "shipEngine";

        private readonly IHttpRequestSubmitterFactory requestFactory;
        private readonly WebClientEnvironmentFactory webClientEnvironmentFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEnginePartnerWebClient(IHttpRequestSubmitterFactory requestFactory,
                                          WebClientEnvironmentFactory webClientEnvironmentFactory,
                                          Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
                                          Func<Type, ILog> logFactory)
        {
            this.requestFactory = requestFactory;
            this.webClientEnvironmentFactory = webClientEnvironmentFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
            log = logFactory(typeof(ShipEnginePartnerWebClient));
        }

        /// <summary>
        /// Creates a new ShipEngine account and returns the account ID
        /// </summary>
        public async Task<string> CreateNewAccount()
        {
            IHttpRequestSubmitter request = requestFactory.GetHttpTextPostRequestSubmitter(string.Empty, "application/json");
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
