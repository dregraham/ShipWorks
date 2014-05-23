﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    [TestClass]
    public class FedExRateOneRateManipulatorTest
    {
        private FedExRateOneRateManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Intialize()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            nativeRequest = new RateRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested
                    {
                        SpecialServiceTypes = new ShipmentSpecialServiceType[0]
                    }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExRateOneRateManipulator();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new RateReply());

            testObject.Manipulate(carrierRequest.Object);
        }
        
        [TestMethod]
        public void Manipulate_AddsOneRateSpecialServiceType_WhenSpecialServiceTypeArrayIsEmpty_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0], ShipmentSpecialServiceType.FEDEX_ONE_RATE);
        }

        [TestMethod]
        public void Manipulate_AddsOneRateSpecialServiceType_AndRetainsExistingSpecialServices_WhenSpecialServiceTypeArrayIsEmpty_Test()
        {
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[1] { ShipmentSpecialServiceType.COD };

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(2, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
            Assert.AreEqual(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.FEDEX_ONE_RATE));
        }
    }
}
