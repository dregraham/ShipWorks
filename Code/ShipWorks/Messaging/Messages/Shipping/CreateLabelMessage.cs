using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a single label be created from the shipping view model
    /// </summary>
    public struct CreateLabelMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelMessage(object sender, long shipmentID)
        {
            Sender = sender;
            ShipmentID = shipmentID;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the shipment to create a label for
        /// </summary>
        public long ShipmentID { get; }
    }
}
