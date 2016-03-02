using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// An order has been deleted
    /// </summary>
    public struct OrderDeletedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderDeletedMessage(object sender, long deletedOrderId)
        {
            Sender = sender;
            DeletedOrderId = deletedOrderId;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the order that was deleted
        /// </summary>
        public long DeletedOrderId { get; }
    }
}
