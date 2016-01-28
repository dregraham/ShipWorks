using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that a shipment has changed.
    /// </summary>
    public class ShipmentChangedMessage : IShipWorksMessage
    {
        private readonly EntityField2 changedField;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentChangedMessage(object sender, ICarrierShipmentAdapter shipment)
        {
            Sender = sender;
            ShipmentAdapter = shipment;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentChangedMessage(object sender, ICarrierShipmentAdapter shipment, EntityField2 changedField)
        {
            Sender = sender;
            ShipmentAdapter = shipment;
            ChangedField = changedField;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Shipment that has changed
        /// </summary>
        public ICarrierShipmentAdapter ShipmentAdapter { get; }

        /// <summary>
        /// The field that has changed
        /// </summary>
        public EntityField2 ChangedField { get; }
    }
}
