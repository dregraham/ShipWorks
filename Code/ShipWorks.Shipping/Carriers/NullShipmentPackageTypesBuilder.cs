using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
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
