using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators
{
    public class FedExCloseDateManipulatorTest
    {
        private FedExCloseDateManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private GroundCloseRequest nativeRequest;

        public FedExCloseDateManipulatorTest()
        {
            nativeRequest = new GroundCloseRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExCloseDateManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotGroundCloseRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SmartPostCloseRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }


        [Fact]
        public void Manipulate_SetsCloseDate_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.TimeUpToWhichShipmentsAreToBeClosed > DateTime.Now.AddSeconds(-2));
        }

        [Fact]
        public void Manipulate_CloseDateIsSpecified_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.TimeUpToWhichShipmentsAreToBeClosedSpecified);
        }
    }
}
