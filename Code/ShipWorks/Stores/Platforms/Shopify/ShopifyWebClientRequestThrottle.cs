using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using log4net;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Shopify implementation of request throttler
    /// </summary>
    public sealed class ShopifyWebClientRequestThrottle : RequestThrottle<ShopifyWebClientThrottleWaitCancelException>
    {
        private readonly static List<RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>> quotas = new List<RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>>();
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyWebClientRequestThrottle));

        /// <summary>
        /// Initialize throttling definitions
        /// </summary>
        static ShopifyWebClientRequestThrottle()
        {
            quotas = new List<RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>>
                {
                    // These retry intervals are not the specific Shopify quota intervals.  Just some rough guesses based on the Shopify intervals on how quickly to retry if we are throttled
                    new RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>(TimeSpan.FromSeconds(5),  ShopifyWebClientApiCall.GetOrders),
                    new RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>(TimeSpan.FromSeconds(5),  ShopifyWebClientApiCall.GetOrderCount),    
                    new RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>(TimeSpan.FromSeconds(5), ShopifyWebClientApiCall.GetOrder),
                    new RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>(TimeSpan.FromSeconds(10),  ShopifyWebClientApiCall.IsRealShopifyShopUrlName),            
                    new RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>(TimeSpan.FromSeconds(10),  ShopifyWebClientApiCall.GetAccessToken),
                    new RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>(TimeSpan.FromSeconds(10), ShopifyWebClientApiCall.GetShop),
                    new RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>(TimeSpan.FromSeconds(5),  ShopifyWebClientApiCall.GetProduct),
                    new RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>(TimeSpan.FromSeconds(10),  ShopifyWebClientApiCall.GetServerCurrentDateTime),                
                    new RequestThrottleQuotaDefinition<ShopifyWebClientApiCall>(TimeSpan.FromSeconds(10),  ShopifyWebClientApiCall.AddFulfillment),
                };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyWebClientRequestThrottle() :
            base("Shopify", log)
        {
        }

        /// <summary>
        /// ExecuteRequest will make a throttled call to webClientMethod and return the result.
        /// If the throttler detects that the number of calls has been reached, the throttler will wait the 
        /// desiered amount of time before making the call to webClientMethod again.  It will continue to make
        /// the calls until a successful call is made, the user clicks cancel, or a cancel exception is thrown.
        /// </summary>
        /// <typeparam name="TWebClientRequestType">Type of the request</typeparam>
        /// <typeparam name="TWebClientReturnType">The type of response that should be returned</typeparam>
        /// <param name="requestThrottleParams">Throttling request parameters</param>
        /// <param name="webClientMethod">Method that will be executed to </param>
        /// <returns></returns>
        public override TWebClientReturnType ExecuteRequest<TWebClientRequestType, TWebClientReturnType>(RequestThrottleParameters requestThrottleParams, Func<TWebClientRequestType, TWebClientReturnType> webClientMethod)
        {
            // Find the quota for the api call, and update the the request throttle params
            RequestThrottleQuotaDefinition<ShopifyWebClientApiCall> definition = quotas.First(q => q.ApiCalls.Contains((ShopifyWebClientApiCall)requestThrottleParams.ApiCall));
            requestThrottleParams.RetryInterval = definition.RetryInterval;

            // Get a logger for this request, serialize the JSON request, and log it.
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Shopify, EnumHelper.GetDescription(requestThrottleParams.ApiCall));
            string requestText = JsonConvert.SerializeObject((TWebClientRequestType)requestThrottleParams.Request);
            logger.LogRequest(requestText, "txt");

            // Ask the base throttler to start making the call
            TWebClientReturnType restResponse = base.ExecuteRequest(requestThrottleParams, webClientMethod);

            // Serialize the response and log it
            string responseText = JsonConvert.SerializeObject(restResponse);
            logger.LogResponse(responseText, "txt");

            return restResponse;
        }
    }
}
