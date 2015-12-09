using ShipWorks.Core.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that dimensions profiles have changed.
    /// </summary>
    public class DimensionsProfilesChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DimensionsProfilesChangedMessage(object sender)
        {
            Sender = sender;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }
    }
}
