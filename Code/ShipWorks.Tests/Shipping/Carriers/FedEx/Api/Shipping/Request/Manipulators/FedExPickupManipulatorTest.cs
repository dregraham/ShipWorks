using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExPickupManipulatorTest
    {
        private FedExPickupManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExPickupManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box;
            shipmentEntity.ShipDate = DateTime.Now.AddDays(1);

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExPickupManipulator();
        }
        
        [Fact]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedDropoffRequestCourrier()
        {
            shipmentEntity.FedEx.DropoffType = (int) FedExDropoffType.RequestCourier;

            testObject.Manipulate(carrierRequest.Object);
            
            // Make sure we got a the same values back
            Assert.Equal(DropoffType.REQUEST_COURIER, nativeRequest.RequestedShipment.DropoffType);
        }

        [Fact]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedDropoffStation()
        {
            shipmentEntity.FedEx.DropoffType = (int)FedExDropoffType.Station;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(DropoffType.STATION, nativeRequest.RequestedShipment.DropoffType);
        }

        [Fact]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedShipDate()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(shipmentEntity.ShipDate, nativeRequest.RequestedShipment.ShipTimestamp);
        }
    }
}
