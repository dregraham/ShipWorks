using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Tests.Shared.Carriers.FedEx;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.GlobalShipAddress.Manipulators.Request
{
    public class FedExGlobalShipAddressConstraintManipulatorTest
    {
        private ShipmentEntity shipment;
        private FedExGlobalShipAddressConstraintManipulator testObject;

        public FedExGlobalShipAddressConstraintManipulatorTest()
        {
            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            testObject = new FedExGlobalShipAddressConstraintManipulator();
        }

        [Fact]
        public void Manipulate_RequestedHoldSerivceGround_ShipmentIsGround()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(SupportedRedirectToHoldServiceType.FEDEX_GROUND, result.Value.Constraints.SupportedRedirectToHoldServices[0]);
        }

        [Fact]
        public void Manipulate_RequestedHoldSerivceGroundHome_ShipmentIsGroundHome()
        {
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;

            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(SupportedRedirectToHoldServiceType.FEDEX_GROUND_HOME_DELIVERY, result.Value.Constraints.SupportedRedirectToHoldServices[0]);
        }

        [Fact]
        public void Manipulate_RequestedHoldSerivceExpress_ShipmentIs2Day()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedEx2Day;

            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(SupportedRedirectToHoldServiceType.FEDEX_EXPRESS, result.Value.Constraints.SupportedRedirectToHoldServices[0]);
        }

        [Fact]
        public void Manipulate_MultipleMatchesSet_ShipmentIsValid()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(MultipleMatchesActionType.RETURN_ALL, result.Value.MultipleMatchesAction);
            Assert.True(result.Value.MultipleMatchesActionSpecified);
        }


    }
}
