using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators
{
    [TestClass]
    public class FedExCloseDateManipulatorTest
    {
        private FedExCloseDateManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private GroundCloseRequest nativeRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeRequest = new GroundCloseRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExCloseDateManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotGroundCloseRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SmartPostCloseRequest());

            testObject.Manipulate(carrierRequest.Object);
        }


        [TestMethod]
        public void Manipulate_SetsCloseDate_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.TimeUpToWhichShipmentsAreToBeClosed > DateTime.Now.AddSeconds(-2));
        }

        [TestMethod]
        public void Manipulate_CloseDateIsSpecified_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.TimeUpToWhichShipmentsAreToBeClosedSpecified);
        }
    }
}
