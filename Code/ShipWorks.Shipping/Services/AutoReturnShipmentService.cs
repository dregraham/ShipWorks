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
        /// An exception thrown while trying to create the return shipment
        /// </summary>
        public ShippingException ReturnException { get; private set; } = null;

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
                    ApplyReturnProfile(returnShipment, shipment.ReturnProfileID);
                }
                catch (NotFoundException ex)
                {
                    ReturnException = new ShippingException(ex.Message, ex);
                }
            }

            // If the return shipment's service is UPS SurePost, set an error
            // but don't overwrite an existing one
            if ((returnShipment.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools ||
                returnShipment.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip) &&
                (returnShipment.Ups.Service.Equals(17) ||
                returnShipment.Ups.Service.Equals(18) ||
                returnShipment.Ups.Service.Equals(19) ||
                returnShipment.Ups.Service.Equals(20)) &&
                ReturnException == null)
            {
                ReturnException = new ShippingException("UPS SurePost does not support returns.");
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
