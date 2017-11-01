using System;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// A manipulator for configuring the recipient information of a rate request.
    /// </summary>
    public class FedExRateRecipientManipulator : IFedExRateRequestManipulator
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

            ConfigureRecipient(shipment, request);

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        public void InitializeRequest(RateRequest request) =>
            request.Ensure(x => x.RequestedShipment);

        /// <summary>
        /// Configures the recipient information on the native FedEx RateRequest based on the shipment's ship address info.
        /// </summary>
        private static void ConfigureRecipient(IShipmentEntity shipment, RateRequest request)
        {
            Address address = FedExRequestManipulatorUtilities.CreateAddress<Address>(shipment.ShipPerson);

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
            request.RequestedShipment.Recipient = recipient;
        }
    }
}
