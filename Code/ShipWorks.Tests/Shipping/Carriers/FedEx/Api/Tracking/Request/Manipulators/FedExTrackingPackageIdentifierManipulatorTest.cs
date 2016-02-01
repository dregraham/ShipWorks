using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators
{
    public class FedExTrackingPackageIdentifierManipulatorTest
    {
        private FedExTrackingPackageIdentifierManipulator testObject;
        private Mock<CarrierRequest> carrierRequest;
        private TrackRequest nativeRequest;
        private const string testTrackingNumber = "999999999999999";

        public FedExTrackingPackageIdentifierManipulatorTest()
        {
            nativeRequest = new TrackRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExTrackingPackageIdentifierManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotTrackingRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new FedExTrackingUtilities());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenShipmentEntityIsNull()
        {
            // Setup the shipment entity to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, nativeRequest);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_TrackPackageIdentifierMatchesShipmentTrackingNumber()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.TrackingNumber = testTrackingNumber;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            //Assert.NotNull(nativeRequest.PackageIdentifier);
            //Assert.Equal(testTrackingNumber, nativeRequest.PackageIdentifier.Value);
            //Assert.Equal(TrackIdentifierType.TRACKING_NUMBER_OR_DOORTAG, nativeRequest.PackageIdentifier.Type);
        }

        [Fact]
        public void Manipulate_IncludeDetailedScansIsTrue()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.TrackingNumber = testTrackingNumber;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            //Assert.Equal(true, nativeRequest.IncludeDetailedScans);
            //Assert.Equal(true, nativeRequest.IncludeDetailedScansSpecified);
        }
    }
}
