using System;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding recipient information to the FedEx request
    /// </summary>
    public class FedExRecipientManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRecipientManipulator" /> class.
        /// </summary>
        public FedExRecipientManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRecipientManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExRecipientManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Add the Recipient info to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            // Get the contact and address for the shipment
            ShipmentEntity shipment = request.ShipmentEntity;
            Contact contact = FedExRequestManipulatorUtilities.CreateContact<Contact>(new PersonAdapter(shipment, "Ship"));
            Address address = FedExRequestManipulatorUtilities.CreateAddress<Address>(new PersonAdapter(shipment, "Ship"));

            if (address.CountryCode.Equals("PR", StringComparison.OrdinalIgnoreCase))
            {
                address.StateOrProvinceCode = "PR";
            }

            // Create the shipper
            Party recipient = new Party()
            {
                Contact = contact,
                Address = address
            };

            // Set residential info
            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipment))
            {
                recipient.Address.Residential = shipment.ResidentialResult;
                recipient.Address.ResidentialSpecified = true;
            }

            // Set the recipient on the requested shipment
            if (shipment.ReturnShipment)
            {
                requestedShipment.Shipper = recipient;
            }
            else
            {
                requestedShipment.Recipient = recipient;                
            }
        }
    }
}
