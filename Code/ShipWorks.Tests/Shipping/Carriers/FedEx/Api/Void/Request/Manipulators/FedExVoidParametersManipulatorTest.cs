using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators
{
    public class FedExVoidParametersManipulatorTest
    {
        private FedExVoidParametersManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private DeleteShipmentRequest nativeRequest;

        [TestInitialize]
        public void Initialize()
        {
            nativeRequest = new DeleteShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExVoidParametersManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVoidRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ProcessShipmentRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_ReturnsGroundTrackingType_WhenServiceIsGround_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.TrackingId.TrackingIdType == TrackingIdType.GROUND);
            Assert.IsTrue(nativeRequest.TrackingId.TrackingIdTypeSpecified);
        }

        [Fact]
        public void Manipulate_ReturnsGroundTrackingType_WhenServiceIsGroundHomeDelivery_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.TrackingId.TrackingIdType == TrackingIdType.GROUND);
            Assert.IsTrue(nativeRequest.TrackingId.TrackingIdTypeSpecified);
        }

        [Fact]
        public void Manipulate_ReturnsUSPSTrackingType_WhenServiceIsSmartPost_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.SmartPost;
            shipmentEntity.FedEx.SmartPostUspsApplicationId = "92";
            shipmentEntity.TrackingNumber = "92kjhkjh";

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.TrackingId.TrackingIdType == TrackingIdType.USPS);
            Assert.IsTrue(nativeRequest.TrackingId.TrackingIdTypeSpecified);
        }

        [Fact]
        public void Manipulate_ReturnsExpressTrackingType_WhenServiceIsFreight_Test()
        {
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx1DayFreight;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.TrackingId.TrackingIdType == TrackingIdType.EXPRESS);
            Assert.IsTrue(nativeRequest.TrackingId.TrackingIdTypeSpecified);
        }

        [Fact]
        public void Manipulate_ReturnsSpecifiedTrackingNumber_Test()
        {
            string trackingNumber = "92xxxxx";
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.TrackingNumber = trackingNumber;
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.TrackingId.TrackingNumber == trackingNumber);
        }

        [Fact]
        public void Manipulate_ReturnsSpecifiedDeletionControl_Test()
        {
            string trackingNumber = "92xxxxx";
            ShipmentEntity shipmentEntity = new ShipmentEntity();
            shipmentEntity.TrackingNumber = trackingNumber;
            shipmentEntity.FedEx = new FedExShipmentEntity();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.DeletionControl == DeletionControlType.DELETE_ALL_PACKAGES);
        }
    }
}
