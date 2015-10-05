using Interapptive.Shared.Messaging;

namespace ShipWorks.Messages
{
    /// <summary>
    /// Create labels for the given shipments
    /// </summary>
    public class CreateLabelMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelMessage(object sender)
        {
            Sender = sender;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; private set; }
    }
}
