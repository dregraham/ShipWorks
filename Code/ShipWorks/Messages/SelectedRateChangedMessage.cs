using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Messages
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
        public object Sender { get; private set; }

        /// <summary>
        /// Shipment that has changed
        /// </summary>
        public RateResult RateResult { get; private set; }
    }
}
