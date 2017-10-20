using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.Builders;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// For carriers that do not support package types, returns an empty list
    /// </summary>
    [KeyedComponent(typeof(IShipmentPackageTypesBuilder), ShipmentTypeCode.DhlExpress, SingleInstance = true)]
    [KeyedComponent(typeof(IShipmentPackageTypesBuilder), ShipmentTypeCode.iParcel, SingleInstance = true)]
    [KeyedComponent(typeof(IShipmentPackageTypesBuilder), ShipmentTypeCode.None, SingleInstance = true)]
    [KeyedComponent(typeof(IShipmentPackageTypesBuilder), ShipmentTypeCode.Other, SingleInstance = true)]
    [KeyedComponent(typeof(IShipmentPackageTypesBuilder), ShipmentTypeCode.BestRate, SingleInstance = true)]
    [KeyedComponent(typeof(IShipmentPackageTypesBuilder), ShipmentTypeCode.Amazon, SingleInstance = true)]

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
