using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Provide a prefetch path for a Ups shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.UpsOnLineTools)]
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.UpsWorldShip)]
    public class UpsPrefetchProvider : IShipmentTypePrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        public PrefetchPathContainer GetPath() =>
            ShipmentEntity.PrefetchPathUps.WithChild(UpsShipmentEntity.PrefetchPathPackages);
    }
}
