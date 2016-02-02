using System.Collections.Generic;
using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Rating service for Web Tools
    /// </summary>
    public class WebToolsRatingService : PostalRatingService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebToolsRatingService"/> class.
        /// </summary>
        public WebToolsRatingService(IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory, IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeManager) : base(ratingServiceFactory, shipmentTypeManager)
        {
        }

        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            List<RateResult> rates = PostalWebClientRates.GetRates(shipment, new LogEntryFactory());
            return new RateGroup(FilterRatesByExcludedServices(shipment, rates));
        }

        /// <summary>
        /// Is the rate for the specified shipment
        /// </summary>
        public override bool IsRateSelectedByShipment(RateResult rateResult, ICarrierShipmentAdapter shipmentAdapter)
        {
            if (rateResult.ShipmentType != ShipmentTypeCode.PostalWebTools ||
                shipmentAdapter.ShipmentTypeCode != ShipmentTypeCode.PostalWebTools)
            {
                return false;
            }

            PostalRateSelection rateSelection = rateResult.Tag as PostalRateSelection;
            if (rateSelection == null)
            {
                return false;
            }

            return shipmentAdapter.ServiceType == (int) rateSelection.ServiceType &&
                shipmentAdapter.Shipment?.Postal?.Confirmation == (int) rateSelection.ConfirmationType;
        }
    }
}
