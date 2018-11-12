using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that the quick search be focused
    /// </summary>
    public struct FocusQuickSearchMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FocusQuickSearchMessage(object sender)
        {
            Sender = sender;
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
    }
}
