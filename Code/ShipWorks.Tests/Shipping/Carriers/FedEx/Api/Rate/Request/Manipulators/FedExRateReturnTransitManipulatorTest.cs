using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRateReturnTransitManipulatorTest
    {
        private FedExRateReturnTransitManipulator testObject;

        private RateRequest nativeRequest;
        private Mock<CarrierRequest> carrierRequest;

        public FedExRateReturnTransitManipulatorTest()
        {
            nativeRequest = new RateRequest();

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            testObject = new FedExRateReturnTransitManipulator();
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
        public void Manipulate_ReturnTransitAndCommitIsTrue()
        {
            testObject.Manipulate(carrierRequest.Object);
            Assert.True(nativeRequest.ReturnTransitAndCommit);
        }

        [Fact]
        public void Manipulate_ReturnTransitAndCommitSpecifiedIsTrue()
        {
            testObject.Manipulate(carrierRequest.Object);
            Assert.True(nativeRequest.ReturnTransitAndCommitSpecified);
        }
    }
}
