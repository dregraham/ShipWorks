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
    public class FedExPickupCarrierManipulatorTest
    {
        private FedExPickupCarrierManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private SmartPostCloseRequest nativeRequest;

        public FedExPickupCarrierManipulatorTest()
        {
            nativeRequest = new SmartPostCloseRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExPickupCarrierManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotSmartPostCloseRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new GroundCloseRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsPickupCarrierToFXSP()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CarrierCodeType.FXSP, nativeRequest.PickUpCarrier);
        }

        [Fact]
        public void Manipulate_PickupCarrierIsSpecified()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.PickUpCarrierSpecified);
        }
    }
}
