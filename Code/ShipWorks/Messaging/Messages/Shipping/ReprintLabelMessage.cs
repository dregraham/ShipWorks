using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a labels be reprinted
    /// </summary>
    public struct ReprintLabelsMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReprintLabelsMessage(object sender, IEnumerable<ShipmentEntity> shipments)
        {
            Sender = sender;
            Shipments = shipments.ToReadOnly();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the shipment to Reprint a label for
        /// </summary>
        public IEnumerable<ShipmentEntity> Shipments { get; }
    }
}
