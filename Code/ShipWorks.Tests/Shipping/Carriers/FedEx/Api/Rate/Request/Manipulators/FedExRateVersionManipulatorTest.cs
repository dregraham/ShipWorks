using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    [TestClass]
    public class FedExRateVersionManipulatorTest
    {
        private FedExRateVersionManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeRequest = new RateRequest { Version = new VersionId() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExRateVersionManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new RateReply());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SetsServiceIdToPmis_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((RateRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("crs", version.ServiceId);
        }

        [TestMethod]
        public void Manipulate_SetsMajorTo18_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((RateRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(18, version.Major);
        }

        [TestMethod]
        public void Manipulate_SetsMinorTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((RateRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [TestMethod]
        public void Manipulate_SetsIntermediateTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((RateRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }
    }
}
