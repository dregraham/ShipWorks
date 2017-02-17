using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages.SingleScan
{
    /// <summary>
    /// Request that we stop filtering bar code scans
    /// </summary>
    public struct DisableSingleScanInputFilterMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DisableSingleScanInputFilterMessage(object sender)
        {
            Sender = sender;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Id of the message used for tracking
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }
    }
}
