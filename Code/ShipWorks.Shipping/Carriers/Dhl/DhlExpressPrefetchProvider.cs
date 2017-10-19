using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Provide a prefetch path for a DhlExpress shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.DhlExpress)]

    public class DhlExpressPrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        public PrefetchPathContainer GetPath() =>
            ShipmentEntity.PrefetchPathDhlExpress.WithChild(DhlExpressShipmentEntity.PrefetchPathPackages);

    }
}
