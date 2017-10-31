using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the Shipper property of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRateShipperManipulator : ICarrierRequestManipulator
    {

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            // Get the contact and address for the shipment
            ShipmentEntity shipment = request.ShipmentEntity;
            ConfigureShipper(nativeRequest, shipment);
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
        /// Configures the shipper information on the native FedEx RateRequest based on the shipment's origin info.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="shipment">The shipment.</param>
        private static void ConfigureShipper(RateRequest nativeRequest, ShipmentEntity shipment)
        {
            PersonAdapter person = new PersonAdapter(shipment, "Origin");
            Address address = FedExRequestManipulatorUtilities.CreateAddress<Address>(person);

            nativeRequest.RequestedShipment.Shipper = new Party { Address = address };

            // Not using the ResidentialDeterminationService here since the determination is for the 
            // origin address and not the ship address. 
            DetermineAddressType(nativeRequest.RequestedShipment.Shipper, shipment);
        }

        /// <summary>
        /// Determines whether the origin address is a residential address for the specified shipment. This is
        /// performed here and not in some other class since the residential status of the origin address only
        /// applies to FedEx shipments at this time.
        /// </summary>
        /// <param name="shipper">The shipper.</param>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="System.InvalidOperationException">FedExAddressLookup is not a valid residential determination type for the shipment's origin address.</exception>
        private static void DetermineAddressType(Party shipper, ShipmentEntity shipment)
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
                // address based on the presence of a company name
                isResidentialAddress = string.IsNullOrEmpty(shipment.OriginCompany);
            }

            shipper.Address.Residential = isResidentialAddress;
            shipper.Address.ResidentialSpecified = true;
        }

    }
}
