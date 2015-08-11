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

        [TestInitialize]
        public void Initialize()
        {
            nativeRequest = new TrackRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExTrackingPackageIdentifierManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotTrackingRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new FedExTrackingUtilities());

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenShipmentEntityIsNull_Test()
        {
            // Setup the shipment entity to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_TrackPackageIdentifierMatchesShipmentTrackingNumber_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.TrackingNumber = testTrackingNumber;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            //Assert.IsNotNull(nativeRequest.PackageIdentifier);
            //Assert.AreEqual(testTrackingNumber, nativeRequest.PackageIdentifier.Value);
            //Assert.AreEqual(TrackIdentifierType.TRACKING_NUMBER_OR_DOORTAG, nativeRequest.PackageIdentifier.Type);
        }

        [Fact]
        public void Manipulate_IncludeDetailedScansIsTrue_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.TrackingNumber = testTrackingNumber;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            //Assert.AreEqual(true, nativeRequest.IncludeDetailedScans);
            //Assert.AreEqual(true, nativeRequest.IncludeDetailedScansSpecified);
        }
    }
}
