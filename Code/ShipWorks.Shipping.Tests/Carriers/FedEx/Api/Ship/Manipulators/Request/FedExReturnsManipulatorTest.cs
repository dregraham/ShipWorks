using System;
using System.Linq;
using Autofac.Extras.Moq;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExReturnsManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExReturnsManipulator testObject;

        public FedExReturnsManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment().AsFedEx().Build();
            shipment.ReturnShipment = true;
            shipment.FedEx.ReturnType = (int) FedExReturnType.EmailReturnLabel;

            processShipmentRequest = new ProcessShipmentRequest();

            testObject = mock.Create<FedExReturnsManipulator>();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void ShouldApply_ReturnsCorrectValue(bool isReturn, bool expectedValue)
        {
            shipment.ReturnShipment = isReturn;
            Assert.Equal(expectedValue, testObject.ShouldApply(shipment));
        }

        [Fact]
        public void Manipulate_ReturnNotAddedToRequest_ShipmentNotAReturn()
        {
            shipment.ReturnShipment = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_ReturnTypePending_ShipmentEmailReturnLabel()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(ReturnType.PENDING, processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType);
        }

        [Fact]
        public void Manipulate_ReturnTypePrintReturnLabel_ShipmentPrintReturnLabel()
        {
            shipment.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(ReturnType.PRINT_RETURN_LABEL, processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType);
        }

        [Fact]
        public void Manipulate_RmaReasonSet_ShipmentHasRmaReason()
        {
            string rmaReason = "test rma reason";

            shipment.FedEx.RmaReason = rmaReason;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(rmaReason, processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.Rma.Reason);
        }

        [Fact]
        public void Manipulate_RmaReasonNotSet_ShipmentHasNoRmaReason()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.Rma);
        }

        [Fact]
        public void Manipulate_RmaNumberSet_ShipmentHasRmaNumber()
        {
            string rmaNumber = "test rma number";

            shipment.FedEx.RmaNumber = rmaNumber;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(1, processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.Count(x => x.CustomerReferenceType == CustomerReferenceType.RMA_ASSOCIATION && x.Value == rmaNumber));
        }

        [Fact]
        public void Manipulate_NoCustomerReferences_ShipmentHasNoRmaNumber()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(0, processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.Count());
        }

        [Fact]
        public void Manipulate_NoEmailDetails_ShipmentPrintReturnLabel()
        {
            shipment.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail);
        }

        [Fact]
        public void Manipulate_NoPendingShipmentDetail_ShipmentPrintReturnLabel()
        {
            shipment.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Null(processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail);
        }

        [Fact]
        public void Manipulate_MerchantPhoneNumberSet_ShipmentEmailReturnLabel()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(
                shipment.OriginPhone,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.MerchantPhoneNumber);
        }

        [Fact]
        public void Manipulate_PendingShipmentTypeSetToEmail_ShipmentEmailReturnLabel()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(
                PendingShipmentType.EMAIL,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail.Type);
        }

        [Fact]
        public void Manipulate_PendingShipmentExpirationSetToThirtyDaysOut_ShipmentEmailReturnLabel()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(
                DateTime.Today.AddDays(30),
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail.ExpirationDate);
        }

        [Fact]
        public void Manipulate_SautrdayPickupSet_ShipmentEmailReturnLabelWithSaturdayPickup()
        {
            shipment.FedEx.ReturnSaturdayPickup = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.Equal(
                1,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices.Count(x => x == ReturnEMailAllowedSpecialServiceType.SATURDAY_PICKUP));
        }

        [Fact]
        public void Manipulate_SautrdayPickupNotSet_ShipmentEmailReturnLabelWithNoSaturdayPickup()
        {
            shipment.FedEx.ReturnSaturdayPickup = false;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            Assert.True(
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices == null ||
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices.Count(x => x == ReturnEMailAllowedSpecialServiceType.SATURDAY_PICKUP) == 0);
        }
    }
}
