using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExReferenceManipulatorTest
    {
        private FedExReferenceManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<IFedExShipmentTokenProcessor> tokenProcessor;

        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExReferenceManipulatorTest()
        {
            // Setup the carrier request that will be sent to the test object
            nativeRequest = new ProcessShipmentRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[1] { new RequestedPackageLineItem() }
                }
            };

            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);


            // Create the test object with the mocked processor and repository
            tokenProcessor = new Mock<IFedExShipmentTokenProcessor>();
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns("I have processed your tokens.");

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.IsInterapptiveUser).Returns(true);

            testObject = new FedExReferenceManipulator(tokenProcessor.Object, settingsRepository.Object);
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedPackageLineItems_Test()
        {
            // Setup the test by configuring the native request to have a null requested package line items
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForEmptyRequestedPackageLineItems_Test()
        {
            // Setup the test by configuring the native request to have an empty arrary for the requested 
            // package line items property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should have one item in the array
            Assert.Equal(1, nativeRequest.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [Fact]
        public void Manipulate_AccountsForNullCustomerReferencesArray_Test()
        {
            // Setup the test by configuring the native request to a null value for the customer references
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should have one item in the array
            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences);
        }

        #region Customer Reference Tests

        [Fact]
        public void Manipulate_AddsCustomerReferenceToRequest_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should be a customer reference
            List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(1, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.CUSTOMER_REFERENCE));
        }

        [Fact]
        public void Manipulate_AssignsReferenceCustomerToFedExShipment_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference customer
            Assert.False(string.IsNullOrEmpty(shipmentEntity.FedEx.ReferenceCustomer));
        }

        [Fact]
        public void Manipulate_DoesNotAddCustomerReferenceToRequest_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a customer reference
            List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.CUSTOMER_REFERENCE));
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceCustomerToFedExShipment_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference customer
            Assert.Equal(string.Empty, shipmentEntity.FedEx.ReferenceCustomer);
        }

        [Fact]
        public void Manipulate_DoesNotAddCustomerReferenceToRequest_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a customer reference
            List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.CUSTOMER_REFERENCE));
        }

        [Fact]
        public void Manipulate_AssignsReferenceCustomerToFedExShipment_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a value on the reference customer
            Assert.Equal(null, shipmentEntity.FedEx.ReferenceCustomer);
        }

        #endregion Customer Reference Tests


        #region Invoice Number Tests

        [Fact]
        public void Manipulate_AddsInvoiceNumberToRequest_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should be a invoice reference
            List<CustomerReference> InvoiceNumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(1, InvoiceNumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.INVOICE_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferenceInvoiceToFedExShipment_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference invoice
            Assert.False(string.IsNullOrEmpty(shipmentEntity.FedEx.ReferenceInvoice));
        }

        [Fact]
        public void Manipulate_DoesNotAddInvoiceNumberToRequest_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a invoice reference
            List<CustomerReference> InvoiceNumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, InvoiceNumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.INVOICE_NUMBER));
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceInvoiceToFedExShipment_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference invoice
            Assert.Equal(string.Empty, shipmentEntity.FedEx.ReferenceInvoice);
        }

        [Fact]
        public void Manipulate_DoesNotAddInvoiceNumberToRequest_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a invoice reference
            List<CustomerReference> InvoiceNumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, InvoiceNumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.INVOICE_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferenceInvoiceToFedExShipment_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a value on the reference invoice
            Assert.Equal(null, shipmentEntity.FedEx.ReferenceInvoice);
        }

        #endregion Invoice Number Tests


        #region PO Number Tests

        [Fact]
        public void Manipulate_AddsPONumberToRequest_WhenReferenceValueIsNotBlank_AndIsInterapptiveUserIsTrue_Test()
        {
            // No additional setup needed since repository was initialized to return true in Initialize method

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should be a PO reference
            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(1, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNotBlank_AndIsInterapptiveUserIsTrue_Test()
        {
            // No additional setup needed since repository was initialized to return true in Initialize method

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference PO
            Assert.False(string.IsNullOrEmpty(shipmentEntity.FedEx.ReferencePO));
        }

        [Fact]
        public void Manipulate_DoesNotAddPONumberToRequest_WhenReferenceValueIsEmptyString_AndIsInterapptiveUserIsTrue_Test()
        {
            // Setup the token processor to return an empty string. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);


            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a PO reference
            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferencePOToFedExShipment_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference PO
            Assert.Equal(string.Empty, shipmentEntity.FedEx.ReferencePO);
        }

        [Fact]
        public void Manipulate_DoesNotAddPONumberToRequest_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a PO reference
            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a value on the reference PO
            Assert.Equal(null, shipmentEntity.FedEx.ReferencePO);
        }

        [Fact]
        public void Manipulate_AddsPONumberToRequest_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should be a PO reference
            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(1, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // This should not be null
            Assert.NotNull(shipmentEntity.FedEx.ReferencePO);
        }

        [Fact]
        public void Manipulate_DoesNotAddPONumberToRequest_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string and setup the repository to return false for this test
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }
        #endregion PO Number Tests

        #region Shipment Integrity Tests

        [Fact]
        public void Manipulate_AssignsReferenceShipmentIntegrityToFedExShipment_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference customer
            Assert.False(string.IsNullOrEmpty(shipmentEntity.FedEx.ReferenceShipmentIntegrity));
        }

        [Fact]
        public void Manipulate_DoesNotAddShipmentIntegrityToRequest_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a customer reference
            List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.SHIPMENT_INTEGRITY));
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceShipmentIntegrityToFedExShipment_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference customer
            Assert.Equal(string.Empty, shipmentEntity.FedEx.ReferenceShipmentIntegrity);
        }

        [Fact]
        public void Manipulate_DoesNotAddShipmentIntegrityReferenceToRequest_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a customer reference
            List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.SHIPMENT_INTEGRITY));
        }

        [Fact]
        public void Manipulate_AssignsReferenceShipmentIntegrityToFedExShipment_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a value on the reference customer
            Assert.Equal(null, shipmentEntity.FedEx.ReferenceShipmentIntegrity);
        }

        #endregion Shipment Integrity Tests

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenReferenceExceedsThirtyCharacters_Test()
        {
            // Setup the token processor to return a string 31 characters long
            string lengthyString = new string('A', 31);
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(lengthyString);

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_DoesNotThrowFedExException_WhenReferenceIsThirtyCharacters_Test()
        {
            // Setup the token processor to return a string 30 characters long
            string lengthyString = new string('A', 30);
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(lengthyString);

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_DoesNotThrowFedExException_WhenReferenceIsLessThanThirtyCharacters_Test()
        {
            // Setup the token processor to return a string 30 characters long
            string lengthyString = new string('A', 29);
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(lengthyString);

            testObject.Manipulate(carrierRequest.Object);
        }

    }
}
