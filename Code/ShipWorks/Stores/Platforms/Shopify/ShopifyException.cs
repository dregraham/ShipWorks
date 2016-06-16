using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Shopify specific Exception 
    /// </summary>
    public class ShopifyException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyException(string message) 
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected ShopifyException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
