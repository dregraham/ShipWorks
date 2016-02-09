using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that the selected rate has changed.
    /// </summary>
    public class SelectedRateChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SelectedRateChangedMessage(object sender, RateResult rateResult)
        {
            Sender = sender;
            RateResult = rateResult;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Shipment that has changed
        /// </summary>
        public RateResult RateResult { get; }
    }
}
