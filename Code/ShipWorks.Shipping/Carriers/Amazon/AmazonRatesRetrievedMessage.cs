using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Core.Messaging;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// AmazonRatesRetrieved Message
    /// </summary>
    class AmazonRatesRetrievedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonRatesRetrievedMessage"/> class.
        /// </summary>
        public AmazonRatesRetrievedMessage(object sender, RateGroup rateGroup)
        {
            Sender = sender;
            RateGroup = rateGroup;
        }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Gets or sets the rate group.
        /// </summary>
        public RateGroup RateGroup { get; set; }
    }
}
