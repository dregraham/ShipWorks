using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateVersionManipulatorTest
    {
        private FedExRateVersionManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest rateRequest;

        public FedExRateVersionManipulatorTest()
        {
            rateRequest = new RateRequest { Version = new VersionId() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), rateRequest);

            testObject = new FedExRateVersionManipulator();
        }

        [Fact]
        public void Shouldapply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(null, FedExRateRequestOptions.None));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenRateRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null));
        }

        [Fact]
        public void Manipulate_SetsServiceIdToPmis()
        {
            testObject.Manipulate(null, rateRequest);

            VersionId version = ((RateRequest) carrierRequest.Object.NativeRequest).Version;
            Assert.Equal("crs", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo22()
        {
            testObject.Manipulate(null, rateRequest);

            VersionId version = ((RateRequest) carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(22, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0()
        {
            testObject.Manipulate(null, rateRequest);

            VersionId version = ((RateRequest) carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0()
        {
            testObject.Manipulate(null, rateRequest);

            VersionId version = ((RateRequest) carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
