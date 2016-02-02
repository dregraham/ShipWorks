using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Gets the rates from Amazon via the IAmazonShippingWebClient
    /// </summary>
    public class AmazonRatingService : IRatingService
    {
        private readonly IAmazonShippingWebClient webClient;
        private readonly IOrderManager orderManager;
        private readonly IAmazonShipmentRequestDetailsFactory requestFactory;
        private readonly IAmazonRateGroupFactory amazonRateGroupFactory;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonRatingService"/> class.
        /// </summary>
        public AmazonRatingService(IAmazonShippingWebClient webClient,
            IOrderManager orderManager,
            IAmazonShipmentRequestDetailsFactory requestFactory,
            IAmazonRateGroupFactory amazonRateGroupFactory,
            IAmazonMwsWebClientSettingsFactory settingsFactory)
        {
            this.webClient = webClient;
            this.orderManager = orderManager;
            this.requestFactory = requestFactory;
            this.amazonRateGroupFactory = amazonRateGroupFactory;
            this.settingsFactory = settingsFactory;
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            orderManager.PopulateOrderDetails(shipment);
            IAmazonOrder amazonOrder = shipment.Order as IAmazonOrder;

            if (amazonOrder?.IsPrime == false)
            {
                throw new AmazonShippingException("Not an Amazon Prime Order");
            }

            if (amazonOrder == null)
            {
                throw new AmazonShippingException("Not an Amazon Order");
            }

            ShipmentRequestDetails requestDetails = requestFactory.Create(shipment, amazonOrder);

            GetEligibleShippingServicesResponse response = webClient.GetRates(requestDetails, settingsFactory.Create(shipment.Amazon));

            return amazonRateGroupFactory.GetRateGroupFromResponse(response);
        }

        /// <summary>
        /// Is the rate for the specified shipment
        /// </summary>
        public bool IsRateSelectedByShipment(RateResult rateResult, ICarrierShipmentAdapter shipmentAdapter)
        {
            throw new NotImplementedException("Amazon is not yet supported");
        }
    }
}