using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that a shipment has changed.
    /// </summary>
    public class ShipmentChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentChangedMessage(object sender, ShipmentEntity shipment)
        {
            Sender = sender;

            ICarrierShipmentAdapterFactory shipmentAdapterFactory = IoC.UnsafeGlobalLifetimeScope.Resolve<ICarrierShipmentAdapterFactory>();
            ShipmentAdapter = shipmentAdapterFactory.Get(shipment);
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Shipment that has changed
        /// </summary>
        public ICarrierShipmentAdapter ShipmentAdapter { get; private set; }
    }
}
