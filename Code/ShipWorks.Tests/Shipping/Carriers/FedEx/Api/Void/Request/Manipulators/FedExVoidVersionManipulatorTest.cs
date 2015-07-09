using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators
{
    [TestClass]
    public class FedExVoidVersionManipulatorTest
    {
        private FedExVoidVersionManipulator testObject;
        private Mock<CarrierRequest> CarrierRequest;
        private DeleteShipmentRequest nativeVoidRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeVoidRequest = new DeleteShipmentRequest { Version = new VersionId() };
            CarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeVoidRequest);

            testObject = new FedExVoidVersionManipulator();
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
            CarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(CarrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVoidRequestOrVoidRequest_Test()
        {
            // Setup the native request to be an unexpected type
            CarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ShipmentReply());

            testObject.Manipulate(CarrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SetsServiceIdToShip_ForVoid_Test()
        {
            testObject.Manipulate(CarrierRequest.Object);

            VersionId version = ((DeleteShipmentRequest)CarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("ship", version.ServiceId);
        }

        [TestMethod]
        public void Manipulate_SetsMinorTo0_ForVoid_Test()
        {
            testObject.Manipulate(CarrierRequest.Object);

            VersionId version = ((DeleteShipmentRequest)CarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [TestMethod]
        public void Manipulate_SetsIntermediateTo0_ForVoid_Test()
        {
            testObject.Manipulate(CarrierRequest.Object);

            VersionId version = ((DeleteShipmentRequest)CarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }
    }
}
