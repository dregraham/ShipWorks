using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateVersionManipulatorTest
    {
        private FedExRateVersionManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;

        public FedExRateVersionManipulatorTest()
        {
            nativeRequest = new RateRequest { Version = new VersionId() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExRateVersionManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new RateReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsServiceIdToPmis()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((RateRequest) carrierRequest.Object.NativeRequest).Version;
            Assert.Equal("crs", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo22()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((RateRequest) carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(22, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((RateRequest) carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((RateRequest) carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
