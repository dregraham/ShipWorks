using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Client to the ShipEngine Partner API
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipEnginePartnerClient : IShipEnginePartnerClient
    {
        private const string CreateAccountUrl = "https://api.shipengine.com/v1/partners/accounts";
        private const string CreateApiKeyUrl = "https://api.shipengine.com/v1/partners/accounts/{0}/api_keys";

        private readonly IHttpRequestSubmitterFactory requestFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEnginePartnerClient(IHttpRequestSubmitterFactory requestFactory,
            Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.requestFactory = requestFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
        }

        /// <summary>
        /// Creates a new ShipEngine account and returns the account ID
        /// </summary>
        public string CreateNewAccount(string partnerApiKey)
        {
            IHttpRequestSubmitter createAccountRequest = requestFactory.GetHttpTextPostRequestSubmitter(string.Empty, "application/json");
            createAccountRequest.Headers.Add("api-key", partnerApiKey);
            createAccountRequest.Uri = new Uri(CreateAccountUrl);
            IHttpResponseReader response;
            JToken accountToken;

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ShipEngine, "CreateNewAccount");
            apiLogEntry.LogRequest(createAccountRequest);

            try
            {
                response = createAccountRequest.GetResponse();
                string result = response.ReadResult();

                apiLogEntry.LogResponse(result);
                accountToken = JObject.Parse(result)["account_id"];
            }
            catch(JsonReaderException ex)
            {
                throw new ShipEngineException("Error reading response from ShipEngine.", ex);
            }
            catch (WebException ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new ShipEngineException("Error communicating with ShipEngine.", ex);
            }

            if (accountToken == null)
            {
                throw new ShipEngineException("Unable to retrieve an account id from ShipEngine.");
            }

            return accountToken.ToString();
        }

        /// <summary>
        /// Gets an ApiKey from the ShipEngine API
        /// </summary>
        public string GetApiKey(string partnerApiKey, string shipEngineAccountId)
        {
            string descriptionJson = "{\"description\": \"ShipWorks Access Key\"}";
            string createApiKeyUrl = string.Format(CreateApiKeyUrl, shipEngineAccountId);

            IHttpRequestSubmitter createAccountRequest = requestFactory.GetHttpTextPostRequestSubmitter(descriptionJson, "application/json");
            createAccountRequest.Headers.Add("api-key", partnerApiKey);
            createAccountRequest.Uri = new Uri(createApiKeyUrl);

            IHttpResponseReader response;
            JToken apiKeyToken;

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.ShipEngine, "CreateNewAccount");
            apiLogEntry.LogRequest(createAccountRequest);

            try
            {
                response = createAccountRequest.GetResponse();
                string result = response.ReadResult();

                apiLogEntry.LogResponse(result);
                apiKeyToken = JObject.Parse(result)["encrypted_api_key"];
            }
            catch (JsonReaderException ex)
            {
                throw new ShipEngineException("Error reading response from ShipEngine.", ex);
            }
            catch (WebException ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new ShipEngineException("Error communicating with ShipEngine.", ex);
            }

            if (apiKeyToken == null)
            {
                throw new ShipEngineException("Unable to retrieve an api key from ShipEngine.");
            }

            return apiKeyToken.ToString();
        }
    }
}
