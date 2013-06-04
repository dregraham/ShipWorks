using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators
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
            Contact contact = FedExApiCore.CreateContact<Contact>(new PersonAdapter(shipment, "Ship"));
            Address address = FedExApiCore.CreateAddress<Address>(new PersonAdapter(shipment, "Ship"));

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
