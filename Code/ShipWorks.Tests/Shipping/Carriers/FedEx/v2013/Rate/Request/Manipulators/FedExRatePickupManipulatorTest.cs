﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Rate.Request.Manipulators
{
    [TestClass]
    public class FedExRatePickupManipulatorTest
    {
        private FedExRatePickupManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box;
            shipmentEntity.ShipDate = DateTime.Now.AddDays(1);

            nativeRequest = new RateRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRatePickupManipulator();
        }

        [TestMethod]
        public void Manipulate_FedExPickupManipulator_ReturnsRequestedDropoffRequestCourrier_Test()
        {
            shipmentEntity.FedEx.DropoffType = (int)FedExDropoffType.RequestCourier;

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
        public void Manipulate_DropoffTypeSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.IsTrue(nativeRequest.RequestedShipment.DropoffTypeSpecified);
        }
    }
}
