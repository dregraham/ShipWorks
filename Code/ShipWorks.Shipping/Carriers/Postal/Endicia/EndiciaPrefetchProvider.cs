using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Endicia
{
    /// <summary>
    /// Provide a prefetch path for a Endicia shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.Endicia)]
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.Express1Endicia)]
    public class EndiciaPrefetchProvider : IShipmentTypePrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        public PrefetchPathContainer GetPath() =>
            ShipmentEntity.PrefetchPathPostal.WithChild(PostalShipmentEntity.PrefetchPathEndicia);
    }
}
