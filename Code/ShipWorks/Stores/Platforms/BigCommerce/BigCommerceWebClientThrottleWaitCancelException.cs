using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// BigCommerce web client throttle wait cancled exception
    /// </summary>
    [Serializable]
    public class BigCommerceWebClientThrottleWaitCancelException : BigCommerceException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceWebClientThrottleWaitCancelException() : 
            base("Waiting for BigCommerce to stop throttling was canceled.", null)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected BigCommerceWebClientThrottleWaitCancelException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        {

        }
    }
}
