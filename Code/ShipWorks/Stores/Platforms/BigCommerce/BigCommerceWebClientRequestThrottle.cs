using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.BigCommerce.Enums;
using log4net;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// BigCommerce implementation of request throttler
    /// </summary>
    public sealed class BigCommerceWebClientRequestThrottle : RequestThrottle<BigCommerceWebClientThrottleWaitCancelException>
    {
        private readonly static List<RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>> quotas = new List<RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>>();
        static readonly ILog log = LogManager.GetLogger(typeof(BigCommerceWebClientRequestThrottle));

        /// <summary>
        /// Initialize throttling definitions
        /// </summary>
        static BigCommerceWebClientRequestThrottle()
        {
            quotas = new List<RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>>
                {
                    // These retry intervals are not the specific BigCommerce quota intervals.  Just some rough guesses based on the BigCommerce intervals on how quickly to retry if we are throttled
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(30),  BigCommerceWebClientApiCall.GetOrders),
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(30),  BigCommerceWebClientApiCall.GetOrderCount),    
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10), BigCommerceWebClientApiCall.GetOrder),
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10),  BigCommerceWebClientApiCall.GetCoupons),            
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10),  BigCommerceWebClientApiCall.GetOrderStatuses),
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10), BigCommerceWebClientApiCall.GetProducts),
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10),  BigCommerceWebClientApiCall.GetProduct),
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10),  BigCommerceWebClientApiCall.GetShipments),                
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10),  BigCommerceWebClientApiCall.GetShippingAddress),
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10),  BigCommerceWebClientApiCall.UpdateOrderStatus),                
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10),  BigCommerceWebClientApiCall.CreateShipment),                
                    new RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall>(TimeSpan.FromSeconds(10),  BigCommerceWebClientApiCall.GetOrderProducts)
                };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceWebClientRequestThrottle() :
            base("BigCommerce", log)
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
            RequestThrottleQuotaDefinition<BigCommerceWebClientApiCall> definition = quotas.First(q => q.ApiCalls.Contains((BigCommerceWebClientApiCall)requestThrottleParams.ApiCall));
            requestThrottleParams.RetryInterval = definition.RetryInterval;

            // Get a logger for this request, serialize the JSON request, and log it.
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.BigCommerce, EnumHelper.GetDescription(requestThrottleParams.ApiCall));
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
