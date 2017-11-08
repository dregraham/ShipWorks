using System;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the Shipper property of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRateShipperManipulator : IFedExRateRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) => true;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);

            ConfigureShipper(shipment, request);

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        public void InitializeRequest(RateRequest request) =>
            request.Ensure(x => x.RequestedShipment);

        /// <summary>
        /// Configures the shipper information on the native FedEx RateRequest based on the shipment's origin info.
        /// </summary>
        private static void ConfigureShipper(IShipmentEntity shipment, RateRequest request)
        {
            Address address = FedExRequestManipulatorUtilities.CreateAddress<Address>(shipment.OriginPerson);

            request.RequestedShipment.Shipper = new Party { Address = address };

            // Not using the ResidentialDeterminationService here since the determination is for the 
            // origin address and not the ship address. 
            DetermineAddressType(request.RequestedShipment.Shipper, shipment);
        }

        /// <summary>
        /// Determines whether the origin address is a residential address for the specified shipment. This is
        /// performed here and not in some other class since the residential status of the origin address only
        /// applies to FedEx shipments at this time.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">FedExAddressLookup is not a valid residential determination type for the shipment's origin address.</exception>
        private static void DetermineAddressType(Party shipper, IShipmentEntity shipment)
        {
            ResidentialDeterminationType residentialDeterminationType = (ResidentialDeterminationType) shipment.FedEx.OriginResidentialDetermination;
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
