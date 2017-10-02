using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    [Component]
    public class ShipEnginePartnerClient : IShipEnginePartnerClient
    {
        private const string CreateAccountUrl = "https://api.shipengine.com/v1/partners/accounts";
        private const string CreateApiKeyUrl = "https://api.shipengine.com/v1/partners/accounts/{0}/api_keys";

        private readonly IHttpRequestSubmitterFactory requestFactory;

        public ShipEnginePartnerClient(IHttpRequestSubmitterFactory requestFactory)
        {
            this.requestFactory = requestFactory;
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

            try
            {
                response = createAccountRequest.GetResponse();
                string result = response.ReadResult();
                accountToken = JObject.Parse(result)["account_id"];
            }
            catch(Exception ex) when (ex is WebException || ex is JsonReaderException)
            {
                throw new ShipEngineException("Error reading response from ShipEngine.", ex);
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

            try
            {
                response = createAccountRequest.GetResponse();
                string result = response.ReadResult();
                apiKeyToken = JObject.Parse(result)["encrypted_api_key"];
            }
            catch (Exception ex) when (ex is WebException || ex is JsonReaderException)
            {
                throw new ShipEngineException("Error reading response from ShipEngine.", ex);
            }

            if (apiKeyToken == null)
            {
                throw new ShipEngineException("Unable to retrieve an api key from ShipEngine.");
            }

            return apiKeyToken.ToString();
        }
    }
}
