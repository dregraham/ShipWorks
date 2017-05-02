using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters
{
    /// <summary>
    /// Filter services based on package dimensions
    /// </summary>
    public class DimensionServiceFilter : IServiceFilter
    {
        /// <summary>
        /// Get eligible services based on the dimensions
        /// </summary>
        public IEnumerable<UpsServiceType> GetEligibleServices(UpsShipmentEntity shipment, IEnumerable<UpsServiceType> services)
        {
            if (shipment.Packages.Any(package => package.LongestSide > 108 ||
                                                 package.Girth + package.LongestSide > 165))
            {
                return Enumerable.Empty<UpsServiceType>();
            }

            return services;
        }
    }
}