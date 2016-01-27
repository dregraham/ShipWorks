using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Rates have been retrieved
    /// </summary>
    public class RatesRetrievedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RatesRetrievedMessage(object sender, string ratingHash)
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

        /// <summary>
        /// Retrieved rates
        /// </summary>
        public RateGroup RateGroup { get; }

        /// <summary>
        /// Shipment for which the rates have been retrieved
        /// </summary>
        public ICarrierShipmentAdapter ShipmentAdapter { get; }
    }
}
