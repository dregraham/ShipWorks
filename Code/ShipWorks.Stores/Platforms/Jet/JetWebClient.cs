using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;
using ShipWorks.Stores.Platforms.Jet.DTO.Requests;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Communicates with the Jet Rest API
    /// </summary>
    /// <seealso cref="IJetWebClient" />
    [Component]
    public class JetWebClient : IJetWebClient
    {
        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        private const string EndpointBase = "https://merchant-api.jet.com/api";
        private readonly string tokenEndpoint = $"{EndpointBase}/token";
        private readonly string orderEndpoint = $"{EndpointBase}/orders";
        private readonly string productEndpoint = $"{EndpointBase}/merchant-skus";

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        private readonly LruCache<string, string> tokenCache;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetWebClient(IHttpRequestSubmitterFactory submitterFactory, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.submitterFactory = submitterFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
            this.encryptionProviderFactory = encryptionProviderFactory;

            tokenCache = new LruCache<string, string>(50, TimeSpan.FromMinutes(50));
        }

        /// <summary>
        /// Get Token
        /// </summary>
        public GenericResult<string> GetToken(string username, string password)
        {
            if (tokenCache.Contains(username))
            {
                return GenericResult.FromSuccess(tokenCache[username]);
            }

            IHttpRequestSubmitter submitter = submitterFactory.GetHttpTextPostRequestSubmitter(
                $"{{\"user\": \"{username}\",\"pass\":\"{password}\"}}",
                "application/json");

            submitter.Uri = new Uri(tokenEndpoint);

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.Jet, "GetToken");
            apiLogEntry.LogRequest(submitter);

            try
            {
                IHttpResponseReader httpResponseReader = submitter.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                JetTokenResponse token = JsonConvert.DeserializeObject<JetTokenResponse>(result, jsonSerializerSettings);

                tokenCache[username] = token.Token;

                return GenericResult.FromSuccess(token.Token);
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.BadRequest)
            {
                apiLogEntry.LogResponse(ex);
                return GenericResult.FromError<string>("Invalid API User or Secret.");
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                return GenericResult.FromError<string>("Error communicating with Jet.");
            }
        }

        /// <summary>
        /// Get jet orders, with order details, that have a status of "ready"
        /// </summary>
        public GenericResult<JetOrderResponse> GetOrders(JetStoreEntity store)
        {
            return ProcessRequest<JetOrderResponse>("GetOrders", new Uri($"{orderEndpoint}/ready"), HttpVerb.Get,
                store);
        }

        /// <summary>
        /// Gets jet product details for the given item
        /// </summary>
        public GenericResult<JetProduct> GetProduct(JetOrderItem item, JetStoreEntity store)
        {
            return ProcessRequest<JetProduct>("GetProduct", new Uri($"{productEndpoint}/{item.MerchantSku}"), HttpVerb.Get, store);
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private GenericResult<T> ProcessRequest<T>(string action, Uri uri, HttpVerb method,JetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = uri;
            request.Verb = method;

            var result =  ProcessRequest(action, store, request);

            if (result.Failure)
            {
                return GenericResult.FromError<T>(result.Message);
            }

            return GenericResult.FromSuccess(JsonConvert.DeserializeObject<T>(result.Value, jsonSerializerSettings));
        }

        /// <summary>
        /// Process the request
        /// </summary>
        private GenericResult<string> ProcessRequest(string action, JetStoreEntity store, IHttpRequestSubmitter request)
        {
            AuthenticateRequest(store, request);

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.Jet, action);
            apiLogEntry.LogRequest(request);
            try
            {
                IHttpResponseReader httpResponseReader = request.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                return GenericResult.FromSuccess(result);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                return GenericResult.FromError<string>(ex.Message);
            }
        }


        /// <summary>
        /// Authenticate the request using the stores token
        /// </summary>
        private void AuthenticateRequest(JetStoreEntity store, IHttpRequestSubmitter request)
        {
            try
            {
                string password = encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.ApiUser)
                    .Decrypt(store.Secret);

                GenericResult<string> token = GetToken(store.ApiUser, password);

                if (token.Failure)
                {
                    throw new JetException($"Failed to get token: {token.Message}");
                }

                request.Headers.Add("Authorization", $"bearer {token.Value}");
            }
            catch (EncryptionException ex)
            {
                throw new JetException("Failed to decrypt the shared secret", ex);
            }
        }

        /// <summary>
        /// Acknowledges the order will be fulfilled by the seller
        /// </summary>
        public void Acknowledge(JetOrderEntity order, JetStoreEntity store)
        {
            JetAcknowledgementRequest acknowledgementRequest = new JetAcknowledgementRequest
            {
                OrderItems = order.OrderItems.Cast<JetOrderItemEntity>()
                    .Select(i => new JetAcknowledgementOrderItem {OrderItemId = i.JetOrderItemID}).ToList()
            };

            IHttpRequestSubmitter submitter =
                submitterFactory.GetHttpTextPostRequestSubmitter(JsonConvert.SerializeObject(acknowledgementRequest),
                    "application/json");

            string acknowledgeEndpoint = $"{orderEndpoint}/{order.MerchantOrderId}/acknowledge";
            submitter.Uri = new Uri(acknowledgeEndpoint);
            submitter.Verb = HttpVerb.Put;

            ProcessRequest("AcknowledgeOrder", store, submitter);
        }

        /// <summary>
        /// Gets the order details for the given order url
        /// </summary>
        public GenericResult<JetOrderDetailsResult> GetOrderDetails(string orderUrl, JetStoreEntity store)
        {
            return ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", new Uri($"{EndpointBase}{orderUrl}"), HttpVerb.Get, store);
        }
    }
}