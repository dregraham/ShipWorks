using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that manipulates the shipping charges
    /// of a IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExShippingChargesManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingChargesManipulator" /> class.
        /// </summary>
        public FedExShippingChargesManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingChargesManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExShippingChargesManipulator(FedExSettings fedExSettings) : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization 
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

            // Use the the FedEx account information associated with the request to populate 
            // the payment account and country code
            FedExAccountEntity fedExAccount = request.CarrierAccountEntity as FedExAccountEntity;
            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;
            
            // Use the FedEx account and shipment to create the shipping charges payment
            ConfigureShippingCharges(nativeRequest.RequestedShipment.ShippingChargesPayment, fedExShipment, fedExAccount);
        }

        /// <summary>
        /// Configures the shipping charges based on the FedEx shipment/account settings.
        /// </summary>
        /// <param name="shippingCharges">The shipping charges being configured.</param>
        /// <param name="fedExShipment">The FedEx shipment.</param>
        /// <param name="fedExAccount">The FedEx account.</param>
        private void ConfigureShippingCharges(Payment shippingCharges, FedExShipmentEntity fedExShipment, FedExAccountEntity fedExAccount)
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
            if (shippingCharges.Payor == null)
            {
                // We'll be manipulating properties of the payor, so make sure it's been created
                shippingCharges.Payor = new Payor();
            }

            if (shippingCharges.Payor.ResponsibleParty == null)
            {
                // We'll be manipulating properties of the responsible party, so make sure it's been created
                shippingCharges.Payor.ResponsibleParty = new Party();
            }

            if (shippingCharges.Payor.ResponsibleParty.Address == null)
            {
                // We'll be manipulating properties of the address, so make sure it's been created
                shippingCharges.Payor.ResponsibleParty.Address = new Address();
            }
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            if (nativeRequest.RequestedShipment.ShippingChargesPayment == null)
            {
                // We'll be manipulating properties of the shipping charge, so make sure it's been created
                nativeRequest.RequestedShipment.ShippingChargesPayment = new Payment();
            }
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
