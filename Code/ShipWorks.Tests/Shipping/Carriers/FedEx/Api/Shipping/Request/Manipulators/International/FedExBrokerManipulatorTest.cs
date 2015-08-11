using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    public class FedExBrokerManipulatorTest
    {
        private FedExBrokerManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
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

            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested() {SpecialServiceTypes = new ShipmentSpecialServiceType[0]},
                    CustomsClearanceDetail = new CustomsClearanceDetail() {Brokers = new BrokerDetail[] {new BrokerDetail()}}
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExBrokerManipulator();
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServiceTypes_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AccountsForEmptySpecialServiceTypes_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested_WhenBrokerIsNotEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = false;
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServiceTypes_WhenBrokerIsNotEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = false;
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AccountsForEmptySpecialServiceTypes_WhenBrokerIsNotEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = false;
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(0, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_BrokerArrayLengthIsOne_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, nativeRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Length);
        }

        [Fact]
        public void Manipulate_BrokerIsNotNull_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0]);
        }

        [Fact]
        public void Manipulate_BrokerAccountIsFedExBrokerAccount_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(shipmentEntity.FedEx.BrokerAccount, nativeRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Broker.AccountNumber);
        }

        [Fact]
        public void Manipulate_BrokerAddressIsNotNull_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            // Not inspecting the details of the address since this is deferred to another object
            Assert.IsNotNull(nativeRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Broker.Address);
        }

        [Fact]
        public void Manipulate_BrokerContactIsNotNull_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            // Not inspecting the details of the contact since this is deferred to another object
            Assert.IsNotNull(nativeRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Broker.Contact);
        }

        [Fact]
        public void Manipulate_BrokerTypeIsImport_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(BrokerType.IMPORT, nativeRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Type);
        }

        [Fact]
        public void Manipulate_BrokerTypeSpecifiedIsTrue_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].TypeSpecified);
        }

        [Fact]
        public void Manipulate_CustomClearanceDetailsIsNotNull_WhenBrokerIsEnabled_Test()
        {
            shipmentEntity.FedEx.BrokerEnabled = true;
            nativeRequest.RequestedShipment.CustomsClearanceDetail = null;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure that the customs detail gets added back to the request
            Assert.IsNotNull(nativeRequest.RequestedShipment.CustomsClearanceDetail);
        }

        [Fact]
        public void Manipulate_BrokerPhoneExtension_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(shipmentEntity.FedEx.BrokerPhoneExtension, nativeRequest.RequestedShipment.CustomsClearanceDetail.Brokers[0].Broker.Contact.PhoneExtension);
        }
    }
}
