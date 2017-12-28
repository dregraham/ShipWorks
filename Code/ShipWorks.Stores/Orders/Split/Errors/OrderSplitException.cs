using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Orders.Split.Errors
{
    [Serializable]
    internal class OrderSplitException : Exception
    {
        public OrderSplitException()
        {
        }

        public OrderSplitException(string message) : base(message)
        {
        }

        public OrderSplitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OrderSplitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}