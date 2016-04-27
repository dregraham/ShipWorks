using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Message that requests processing of shipments
    /// </summary>
    public struct ProcessShipmentsMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessShipmentsMessage(object sender, IEnumerable<ShipmentEntity> shipments, RateResult selectedRate)
        {
            Sender = sender;
            Shipments = shipments.ToReadOnly();
            SelectedRate = selectedRate;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Read only collection of shipments that should be processed
        /// </summary>
        public IEnumerable<ShipmentEntity> Shipments { get; }

        /// <summary>
        /// Selected rate that should be used if processing requires it
        /// </summary>
        public RateResult SelectedRate { get; }
    }
}
