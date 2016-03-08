using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;

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
        public ShipAgainMessage(object sender, ShipmentEntity shipment)
        {
            Sender = sender;
            Shipment = shipment;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Shipment to ship again
        /// </summary>
        public ShipmentEntity Shipment { get; }
    }
}
