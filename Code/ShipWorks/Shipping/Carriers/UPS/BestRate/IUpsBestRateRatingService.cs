using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    public interface IUpsBestRateRatingService
    {
        /// <summary>
        /// Get the UPS rates for the given shipment
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment);
    }
}