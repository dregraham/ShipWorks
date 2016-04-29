using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that shipping accounts have changed for a carrier.
    /// </summary>
    public class OriginAddressChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OriginAddressChangedMessage(object sender)
        {
            Sender = sender;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }
    }
}
