using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExShippingVersionManipulatorTest
    {
        private FedExShippingVersionManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeRequest = new ProcessShipmentRequest { Version = new VersionId()};
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExShippingVersionManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new object());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SetsServiceIdToShip_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("ship", version.ServiceId);
        }

        [TestMethod]
        public void Manipulate_SetsMajorTo12_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(17, version.Major);
        }

        [TestMethod]
        public void Manipulate_SetsMinorTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [TestMethod]
        public void Manipulate_SetsIntermediateTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }
    }
}
