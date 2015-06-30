using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators
{
    [TestClass]
    public class FedExTrackingClientDetailManipulatorTest
    {
        private FedExTrackingClientDetailManipulator testObject;

        private Mock<CarrierRequest> trackingCarrierRequest;
        private TrackRequest nativeRequest;

        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity {AccountNumber = "12345", MeterNumber = "67890"};

            nativeRequest = new TrackRequest { ClientDetail = new ClientDetail() };
            trackingCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            trackingCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);
            
            testObject = new FedExTrackingClientDetailManipulator();
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
            trackingCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(trackingCarrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotTrackingRequest_AndIsNotTrackingRequest_Test()
        {
            // Setup the native request to be an unexpected type
            trackingCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new TrackReply());

            testObject.Manipulate(trackingCarrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_DelegatesToRequestForFedExAccount_ForTracking_Test()
        {
            testObject.Manipulate(trackingCarrierRequest.Object);

            trackingCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [TestMethod]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull_ForTracking_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.ClientDetail = null;

            testObject.Manipulate(trackingCarrierRequest.Object);

            ClientDetail detail = ((TrackRequest)trackingCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNotNull_ForTracking_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(trackingCarrierRequest.Object);

            ClientDetail detail = ((TrackRequest)trackingCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }
    }
}
