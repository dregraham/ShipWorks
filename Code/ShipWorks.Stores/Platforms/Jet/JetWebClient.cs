﻿using System;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
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
        private const string EndpointBase = "https://merchant-api.jet.com/api";
        private readonly string orderEndpointPath = $"{EndpointBase}/orders";
        private readonly string productEndpointPath = $"{EndpointBase}/merchant-skus";

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly IJetAuthenticatedRequest jetAuthenticatedRequest;
        private readonly IJetShipmentRequestFactory jetShipmentRequestFactory;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetWebClient(IHttpRequestSubmitterFactory submitterFactory,
            IJetAuthenticatedRequest jetAuthenticatedRequest,
            IJetShipmentRequestFactory jetShipmentRequestFactory,
            IShippingManager shippingManager)
        {
            this.submitterFactory = submitterFactory;
            this.jetAuthenticatedRequest = jetAuthenticatedRequest;
            this.jetShipmentRequestFactory = jetShipmentRequestFactory;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Get jet orders, with order details, that have a status of "ready"
        /// </summary>
        public GenericResult<JetOrderResponse> GetOrders(JetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri($"{orderEndpointPath}/ready");
            request.Verb = HttpVerb.Get;

            return jetAuthenticatedRequest.ProcessRequest<JetOrderResponse>("GetOrders", request, store);
        }

        /// <summary>
        /// Gets jet product details for the given item
        /// </summary>
        public GenericResult<JetProduct> GetProduct(JetOrderItem item, JetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri($"{productEndpointPath}/{item.MerchantSku}");
            request.Verb = HttpVerb.Get;

            return jetAuthenticatedRequest.ProcessRequest<JetProduct>("GetProduct", request, store);
        }

        /// <summary>
        /// Acknowledges the order will be fulfilled by the seller
        /// </summary>
        public void Acknowledge(JetOrderEntity order, JetStoreEntity store)
        {
            JetAcknowledgementRequest jetAcknowledgement = new JetAcknowledgementRequest
            {
                OrderItems = order.OrderItems.Cast<JetOrderItemEntity>()
                    .Select(i => new JetAcknowledgementOrderItem { OrderItemId = i.JetOrderItemID }).ToList()
            };

            IHttpRequestSubmitter submitter = submitterFactory.GetHttpTextPostRequestSubmitter(JsonConvert.SerializeObject(jetAcknowledgement, jsonSerializerSettings),
                "application/json");

            submitter.Uri = new Uri($"{orderEndpointPath}/{order.MerchantOrderId}/acknowledge");
            submitter.Verb = HttpVerb.Put;

            jetAuthenticatedRequest.ProcessRequest<string>("AcknowledgeOrder", submitter, store);
        }

        /// <summary>
        /// Gets the order details for the given order url
        /// </summary>
        public GenericResult<JetOrderDetailsResult> GetOrderDetails(string orderUrl, JetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri(EndpointBase + orderUrl);
            request.Verb = HttpVerb.Get;

            return jetAuthenticatedRequest.ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", request, store);
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            shippingManager.EnsureShipmentIsLoadedWithOrder(shipment);

            JetOrderEntity order = (JetOrderEntity) shipment.Order;
            string merchantOrderId = order.MerchantOrderId;

            JetShipmentRequest shipmentRequest = jetShipmentRequestFactory.Create(shipment);
            IHttpRequestSubmitter submitter = submitterFactory.GetHttpTextPostRequestSubmitter(JsonConvert.SerializeObject(shipmentRequest, jsonSerializerSettings),
                "application/json");

            submitter.Uri = new Uri($"{orderEndpointPath}/{merchantOrderId}/shipped");
            submitter.Verb = HttpVerb.Put;

            jetAuthenticatedRequest.ProcessRequest<object>("UpdateShipmentDetails", submitter, (JetStoreEntity) order.Store);
        }
    }
}