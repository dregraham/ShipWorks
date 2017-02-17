using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages.SingleScan
{
    /// <summary>
    /// Request that we start filtering bar code scans
    /// </summary>
    public struct EnableSingleScanInputFilterMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EnableSingleScanInputFilterMessage(object sender)
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
