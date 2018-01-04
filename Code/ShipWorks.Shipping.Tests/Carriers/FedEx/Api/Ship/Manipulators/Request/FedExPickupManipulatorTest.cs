using System;
using Autofac.Extras.Moq;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExPickupManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExPickupManipulator testObject;

        public FedExPickupManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment().AsFedEx().Build();
            shipment.FedEx.PackagingType = (int) FedExPackagingType.Box;
            shipment.ShipDate = DateTime.Now.AddDays(1);

            processShipmentRequest = new ProcessShipmentRequest();
            
            testObject = mock.Create<FedExPickupManipulator>();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.True(testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedDropoffRequestCourrier()
        {
            shipment.FedEx.DropoffType = (int) FedExDropoffType.RequestCourier;

            testObject.Manipulate(shipment, processShipmentRequest, 0);
            
            // Make sure we got a the same values back
            Assert.Equal(DropoffType.REQUEST_COURIER, processShipmentRequest.RequestedShipment.DropoffType);
        }

        [Fact]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedDropoffStation()
        {
            shipment.FedEx.DropoffType = (int)FedExDropoffType.Station;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Make sure we got a the same values back
            Assert.Equal(DropoffType.STATION, processShipmentRequest.RequestedShipment.DropoffType);
        }

        [Fact]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedShipDate()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Make sure we got a the same values back
            Assert.Equal(shipment.ShipDate, processShipmentRequest.RequestedShipment.ShipTimestamp);
        }
    }
}
