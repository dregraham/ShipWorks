using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Moq;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ProcessShipmentRequest = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExPickupManipulatorTest
    {
        private FedExPickupManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box;
            shipmentEntity.ShipDate = DateTime.Now.AddDays(1);

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExPickupManipulator();
        }
        
        [TestMethod]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedDropoffRequestCourrier_Test()
        {
            shipmentEntity.FedEx.DropoffType = (int) FedExDropoffType.RequestCourier;

            testObject.Manipulate(carrierRequest.Object);
            
            // Make sure we got a the same values back
            Assert.AreEqual(DropoffType.REQUEST_COURIER, nativeRequest.RequestedShipment.DropoffType);
        }

        [TestMethod]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedDropoffStation_Test()
        {
            shipmentEntity.FedEx.DropoffType = (int)FedExDropoffType.Station;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(DropoffType.STATION, nativeRequest.RequestedShipment.DropoffType);
        }

        [TestMethod]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedShipDate_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(shipmentEntity.ShipDate, nativeRequest.RequestedShipment.ShipTimestamp);
        }
    }
}
