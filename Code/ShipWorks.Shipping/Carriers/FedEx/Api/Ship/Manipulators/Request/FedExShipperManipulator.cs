using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Manipulator for adding shipper information to the FedEx request
    /// </summary>
    public class FedExShipperManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipperManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => true;

        /// <summary>
        /// Add the Shipper info to the FedEx carrier request
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            request.Ensure(r => r.RequestedShipment);

            // Get the contact and address for the shipment
            Contact contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(shipment.OriginPerson);
            Address address = FedExRequestManipulatorUtilities.CreateAddress<Address>(shipment.OriginPerson);

            // Create the shipper
            Party shipper = new Party()
            {
                Contact = contact,
                Address = address
            };

            // Not using the ResidentialDeterminationService here since the determination is for the 
            // origin address and not the ship address. 
            return DetermineAddressType(shipper, shipment)
                .Map(() =>
                {
                    // Set the shipper on the requested shipment
                    if (shipment.ReturnShipment)
                    {
                        request.RequestedShipment.Recipient = shipper;
                    }
                    else
                    {
                        request.RequestedShipment.Shipper = shipper;
                    }

                    return request;
                });
        }

        /// <summary>
        /// Determines whether the origin address is a residential address for the specified shipment. This is
        /// performed here and not in some other class since the residential status of the origin address only
        /// applies to FedEx shipments at this time.
        /// </summary>
        /// <param name="shipper">The shipper.</param>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="System.InvalidOperationException">FedExAddressLookup is not a valid residential determination type for the shipment's origin address.</exception>
        private Result DetermineAddressType(Party shipper, IShipmentEntity shipment)
        {
            ResidentialDeterminationType residentialDeterminationType = (ResidentialDeterminationType) shipment.FedEx.OriginResidentialDetermination;
            if (residentialDeterminationType == ResidentialDeterminationType.FedExAddressLookup)
            {
                // Don't allow this determination type for the origin address so we cut down on service calls
                return new InvalidOperationException("FedExAddressLookup is not a valid residential determination type for the shipment's origin address.");
            }

            // Try to determine if the address is residential just based on the whether the residential value was selected
            bool isResidentialAddress = residentialDeterminationType == ResidentialDeterminationType.Residential;
            if (residentialDeterminationType == ResidentialDeterminationType.CommercialIfCompany)
            {
                // We need to inspect the origin address further to determine whether the address is a residential
                // address based on the presence of a company name
                isResidentialAddress = string.IsNullOrEmpty(shipment.OriginCompany);
            }

            shipper.Address.Residential = isResidentialAddress;
            shipper.Address.ResidentialSpecified = true;

            return Result.FromSuccess();
        }
    }
}
