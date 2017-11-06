using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExShippingVersionManipulatorTest
    {
        private FedExShippingVersionManipulator testObject;
        private readonly ShipmentEntity shipment;
        private ProcessShipmentRequest processShipmentRequest;

        public FedExShippingVersionManipulatorTest()
        {
            processShipmentRequest = new ProcessShipmentRequest { Version = new VersionId() };
            shipment = new ShipmentEntity();
            testObject = new FedExShippingVersionManipulator();
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
