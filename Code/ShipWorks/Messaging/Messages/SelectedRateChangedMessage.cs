using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that the selected rate has changed.
    /// </summary>
    public struct SelectedRateChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SelectedRateChangedMessage(object sender, RateResult rateResult)
        {
            Sender = sender;
            RateResult = rateResult;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Shipment that has changed
        /// </summary>
        public RateResult RateResult { get; }
    }
}
