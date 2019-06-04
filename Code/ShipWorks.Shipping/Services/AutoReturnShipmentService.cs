using System;
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
        /// An exception thrown while trying to apply a profile to the return shipment
        /// </summary>
        public ShippingException ApplyProfileException { get; private set; } = null;

        /// <summary>
        /// Creates a new auto return shipments
        /// </summary>
        public ShipmentEntity CreateReturn(ShipmentEntity shipment)
        {
            // Create new return shipment
            ShipmentEntity returnShipment = shippingManager.CreateReturnShipment(shipment);

            // Apply profile if needed
            if (shipment.ApplyReturnProfile)
            {
                try
                {
                    // Throw an exception if the service is UPS SurePost
                    if (shipment.Ups != null &&
                        (shipment.Ups.Service.Equals(17) ||
                        shipment.Ups.Service.Equals(18) ||
                        shipment.Ups.Service.Equals(19) ||
                        shipment.Ups.Service.Equals(20)))
                    {
                        throw new ShippingException("UPS SurePost does not support returns");
                    }
                    ApplyReturnProfile(returnShipment, shipment.ReturnProfileID);
                }
                catch (Exception ex)
                {
                    ApplyProfileException = new ShippingException(ex.Message, ex);
                }
            }

            return returnShipment;
        }

        /// <summary>
        /// Applies the given return profile ID to the shipment
        /// </summary>
        private void ApplyReturnProfile(ShipmentEntity shipment, long returnProfileID)
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
    }
}
