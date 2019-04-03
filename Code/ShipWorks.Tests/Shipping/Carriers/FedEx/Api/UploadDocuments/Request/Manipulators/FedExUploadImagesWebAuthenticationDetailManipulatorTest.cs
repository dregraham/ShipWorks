using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;
using Xunit;
using WebAuthenticationDetail = ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument.WebAuthenticationDetail;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators
{
    public class FedExUploadImagesWebAuthenticationDetailManipulatorTest
    {
        private FedExUploadImagesWebAuthenticationDetailManipulator testObject;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;
        private Mock<CarrierRequest> carrierRequest;
        private UploadImagesRequest nativeRequest;

        public FedExUploadImagesWebAuthenticationDetailManipulatorTest()
        {
            shippingSettings = new ShippingSettingsEntity {FedExUsername = "username", FedExPassword = "password"};

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(a => a.GetShippingSettings()).Returns(shippingSettings);

            nativeRequest = new UploadImagesRequest {WebAuthenticationDetail = new WebAuthenticationDetail() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, nativeRequest);

            testObject = new FedExUploadImagesWebAuthenticationDetailManipulator(settingsRepository.Object);
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotUploadImagesRequest()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, new TrackRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationIsNull_ForUploadImages()
        {
            // Setup the detail to null
            nativeRequest.WebAuthenticationDetail = null;

            testObject.Manipulate(carrierRequest.Object);

            WebAuthenticationDetail detail = ((UploadImagesRequest) carrierRequest.Object.NativeRequest).WebAuthenticationDetail;

            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationIsNotNull_ForUploadImages()
        {
            testObject.Manipulate(carrierRequest.Object);

            WebAuthenticationDetail detail = ((UploadImagesRequest) carrierRequest.Object.NativeRequest).WebAuthenticationDetail;

            Assert.NotNull(detail);
        }
    }
}