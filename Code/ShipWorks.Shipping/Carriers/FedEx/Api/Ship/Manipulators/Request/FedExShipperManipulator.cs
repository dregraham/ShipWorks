using System;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding shipper information to the FedEx request
    /// </summary>
    public class FedExShipperManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipperManipulator" /> class.
        /// </summary>
        public FedExShipperManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipperManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExShipperManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Add the Shipper info to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            // Get the contact and address for the shipment
            ShipmentEntity shipment = request.ShipmentEntity;
            Contact contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(new PersonAdapter(shipment, "Origin"));
            Address address = FedExRequestManipulatorUtilities.CreateAddress<Address>(new PersonAdapter(shipment, "Origin"));

            // Create the shipper
            Party shipper = new Party()
            {
                Contact = contact,
                Address = address
            };

            // Not using the ResidentialDeterminationService here since the determination is for the 
            // origin address and not the ship address. 
            DetermineAddressType(shipper, shipment);

            // Set the shipper on the requested shipment
            if (shipment.ReturnShipment)
            {
                requestedShipment.Recipient = shipper;
            }
            else
            {
                requestedShipment.Shipper = shipper;
            }
        }

        /// <summary>
        /// Determines whether the origin address is a residential address for the specified shipment. This is
        /// performed here and not in some other class since the residential status of the origin address only
        /// applies to FedEx shipments at this time.
        /// </summary>
        /// <param name="shipper">The shipper.</param>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="System.InvalidOperationException">FedExAddressLookup is not a valid residential determination type for the shipment's origin address.</exception>
        private void DetermineAddressType(Party shipper, ShipmentEntity shipment)
        {
            ResidentialDeterminationType residentialDeterminationType = (ResidentialDeterminationType)shipment.FedEx.OriginResidentialDetermination;
            if (residentialDeterminationType == ResidentialDeterminationType.FedExAddressLookup)
            {
                // Don't allow this determination type for the origin address so we cut down on service calls
                throw new InvalidOperationException("FedExAddressLookup is not a valid residential determination type for the shipment's origin address.");
            }

            // Try to determine if the address is residential just based on the whether the residential value was selected
            bool isResidentialAddress = residentialDeterminationType == ResidentialDeterminationType.Residential;
            if (residentialDeterminationType == ResidentialDeterminationType.CommercialIfCompany)
            {
                // We need to inspect the origin address further to determine whether the address is a residential
                // address based on the presense of a company name
                isResidentialAddress = string.IsNullOrEmpty(shipment.OriginCompany);
            }

            shipper.Address.Residential = isResidentialAddress;
            shipper.Address.ResidentialSpecified = true;
        }
    }
}
