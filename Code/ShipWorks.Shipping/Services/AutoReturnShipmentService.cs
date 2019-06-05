using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
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

            // Set an error if the return shipment's service doesn't allow returns,
            // but don't overwrite any existing error
            if (ReturnException == null)
            {
                try
                {
                    CheckReturnsAllowed(returnShipment);
                }
                catch (ShippingException ex)
                {
                    ReturnException = ex;
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

        /// <summary>
        /// Throws an exception if the service of the given shipment doesn't allow returns
        /// </summary>
        private void CheckReturnsAllowed(ShipmentEntity returnShipment)
        {
            // UPS
            if (returnShipment.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
            {
                UpsServiceType service = (UpsServiceType) returnShipment.Ups.Service;

                // SurePost
                if (UpsUtility.IsUpsSurePostService(service))
                {
                    throw new ShippingException("UPS SurePost does not support returns.");
                }

                // Mail Innovations
                if (UpsUtility.IsUpsMiService(service))
                {
                    throw new ShippingException("UPS Mail Innovations does not support returns.");
                }
            }
        }
    }
}
