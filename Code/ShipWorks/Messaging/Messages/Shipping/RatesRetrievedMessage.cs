using System;
using System.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Utility;
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
        public RatesRetrievedMessage(object sender, string ratingHash, GenericResult<RateGroup> rates, ICarrierShipmentAdapter shipmentAdapter)
        {
            Sender = sender;
            RatingHash = ratingHash;
            RateGroup = rates.Value ?? new RateGroup(Enumerable.Empty<RateResult>());
            Success = rates.Success;
            ErrorMessage = rates.Message;
            ShipmentAdapter = shipmentAdapter;
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
        /// Hash of the shipment for which rates are being retrieved
        /// </summary>
        public string RatingHash { get; }

        /// <summary>
        /// Retrieved rates
        /// </summary>
        public RateGroup RateGroup { get; }

        /// <summary>
        /// Was the rates retrieval successful
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Message that is set when rate retrieval is not successful
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Shipment for which the rates have been retrieved
        /// </summary>
        public ICarrierShipmentAdapter ShipmentAdapter { get; }
    }
}
