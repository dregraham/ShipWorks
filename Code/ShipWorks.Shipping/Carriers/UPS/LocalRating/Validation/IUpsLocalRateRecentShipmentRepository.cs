using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Represents a repository for getting recent shipments 
    /// </summary>
    [Service]
    public interface IUpsLocalRateRecentShipmentRepository
    {
        /// <summary>
        /// Get recent shipments for the given account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IEnumerable<ShipmentEntity> GetRecentShipments(IUpsAccountEntity account);
    }
}