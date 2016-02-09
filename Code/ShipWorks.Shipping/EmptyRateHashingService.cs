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
        public string GetRatingHash(ShipmentEntity shipment) =>
            string.Empty;

        /// <summary>
        /// Is the given field a rating field
        /// </summary>
        public bool IsRatingField(string changedField) => false;
    }
}
