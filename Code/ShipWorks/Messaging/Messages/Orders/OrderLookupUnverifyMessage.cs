using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages.Orders
{
    /// <summary>
    /// Request that a order be unverified
    /// </summary>
    public struct OrderLookupUnverifyMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupUnverifyMessage(object sender, long orderID)
        {
            Sender = sender;
            OrderID= orderID;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// The orderID to ship again
        /// </summary>
        public long OrderID { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }
    }
}
