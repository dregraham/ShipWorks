using System;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// An order has been deleted
    /// </summary>
    public struct OrderDeletedMessage : IEntityDeletedMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDeletedMessage(object sender, long deletedOrderId)
        {
            Sender = sender;
            DeletedEntityID = deletedOrderId;
            MessageId = Guid.NewGuid();
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
        /// Id of the order that was deleted
        /// </summary>
        public long DeletedEntityID { get; }
    }
}
