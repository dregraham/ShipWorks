using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExCertificationManipulatorTest
    {
        private FedExCertificationManipulator testObject;


        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<IFedExShipmentTokenProcessor> tokenProcessor;

        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
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
                FedEx = new FedExShipmentEntity() { ReferencePO = "//Order/OrderNumber"}
            };

            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.IsInterapptiveUser).Returns(true);

            tokenProcessor = new Mock<IFedExShipmentTokenProcessor>();
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns("Processed Token");

            testObject = new FedExCertificationManipulator(tokenProcessor.Object, settingsRepository.Object);
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
        public void Manipulate_AccountsForNullTransactionDetail_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.TransactionDetail = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The transaction detail property should be created now
            Assert.IsNotNull(nativeRequest.TransactionDetail);
        }

        [Fact]
        public void Manipulate_DelegatesToSettingsRepository_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            settingsRepository.Verify(r=> r.IsInterapptiveUser, Times.Once());
        }

        [Fact]
        public void Manipulate_DelegatesToTokenProcessor_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            tokenProcessor.Verify(p => p.ProcessTokens(shipmentEntity.FedEx.ReferencePO, shipmentEntity), Times.Once());
        }

        [Fact]
        public void Manipulate_AssignsTransactionId_WhenIsInterapptiveUserIsTrue_AndReferencePOIsNonEmptyString_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The token processor was mocked in the initializer to return "Processed Token"
            TransactionDetail transactionDetail = ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).TransactionDetail;
            Assert.AreEqual("Processed Token", transactionDetail.CustomerTransactionId);
        }

        [Fact]
        public void Manipulate_DoesNotAssignTransactionId_WhenIsInterapptiveUserIsFalse_Test()
        {
            // Setup the repository to say the user is not an interapptive user
            settingsRepository.Setup(r => r.IsInterapptiveUser).Returns(false);

            testObject.Manipulate(carrierRequest.Object);

            TransactionDetail transactionDetail = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).TransactionDetail;
            Assert.IsNull(transactionDetail.CustomerTransactionId);
        }

        [Fact]
        public void Manipulate_DoesNotAssignTransactionId_WhenReferencePOIsEmptyString_Test()
        {
            // Setup the shipment's reference PO
            carrierRequest.Object.ShipmentEntity.FedEx.ReferencePO = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            TransactionDetail transactionDetail = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).TransactionDetail;
            Assert.IsNull(transactionDetail.CustomerTransactionId);
        }

        [Fact]
        public void Manipulate_DoesNotAssignTransactionId_WhenReferencePOIsNull_Test()
        {
            // Setup the shipment's reference PO to be a null value
            shipmentEntity = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity() {ReferencePO = null}
            };
            
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            TransactionDetail transactionDetail = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).TransactionDetail;
            Assert.IsNull(transactionDetail.CustomerTransactionId);
        }
    }
}
