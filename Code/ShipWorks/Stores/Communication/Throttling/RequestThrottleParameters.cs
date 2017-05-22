using System;
using Interapptive.Shared.Threading;

namespace ShipWorks.Stores.Communication.Throttling
{
    /// <summary>
    /// Helper class that stores the parameters needed to make a RequestThrottle call
    /// </summary>
    public class RequestThrottleParameters
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiCall">Enum that represents the type of call to be throttled.  Used for logging and determining retry interval.</param>
        /// <param name="request">The request to be made.</param>
        /// <param name="progress">IProgressReporter for detecting cancel and updating status.</param>
        public RequestThrottleParameters(Enum apiCall, object request, IProgressReporter progress)
        {
            ApiCall = apiCall;
            Request = request;
            Progress = progress;

            // Default to 10 seconds
            RetryInterval = new TimeSpan(0, 0, 10);
        }

        /// <summary>
        /// Enum that represents the type of call to be throttled.  Used for logging and determining retry interval.
        /// </summary>
        public Enum ApiCall { get; set; }

        /// <summary>
        /// The request to be made.
        /// </summary>
        public object Request { get; set; }

        /// <summary>
        /// IProgressReporter for detecting cancel and updating status.
        /// </summary>
        public IProgressReporter Progress { get; set; }

        /// <summary>
        /// The response that was returned from the throttled call
        /// </summary>
        public object Response { get; set; }

        /// <summary>
        /// The timespan
        /// </summary>
        public TimeSpan RetryInterval { get; set; }
    }
}
