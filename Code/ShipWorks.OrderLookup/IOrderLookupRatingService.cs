using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Rating service for use in Order Lookup mode
    /// </summary>
    public interface IOrderLookupRatingService
    {
        /// <summary>
        /// The last rate hash to be calculated
        /// </summary>
        string LatestRateHash { get; }
        
        /// <summary>
        /// Get rates for the given shipment
        /// </summary>
        GenericResult<RateGroup> GetRates(ShipmentEntity shipment);
    }
}