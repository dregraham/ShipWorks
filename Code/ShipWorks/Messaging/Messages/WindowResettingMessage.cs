using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// The main window is unloading
    /// </summary>
    /// <remarks>
    /// This can happen when the main form closes, when the user logs off, or when restoring a database
    /// </remarks>
    public class WindowResettingMessage : IShipWorksMessage
    {
        private readonly object sender;

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowResettingMessage(object sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender => sender;
    }
}
