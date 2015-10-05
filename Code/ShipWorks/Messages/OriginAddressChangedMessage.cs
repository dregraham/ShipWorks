using ShipWorks.Core.Messaging;

namespace ShipWorks.Messages
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
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }
    }
}
