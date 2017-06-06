using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Shipment selection has changed
    /// </summary>
    public struct ShipmentSelectionChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentSelectionChangedMessage(object sender, IEnumerable<long> selectedShipmentIDs, ICarrierShipmentAdapter selectedShipment)
        {
            Sender = sender;
            MessageId = Guid.NewGuid();
            SelectedShipmentIDs = selectedShipmentIDs.ToReadOnly();
            SelectedShipment = selectedShipment;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// IDs of the shipments that were selected
        /// </summary>
        public IEnumerable<long> SelectedShipmentIDs { get; }

        /// <summary>
        /// Gets the selected shipment if there is only one
        /// </summary>
        public ICarrierShipmentAdapter SelectedShipment { get; }
    }
}
