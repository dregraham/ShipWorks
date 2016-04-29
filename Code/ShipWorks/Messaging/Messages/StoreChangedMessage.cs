using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// An IShipWorksMessage used to inform subscribers that a store has changed.
    /// </summary>
    public struct StoreChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreChangedMessage(object sender, StoreEntity store)
        {
            Sender = sender;
            StoreEntity = store;
            MessageId = Guid.NewGuid();
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
        /// Store that has changed
        /// </summary>
        public StoreEntity StoreEntity { get; }
    }
}
