using System;
using Autofac.Extras.Moq;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request.International
{
    public class FedExBrokerManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExBrokerManipulator testObject;

        public FedExBrokerManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity()
                {
                    BrokerEnabled = true,
                    BrokerAccount = "123456",
                    BrokerCity = "St. Louis",
                    BrokerCompany = "ACME",
                    BrokerCountryCode = "US",
                    BrokerEmail = "someone@somewhere.com",
                    BrokerFirstName = "Broker",
                    BrokerLastName = "McBrokerson",
                    BrokerPhone = "555-555-5555",
                    BrokerPhoneExtension = "123",
                    BrokerPostalCode = "63102",
                    BrokerStateProvCode = "MO",
                    BrokerStreet1 = "1 Memorial Drive",
                    BrokerStreet2 = "Suite 2000"
                }
            };

            processShipmentRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested() { SpecialServiceTypes = new ShipmentSpecialServiceType[0] },
                    CustomsClearanceDetail = new CustomsClearanceDetail() { Brokers = new BrokerDetail[] { new BrokerDetail() } }
                }
            };

            testObject = mock.Create<FedExBrokerManipulator>();
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

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void ShouldApply_ReturnsCorrectValue(bool brokerEnabled, bool expectedValue)
        {
            shipment.FedEx.BrokerEnabled = brokerEnabled;
            Assert.Equal(expectedValue, testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // setup the test by setting the requested shipment to null
            processShipmentRequest.RequestedShipment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;
            processShipmentRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServiceTypes_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;
            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AccountsForEmptySpecialServiceTypes_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;
            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(1, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested_WhenBrokerIsNotEnabled()
        {
            shipment.FedEx.BrokerEnabled = false;
            processShipmentRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServiceTypes_WhenBrokerIsNotEnabled()
        {
            shipment.FedEx.BrokerEnabled = false;
            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AccountsForEmptySpecialServiceTypes_WhenBrokerIsNotEnabled()
        {
            shipment.FedEx.BrokerEnabled = false;
            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(0, processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_BrokerArrayLengthIsOne_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(1, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Length);
        }

        [Fact]
        public void Manipulate_BrokerIsNotNull_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.NotNull(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0]);
        }

        [Fact]
        public void Manipulate_BrokerAccountIsFedExBrokerAccount_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(shipment.FedEx.BrokerAccount, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Broker.AccountNumber);
        }

        [Fact]
        public void Manipulate_BrokerAddressIsNotNull_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Not inspecting the details of the address since this is deferred to another object
            Assert.NotNull(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Broker.Address);
        }

        [Fact]
        public void Manipulate_BrokerContactIsNotNull_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Not inspecting the details of the contact since this is deferred to another object
            Assert.NotNull(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Broker.Contact);
        }

        [Fact]
        public void Manipulate_BrokerTypeIsImport_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(BrokerType.IMPORT, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Type);
        }

        [Fact]
        public void Manipulate_BrokerTypeSpecifiedIsTrue_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomClearanceDetailsIsNotNull_WhenBrokerIsEnabled()
        {
            shipment.FedEx.BrokerEnabled = true;
            processShipmentRequest.RequestedShipment.CustomsClearanceDetail = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Make sure that the customs detail gets added back to the request
            Assert.NotNull(processShipmentRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_BrokerPhoneExtension()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(shipment.FedEx.BrokerPhoneExtension, processShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Broker.Contact.PhoneExtension);
        }
    }
}
