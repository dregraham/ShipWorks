using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators
{
    public class FedExTrackingClientDetailManipulatorTest
    {
        private FedExTrackingClientDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;

        private Mock<CarrierRequest> trackingCarrierRequest;
        private TrackRequest nativeRequest;

        private FedExAccountEntity account;

        public FedExTrackingClientDetailManipulatorTest()
        {
            account = new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "67890" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(account);

            nativeRequest = new TrackRequest { ClientDetail = new ClientDetail() };
            trackingCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            trackingCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExTrackingClientDetailManipulator(settingsRepository.Object);
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
            trackingCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(trackingCarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotTrackingRequest_AndIsNotTrackingRequest_Test()
        {
            // Setup the native request to be an unexpected type
            trackingCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new TrackReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(trackingCarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount_ForTracking_Test()
        {
            testObject.Manipulate(trackingCarrierRequest.Object);

            trackingCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull_ForTracking_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.ClientDetail = null;

            testObject.Manipulate(trackingCarrierRequest.Object);

            ClientDetail detail = ((TrackRequest)trackingCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNotNull_ForTracking_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(trackingCarrierRequest.Object);

            ClientDetail detail = ((TrackRequest)trackingCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.NotNull(detail);
        }
    }
}
