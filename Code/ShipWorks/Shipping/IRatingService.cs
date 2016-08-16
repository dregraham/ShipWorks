using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

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
    }
}
