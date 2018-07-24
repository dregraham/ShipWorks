using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Exception thrown when Shopify returns 401 or 403
    /// </summary>
    [Serializable]
    internal class ShopifyAuthorizationException : ShopifyException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAuthorizationException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAuthorizationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAuthorizationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ShopifyAuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}