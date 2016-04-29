using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// The main window is unloading
    /// </summary>
    /// <remarks>
    /// This can happen when the main form closes, when the user logs off, or when restoring a database
    /// </remarks>
    public struct WindowResettingMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WindowResettingMessage(object sender)
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
