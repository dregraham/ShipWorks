using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Empty RateHashingService
    /// </summary>
    public class EmptyRateHashingService : IRateHashingService
    {
        /// <summary>
        /// Returns an empty list of rating fields
        /// </summary>
        public RatingFields RatingFields =>
                    new RatingFields();

        /// <summary>
        /// Returns empty string
        /// </summary>
        public string GetRatingHash(ShipmentEntity shipment)
        {
            return string.Empty;
        }
    }
}
