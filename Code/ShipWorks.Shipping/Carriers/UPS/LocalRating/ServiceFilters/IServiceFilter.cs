using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters
{
    /// <summary>
    /// Filter for determining eligible services
    /// </summary>
    [Service]
    public interface IServiceFilter
    {
        /// <summary>
        /// Gets the list of eligible service types for the given shipment, from the given list of service types
        /// </summary>
        IEnumerable<UpsServiceType> GetEligibleServices(UpsShipmentEntity shipment, IEnumerable<UpsServiceType> services);
    }
}