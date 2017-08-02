using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
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

        private const string EndpointBase = "https://merchant-api.jet.com/api";
        private readonly string tokenEndpoint = $"{EndpointBase}/token";
        private readonly string orderEndpoint = $"{EndpointBase}/orders";
        private readonly string productEndpoint = $"{EndpointBase}/merchant-skus";

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public JetWebClient(IHttpRequestSubmitterFactory submitterFactory, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.submitterFactory = submitterFactory;
            this.apiLogEntryFactory = apiLogEntryFactory;
        }

        /// <summary>
        /// Get Token
        /// </summary>
        public GenericResult<string> GetToken(string username, string password)
        {
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
        public GenericResult<IEnumerable<JetOrderDetailsResult>> GetOrders()
        {
            IHttpRequestSubmitter submitter = submitterFactory.GetHttpVariableRequestSubmitter();
            string getOrdersEndpoints = $"{orderEndpoint}/ready";
            submitter.Uri = new Uri(getOrdersEndpoints);

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.Jet, "GetOrders");
            apiLogEntry.LogRequest(submitter);

            try
            {
                IHttpResponseReader httpResponseReader = submitter.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                JetOrderResponse orders = JsonConvert.DeserializeObject<JetOrderResponse>(result, jsonSerializerSettings);

                IEnumerable<JetOrderDetailsResult> ordersWithDetails = orders.OrderUrls.Select(GetOrderDetails);

                return GenericResult.FromSuccess(ordersWithDetails);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                return GenericResult.FromError<IEnumerable<JetOrderDetailsResult>>("Error communicating with Jet. Failed to get orders.");
            }
        }

        /// <summary>
        /// Gets jet product details for the given item
        /// </summary>
        public GenericResult<JetProduct> GetProduct(JetOrderItem item)
        {
            IHttpRequestSubmitter submitter = submitterFactory.GetHttpVariableRequestSubmitter();

            submitter.Uri = new Uri($"{productEndpoint}/{item.MerchantSku}");

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.Jet, "GetProduct");
            apiLogEntry.LogRequest(submitter);

            try
            {
                IHttpResponseReader httpResponseReader = submitter.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                JetProduct product = JsonConvert.DeserializeObject<JetProduct>(result, jsonSerializerSettings);

                return GenericResult.FromSuccess(product);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                return GenericResult.FromError<JetProduct>("Error communicating with Jet. Failed to get product.");
            }
        }

        /// <summary>
        /// Acknowledges the order will be fulfilled by the seller
        /// </summary>
        public void Acknowledge(JetOrderEntity order)
        {
            JetAcknowledgementRequest acknowledgementRequest = new JetAcknowledgementRequest
            {
                OrderItems = order.OrderItems.Cast<JetOrderItemEntity>()
                    .Select(i => new JetAcknowledgementOrderItem {OrderItemId = i.MerchantSku}).ToList()
            };

            IHttpRequestSubmitter submitter =
                submitterFactory.GetHttpTextPostRequestSubmitter(JsonConvert.SerializeObject(acknowledgementRequest),
                    "application/json");

            string acknowledgeEndpoint = $"{orderEndpoint}/{order.MerchantOrderId}/acknowledge";
            submitter.Uri = new Uri(acknowledgeEndpoint);

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.Jet, "Acknowledge");
            apiLogEntry.LogRequest(submitter);

            try
            {
                IHttpResponseReader httpResponseReader = submitter.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new JetException(
                    $"Error communicating with Jet. Failed to acknowledge order {order.MerchantOrderId}");
            }
        }

        /// <summary>
        /// Gets the order details for the given order url
        /// </summary>
        private JetOrderDetailsResult GetOrderDetails(string orderUrl)
        {
            IHttpRequestSubmitter submitter = submitterFactory.GetHttpVariableRequestSubmitter();

            submitter.Uri = new Uri($"{EndpointBase}{orderUrl}");

            IApiLogEntry apiLogEntry = apiLogEntryFactory(ApiLogSource.Jet, "GetOrderDetails");
            apiLogEntry.LogRequest(submitter);

            try
            {
                IHttpResponseReader httpResponseReader = submitter.GetResponse();
                string result = httpResponseReader.ReadResult();
                apiLogEntry.LogResponse(result, "json");

                return JsonConvert.DeserializeObject<JetOrderDetailsResult>(result, jsonSerializerSettings);
            }
            catch (Exception ex)
            {
                apiLogEntry.LogResponse(ex);
                throw new JetException($"Error communicating with Jet. Failed to get order details at url {orderUrl}.");
            }
        }
    }
}