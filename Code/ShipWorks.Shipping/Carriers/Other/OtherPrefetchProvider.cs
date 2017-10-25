using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// Provide a prefetch path for a Other shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.Other)]
    public class OtherPrefetchProvider : IShipmentTypePrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        public PrefetchPathContainer GetPath() =>
            ShipmentEntity.PrefetchPathOther.ToContainer();
    }
}
