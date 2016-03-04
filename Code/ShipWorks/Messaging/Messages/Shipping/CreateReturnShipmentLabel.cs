using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a return shipment be created
    /// </summary>
    public struct CreateReturnShipmentMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateReturnShipmentMessage(object sender, long shipmentID)
        {
            Sender = sender;
            ShipmentID = shipmentID;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the shipment to create a return shipment for
        /// </summary>
        public long ShipmentID { get; }
    }
}
