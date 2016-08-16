using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Shopify web client throttle wait cancled exception
    /// </summary>
    [Serializable]
    public class ShopifyWebClientThrottleWaitCancelException : ShopifyException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyWebClientThrottleWaitCancelException() :
            base("Waiting for Shopify to stop throttling was canceled.", null)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected ShopifyWebClientThrottleWaitCancelException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
