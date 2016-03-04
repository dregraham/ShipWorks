using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a shipment be shipped again
    /// </summary>
    public struct ShipAgainMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipAgainMessage(object sender, long shipmentID)
        {
            Sender = sender;
            ShipmentID = shipmentID;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the shipment to ship again
        /// </summary>
        public long ShipmentID { get; }
    }
}
