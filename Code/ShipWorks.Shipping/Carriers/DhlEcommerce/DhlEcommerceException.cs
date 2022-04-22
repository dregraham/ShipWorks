using System;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Base exception for all exceptions thrown by the DHL eCommerce integration.
    /// </summary>
    [Serializable]
    public class DhlEcommerceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhlEcommerceException" /> class.
        /// </summary>
        public DhlEcommerceException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DhlEcommerceException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DhlEcommerceException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DhlEcommerceException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DhlEcommerceException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected DhlEcommerceException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        { }
    }
}
