using System;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExReturnsManipulatorTest
    {
        private FedExShipRequest shipRequest;

        private ShipmentEntity shipmentEntity;

        private FedExReturnsManipulator testObject;

        /// <summary>
        /// NativeRequest from shipRequest converted to ProcessShipmentRequest
        /// </summary>
        private ProcessShipmentRequest processShipmentRequest
        {
            get
            {
                return shipRequest.NativeRequest as ProcessShipmentRequest;
            }
        }

        public FedExReturnsManipulatorTest()
        {
            testObject = new FedExReturnsManipulator();

            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            shipmentEntity.ReturnShipment = true;
            shipmentEntity.FedEx.ReturnType = (int) FedExReturnType.EmailReturnLabel;

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();

            shipRequest = new FedExShipRequest(
                null,
                shipmentEntity,
                null,
                null,
                settingsRepository.Object,
                new ProcessShipmentRequest());
        }

        [Fact]
        public void Manipulate_ReturnNotAddedToRequest_ShipmentNotAReturn()
        {
            shipmentEntity.ReturnShipment = false;

            testObject.Manipulate(shipRequest);

            Assert.Null(processShipmentRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_ReturnTypePending_ShipmentEmailReturnLabel()
        {
            testObject.Manipulate(shipRequest);

            Assert.Equal(ReturnType.PENDING, processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType);
        }

        [Fact]
        public void Manipulate_ReturnTypePrintReturnLabel_ShipmentPrintReturnLabel()
        {
            shipmentEntity.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;

            testObject.Manipulate(shipRequest);

            Assert.Equal(ReturnType.PRINT_RETURN_LABEL, processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType);
        }

        [Fact]
        public void Manipulate_RmaReasonSet_ShipmentHasRmaReason()
        {
            string rmaReason = "test rma reason";

            shipmentEntity.FedEx.RmaReason = rmaReason;

            testObject.Manipulate(shipRequest);

            Assert.Equal(rmaReason, processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.Rma.Reason);
        }

        [Fact]
        public void Manipulate_RmaReasonNotSet_ShipmentHasNoRmaReason()
        {
            testObject.Manipulate(shipRequest);

            Assert.Null(processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.Rma);
        }

        [Fact]
        public void Manipulate_RmaNumberSet_ShipmentHasRmaNumber()
        {
            string rmaNumber = "test rma number";

            shipmentEntity.FedEx.RmaNumber = rmaNumber;

            testObject.Manipulate(shipRequest);

            Assert.Equal(1, processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.Count(x => x.CustomerReferenceType == CustomerReferenceType.RMA_ASSOCIATION && x.Value == rmaNumber));
        }

        [Fact]
        public void Manipulate_NoCustomerReferences_ShipmentHasNoRmaNumber()
        {
            testObject.Manipulate(shipRequest);

            Assert.Equal(0, processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.Count());
        }

        [Fact]
        public void Manipulate_NoEmailDetails_ShipmentPrintReturnLabel()
        {
            shipmentEntity.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;

            testObject.Manipulate(shipRequest);

            Assert.Null(processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail);
        }

        [Fact]
        public void Manipulate_NoPendingShipmentDetail_ShipmentPrintReturnLabel()
        {
            shipmentEntity.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;

            testObject.Manipulate(shipRequest);

            Assert.Null(processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail);
        }

        [Fact]
        public void Manipulate_MerchantPhoneNumberSet_ShipmentEmailReturnLabel()
        {
            testObject.Manipulate(shipRequest);

            Assert.Equal(
                shipmentEntity.OriginPhone,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.MerchantPhoneNumber);
        }

        [Fact]
        public void Manipulate_PendingShipmentTypeSetToEmail_ShipmentEmailReturnLabel()
        {
            testObject.Manipulate(shipRequest);

            Assert.Equal(
                PendingShipmentType.EMAIL,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail.Type);
        }

        [Fact]
        public void Manipulate_PendingShipmentExpirationSetToThirtyDaysOut_ShipmentEmailReturnLabel()
        {
            testObject.Manipulate(shipRequest);

            Assert.Equal(
                DateTime.Today.AddDays(30),
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail.ExpirationDate);
        }

        [Fact]
        public void Manipulate_SautrdayPickupSet_ShipmentEmailReturnLabelWithSaturdayPickup()
        {
            shipmentEntity.FedEx.ReturnSaturdayPickup = true;

            testObject.Manipulate(shipRequest);

            Assert.Equal(
                1,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices.Count(x => x == ReturnEMailAllowedSpecialServiceType.SATURDAY_PICKUP));
        }

        [Fact]
        public void Manipulate_SautrdayPickupNotSet_ShipmentEmailReturnLabelWithNoSaturdayPickup()
        {
            shipmentEntity.FedEx.ReturnSaturdayPickup = false;

            testObject.Manipulate(shipRequest);

            Assert.True(
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices == null ||
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices.Count(x => x == ReturnEMailAllowedSpecialServiceType.SATURDAY_PICKUP) == 0);
        }
    }
}
