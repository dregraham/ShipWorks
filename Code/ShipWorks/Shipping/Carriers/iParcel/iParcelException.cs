using System;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Base exception for all exceptions thrown by the i-Parcel integration.
    /// </summary>    
    [Serializable]
    public class iParcelException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelException" /> class.
        /// </summary>
        public iParcelException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public iParcelException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public iParcelException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected iParcelException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
