using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Message for OrderLookup Search
    /// </summary>
    public struct OrderLookupLoadOrderMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupLoadOrderMessage(object sender, OrderEntity order)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
            Order = order;
        }

        /// <summary>
        /// MessageId
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Sender
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// The order to load
        /// </summary>
        public OrderEntity Order { get; }
    }
}
