using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.Builders
{
    /// <summary>
    /// Tool to build collection with package type number and string based on shipments
    /// </summary>
    public interface IShipmentPackageTypesBuilder
    {
        /// <summary>
        /// Builds the package type dictionary.
        /// </summary>
        Dictionary<int, string> BuildPackageTypeDictionary(IEnumerable<ShipmentEntity> shipments);
    }
}
