using System.Collections.Generic;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Shipping
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
            Shipment = shipment;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Shipment that has changed
        /// </summary>
        public ShipmentEntity Shipment { get; private set; }
    }
}
