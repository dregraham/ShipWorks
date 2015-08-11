using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExShippingChargesManipulatorTest
    {
        private FedExShippingChargesManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity fedExAccount;

        public FedExShippingChargesManipulatorTest()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    ShippingChargesPayment = new Payment()
                    {
                        Payor = new Payor()
                        {
                            ResponsibleParty = new Party() { Address = new Address() }
                        }
                    }
                }
            };

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity() { PayorTransportType = (int)FedExPayorType.Sender, PayorTransportAccount = "789XYZ", PayorTransportName = "Peter Gibbons" }
            };

            // Use UK as country code since there is a path in manipulator code that explicitly sets country code to US
            fedExAccount = new FedExAccountEntity() { AccountNumber = "ABCD1234", CountryCode = "UK", FirstName = "Peter", LastName = "Griffin" };

            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(fedExAccount);

            testObject = new FedExShippingChargesManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            //carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullShippingChargesPayment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment.ShippingChargesPayment = null;
            //carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.ShippingChargesPayment);
        }

        [Fact]
        public void Manipulate_AccountsForNullPayor_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment.ShippingChargesPayment.Payor = null;
            //carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.ShippingChargesPayment.Payor);
        }

        [Fact]
        public void Manipulate_AccountsForNullResponsibleParty_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty = null;
            //carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty);
        }

        [Fact]
        public void Manipulate_AccountsForNullAddress_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Address = null;
            //carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Address);
        }

        [Fact]
        public void Manipulate_DelegatesToRequest_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Verify the account was obtained from the request
            carrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToSender_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(PaymentType.SENDER, configuredShippingCharges.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountNumber_WhenPaymentTypeIsSender_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(fedExAccount.AccountNumber, configuredShippingCharges.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountCountryCode_WhenPaymentTypeIsSender_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(fedExAccount.CountryCode, configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountNameAsContactName_WhenPaymentTypeIsSender_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Sender;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(fedExAccount.FirstName + " " + fedExAccount.LastName, configuredShippingCharges.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToRecipient_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Recipient;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(PaymentType.RECIPIENT, configuredShippingCharges.PaymentType);
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsRecipient_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Recipient;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorTransportAccount_WhenPaymentTypeIsRecipient_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Recipient;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(shipmentEntity.FedEx.PayorTransportAccount, configuredShippingCharges.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_AssignsContactPersonName_WhenPaymentTypeIsRecepient_Test()
        {
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Recipient;

            testObject.Manipulate(carrierRequest.Object);

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(shipmentEntity.FedEx.PayorTransportName, configuredShippingCharges.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToThirdParty_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.ThirdParty;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(PaymentType.THIRD_PARTY, configuredShippingCharges.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorTransportAccount_WhenPaymentTypeIsThirdParty_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.ThirdParty;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(shipmentEntity.FedEx.PayorTransportAccount, configuredShippingCharges.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_AssignsContactPersonName_WhenPaymentTypeIsThirdParty_Test()
        {
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.ThirdParty;

            testObject.Manipulate(carrierRequest.Object);

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(shipmentEntity.FedEx.PayorTransportName, configuredShippingCharges.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsThirdPartyAndPayCountryCodeIsEmpty_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.ThirdParty;
            shipmentEntity.FedEx.PayorDutiesCountryCode = string.Empty;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsThirdPartyAndPayCountryCodeIsNull_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.ThirdParty;
            shipmentEntity.FedEx.PayorDutiesCountryCode = null;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsCountryCode_WhenPaymentTypeIsThirdParty_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.ThirdParty;
            shipmentEntity.FedEx.PayorDutiesCountryCode = "CA";

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal("CA", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToCollect_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Collect;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal(PaymentType.COLLECT, configuredShippingCharges.PaymentType);
        }

        [Fact]
        public void Manipulate_ContactPersonNameIsNotNullOrEmpty_WhenPaymentTypeIsCollect_Test()
        {
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Collect;

            testObject.Manipulate(carrierRequest.Object);

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;

            if (configuredShippingCharges.Payor != null && configuredShippingCharges.Payor.ResponsibleParty.Contact != null)
            {
                // There is a chance that the contact will be null for the "collect" payment type which is fine
                // since we're not using it for the "collect" type so only perform this check if it has a value
                Assert.True(!string.IsNullOrEmpty(configuredShippingCharges.Payor.ResponsibleParty.Contact.PersonName));
            }
        }

        [Fact]
        public void Manipulate_AccountNumberIsNotNullOrEmpty_WhenPaymentTypeIsCollect_Test()
        {
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Collect;

            testObject.Manipulate(carrierRequest.Object);

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;

            if (configuredShippingCharges.Payor != null && configuredShippingCharges.Payor.ResponsibleParty.Contact != null)
            {
                // There is a chance that the contact will be null for the "collect" payment type which is fine
                // since we're not using it for the "collect" type so only perform this check if it has a value
                Assert.True(!string.IsNullOrEmpty(configuredShippingCharges.Payor.ResponsibleParty.AccountNumber));
            }
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsCollect_AndPayCountryCodeIsEmpty_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.Collect;
            shipmentEntity.FedEx.PayorDutiesCountryCode = string.Empty;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsCollect_AndPayCountryCodeIsNull_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.ThirdParty;
            shipmentEntity.FedEx.PayorDutiesCountryCode = null;

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsCountryCode_WhenPaymentTypeIsCollect_Test()
        {
            // Setup the fedex shipment payor type for the test
            shipmentEntity.FedEx.PayorTransportType = (int)FedExPayorType.ThirdParty;
            shipmentEntity.FedEx.PayorDutiesCountryCode = "GB";

            testObject.Manipulate((carrierRequest.Object));

            Payment configuredShippingCharges = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.ShippingChargesPayment;
            Assert.Equal("GB", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_ForUnknownPayorType_Test()
        {
            // Setup the fedex shipment payor type for the test by setting the type to an unsupported value
            shipmentEntity.FedEx.PayorTransportType = 23;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate((carrierRequest.Object)));
        }
    }
}
