using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
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
