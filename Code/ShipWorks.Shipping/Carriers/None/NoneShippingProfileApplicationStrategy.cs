using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.None
{
    [KeyedComponent(typeof(IShippingProfileApplicationStrategy), ShipmentTypeCode.None)]
    public class NoneShippingProfileApplicationStrategy : BaseShippingProfileApplicationStrategy
    {
        public NoneShippingProfileApplicationStrategy(IShipmentTypeManager shipmentTypeManager) 
            : base(shipmentTypeManager)
        {
        }
    }
}