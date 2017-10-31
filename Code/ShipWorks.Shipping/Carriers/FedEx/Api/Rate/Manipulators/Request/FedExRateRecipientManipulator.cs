using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// A manipulator for configuring the recipient information of a rate request.
    /// </summary>
    public class FedExRateRecipientManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            // Get the contact and address for the shipment
            ShipmentEntity shipment = request.ShipmentEntity;
            ConfigureRecipient(nativeRequest, shipment);
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        public void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a RateRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (nativeRequest.RequestedShipment == null)
            {
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
        }

        /// <summary>
        /// Configures the recipient information on the native FedEx RateRequest based on the shipment's ship address info.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="shipment">The shipment.</param>
        private static void ConfigureRecipient(RateRequest nativeRequest, ShipmentEntity shipment)
        {
            PersonAdapter person = new PersonAdapter(shipment, "Ship");
            Address address = FedExRequestManipulatorUtilities.CreateAddress<Address> (person);

            if (address.CountryCode.Equals("PR", StringComparison.OrdinalIgnoreCase))
            {
                address.StateOrProvinceCode = "PR";
            }

            if (FedExRequestManipulatorUtilities.IsGuam(address.CountryCode) || FedExRequestManipulatorUtilities.IsGuam(address.StateOrProvinceCode))
            {
                address.StateOrProvinceCode = string.Empty;
                address.CountryCode = "GU";
            }

            // Use the address to create the party/recipient
            Party recipient = new Party { Address = address };

            // Set residential info
            if (ShipmentTypeManager.GetType(ShipmentTypeCode.FedEx).IsResidentialStatusRequired(shipment))
            {
                recipient.Address.Residential = shipment.ResidentialResult;
                recipient.Address.ResidentialSpecified = true;
            }

            // Set the recipient on the FedEx WSDL object
            nativeRequest.RequestedShipment.Recipient = recipient;
        }
    }
}
