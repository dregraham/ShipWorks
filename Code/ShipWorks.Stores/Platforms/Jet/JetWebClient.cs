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
        private readonly string orderEndpointPath = $"/orders";
        private readonly string productEndpointPath = $"/merchant-skus";

        private readonly JetRequest jetRequest;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetWebClient(IHttpRequestSubmitterFactory submitterFactory, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory, IEncryptionProviderFactory encryptionProviderFactory)
        {
            jetRequest = new JetRequest(submitterFactory, apiLogEntryFactory, encryptionProviderFactory, new LruCache<string, string>(50, TimeSpan.FromHours(9)));
        }

        /// <summary>
        /// Get Token
        /// </summary>
        public GenericResult<string> GetToken(string username, string password)
        {
            return jetRequest.GetToken(username, password);
        }

        /// <summary>
        /// Get jet orders, with order details, that have a status of "ready"
        /// </summary>
        public GenericResult<JetOrderResponse> GetOrders(JetStoreEntity store)
        {
            return jetRequest.ProcessRequest<JetOrderResponse>("GetOrders", $"{orderEndpointPath}/ready", HttpVerb.Get,
                store);
        }

        /// <summary>
        /// Gets jet product details for the given item
        /// </summary>
        public GenericResult<JetProduct> GetProduct(JetOrderItem item, JetStoreEntity store)
        {
            return jetRequest.ProcessRequest<JetProduct>("GetProduct", $"{productEndpointPath}/{item.MerchantSku}", HttpVerb.Get, store);
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

            IHttpRequestSubmitter submitter = jetRequest.submitterFactory.GetHttpTextPostRequestSubmitter(JsonConvert.SerializeObject(acknowledgementRequest),
                    "application/json");

            string acknowledgeEndpoint = $"{orderEndpointPath}/{order.MerchantOrderId}/acknowledge";
            submitter.Uri = new Uri(acknowledgeEndpoint);
            submitter.Verb = HttpVerb.Put;

            jetRequest.ProcessRequest("AcknowledgeOrder", store, submitter);
        }

        /// <summary>
        /// Gets the order details for the given order url
        /// </summary>
        public GenericResult<JetOrderDetailsResult> GetOrderDetails(string orderUrl, JetStoreEntity store)
        {
            return jetRequest.ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", orderUrl, HttpVerb.Get, store);
        }
    }
}