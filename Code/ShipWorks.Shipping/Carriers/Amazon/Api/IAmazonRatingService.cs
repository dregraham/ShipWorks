using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Gets Amazon Rates
    /// </summary>
    public interface IAmazonRatingService
    {
        /// <summary>
        /// Gets the rates.
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment);
    }
}
