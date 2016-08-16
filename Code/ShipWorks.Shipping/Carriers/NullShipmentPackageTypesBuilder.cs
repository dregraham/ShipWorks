using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// For carriers that do not support package types, returns an empty list
    /// </summary>
    public class NullShipmentPackageTypesBuilder : IShipmentPackageTypesBuilder
    {
        public NullShipmentPackageTypesBuilder()
        {
        }

        /// <summary>
        /// Returns an empty list
        /// </summary>
        public Dictionary<int, string> BuildPackageTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            return new Dictionary<int, string>();
        }
    }
}
