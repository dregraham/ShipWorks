using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.WebTools
{
    /// <summary>
    /// Provide a prefetch path for a WebTools shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypePrefetchProvider), ShipmentTypeCode.PostalWebTools)]
    public class WebToolsPrefetchProvider : IShipmentTypePrefetchProvider
    {
        /// <summary>
        /// Get the path
        /// </summary>
        public PrefetchPathContainer GetPath() =>
            ShipmentEntity.PrefetchPathPostal.ToContainer();
    }
}
