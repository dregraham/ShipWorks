using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.SparkPay.Enums;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// Handles the throttling and submitting of requests to a store api per their defined limits.
    /// </summary>
    [Component]
    public class SparkPayWebClientRequestThrottle : RequestThrottle<SparkPayWebClientThrottleWaitCancelException>, ISparkPayWebClientRequestThrottle
    {
        private readonly static List<RequestThrottleQuotaDefinition<SparkPayWebClientApiCall>> quotas = new List<RequestThrottleQuotaDefinition<SparkPayWebClientApiCall>>();
        static readonly ILog log = LogManager.GetLogger(typeof(SparkPayWebClientRequestThrottle));

        /// <summary>
        /// Initialize throttling definitions
        /// </summary>
        static SparkPayWebClientRequestThrottle()
        {
            quotas = new List<RequestThrottleQuotaDefinition<SparkPayWebClientApiCall>>
                {
                    // These retry intervals are not the specific SparkPay quota intervals.  Just some rough guesses based on the SparkPay intervals on how quickly to retry if we are throttled
                    new RequestThrottleQuotaDefinition<SparkPayWebClientApiCall>(TimeSpan.FromSeconds(5),  SparkPayWebClientApiCall.GetOrders),
                    new RequestThrottleQuotaDefinition<SparkPayWebClientApiCall>(TimeSpan.FromSeconds(5),  SparkPayWebClientApiCall.GetStatuses),
                    new RequestThrottleQuotaDefinition<SparkPayWebClientApiCall>(TimeSpan.FromSeconds(5),  SparkPayWebClientApiCall.GetAddresses),
                    new RequestThrottleQuotaDefinition<SparkPayWebClientApiCall>(TimeSpan.FromSeconds(5),  SparkPayWebClientApiCall.AddShipment),
                    new RequestThrottleQuotaDefinition<SparkPayWebClientApiCall>(TimeSpan.FromSeconds(5),  SparkPayWebClientApiCall.UpdateOrderStatus)
                };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayWebClientRequestThrottle() :
            base("SparkPay", log)
        {
        }

        /// <summary>
        /// ExecuteRequest will make a throttled call to webClientMethod and return the result.
        /// If the throttler detects that the number of calls has been reached, the throttler will wait the
        /// desired amount of time before making the call to webClientMethod again.  It will continue to make
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
            RequestThrottleQuotaDefinition<SparkPayWebClientApiCall> definition = quotas.First(q => q.ApiCalls.Contains((SparkPayWebClientApiCall) requestThrottleParams.ApiCall));
            requestThrottleParams.RetryInterval = definition.RetryInterval;

            // Get a logger for this request, serialize the JSON request, and log it.
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Shopify, EnumHelper.GetDescription(requestThrottleParams.ApiCall));
            string requestText = JsonConvert.SerializeObject((TWebClientRequestType) requestThrottleParams.Request);
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
