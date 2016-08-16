using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Exception to represent errors when creating online action update tasks
    /// </summary>
    [Serializable]
    public class OnlineUpdateActionCreateException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateActionCreateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateActionCreateException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected OnlineUpdateActionCreateException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
