using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Messaging.Messages.Orders
{
    /// <summary>
    /// Message sent when an order is verified through scan to ship
    /// </summary>
    public struct OrderVerifiedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderVerifiedMessage(object sender, IOrderEntity order)
        {
            Sender = sender;
            MessageId = Guid.NewGuid();
            Order = order.AsReadOnly();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// The order that was verified
        /// </summary>
        public IOrderEntity Order { get; }
    }
}
