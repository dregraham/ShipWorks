﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ProcessShipmentRequest = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExServiceTypeManipulatorTest
    {
        private FedExServiceTypeManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExServiceTypeManipulator();
        }

        [TestMethod]
        public void Manipulate_FedExServiceTypeManipulator_ReturnsServiceType_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.ServiceType, ServiceType.PRIORITY_OVERNIGHT);
        }
    }
}
