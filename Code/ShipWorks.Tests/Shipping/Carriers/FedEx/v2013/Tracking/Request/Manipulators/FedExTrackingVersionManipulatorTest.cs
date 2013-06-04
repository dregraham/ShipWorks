using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Tracking.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Tracking.Request.Manipulators
{
    [TestClass]
    public class FedExTrackingVersionManipulatorTest
    {
        private FedExTrackingVersionManipulator testObject;
        private Mock<CarrierRequest> carrierRequest;
        private TrackRequest nativeTrackingRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeTrackingRequest = new TrackRequest { Version = new VersionId() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeTrackingRequest);

            testObject = new FedExTrackingVersionManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotTrackingRequestOrTrackingRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new TrackReply());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SetsServiceIdToTrck_ForTracking_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((TrackRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("trck", version.ServiceId);
        }

        [TestMethod]
        public void Manipulate_SetsMajorTo6_ForTracking_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((TrackRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(6, version.Major);
        }

        [TestMethod]
        public void Manipulate_SetsMinorTo0_ForTracking_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((TrackRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [TestMethod]
        public void Manipulate_SetsIntermediateTo0_ForTracking_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((TrackRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }
    }
}
