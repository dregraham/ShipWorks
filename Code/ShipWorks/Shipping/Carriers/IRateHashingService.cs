using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Rate Hasing Service Interface
    /// </summary>
    public interface IRateHashingService
    {
        /// <summary>
        /// Fields of a shipment 
        /// </summary>
        RatingFields RatingFields { get; }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.
        /// </summary>
        string GetRatingHash(ShipmentEntity shipment);
    }
}
