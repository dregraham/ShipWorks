using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExShippingVersionManipulatorTest
    {
        private FedExShippingVersionManipulator testObject;
        private readonly ShipmentEntity shipment;
        private readonly AutoMock mock;
        private ProcessShipmentRequest processShipmentRequest;

        public FedExShippingVersionManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            processShipmentRequest = new ProcessShipmentRequest { Version = new VersionId() };
            shipment = new ShipmentEntity();
            testObject = mock.Create<FedExShippingVersionManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_SetsServiceIdToShip()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            VersionId version = processShipmentRequest.Version;
            Assert.Equal("ship", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo21()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            VersionId version = processShipmentRequest.Version;
            Assert.Equal(21, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            VersionId version = processShipmentRequest.Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            VersionId version = processShipmentRequest.Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
