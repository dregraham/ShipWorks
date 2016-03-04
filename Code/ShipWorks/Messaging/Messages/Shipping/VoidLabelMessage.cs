using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a single label be void from the shipping view model
    /// </summary>
    public struct VoidLabelMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VoidLabelMessage(object sender, long shipmentID)
        {
            Sender = sender;
            ShipmentID = shipmentID;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the shipment to void a label for
        /// </summary>
        public long ShipmentID { get; }
    }
}
