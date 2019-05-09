using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Service that creates automatic returns for shipments
    /// </summary>
    [Component]
    public class AutoReturnShipmentService : IAutoReturnShipmentService
    {
        private readonly IShippingManager shippingManager;
        private readonly IShippingProfileService shippingProfileService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoReturnShipmentService(IShippingManager shippingManager,
            IShippingProfileService shippingProfileService)
        {
            this.shippingManager = shippingManager;
            this.shippingProfileService = shippingProfileService;
        }

        /// <summary>
        /// Applies the given return profile ID to the shipment
        /// </summary>
        public void ApplyReturnProfile(ShipmentEntity shipment, long returnProfileID)
        {
            IShippingProfileEntity returnProfile = ShippingProfileManager.GetProfileReadOnly(returnProfileID);
            if (returnProfile != null)
            {
                shippingProfileService.Get(returnProfile).Apply(shipment);
            }
            else
            {
                throw new NotFoundException("The selected return profile could not be found.");
            }
        }

        /// <summary>
        /// Creates a new auto return shipments
        /// </summary>
        public ShipmentEntity CreateReturn(ShipmentEntity shipment)
        {
            // Create new auto return shipment
            return shippingManager.CreateReturnShipment(shipment);
        }
    }
}