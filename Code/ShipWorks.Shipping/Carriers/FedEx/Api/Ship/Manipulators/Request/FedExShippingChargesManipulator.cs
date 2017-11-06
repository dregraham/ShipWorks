using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that manipulates the shipping charges
    /// of a IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExShippingChargesManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShippingChargesManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment) => true;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request, shipment);

            // Use the the FedEx account information associated with the request to populate 
            // the payment account and country code
            IFedExAccountEntity fedExAccount = settings.GetAccountReadOnly(shipment);
            IFedExShipmentEntity fedExShipment = shipment.FedEx;
            
            // Use the FedEx account and shipment to create the shipping charges payment
            ConfigureShippingCharges(request.RequestedShipment.ShippingChargesPayment, fedExShipment, fedExAccount);

            return request;
        }

        /// <summary>
        /// Configures the shipping charges based on the FedEx shipment/account settings.
        /// </summary>
        /// <param name="shippingCharges">The shipping charges being configured.</param>
        /// <param name="fedExShipment">The FedEx shipment.</param>
        /// <param name="fedExAccount">The FedEx account.</param>
        private void ConfigureShippingCharges(Payment shippingCharges, IFedExShipmentEntity fedExShipment, IFedExAccountEntity fedExAccount)
        {
            shippingCharges.PaymentType = GetApiPaymentType((FedExPayorType)fedExShipment.PayorTransportType);

            InitializePayor(shippingCharges);

            switch (shippingCharges.PaymentType)
            {
                case PaymentType.COLLECT:
                    shippingCharges.Payor.ResponsibleParty.Contact = new Contact();
                    shippingCharges.Payor.ResponsibleParty.Contact.PersonName = fedExAccount.FirstName + " " + fedExAccount.LastName;

                    shippingCharges.Payor.ResponsibleParty.AccountNumber = fedExAccount.AccountNumber;
                    shippingCharges.Payor.ResponsibleParty.Address.CountryCode = string.IsNullOrEmpty(fedExShipment.PayorDutiesCountryCode) ? "US" : fedExShipment.PayorDutiesCountryCode;
                    break;
                case PaymentType.SENDER:
                    shippingCharges.Payor.ResponsibleParty.Contact = new Contact();
                    shippingCharges.Payor.ResponsibleParty.Contact.PersonName = fedExAccount.FirstName + " " + fedExAccount.LastName;

                    shippingCharges.Payor.ResponsibleParty.AccountNumber = fedExAccount.AccountNumber;
                    shippingCharges.Payor.ResponsibleParty.Address.CountryCode = fedExAccount.CountryCode;
                    break;
                case PaymentType.RECIPIENT:
                case PaymentType.ACCOUNT:
                case PaymentType.THIRD_PARTY:

                    shippingCharges.Payor.ResponsibleParty.Contact = new Contact();
                    shippingCharges.Payor.ResponsibleParty.Contact.PersonName = fedExShipment.PayorTransportName;

                    // For this to work correctly, CountryCode needs to be specified (as opposed to Duties)
                    shippingCharges.Payor.ResponsibleParty.AccountNumber = fedExShipment.PayorTransportAccount;
                    shippingCharges.Payor.ResponsibleParty.Address.CountryCode = string.IsNullOrEmpty(fedExShipment.PayorDutiesCountryCode) ? "US" : fedExShipment.PayorDutiesCountryCode;
                    break;
            }
        }

        /// <summary>
        /// Initializes the payor.
        /// </summary>
        /// <param name="shippingCharges"></param>
        private void InitializePayor(Payment shippingCharges)
        {
            shippingCharges.Ensure(sc => sc.Payor)
                .Ensure(p => p.ResponsibleParty)
                .Ensure(rp => rp.Address);
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(ProcessShipmentRequest request, IShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(request, nameof(request));
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            request.Ensure(r => r.RequestedShipment)
                        .Ensure(r => r.ShippingChargesPayment);
        }

        /// <summary>
        /// Maps the FedExPayorType to the the FedEx PaymentType value.
        /// </summary>
        /// <param name="payorType">Type of the payor.</param>
        /// <returns>The FedEx PaymentType value.</returns>
        /// <exception cref="System.InvalidOperationException">Invalid FedEx payor type  + payorType</exception>
        private PaymentType GetApiPaymentType(FedExPayorType payorType)
        {
            switch (payorType)
            {
                case FedExPayorType.Sender: return PaymentType.SENDER;
                case FedExPayorType.Recipient: return PaymentType.RECIPIENT;
                case FedExPayorType.ThirdParty: return PaymentType.THIRD_PARTY;
                case FedExPayorType.Collect: return PaymentType.COLLECT;
            }

            throw new InvalidOperationException("Invalid FedEx payor type " + payorType);
        }
    }
}
