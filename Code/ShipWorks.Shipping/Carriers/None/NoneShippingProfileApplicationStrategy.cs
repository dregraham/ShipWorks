using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.None
{
    /// <summary>
    /// None shipping profile application strategy
    /// </summary>
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.None)]
    public class NoneShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NoneShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager) 
            : base(shipmentTypeManager)
        {
        }
    }
}