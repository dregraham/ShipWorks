using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ShipWorks.Stores.Communication.Throttling;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Handles the throttling and submitting of requests to Amazon per their defined limits.
    /// </summary>
    public sealed class AmazonMwsRequestThrottle : RequestThrottle<AmazonMwsThrottleWaitCancelException>
    {
        private readonly static List<RequestThrottleQuotaDefinition<AmazonMwsApiCall>> quotas = new List<RequestThrottleQuotaDefinition<AmazonMwsApiCall>>();
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonMwsRequestThrottle));

        /// <summary>
        /// Initialize throttling definitions
        /// </summary>
        static AmazonMwsRequestThrottle()
        {
            quotas = new List<RequestThrottleQuotaDefinition<AmazonMwsApiCall>>
            {
                // These retry intervals are not the specific Amazon quota intervals.  Just some rough guesses based on the Amazon intervals on how quickly to retry if we are throttled
                new RequestThrottleQuotaDefinition<AmazonMwsApiCall>(TimeSpan.FromSeconds(30),  AmazonMwsApiCall.ListOrders, AmazonMwsApiCall.ListOrdersByNextToken),
                new RequestThrottleQuotaDefinition<AmazonMwsApiCall>(TimeSpan.FromSeconds(10), AmazonMwsApiCall.ListOrderItems, AmazonMwsApiCall.ListOrderItemsByNextToken),
                new RequestThrottleQuotaDefinition<AmazonMwsApiCall>(TimeSpan.FromSeconds(30),  AmazonMwsApiCall.GetServiceStatus),
                new RequestThrottleQuotaDefinition<AmazonMwsApiCall>(TimeSpan.FromSeconds(30),  AmazonMwsApiCall.SubmitFeed),
                new RequestThrottleQuotaDefinition<AmazonMwsApiCall>(TimeSpan.FromSeconds(30),  AmazonMwsApiCall.GetMatchingProductForId),
                new RequestThrottleQuotaDefinition<AmazonMwsApiCall>(TimeSpan.FromSeconds(30),  AmazonMwsApiCall.ListOrdersByNextToken),
                new RequestThrottleQuotaDefinition<AmazonMwsApiCall>(TimeSpan.FromSeconds(30),  AmazonMwsApiCall.ListOrderItemsByNextToken),
                new RequestThrottleQuotaDefinition<AmazonMwsApiCall>(TimeSpan.FromSeconds(30),  AmazonMwsApiCall.ListMarketplaceParticipations),
                new RequestThrottleQuotaDefinition<AmazonMwsApiCall>(TimeSpan.FromSeconds(10), AmazonMwsApiCall.GetAuthToken)
            };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsRequestThrottle() :
            base("Amazon", log)
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
        public override TWebClientReturnType ExecuteRequest<TWebClientRequestType, TWebClientReturnType>(
            RequestThrottleParameters requestThrottleParams,
            Func<TWebClientRequestType, TWebClientReturnType> webClientMethod)
        {
            // Find the quota for the api call, and update the request throttle params
            RequestThrottleQuotaDefinition<AmazonMwsApiCall> definition = quotas.First(q => q.ApiCalls.Contains((AmazonMwsApiCall) requestThrottleParams.ApiCall));
            requestThrottleParams.RetryInterval = definition.RetryInterval;

            // Ask the base throttler to start making the call
            TWebClientReturnType restResponse = base.ExecuteRequest(requestThrottleParams, webClientMethod);

            return restResponse;
        }
    }
}
