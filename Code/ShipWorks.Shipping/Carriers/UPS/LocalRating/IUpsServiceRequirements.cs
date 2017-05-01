using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Requirements for to determine the eligibility of a Ups service
    /// </summary>
    public interface IUpsServiceRequirements
    {
        /// <summary>
        /// Gets the list of eligible service types for the given shipment
        /// </summary>
        IEnumerable<UpsServiceType> GetEligibleServices(UpsShipmentEntity shipment);
    }
}