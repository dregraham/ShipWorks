using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Xunit;

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

        public FedExCertificationManipulatorTest()
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
                FedEx = new FedExShipmentEntity() { ReferencePO = "//Order/OrderNumber" }
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
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullTransactionDetail()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.TransactionDetail = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The transaction detail property should be created now
            Assert.NotNull(nativeRequest.TransactionDetail);
        }

        [Fact]
        public void Manipulate_DelegatesToSettingsRepository()
        {
            testObject.Manipulate(carrierRequest.Object);

            settingsRepository.Verify(r => r.IsInterapptiveUser, Times.Once());
        }

        [Fact]
        public void Manipulate_DelegatesToTokenProcessor()
        {
            testObject.Manipulate(carrierRequest.Object);

            tokenProcessor.Verify(p => p.ProcessTokens(shipmentEntity.FedEx.ReferencePO, shipmentEntity), Times.Once());
        }

        [Fact]
        public void Manipulate_AssignsTransactionId_WhenIsInterapptiveUserIsTrue_AndReferencePOIsNonEmptyString()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The token processor was mocked in the initializer to return "Processed Token"
            TransactionDetail transactionDetail = ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).TransactionDetail;
            Assert.Equal("Processed Token", transactionDetail.CustomerTransactionId);
        }

        [Fact]
        public void Manipulate_DoesNotAssignTransactionId_WhenIsInterapptiveUserIsFalse()
        {
            // Setup the repository to say the user is not an interapptive user
            settingsRepository.Setup(r => r.IsInterapptiveUser).Returns(false);

            testObject.Manipulate(carrierRequest.Object);

            TransactionDetail transactionDetail = ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).TransactionDetail;
            Assert.Null(transactionDetail.CustomerTransactionId);
        }

        [Fact]
        public void Manipulate_DoesNotAssignTransactionId_WhenReferencePOIsEmptyString()
        {
            // Setup the shipment's reference PO
            carrierRequest.Object.ShipmentEntity.FedEx.ReferencePO = string.Empty;

            testObject.Manipulate(carrierRequest.Object);

            TransactionDetail transactionDetail = ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).TransactionDetail;
            Assert.Null(transactionDetail.CustomerTransactionId);
        }

        [Fact]
        public void Manipulate_DoesNotAssignTransactionId_WhenReferencePOIsNull()
        {
            // Setup the shipment's reference PO to be a null value
            shipmentEntity = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity() { ReferencePO = null }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            TransactionDetail transactionDetail = ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).TransactionDetail;
            Assert.Null(transactionDetail.CustomerTransactionId);
        }
    }
}
