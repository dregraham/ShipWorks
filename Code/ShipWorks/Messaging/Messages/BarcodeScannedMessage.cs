using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// A barcode was scanned
    /// </summary>
    public struct BarcodeScannedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BarcodeScannedMessage(object sender, string barcode)
        {
            Sender = sender;
            Barcode = barcode;
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
        /// Barcode that was scanned
        /// </summary>
        public string Barcode { get; }
    }
}
