using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Dialogs
{
    /// <summary>
    /// Profile manager dialog was closed
    /// </summary>
    public class ProfileManagerDialogClosedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileManagerDialogClosedMessage(object sender, OpenProfileManagerDialogMessage openMessage)
        {
            Sender = sender;
            OpenMessage = openMessage;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Message that was sent to open the dialog
        /// </summary>
        public OpenProfileManagerDialogMessage OpenMessage { get; }
    }
}
