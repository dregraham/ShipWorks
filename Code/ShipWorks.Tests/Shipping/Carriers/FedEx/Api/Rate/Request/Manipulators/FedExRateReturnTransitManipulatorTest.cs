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
        public void Manipulate_ReturnTransitAndCommitIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);
            Assert.IsTrue(nativeRequest.ReturnTransitAndCommit);
        }

        [TestMethod]
        public void Manipulate_ReturnTransitAndCommitSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);
            Assert.IsTrue(nativeRequest.ReturnTransitAndCommitSpecified);
        }
    }
}
