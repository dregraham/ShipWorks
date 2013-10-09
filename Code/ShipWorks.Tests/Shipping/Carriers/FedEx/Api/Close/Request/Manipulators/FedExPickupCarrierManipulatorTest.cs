using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators
{
    [TestClass]
    public class FedExPickupCarrierManipulatorTest
    {
        private FedExPickupCarrierManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private SmartPostCloseRequest nativeRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeRequest = new SmartPostCloseRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExPickupCarrierManipulator();
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotSmartPostCloseRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new GroundCloseRequest());

            testObject.Manipulate(carrierRequest.Object);
        }


        [TestMethod]
        public void Manipulate_SetsPickupCarrierToFXSP_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CarrierCodeType.FXSP, nativeRequest.PickUpCarrier);
        }

        [TestMethod]
        public void Manipulate_PickupCarrierIsSpecified_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.PickUpCarrierSpecified);
        }
    }
}
