using System;
using Interapptive.Shared.Messaging;

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
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Object that sent the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Message to display to the user
        /// </summary>
        public string Message { get; }
    }
}
