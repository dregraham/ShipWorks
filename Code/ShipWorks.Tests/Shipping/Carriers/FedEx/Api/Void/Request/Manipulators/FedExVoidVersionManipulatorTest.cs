using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators
{
    public class FedExVoidVersionManipulatorTest
    {
        private FedExVoidVersionManipulator testObject;
        private Mock<CarrierRequest> CarrierRequest;
        private DeleteShipmentRequest nativeVoidRequest;

        public FedExVoidVersionManipulatorTest()
        {
            nativeVoidRequest = new DeleteShipmentRequest { Version = new VersionId() };
            CarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeVoidRequest);

            testObject = new FedExVoidVersionManipulator();
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
            CarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(CarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVoidRequestOrVoidRequest_Test()
        {
            // Setup the native request to be an unexpected type
            CarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ShipmentReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(CarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsServiceIdToShip_ForVoid_Test()
        {
            testObject.Manipulate(CarrierRequest.Object);

            VersionId version = ((DeleteShipmentRequest)CarrierRequest.Object.NativeRequest).Version;
            Assert.Equal("ship", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo13_ForVoid_Test()
        {
            testObject.Manipulate(CarrierRequest.Object);

            VersionId version = ((DeleteShipmentRequest)CarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(15, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_ForVoid_Test()
        {
            testObject.Manipulate(CarrierRequest.Object);

            VersionId version = ((DeleteShipmentRequest)CarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0_ForVoid_Test()
        {
            testObject.Manipulate(CarrierRequest.Object);

            VersionId version = ((DeleteShipmentRequest)CarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
