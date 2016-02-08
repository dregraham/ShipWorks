using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Rate retrieval has begun
    /// </summary>
    public class RatesRetrievingMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RatesRetrievingMessage(object sender, string ratingHash)
        {
            Sender = sender;
            RatingHash = ratingHash;
        }

        /// <summary>
        /// Object that sent the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Hash of the shipment for which rates are being retrieved
        /// </summary>
        public string RatingHash { get; }
    }
}
