﻿using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.OrderLookup.Messages
{
    /// <summary>
    /// Request that a shipment be shipped again
    /// </summary>
    public struct OrderLookupShipAgainMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupShipAgainMessage(object sender, long shipmentID)
        {
            Sender = sender;
            ShipmentID = shipmentID;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// The shipmentID to ship again
        /// </summary>
        public long ShipmentID { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }
    }
}
