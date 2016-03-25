using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Rates are not supported for current order or shipment
    /// </summary>
    public class RatesNotSupportedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RatesNotSupportedMessage(object sender, string message)
        {
            Sender = sender;
            Message = message;
        }

        /// <summary>
        /// Object that sent the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Message to display to the user
        /// </summary>
        public string Message { get; }
    }
}
