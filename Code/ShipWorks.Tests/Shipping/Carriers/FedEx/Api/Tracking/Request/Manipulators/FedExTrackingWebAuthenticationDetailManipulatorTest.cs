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
    public class FedExTrackingWebAuthenticationDetailManipulatorTest
    {
        private FedExTrackingWebAuthenticationDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;

        private Mock<CarrierRequest> CarrierRequest;
        private TrackRequest nativeRequest;

        public FedExTrackingWebAuthenticationDetailManipulatorTest()
        {
            shippingSettings = new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            nativeRequest = new TrackRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };
            CarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExTrackingWebAuthenticationDetailManipulator(settingsRepository.Object);
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotTrackRequest_Test()
        {
            // Setup the native request to be an unexpected type
            CarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new FedExAccountEntity());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(CarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_ForTracking_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.WebAuthenticationDetail = null;

            testObject.Manipulate(CarrierRequest.Object);

            WebAuthenticationDetail detail = ((TrackRequest)CarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_ForTracking_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(CarrierRequest.Object);

            WebAuthenticationDetail detail = ((TrackRequest)CarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }
    }
}
