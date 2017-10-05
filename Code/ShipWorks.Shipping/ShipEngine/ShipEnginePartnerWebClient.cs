using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client to the ShipEngine Partner API
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipEnginePartnerWebClient : IShipEnginePartnerWebClient
    {
        private const string CreateAccountUrl = "https://api.shipengine.com/v1/partners/accounts";
        private const string CreateApiKeyUrl = "https://api.shipengine.com/v1/partners/accounts/{0}/api_keys";

        private readonly IHttpRequestSubmitterFactory requestFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEnginePartnerWebClient(IHttpRequestSubmitterFactory requestFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory,
            Func<Type, ILog> logFactory)
        {
            this.requestFactory = requestFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
            log = logFactory(typeof(ShipEnginePartnerWebClient));
        }

        /// <summary>
        /// Creates a new ShipEngine account and returns the account ID
        /// </summary>
        public async Task<string> CreateNewAccount(string partnerApiKey)
        {
            return await SendPartnerRequest(partnerApiKey, CreateAccountUrl, string.Empty, "CreateNewAccount", "account_id");
        }

        /// <summary>
        /// Gets an ApiKey from the ShipEngine API
        /// </summary>
        public async Task<string> GetApiKey(string partnerApiKey, string shipEngineAccountId)
        {
            string requestUrl = string.Format(CreateApiKeyUrl, shipEngineAccountId);
            string postText = "{\"description\": \"ShipWorks Access Key\"}";

            return await SendPartnerRequest(partnerApiKey, requestUrl, postText, "GetApiKey", "encrypted_api_key");
        }

        /// <summary>
        /// Send actual request to ShipEngine
        /// </summary>
        private async Task<string> SendPartnerRequest(string partnerApiKey, string requestUrl, string postText, string logName, string responseFieldName)
        {
            IHttpRequestSubmitter request = requestFactory.GetHttpTextPostRequestSubmitter(postText, "application/json");
            request.Headers.Add("api-key", partnerApiKey);
            request.Uri = new Uri(requestUrl);

            JToken responseToken = null;

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ShipEngine, logName);
            apiLogEntry.LogRequest(request);

            try
            {
                IHttpResponseReader response = await request.GetResponseAsync();
                string result = response.ReadResult();

                apiLogEntry.LogResponse(result);
                responseToken = JObject.Parse(result)[responseFieldName];
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
                log.Error($"Unable to get {responseFieldName} from ShipEngine");
                throw new ShipEngineException($"Unable to get {responseFieldName} from ShipEngine.");
            }

            return responseToken.ToString();
        }
    }
}
