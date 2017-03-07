using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Common.IO.KeyboardShortcuts.Messages
{
    /// <summary>
    /// Apply the weight in a weight control
    /// </summary>
    public struct ApplyWeightMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ApplyWeightMessage(object sender)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
        }

        /// <summary>
        /// Id of the message, used for tracking
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }
    }
}
