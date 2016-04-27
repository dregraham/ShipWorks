using System;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// A shipment has been deleted
    /// </summary>
    public struct ShipmentDeletedMessage : IEntityDeletedMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDeletedMessage(object sender, long deletedShipmentId)
        {
            Sender = sender;
            DeletedEntityID = deletedShipmentId;
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
        /// Id of the shipment that was deleted
        /// </summary>
        public long DeletedEntityID { get; }
    }
}
