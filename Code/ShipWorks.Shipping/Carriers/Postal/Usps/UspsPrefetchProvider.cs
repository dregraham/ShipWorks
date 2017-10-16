using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Usps
{
    /// <summary>
    /// Provide a prefetch path for a Usps shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.Express1Usps)]
    public class UspsPrefetchProvider : IShipmentTypePrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        public PrefetchPathContainer GetPath() =>
            ShipmentEntity.PrefetchPathPostal.WithChild(PostalShipmentEntity.PrefetchPathUsps);
    }
}
