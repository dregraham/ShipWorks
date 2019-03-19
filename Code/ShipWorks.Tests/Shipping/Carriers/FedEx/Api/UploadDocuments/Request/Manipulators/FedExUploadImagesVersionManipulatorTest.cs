using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;
using VersionId = ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument.VersionId;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators
{
    public class FedExUploadImagesVersionManipulatorTest
    {
        private FedExUploadImagesVersionManipulator testObject;
        private Mock<CarrierRequest> carrierRequest;
        private UploadImagesRequest nativeRequest;

        public FedExUploadImagesVersionManipulatorTest()
        {
            nativeRequest = new UploadImagesRequest { Version = new VersionId() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, nativeRequest);

            testObject = new FedExUploadImagesVersionManipulator();
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotUploadImagesRequest()
        {
            // Set up the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, new TrackRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsServiceID_ForUploadImages()
        {
            testObject.Manipulate(carrierRequest.Object);
            VersionId version = ((UploadImagesRequest) carrierRequest.Object.NativeRequest).Version;

            // This assertion is based on the FedEx development guide.
            // Please advice this before making changes.
            Assert.Equal("cdus", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajor_ForUploadImages()
        {
            testObject.Manipulate(carrierRequest.Object);
            VersionId version = ((UploadImagesRequest) carrierRequest.Object.NativeRequest).Version;

            // This assertion is based on the FedEx development guide.
            // Please advice this before making changes.
            Assert.Equal(5, version.Major);
        }

        [Fact]
        public void Manipulate_SetsIntermediate_ForUploadImages()
        {
            testObject.Manipulate(carrierRequest.Object);
            VersionId version = ((UploadImagesRequest) carrierRequest.Object.NativeRequest).Version;

            // This assertion is based on the FedEx development guide.
            // Please advice this before making changes.
            Assert.Equal(0, version.Intermediate);
        }

        [Fact]
        public void Manipulate_SetsMinor_ForUploadImages()
        {
            testObject.Manipulate(carrierRequest.Object);
            VersionId version = ((UploadImagesRequest) carrierRequest.Object.NativeRequest).Version;

            // This assertion is based on the FedEx development guide.
            // Please advice this before making changes.
            Assert.Equal(0, version.Minor);
        }
    }
}