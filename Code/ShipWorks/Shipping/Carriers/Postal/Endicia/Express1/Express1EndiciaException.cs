using System;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Express1 Exception
    /// </summary>
    [Serializable]
    public class Express1EndiciaException : EndiciaException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1EndiciaException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public Express1EndiciaException(string message)
            : base(message)
        { }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected Express1EndiciaException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        {

        }
    }
}
