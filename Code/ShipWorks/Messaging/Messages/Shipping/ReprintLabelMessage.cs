using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a single label be reprinted
    /// </summary>
    public struct ReprintLabelMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReprintLabelMessage(object sender, long shipmentID)
        {
            Sender = sender;
            ShipmentID = shipmentID;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the shipment to Reprint a label for
        /// </summary>
        public long ShipmentID { get; }
    }
}
