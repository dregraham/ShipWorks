using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Order was found via OrderLookup
    /// </summary>
    public struct OrderFoundMessage : IShipWorksMessage
    {
        public OrderFoundMessage(object sender, OrderEntity order)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
            Order = order;
        }

        /// <summary>
        /// Id of the message used for tracking
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Order that was found
        /// </summary>
        public OrderEntity Order { get; }

    }
}