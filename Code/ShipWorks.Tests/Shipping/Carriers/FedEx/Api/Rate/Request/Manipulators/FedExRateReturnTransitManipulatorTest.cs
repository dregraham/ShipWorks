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
        
        [TestInitialize]
        public void Initialize()
        {
            nativeRequest = new RateRequest();
            
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            testObject = new FedExRateReturnTransitManipulator();
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new RateReply());

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_ReturnTransitAndCommitIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);
            Assert.IsTrue(nativeRequest.ReturnTransitAndCommit);
        }

        [Fact]
        public void Manipulate_ReturnTransitAndCommitSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);
            Assert.IsTrue(nativeRequest.ReturnTransitAndCommitSpecified);
        }
    }
}
