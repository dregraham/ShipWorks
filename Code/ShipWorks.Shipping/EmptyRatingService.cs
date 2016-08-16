using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Empty Rating Service
    /// </summary>
    public class EmptyRatingService : IRatingService
    {
        /// <summary>
        /// GetRates not supported by carrier
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            RateGroup rateGroupWithNoRatesFooter = new RateGroup(Enumerable.Empty<RateResult>());
            rateGroupWithNoRatesFooter.AddFootnoteFactory(new InformationFootnoteFactory("Select another provider to get rates."));
            return rateGroupWithNoRatesFooter;
        }

        /// <summary>
        /// Is the rate for the specified shipment
        /// </summary>
        public bool IsRateSelectedByShipment(RateResult rateResult, ICarrierShipmentAdapter shipmentAdapter) => false;
    }
}