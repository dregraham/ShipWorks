﻿using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a profile be applied to a shipment
    /// </summary>
    public struct ApplyProfileMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplyProfileMessage(object sender, long shipmentID, long profileID)
        {
            Sender = sender;
            ShipmentID = shipmentID;
            ProfileID = profileID;
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
        /// Id of the shipment to apply the profile on
        /// </summary>
        public long ShipmentID { get; }

        /// <summary>
        /// Profile to be applied
        /// </summary>
        public long ProfileID { get; }
    }
}
