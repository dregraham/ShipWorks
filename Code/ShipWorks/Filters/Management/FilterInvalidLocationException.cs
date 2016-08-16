using System;
using System.Runtime.Serialization;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Exception that is thrown when a filter cannot go where the user was trying to put it.
    /// </summary>
    [Serializable]
    public class FilterInvalidLocationException : FilterException
    {
        public FilterInvalidLocationException()
        {

        }

        public FilterInvalidLocationException(string message)
            : base(message)
        {

        }

        public FilterInvalidLocationException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected FilterInvalidLocationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
