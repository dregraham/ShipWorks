using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.UI.Profiles
{
    [Component]
    public class ProfileControlFactory : IProfileControlFactory
    {
        private readonly IIndex<ShipmentTypeCode, ShippingProfileControlBase> shippingProfileControlBaseIndex;

        public ProfileControlFactory(IIndex<ShipmentTypeCode, ShippingProfileControlBase> shippingProfileControlBaseIndex)
        {
            this.shippingProfileControlBaseIndex = shippingProfileControlBaseIndex;
        }

        /// <summary>
        /// Creats a GlobalProfileControl
        /// </summary>
        public ShippingProfileControlBase Create()
        {
            return new GlobalProfileControl();
        }

        /// <summary>
        /// Creates a ShippingProfileControl for the specified shipmentType
        /// </summary>
        public ShippingProfileControlBase Create(ShipmentTypeCode shipmentTypeCode) => 
            shippingProfileControlBaseIndex[shipmentTypeCode];
    }
}