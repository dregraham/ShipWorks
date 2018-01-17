using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Provide a prefetch path for a DhlExpress shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.Asendia)]
    public class AsendiaPrefetchProvider : IShipmentTypePrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        public PrefetchPathContainer GetPath() => ShipmentEntity.PrefetchPathAsendia.ToContainer();
    }
}
