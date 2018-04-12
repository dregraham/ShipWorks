using System;
using System.Collections.Generic;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Message that is sent after a profile applied
    /// </summary>
    public class ProfileAppliedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileAppliedMessage(object sender,
            IEnumerable<ShipmentEntity> originalShipments,
            IEnumerable<ShipmentEntity> updatedShipments)
        {
            Sender = sender;
            OriginalShipments = originalShipments;
            UpdatedShipments = updatedShipments;
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
        /// Shipments before the profile has been applied
        /// </summary>
        public IEnumerable<ShipmentEntity> OriginalShipments { get; }

        /// <summary>
        /// Shipments after the profile has been applied
        /// </summary>
        public IEnumerable<ShipmentEntity> UpdatedShipments { get; }
    }
}