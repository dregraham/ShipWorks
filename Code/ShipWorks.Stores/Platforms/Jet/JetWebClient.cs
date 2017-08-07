using System;
using System.Linq;
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
        private const string EndpointBase = "https://merchant-api.jet.com/api";
        private readonly string orderEndpointPath = $"{EndpointBase}/orders";
        private readonly string productEndpointPath = $"{EndpointBase}/merchant-skus";

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        
        private readonly IJsonRequest jsonRequest;
        private readonly IHttpRequestSubmitterFactory submitterFactory;
        private readonly IJetTokenRepository tokenRepo;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetWebClient(IJsonRequest request, IHttpRequestSubmitterFactory submitterFactory, IJetTokenRepository tokenRepo)
        {
            jsonRequest = request;
            this.submitterFactory = submitterFactory;
            this.tokenRepo = tokenRepo;
        }

        /// <summary>
        /// Get jet orders, with order details, that have a status of "ready"
        /// </summary>
        public GenericResult<JetOrderResponse> GetOrders(JetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri($"{orderEndpointPath}/ready");
            request.Verb = HttpVerb.Get;

            return ProcessRequest<JetOrderResponse>("GetOrders", request, store);
        }
        
        /// <summary>
        /// Gets jet product details for the given item
        /// </summary>
        public GenericResult<JetProduct> GetProduct(JetOrderItem item, JetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri($"{productEndpointPath}/{item.MerchantSku}");
            request.Verb = HttpVerb.Get;

            return ProcessRequest<JetProduct>("GetProduct", request, store);
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

            submitter.Uri = new Uri(EndpointBase + $"{orderEndpointPath}/{order.MerchantOrderId}/acknowledge");
            submitter.Verb = HttpVerb.Put;

            ProcessRequest<string>("AcknowledgeOrder", submitter, store);
        }

        /// <summary>
        /// Gets the order details for the given order url
        /// </summary>
        public GenericResult<JetOrderDetailsResult> GetOrderDetails(string orderUrl, JetStoreEntity store)
        {
            IHttpRequestSubmitter request = submitterFactory.GetHttpVariableRequestSubmitter();
            request.Uri = new Uri(EndpointBase + orderUrl);
            request.Verb = HttpVerb.Get;

            return ProcessRequest<JetOrderDetailsResult>("GetOrderDetails", request, store);
        }
        
        /// <summary>
        /// Process the request
        /// </summary>
        private GenericResult<T> ProcessRequest<T>(string action, IHttpRequestSubmitter request, JetStoreEntity store)
        {
            try
            {
                JetToken token = tokenRepo.GetToken(store);

                if (!token.IsValid)
                {
                    throw new JetException("Unable to obtain a valid token to authenticate request.");   
                }

                token.AttachTo(request);

                return GenericResult.FromSuccess(jsonRequest.ProcessRequest<T>(action, ApiLogSource.Jet, request));
            }
            catch (Exception e)
            {
                return GenericResult.FromError<T>(e.Message);
            }
        }

    }
}