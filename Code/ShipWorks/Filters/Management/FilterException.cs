using System;
using System.Runtime.Serialization;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Base for all exceptions related to filters that we handle
    /// </summary>
    [Serializable]
    public class FilterException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FilterException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected FilterException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
