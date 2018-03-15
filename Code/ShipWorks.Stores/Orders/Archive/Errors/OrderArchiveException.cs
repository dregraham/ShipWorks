using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Orders.Archive.Errors
{
    /// <summary>
    /// Exception created when archiving orders
    /// </summary>
    [Serializable]
    public class OrderArchiveException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiveException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiveException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected OrderArchiveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}