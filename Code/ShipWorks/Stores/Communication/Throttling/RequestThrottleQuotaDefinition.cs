using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Communication.Throttling
{
    /// <summary>
    /// Defines how an api call is throttled by the store.
    /// </summary>
    public class RequestThrottleQuotaDefinition<T>
    {
        // All of the api calls that share a quota
        readonly List<T> apiCalls = new List<T>();

        /// <summary>
        /// Constructor
        /// </summary>
        public RequestThrottleQuotaDefinition(TimeSpan retryInterval, params T[] api)
        {
            if (api == null)
            {
                throw new ArgumentNullException("api");
            }

            apiCalls.AddRange(api);
            RetryInterval = retryInterval;

            if (apiCalls.Count > 0)
            {
                MasterApiCall = apiCalls.First();
            }
        }

        /// <summary>
        /// The store API calls that share the same quota 
        /// </summary>
        public IList<T> ApiCalls
        {
            get { return apiCalls; }
        }

        /// <summary>
        /// The rate at which to retry if we get throttled
        /// </summary>
        public TimeSpan RetryInterval { get; private set; }

        /// <summary>
        /// Api calls get throttled in groups, the master is the throttling key for a group
        /// </summary>
        public T MasterApiCall { get; private set; }
    }
}
