using System;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Carriers.Api
{
    /// <summary>
    /// An exception for shipping carrier related errors.
    /// </summary>
    [Serializable]
    public class CarrierException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierException" /> class.
        /// </summary>
        public CarrierException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
       public CarrierException(string message)
            : base(message)
        { }

       /// <summary>
       /// Initializes a new instance of the <see cref="CarrierException" /> class.
       /// </summary>
       /// <param name="message">The message.</param>
       /// <param name="innerException">The inner exception.</param>
        public CarrierException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected CarrierException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
