using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// AmazonRatesRetrieved Message
    /// </summary>
    struct AmazonSFPRatesRetrievedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSFPRatesRetrievedMessage"/> class.
        /// </summary>
        public AmazonSFPRatesRetrievedMessage(object sender, RateGroup rateGroup)
        {
            Sender = sender;
            RateGroup = rateGroup;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Gets or sets the rate group.
        /// </summary>
        public RateGroup RateGroup { get; }
    }
}
