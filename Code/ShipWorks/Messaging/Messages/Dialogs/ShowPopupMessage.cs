using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages.Dialogs
{
    /// <summary>
    /// Message to trigger a popup window
    /// </summary>
    public struct ShowPopupMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShowPopupMessage(object sender, string messageText)
        {
            Sender = sender;
            MessageText = messageText;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Text to display in message
        /// </summary>
        public string MessageText { get; }

        /// <summary>
        /// Message ID
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }
    }
}