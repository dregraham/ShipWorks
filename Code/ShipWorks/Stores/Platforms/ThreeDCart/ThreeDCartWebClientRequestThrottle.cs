using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Communication.Throttling;
using log4net;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.ThreeDCart.Enums;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Class that handles the throttling and submitting of requests to a store api per their defined limits.
    /// </summary>
    public sealed class ThreeDCartWebClientRequestThrottle : RequestThrottle<ThreeDCartWebClientThrottleWaitCancelException>
    {
        private readonly static List<RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>> quotas;
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartWebClientRequestThrottle));

        /// <summary>
        /// Initialize throttling definitions
        /// </summary>
        static ThreeDCartWebClientRequestThrottle()
        {
            quotas = new List<RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>>
                {
                    // These retry intervals are not the specific ThreeDCart quota intervals.  Just some rough guesses based on the ThreeDCart intervals on how quickly to retry if we are throttled
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.CreateFulfillment),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.UpdateOrderStatus),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.GetOrder),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.GetOrders),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.GetOrderCount),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.GetOrderStatuses),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.GetProducts),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.GetProduct),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.DetermineDbVersionSqlServer),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.DetermineDbVersionMsAccess),
                    new RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall>(TimeSpan.FromSeconds(10), ThreeDCartWebClientApiCall.TestConnection)
                };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartWebClientRequestThrottle() :
            base("ThreeDCart", log)
        {
        }

        /// <summary>
        /// ExecuteRequest will make a throttled call to webClientMethod and return the result.
        /// If the throttler detects that the number of calls has been reached, the throttler will wait the 
        /// desiered amount of time before making the call to webClientMethod again.  It will continue to make
        /// the calls until a successful call is made, the user clicks cancel, or a cancel exception is thrown.
        /// </summary>
        /// <param name="requestThrottleParams">Throttling request parameters</param>
        /// <param name="webClientMethod">Method that will be executed to </param>
        /// <returns></returns>
        public override void ExecuteRequest(RequestThrottleParameters requestThrottleParams, Action webClientMethod)
        {
            // Find the quota for the api call, and update the the request throttle params
            RequestThrottleQuotaDefinition<ThreeDCartWebClientApiCall> definition = quotas.First(q => q.ApiCalls.Contains((ThreeDCartWebClientApiCall)requestThrottleParams.ApiCall));
            requestThrottleParams.RetryInterval = definition.RetryInterval;

            // Ask the base throttler to start making the call
            base.ExecuteRequest(requestThrottleParams, webClientMethod);
        }
    }
}
