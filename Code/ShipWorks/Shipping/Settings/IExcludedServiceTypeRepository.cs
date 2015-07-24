using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// An interface for persisting excluded service type entities.
    /// </summary>
    public interface IExcludedServiceTypeRepository
    {
        /// <summary>
        /// Saves the list of excluded service types.
        /// </summary>
        void Save(List<ExcludedServiceTypeEntity> excludedServiceTypes);

        /// <summary>
        /// Gets the excluded service types for the given shipment type.
        /// </summary>
        List<ExcludedServiceTypeEntity> GetExcludedServiceTypes(ShipmentType shipmentType);
    }
}
