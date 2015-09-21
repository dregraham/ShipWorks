using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators
{
    public class FedExPackageMovementVersionManipulatorTest
    {

        private FedExPackageMovementVersionManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private PostalCodeInquiryRequest nativeRequest;

        public FedExPackageMovementVersionManipulatorTest()
        {
            nativeRequest = new PostalCodeInquiryRequest { Version = new VersionId() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExPackageMovementVersionManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ServiceAvailabilityRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsServiceIdToPmis_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal("pmis", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo5_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(5, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
