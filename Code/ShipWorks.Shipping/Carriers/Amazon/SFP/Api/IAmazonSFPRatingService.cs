using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Api
{
    /// <summary>
    /// Gets Amazon Rates
    /// </summary>
    public interface IAmazonSFPRatingService
    {
        /// <summary>
        /// Gets the rates.
        /// </summary>
        RateGroup GetRates(ShipmentEntity shipment);
    }
}
