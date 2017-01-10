using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Text was scanned
    /// </summary>
    public struct ScanMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ScanMessage(object sender, string scannedText)
        {
            Sender = sender;
            ScannedText = scannedText;
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

        /// <summary>
        /// Text that was scanned
        /// </summary>
        public string ScannedText { get; }
    }
}
