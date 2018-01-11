using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared.Carriers.FedEx;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExMasterTrackingManipulatorTest
    {
        private FedExMasterTrackingManipulator testObject;
        private readonly ShipmentEntity shipment;

        public FedExMasterTrackingManipulatorTest()
        {
            shipment = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            shipment.TrackingNumber = "foo";
            shipment.FedEx.MasterFormID = "bar";

            testObject = new FedExMasterTrackingManipulator();
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        public void ShouldApply_ReturnsAppropriateValue_ForGivenInput(int sequence, bool expected)
        {
            var result = testObject.ShouldApply(shipment, sequence);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_CorrectFormIdSet_FromShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 1);

            Assert.Equal("bar", result.Value.RequestedShipment.MasterTrackingId.FormId);
        }

        [Fact]
        public void Manipulate_CorrectTrackingIdSet_FromShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 1);

            Assert.Equal("foo", result.Value.RequestedShipment.MasterTrackingId.TrackingNumber);
        }
    }
}
