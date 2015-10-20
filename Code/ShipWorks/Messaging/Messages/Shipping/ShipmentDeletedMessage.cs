using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// A shipment has been deleted
    /// </summary>
    public struct ShipmentDeletedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDeletedMessage(object sender, long deletedShipmentId)
        {
            Sender = sender;
            DeletedShipmentId = deletedShipmentId;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the shipment that was deleted
        /// </summary>
        public long DeletedShipmentId { get; }
    }
}
