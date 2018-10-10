using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Clear the order lookup order
    /// </summary>
    public struct OrderLookupClearOrderMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupClearOrderMessage(object sender)
        {
            Sender = sender;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }
    }
}
