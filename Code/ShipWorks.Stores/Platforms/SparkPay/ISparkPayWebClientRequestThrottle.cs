using System;
using ShipWorks.Stores.Communication.Throttling;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// Handles the throttling and submitting of requests to a store api per their defined limits.
    /// </summary>
    public interface ISparkPayWebClientRequestThrottle
    {
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
        TWebClientReturnType ExecuteRequest<TWebClientRequestType, TWebClientReturnType>(RequestThrottleParameters requestThrottleParams, Func<TWebClientRequestType, TWebClientReturnType> webClientMethod);
    }
}