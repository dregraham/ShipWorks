using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// PostalWebTools shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.PostalWebTools)]
    class PostalWebToolsShippingProfileApplicationStrategy : PostalShippingProfileApplicationStrategy
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebToolsShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager) : base(shipmentTypeManager)
        {
        }
    }
}
