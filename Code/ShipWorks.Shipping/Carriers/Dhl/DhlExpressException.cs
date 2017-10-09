using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Base exception for all exceptions thrown by the i-Parcel integration.
    /// </summary>
    [Serializable]
    public class DhlExpressException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhlExpressException" /> class.
        /// </summary>
        public DhlExpressException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DhlExpressException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DhlExpressException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DhlExpressException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DhlExpressException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected DhlExpressException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        { }
    }
}
