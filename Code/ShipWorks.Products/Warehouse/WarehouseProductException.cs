using System;
using System.Runtime.Serialization;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// A warehouse specific product exception
    /// </summary>
    [Serializable]
    public class WarehouseProductException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseProductException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected WarehouseProductException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}