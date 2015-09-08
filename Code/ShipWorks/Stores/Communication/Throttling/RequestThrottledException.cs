using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Communication.Throttling
{
    /// <summary>
    /// Request throttled exception
    /// </summary>
    [Serializable]
    public class RequestThrottledException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RequestThrottledException(string message) :
            base(message, null)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected RequestThrottledException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
