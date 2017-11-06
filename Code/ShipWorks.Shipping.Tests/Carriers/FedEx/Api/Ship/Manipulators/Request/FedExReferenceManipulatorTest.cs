using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExReferenceManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExReferenceManipulator testObject;
        private readonly AutoMock mock;
        private Mock<IFedExShipmentTokenProcessor> tokenProcessor;

        public FedExReferenceManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            // Setup the carrier request that will be sent to the test object
            processShipmentRequest = new ProcessShipmentRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[1] { new RequestedPackageLineItem() }
                }
            };

            shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };
            
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.IsInterapptiveUser)
                .Returns(true);

            tokenProcessor = mock.Mock<IFedExShipmentTokenProcessor>();
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>()))
                .Returns("I have processed your tokens.");

            testObject = mock.Create<FedExReferenceManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(shipment, 0));
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
        public void Manipulate_AccountsForNullRequestedPackageLineItems()
        {
            // Setup the test by configuring the native request to have a null requested package line items
            // property and re-initialize the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.RequestedPackageLineItems = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested package line items property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForEmptyRequestedPackageLineItems()
        {
            // Setup the test by configuring the native request to have an empty arrary for the requested 
            // package line items property and re-initialize the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested package line items property should have one item in the array
            Assert.Equal(1, processShipmentRequest.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [Fact]
        public void Manipulate_AccountsForNullCustomerReferencesArray()
        {
            // Setup the test by configuring the native request to a null value for the customer references
            // property and re-initialize the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested package line items property should have one item in the array
            Assert.NotNull(processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences);
        }

        #region Customer Reference Tests

        [Fact]
        public void Manipulate_AddsCustomerReferenceToRequest_WhenReferenceValueIsNotBlank()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup in the initialize method to always return a value, so there should be a customer reference
            List<CustomerReference> customerReferences = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(1, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.CUSTOMER_REFERENCE));
        }

        [Fact]
        public void Manipulate_AssignsReferenceCustomerToFedExShipment_WhenReferenceValueIsNotBlank()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference customer
            Assert.False(string.IsNullOrEmpty(shipment.FedEx.ReferenceCustomer));
        }

        [Fact]
        public void Manipulate_DoesNotAddCustomerReferenceToRequest_WhenReferenceValueIsEmptyString()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return an empty string, so there should not be a customer reference
            List<CustomerReference> customerReferences = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.CUSTOMER_REFERENCE));
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceCustomerToFedExShipment_WhenReferenceValueIsEmptyString()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference customer
            Assert.Equal(string.Empty, shipment.FedEx.ReferenceCustomer);
        }

        [Fact]
        public void Manipulate_DoesNotAddCustomerReferenceToRequest_WhenReferenceValueIsNull()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return null, so there should not be a customer reference
            List<CustomerReference> customerReferences = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();

            Assert.Equal(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.CUSTOMER_REFERENCE));
        }

        [Fact]
        public void Manipulate_AssignsReferenceCustomerToFedExShipment_WhenReferenceValueIsNull()
        {
            // Setup the token processor to return null
            tokenProcessor = mock.Mock<IFedExShipmentTokenProcessor>();
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return null, so there should not be a value on the reference customer
            Assert.Equal(null, shipment.FedEx.ReferenceCustomer);
        }

        #endregion Customer Reference Tests


        #region Invoice Number Tests

        [Fact]
        public void Manipulate_AddsInvoiceNumberToRequest_WhenReferenceValueIsNotBlank()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup in the initialize method to always return a value, so there should be a invoice reference
            List<CustomerReference> InvoiceNumbers = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(1, InvoiceNumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.INVOICE_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferenceInvoiceToFedExShipment_WhenReferenceValueIsNotBlank()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference invoice
            Assert.False(string.IsNullOrEmpty(shipment.FedEx.ReferenceInvoice));
        }

        [Fact]
        public void Manipulate_DoesNotAddInvoiceNumberToRequest_WhenReferenceValueIsEmptyString()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return an empty string, so there should not be a invoice reference
            List<CustomerReference> InvoiceNumbers = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, InvoiceNumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.INVOICE_NUMBER));
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceInvoiceToFedExShipment_WhenReferenceValueIsEmptyString()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference invoice
            Assert.Equal(string.Empty, shipment.FedEx.ReferenceInvoice);
        }

        [Fact]
        public void Manipulate_DoesNotAddInvoiceNumberToRequest_WhenReferenceValueIsNull()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return null, so there should not be a invoice reference
            List<CustomerReference> InvoiceNumbers = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, InvoiceNumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.INVOICE_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferenceInvoiceToFedExShipment_WhenReferenceValueIsNull()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return null, so there should not be a value on the reference invoice
            Assert.Equal(null, shipment.FedEx.ReferenceInvoice);
        }

        #endregion Invoice Number Tests


        #region PO Number Tests

        [Fact]
        public void Manipulate_AddsPONumberToRequest_WhenReferenceValueIsNotBlank_AndIsInterapptiveUserIsTrue()
        {
            // No additional setup needed since repository was initialized to return true in Initialize method

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup in the initialize method to always return a value, so there should be a PO reference
            List<CustomerReference> PONumbers = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(1, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNotBlank_AndIsInterapptiveUserIsTrue()
        {
            // No additional setup needed since repository was initialized to return true in Initialize method

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference PO
            Assert.False(string.IsNullOrEmpty(shipment.FedEx.ReferencePO));
        }

        [Fact]
        public void Manipulate_DoesNotAddPONumberToRequest_WhenReferenceValueIsEmptyString_AndIsInterapptiveUserIsTrue()
        {
            // Setup the token processor to return an empty string. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);


            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return an empty string, so there should not be a PO reference
            List<CustomerReference> PONumbers = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferencePOToFedExShipment_WhenReferenceValueIsEmptyString()
        {
            // Setup the token processor to return an empty string. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference PO
            Assert.Equal(string.Empty, shipment.FedEx.ReferencePO);
        }

        [Fact]
        public void Manipulate_DoesNotAddPONumberToRequest_WhenReferenceValueIsNull()
        {
            // Setup the token processor to return null. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return null, so there should not be a PO reference
            List<CustomerReference> PONumbers = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNull()
        {
            // Setup the token processor to return null. No additional setup needed since 
            // repository was initialized to return true in Initialize method
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return null, so there should not be a value on the reference PO
            Assert.Equal(null, shipment.FedEx.ReferencePO);
        }

        [Fact]
        public void Manipulate_AddsPONumberToRequest_WhenReferenceValueIsNotBlank()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup in the initialize method to always return a value, so there should be a PO reference
            List<CustomerReference> PONumbers = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(1, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }

        [Fact]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNotBlank()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // This should not be null
            Assert.NotNull(shipment.FedEx.ReferencePO);
        }

        [Fact]
        public void Manipulate_DoesNotAddPONumberToRequest_WhenReferenceValueIsEmptyString()
        {
            // Setup the token processor to return an empty string and setup the repository to return false for this test
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            List<CustomerReference> PONumbers = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, PONumbers.Count(r => r.CustomerReferenceType == CustomerReferenceType.P_O_NUMBER));
        }
        #endregion PO Number Tests

        #region Shipment Integrity Tests

        [Fact]
        public void Manipulate_AssignsReferenceShipmentIntegrityToFedExShipment_WhenReferenceValueIsNotBlank()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup in the initialize method to always return a value, so there should 
            // be a value on the reference customer
            Assert.False(string.IsNullOrEmpty(shipment.FedEx.ReferenceShipmentIntegrity));
        }

        [Fact]
        public void Manipulate_DoesNotAddShipmentIntegrityToRequest_WhenReferenceValueIsEmptyString()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return an empty string, so there should not be a customer reference
            List<CustomerReference> customerReferences = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.SHIPMENT_INTEGRITY));
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceShipmentIntegrityToFedExShipment_WhenReferenceValueIsEmptyString()
        {
            // Setup the token processor to return an empty string
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(string.Empty);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return an empty string, so there should not be a value on the reference customer
            Assert.Equal(string.Empty, shipment.FedEx.ReferenceShipmentIntegrity);
        }

        [Fact]
        public void Manipulate_DoesNotAddShipmentIntegrityReferenceToRequest_WhenReferenceValueIsNull()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return null, so there should not be a customer reference
            List<CustomerReference> customerReferences = processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.ToList();
            Assert.Equal(0, customerReferences.Count(r => r.CustomerReferenceType == CustomerReferenceType.SHIPMENT_INTEGRITY));
        }

        [Fact]
        public void Manipulate_AssignsReferenceShipmentIntegrityToFedExShipment_WhenReferenceValueIsNull()
        {
            // Setup the token processor to return null
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns<string>(null);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Our token processor is setup to return null, so there should not be a value on the reference customer
            Assert.Equal(null, shipment.FedEx.ReferenceShipmentIntegrity);
        }

        #endregion Shipment Integrity Tests

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenReferenceExceedsThirtyCharacters()
        {
            // Setup the token processor to return a string 31 characters long
            string lengthyString = new string('A', 31);
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(lengthyString);

            Assert.Throws<FedExException>(() => testObject.Manipulate(shipment, processShipmentRequest, 0));
        }

        [Fact]
        public void Manipulate_DoesNotThrowFedExException_WhenReferenceIsThirtyCharacters()
        {
            // Setup the token processor to return a string 30 characters long
            string lengthyString = new string('A', 30);
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(lengthyString);

            testObject.Manipulate(shipment, processShipmentRequest, 0);
        }

        [Fact]
        public void Manipulate_DoesNotThrowFedExException_WhenReferenceIsLessThanThirtyCharacters()
        {
            // Setup the token processor to return a string 30 characters long
            string lengthyString = new string('A', 29);
            tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<ShipmentEntity>())).Returns(lengthyString);

            testObject.Manipulate(shipment, processShipmentRequest, 0);
        }

    }
}
