using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Provide a prefetch path for a FedEx shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.FedEx)]
    public class FedExPrefetchProvider : IShipmentTypePrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        public PrefetchPathContainer GetPath() =>
            ShipmentEntity.PrefetchPathFedEx.WithChild(FedExShipmentEntity.PrefetchPathPackages);
    }
}
