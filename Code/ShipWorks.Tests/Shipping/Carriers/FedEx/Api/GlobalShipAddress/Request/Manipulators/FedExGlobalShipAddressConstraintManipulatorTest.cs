using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    public class FedExGlobalShipAddressConstraintManipulatorTest
    {
        private Mock<CarrierRequest> mockCarrierRequest;

        private SearchLocationsRequest request;

        private ShipmentEntity shipmentEntity;

        private FedExGlobalShipAddressConstraintManipulator testObject;

        public FedExGlobalShipAddressConstraintManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            request = new SearchLocationsRequest();

            mockCarrierRequest = new Mock<CarrierRequest>(null, shipmentEntity, request);

            testObject = new FedExGlobalShipAddressConstraintManipulator();
        }

        [Fact]
        public void Manipulate_RequestedHoldSerivceGround_ShipmentIsGround_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(SupportedRedirectToHoldServiceType.FEDEX_GROUND, request.Constraints.SupportedRedirectToHoldServices[0]);
        }

        [Fact]
        public void Manipulate_RequestedHoldSerivceGroundHome_ShipmentIsGroundHome_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;

            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(SupportedRedirectToHoldServiceType.FEDEX_GROUND_HOME_DELIVERY, request.Constraints.SupportedRedirectToHoldServices[0]);
        }

        [Fact]
        public void Manipulate_RequestedHoldSerivceExpress_ShipmentIs2Day_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx2Day;

            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(SupportedRedirectToHoldServiceType.FEDEX_EXPRESS, request.Constraints.SupportedRedirectToHoldServices[0]);
        }

        [Fact]
        public void Manipulate_MultipleMatchesSet_ShipmentIsValid_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(MultipleMatchesActionType.RETURN_ALL, request.MultipleMatchesAction);
            Assert.True(request.MultipleMatchesActionSpecified);
        }


    }
}
