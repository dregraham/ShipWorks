using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators
{
    public class FedExTrackingVersionManipulatorTest
    {
        private FedExTrackingVersionManipulator testObject;
        private Mock<CarrierRequest> carrierRequest;
        private TrackRequest nativeTrackingRequest;

        public FedExTrackingVersionManipulatorTest()
        {
            nativeTrackingRequest = new TrackRequest { Version = new VersionId() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeTrackingRequest);

            testObject = new FedExTrackingVersionManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotTrackingRequestOrTrackingRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new TrackReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsServiceIdToTrck_ForTracking()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((TrackRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal("trck", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo14_ForTracking()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((TrackRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(14, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_ForTracking()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((TrackRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0_ForTracking()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((TrackRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
