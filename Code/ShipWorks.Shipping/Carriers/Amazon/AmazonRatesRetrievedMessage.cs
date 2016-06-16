using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// AmazonRatesRetrieved Message
    /// </summary>
    struct AmazonRatesRetrievedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonRatesRetrievedMessage"/> class.
        /// </summary>
        public AmazonRatesRetrievedMessage(object sender, RateGroup rateGroup)
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
