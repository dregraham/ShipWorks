using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Orders.Split.Errors
{
    /// <summary>
    /// Exception created when splitting orders
    /// </summary>
    [Serializable]
    public class OrderSplitException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected OrderSplitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}