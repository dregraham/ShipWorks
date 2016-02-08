using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// RatingService Interface
    /// </summary>
    public interface IRatingService
    {
        /// <summary>
        /// Called to get the latest rates for the shipment
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment);

        /// <summary>
        /// Is the rate for the specified shipment
        /// </summary>
        bool IsRateSelectedByShipment(RateResult rateResult, ICarrierShipmentAdapter shipmentAdapter);
    }
}
