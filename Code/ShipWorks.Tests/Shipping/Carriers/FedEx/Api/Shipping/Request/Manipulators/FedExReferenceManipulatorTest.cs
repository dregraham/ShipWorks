using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
    public class FedExReferenceManipulatorTest
    {
        private FedExReferenceManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<IFedExShipmentTokenProcessor> tokenProcessor;

        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            // Setup the carrier request that will be sent to the test object
            nativeRequest = new ProcessShipmentRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[1] {new RequestedPackageLineItem()}
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedPackageLineItems_Test()
        {
            // Setup the test by configuring the native request to have a null requested package line items
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [TestMethod]
        public void Manipulate_AccountsForEmptyRequestedPackageLineItems_Test()
        {
            // Setup the test by configuring the native request to have an empty arrary for the requested 
            // package line items property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should have one item in the array
            Assert.AreEqual(1, nativeRequest.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullCustomerReferencesArray_Test()
        {
            // Setup the test by configuring the native request to a null value for the customer references
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should have one item in the array
            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences);
        }
        
        #region Customer Reference Tests

        [TestMethod]
        public void Manipulate_AddsCustomerReferenceToRequest_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should be a customer reference
            List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(1, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.CUSTOMER_REFERENCE));
        }

        [TestMethod]
        public void Manipulate_AssignsReferenceCustomerToFedExShipment_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference customer
            Assert.IsFalse(string.IsNullOrEmpty(shipmentEntity.FedEx.ReferenceCustomer));
        }

        [TestMethod]
        public void Manipulate_DoesNotAddCustomerReferenceToRequest_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a customer reference
            List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.CUSTOMER_REFERENCE));
        }

        [TestMethod]
        public void Manipulate_DoesNotAssignReferenceCustomerToFedExShipment_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference customer
            Assert.AreEqual(string.Empty, shipmentEntity.FedEx.ReferenceCustomer);
        }

        [TestMethod]
        public void Manipulate_DoesNotAddCustomerReferenceToRequest_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a customer reference
            List<CustomerReference> customerReferences = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.CUSTOMER_REFERENCE));
        }

        [TestMethod]
        public void Manipulate_AssignsReferenceCustomerToFedExShipment_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a value on the reference customer
            Assert.AreEqual(null, shipmentEntity.FedEx.ReferenceCustomer);
        }

        #endregion Customer Reference Tests


        #region Invoice Number Tests

        [TestMethod]
        public void Manipulate_AddsInvoiceNumberToRequest_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should be a invoice reference
            List<CustomerReference> InvoiceNumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(1, InvoiceNumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.INVOICE_NUMBER));
        }

        [TestMethod]
        public void Manipulate_AssignsReferenceInvoiceToFedExShipment_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference invoice
            Assert.IsFalse(string.IsNullOrEmpty(shipmentEntity.FedEx.ReferenceInvoice));
        }

        [TestMethod]
        public void Manipulate_DoesNotAddInvoiceNumberToRequest_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a invoice reference
            List<CustomerReference> InvoiceNumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(0, InvoiceNumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.INVOICE_NUMBER));
        }

        [TestMethod]
        public void Manipulate_DoesNotAssignReferenceInvoiceToFedExShipment_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference invoice
            Assert.AreEqual(string.Empty, shipmentEntity.FedEx.ReferenceInvoice);
        }

        [TestMethod]
        public void Manipulate_DoesNotAddInvoiceNumberToRequest_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a invoice reference
            List<CustomerReference> InvoiceNumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(0, InvoiceNumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.INVOICE_NUMBER));
        }

        [TestMethod]
        public void Manipulate_AssignsReferenceInvoiceToFedExShipment_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a value on the reference invoice
            Assert.AreEqual(null, shipmentEntity.FedEx.ReferenceInvoice);
        }

        #endregion Invoice Number Tests


        #region PO Number Tests

        [TestMethod]
        public void Manipulate_AddsPONumberToRequest_WhenReferenceValueIsNotBlank_AndIsInterapptiveUserIsTrue_Test()
        {
            // No additional setup needed since repository was initialized to return true in Initialize method

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should be a PO reference
            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(1, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [TestMethod]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNotBlank_AndIsInterapptiveUserIsTrue_Test()
        {
            // No additional setup needed since repository was initialized to return true in Initialize method
            
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference PO
            Assert.IsFalse(string.IsNullOrEmpty(shipmentEntity.FedEx.ReferencePO));
        }

        [TestMethod]
        public void Manipulate_DoesNotAddPONumberToRequest_WhenReferenceValueIsEmptyString_AndIsInterapptiveUserIsTrue_Test()
        {
            // Setup the token processor to return an empty string. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);


            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a PO reference
            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(0, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [TestMethod]
        public void Manipulate_DoesNotAssignReferencePOToFedExShipment_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference PO
            Assert.AreEqual(string.Empty, shipmentEntity.FedEx.ReferencePO);
        }

        [TestMethod]
        public void Manipulate_DoesNotAddPONumberToRequest_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a PO reference
            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(0, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [TestMethod]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNull_Test()
        {
            // Setup the token processor to return null. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup to return null, so there should not be a value on the reference PO
            Assert.AreEqual(null, shipmentEntity.FedEx.ReferencePO);
        }

        [TestMethod]
        public void Manipulate_AddsPONumberToRequest_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Our token processor is setup in the initialize method to always return a value, so there should be a PO reference
            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(1, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [TestMethod]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNotBlank_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // This should not be null
            Assert.IsNotNull(shipmentEntity.FedEx.ReferencePO);
        }

        [TestMethod]
        public void Manipulate_DoesNotAddPONumberToRequest_WhenReferenceValueIsEmptyString_Test()
        {
            // Setup the token processor to return an empty string and setup the repository to return false for this test
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(carrierRequest.Object);

            List<CustomerReference> PONumbers = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.AreEqual(0, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }
        #endregion PO Number Tests

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenReferenceExceedsThirtyCharacters_Test()
        {
            // Setup the token processor to return a string 31 characters long
            string lengthyString = new string('A', 31);
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(lengthyString);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_DoesNotThrowFedExException_WhenReferenceIsThirtyCharacters_Test()
        {
            // Setup the token processor to return a string 30 characters long
            string lengthyString = new string('A', 30);
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(lengthyString);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_DoesNotThrowFedExException_WhenReferenceIsLessThanThirtyCharacters_Test()
        {
            // Setup the token processor to return a string 30 characters long
            string lengthyString = new string('A', 29);
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(lengthyString);

            testObject.Manipulate(carrierRequest.Object);
        }

    }
}
