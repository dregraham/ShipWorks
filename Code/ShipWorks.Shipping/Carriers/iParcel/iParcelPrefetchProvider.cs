using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.IParcel
{
    /// <summary>
    /// Provide a prefetch path for a IParcel shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.iParcel)]
    public class IParcelPrefetchProvider : IShipmentTypePrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        public PrefetchPathContainer GetPath() =>
            ShipmentEntity.PrefetchPathIParcel.WithChild(IParcelShipmentEntity.PrefetchPathPackages);
    }
}
