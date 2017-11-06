using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExShippingChargesManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExShippingChargesManipulator testObject;
        private readonly FedExAccountEntity fedExAccount;

        public FedExShippingChargesManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            processShipmentRequest = new ProcessShipmentRequest();
            processShipmentRequest.Ensure(r => r.RequestedShipment)
                .Ensure(r => r.ShippingChargesPayment)
                .Ensure(sc => sc.Payor)
                .Ensure(p => p.ResponsibleParty)
                .Ensure(rp => rp.Address); ;

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipment = Create.Shipment().AsFedEx().Build();
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Sender;
            shipment.FedEx.PayorTransportAccount = "789XYZ";
            shipment.FedEx.PayorTransportName = "Peter Gibbons";

            // Use UK as country code since there is a path in manipulator code that explicitly sets country code to US
            fedExAccount = new FedExAccountEntity() { AccountNumber = "ABCD1234", CountryCode = "UK", FirstName = "Peter", LastName = "Griffin" };
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(fedExAccount);

            testObject = mock.Create<FedExShippingChargesManipulator>();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null, new ProcessShipmentRequest(), 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenProcessShipmentRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null, 0));
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullShippingChargesPayment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.ShippingChargesPayment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.ShippingChargesPayment);
        }

        [Fact]
        public void Manipulate_AccountsForNullPayor()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor);
        }

        [Fact]
        public void Manipulate_AccountsForNullResponsibleParty()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty);
        }

        [Fact]
        public void Manipulate_AccountsForNullAddress()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Address = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Address);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToSender()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Sender;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(PaymentType.SENDER, configuredShippingCharges.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountNumber_WhenPaymentTypeIsSender()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Sender;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(fedExAccount.AccountNumber, configuredShippingCharges.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountCountryCode_WhenPaymentTypeIsSender()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Sender;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(fedExAccount.CountryCode, configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExAccountNameAsContactName_WhenPaymentTypeIsSender()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Sender;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(fedExAccount.FirstName + " " + fedExAccount.LastName, configuredShippingCharges.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToRecipient()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Recipient;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(PaymentType.RECIPIENT, configuredShippingCharges.PaymentType);
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsRecipient()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Recipient;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorTransportAccount_WhenPaymentTypeIsRecipient()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Recipient;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(shipment.FedEx.PayorTransportAccount, configuredShippingCharges.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_AssignsContactPersonName_WhenPaymentTypeIsRecepient()
        {
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Recipient;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(shipment.FedEx.PayorTransportName, configuredShippingCharges.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToThirdParty()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.ThirdParty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(PaymentType.THIRD_PARTY, configuredShippingCharges.PaymentType);
        }

        [Fact]
        public void Manipulate_UsesFedExShipmentPayorTransportAccount_WhenPaymentTypeIsThirdParty()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.ThirdParty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(shipment.FedEx.PayorTransportAccount, configuredShippingCharges.Payor.ResponsibleParty.AccountNumber);
        }

        [Fact]
        public void Manipulate_AssignsContactPersonName_WhenPaymentTypeIsThirdParty()
        {
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.ThirdParty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(shipment.FedEx.PayorTransportName, configuredShippingCharges.Payor.ResponsibleParty.Contact.PersonName);
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsThirdPartyAndPayCountryCodeIsEmpty()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.ThirdParty;
            shipment.FedEx.PayorDutiesCountryCode = string.Empty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsThirdPartyAndPayCountryCodeIsNull()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.ThirdParty;
            shipment.FedEx.PayorDutiesCountryCode = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsCountryCode_WhenPaymentTypeIsThirdParty()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.ThirdParty;
            shipment.FedEx.PayorDutiesCountryCode = "CA";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal("CA", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsPaymentTypeToCollect()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Collect;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal(PaymentType.COLLECT, configuredShippingCharges.PaymentType);
        }

        [Fact]
        public void Manipulate_ContactPersonNameIsNotNullOrEmpty_WhenPaymentTypeIsCollect()
        {
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Collect;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;

            if (configuredShippingCharges.Payor != null && configuredShippingCharges.Payor.ResponsibleParty.Contact != null)
            {
                // There is a chance that the contact will be null for the "collect" payment type which is fine
                // since we're not using it for the "collect" type so only perform this check if it has a value
                Assert.True(!string.IsNullOrEmpty(configuredShippingCharges.Payor.ResponsibleParty.Contact.PersonName));
            }
        }

        [Fact]
        public void Manipulate_AccountNumberIsNotNullOrEmpty_WhenPaymentTypeIsCollect()
        {
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Collect;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;

            if (configuredShippingCharges.Payor != null && configuredShippingCharges.Payor.ResponsibleParty.Contact != null)
            {
                // There is a chance that the contact will be null for the "collect" payment type which is fine
                // since we're not using it for the "collect" type so only perform this check if it has a value
                Assert.True(!string.IsNullOrEmpty(configuredShippingCharges.Payor.ResponsibleParty.AccountNumber));
            }
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsCollect_AndPayCountryCodeIsEmpty()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.Collect;
            shipment.FedEx.PayorDutiesCountryCode = string.Empty;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsCountryCodeToUS_WhenPaymentTypeIsCollect_AndPayCountryCodeIsNull()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.ThirdParty;
            shipment.FedEx.PayorDutiesCountryCode = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal("US", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_SetsCountryCode_WhenPaymentTypeIsCollect()
        {
            // Setup the fedex shipment payor type for the test
            shipment.FedEx.PayorTransportType = (int) FedExPayorType.ThirdParty;
            shipment.FedEx.PayorDutiesCountryCode = "GB";

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Payment configuredShippingCharges = processShipmentRequest.RequestedShipment.ShippingChargesPayment;
            Assert.Equal("GB", configuredShippingCharges.Payor.ResponsibleParty.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_ForUnknownPayorType()
        {
            // Setup the fedex shipment payor type for the test by setting the type to an unsupported value
            shipment.FedEx.PayorTransportType = 23;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }
    }
}
