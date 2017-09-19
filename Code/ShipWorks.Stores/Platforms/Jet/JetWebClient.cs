using System;
using System.Linq;
using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
            MissingMemberHandling = MissingMemberHandling.Ignore,
            // manually format the date with an offset of -00:00 because the jet api requires it
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffff-00:00"
        };

        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly IJetAuthenticatedRequest jetAuthenticatedRequest;
        private readonly IJetShipmentRequestFactory jetShipmentRequestFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetWebClient(IHttpRequestSubmitterFactory submitterFactory,
            IJetAuthenticatedRequest jetAuthenticatedRequest,
            IJetShipmentRequestFactory jetShipmentRequestFactory)
        {
            this.submitterFactory = submitterFactory;
            this.jetAuthenticatedRequest = jetAuthenticatedRequest;
            this.jetShipmentRequestFactory = jetShipmentRequestFactory;
        }

        /// <summary>
        /// Get jet orders, with order details, that have a status of "ready"
        /// </summary>
        public GenericResult<JetOrderResponse> GetOrders(IJetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri($"{orderEndpointPath}/ready");
            request.Verb = HttpVerb.Get;

            return jetAuthenticatedRequest.Submit<JetOrderResponse>("GetOrders", request, store);
        }

        /// <summary>
        /// Gets jet product details for the given item
        /// </summary>
        public GenericResult<JetProduct> GetProduct(JetOrderItem item, IJetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri($"{productEndpointPath}/{item.MerchantSku}");
            request.Verb = HttpVerb.Get;

            return jetAuthenticatedRequest.Submit<JetProduct>("GetProduct", request, store);
        }

        /// <summary>
        /// Acknowledges the order will be fulfilled by the seller
        /// </summary>
        public void Acknowledge(JetOrderDetailsResult order, IJetStoreEntity store)
        {
            JetAcknowledgementRequest jetAcknowledgment = new JetAcknowledgementRequest
            {
                OrderItems = order.OrderItems
                    .Select(i => new JetAcknowledgementOrderItem { OrderItemId = i.OrderItemId }).ToList()
            };

            IHttpRequestSubmitter submitter = submitterFactory.GetHttpTextPostRequestSubmitter(JsonConvert.SerializeObject(jetAcknowledgment, jsonSerializerSettings),
                "application/json");

            submitter.Uri = new Uri($"{orderEndpointPath}/{order.MerchantOrderId}/acknowledge");
            submitter.Verb = HttpVerb.Put;
            submitter.AllowHttpStatusCodes(HttpStatusCode.NoContent);

            jetAuthenticatedRequest.Submit<string>("AcknowledgeOrder", submitter, store);
        }

        /// <summary>
        /// Gets the order details for the given order url
        /// </summary>
        public GenericResult<JetOrderDetailsResult> GetOrderDetails(string orderUrl, IJetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri(EndpointBase + orderUrl);
            request.Verb = HttpVerb.Get;

            return jetAuthenticatedRequest.Submit<JetOrderDetailsResult>("GetOrderDetails", request, store);
        }

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        public void UploadShipmentDetails(string merchantOrderId, ShipmentEntity shipment, IJetStoreEntity store)
        {
            JetShipmentRequest shipmentRequest = jetShipmentRequestFactory.Create(shipment);
            IHttpRequestSubmitter submitter = submitterFactory.GetHttpTextPostRequestSubmitter(JsonConvert.SerializeObject(shipmentRequest, jsonSerializerSettings),
                "application/json");

            submitter.Uri = new Uri($"{orderEndpointPath}/{merchantOrderId}/shipped");
            submitter.Verb = HttpVerb.Put;
            submitter.AllowHttpStatusCodes(HttpStatusCode.BadRequest, HttpStatusCode.NoContent);

            GenericResult<JetShipResponse> result = jetAuthenticatedRequest.Submit<JetShipResponse>("UploadShipmentDetails", submitter, store);

            if (result.Failure)
            {
                throw new JetException(result.Message);
            }
            if (result.Value?.Errors?.Any() ?? false)
            {
                throw new JetException($"Error message when uploading shipment details to Jet.com: \r\n\r\n{result.Value.Errors.First()}");
            }
        }
    }
}