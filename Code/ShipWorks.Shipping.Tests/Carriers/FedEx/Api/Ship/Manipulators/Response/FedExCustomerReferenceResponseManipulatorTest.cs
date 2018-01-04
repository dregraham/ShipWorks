using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Response
{
    public class FedExCustomerReferenceResponseManipulatorTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ProcessShipmentRequest request;
        private readonly FedExCustomerReferenceResponseManipulator testObject;
        private readonly RequestedPackageLineItem lineItem;

        public FedExCustomerReferenceResponseManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            request = new ProcessShipmentRequest();
            lineItem = request.Ensure(x => x.RequestedShipment).EnsureAtLeastOne(x => x.RequestedPackageLineItems);

            testObject = mock.Create<FedExCustomerReferenceResponseManipulator>();
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceShipmentIntegrityToFedExShipment_WhenRequestIsNull()
        {
            var result = testObject.Manipulate(null, new ProcessShipmentRequest(), Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Null(result.Value.FedEx.ReferenceShipmentIntegrity);
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceShipmentIntegrityToFedExShipment_WhenRequestDoesNotHaveReference()
        {
            request.RequestedShipment.RequestedPackageLineItems = null;

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Null(result.Value.FedEx.ReferenceShipmentIntegrity);
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceShipmentIntegrityToFedExShipment_WhenReferenceDoesNotExist()
        {
            lineItem.Ensure(x => x.CustomerReferences);

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Null(result.Value.FedEx.ReferenceShipmentIntegrity);
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferencePOToFedExShipment_WhenReferenceDoesNotExist()
        {
            lineItem.Ensure(x => x.CustomerReferences);

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Null(result.Value.FedEx.ReferencePO);
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceInvoiceToFedExShipment_WhenReferenceDoesNotExist()
        {
            lineItem.Ensure(x => x.CustomerReferences);

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Null(result.Value.FedEx.ReferenceInvoice);
        }

        [Fact]
        public void Manipulate_DoesNotAssignReferenceCustomerToFedExShipment_WhenReferenceDoesNotExist()
        {
            lineItem.Ensure(x => x.CustomerReferences);

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Null(result.Value.FedEx.ReferenceCustomer);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Manipulate_DoesNotAssignReferenceShipmentIntegrityToFedExShipment_WhenReferenceValueIsEmptyString(string value)
        {
            lineItem.CustomerReferences = new[] { new CustomerReference { CustomerReferenceType = CustomerReferenceType.SHIPMENT_INTEGRITY, Value = value } };

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Equal(value, result.Value.FedEx.ReferenceShipmentIntegrity);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Manipulate_DoesNotAssignReferencePOToFedExShipment_WhenReferenceValueIsEmptyString(string value)
        {
            lineItem.CustomerReferences = new[] { new CustomerReference { CustomerReferenceType = CustomerReferenceType.P_O_NUMBER, Value = value } };

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Equal(value, result.Value.FedEx.ReferencePO);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Manipulate_DoesNotAssignReferenceInvoiceToFedExShipment_WhenReferenceValueIsEmptyString(string value)
        {
            lineItem.CustomerReferences = new[] { new CustomerReference { CustomerReferenceType = CustomerReferenceType.INVOICE_NUMBER, Value = value } };

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Equal(value, result.Value.FedEx.ReferenceInvoice);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Manipulate_DoesNotAssignReferenceCustomerToFedExShipment_WhenReferenceValueIsEmptyString(string value)
        {
            lineItem.CustomerReferences = new[] { new CustomerReference { CustomerReferenceType = CustomerReferenceType.CUSTOMER_REFERENCE, Value = value } };

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx(f => f.DoNotSetDefaults()).Build());

            Assert.Equal(value, result.Value.FedEx.ReferenceCustomer);
        }

        [Fact]
        public void Manipulate_AssignsReferencePOToFedExShipment_WhenReferenceValueIsNotBlank()
        {
            lineItem.CustomerReferences = new[] { new CustomerReference { CustomerReferenceType = CustomerReferenceType.P_O_NUMBER, Value = "foo" } };

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx().Build());

            Assert.Equal("foo", result.Value.FedEx.ReferencePO);
        }

        [Fact]
        public void Manipulate_AssignsReferenceShipmentIntegrityToFedExShipment_WhenReferenceValueIsNotBlank()
        {
            lineItem.CustomerReferences = new[] { new CustomerReference { CustomerReferenceType = CustomerReferenceType.SHIPMENT_INTEGRITY, Value = "foo" } };

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx().Build());

            Assert.Equal("foo", result.Value.FedEx.ReferenceShipmentIntegrity);
        }

        [Fact]
        public void Manipulate_AssignsReferenceCustomerToFedExShipment_WhenReferenceValueIsNotBlank()
        {
            lineItem.CustomerReferences = new[] { new CustomerReference { CustomerReferenceType = CustomerReferenceType.CUSTOMER_REFERENCE, Value = "foo" } };

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx().Build());

            Assert.Equal("foo", result.Value.FedEx.ReferenceCustomer);
        }

        [Fact]
        public void Manipulate_AssignsReferenceInvoiceToFedExShipment_WhenReferenceValueIsNotBlank()
        {
            lineItem.CustomerReferences = new[] { new CustomerReference { CustomerReferenceType = CustomerReferenceType.INVOICE_NUMBER, Value = "foo" } };

            var result = testObject.Manipulate(null, request, Create.Shipment().AsFedEx().Build());

            Assert.Equal("foo", result.Value.FedEx.ReferenceInvoice);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
