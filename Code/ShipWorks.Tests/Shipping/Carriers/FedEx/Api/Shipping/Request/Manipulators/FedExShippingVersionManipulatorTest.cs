using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExShippingVersionManipulatorTest
    {
        private FedExShippingVersionManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;

        public FedExShippingVersionManipulatorTest()
        {
            nativeRequest = new ProcessShipmentRequest { Version = new VersionId() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExShippingVersionManipulator();
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsServiceIdToShip_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal("ship", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo12_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(17, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            VersionId version = ((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
