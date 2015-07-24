using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// An interface for persisting excluded Package type entities.
    /// </summary>
    public interface IExcludedPackageTypeRepository
    {
        /// <summary>
        /// Saves the list of excluded Package types.
        /// </summary>
        void Save(List<ExcludedPackageTypeEntity> excludedPackageTypes);

        /// <summary>
        /// Gets the excluded Package types for the given shipment type.
        /// </summary>
        List<ExcludedPackageTypeEntity> GetExcludedPackageTypes(ShipmentType shipmentType);
    }
}