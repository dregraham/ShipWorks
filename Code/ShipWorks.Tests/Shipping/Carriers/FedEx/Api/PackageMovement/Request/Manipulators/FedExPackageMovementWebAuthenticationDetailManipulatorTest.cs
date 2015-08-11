using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators
{
    public class FedExPackageMovementWebAuthenticationDetailManipulatorTest
    {
        private FedExPackageMovementWebAuthenticationDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;
        private FedExSettings settings;

        private Mock<CarrierRequest> carrierRequest;
        private PostalCodeInquiryRequest nativeRequest;

        public FedExPackageMovementWebAuthenticationDetailManipulatorTest()
        {
            shippingSettings = new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            settings = new FedExSettings(settingsRepository.Object);

            nativeRequest = new PostalCodeInquiryRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);

            testObject = new FedExPackageMovementWebAuthenticationDetailManipulator(settings);
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ServiceAvailabilityRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.WebAuthenticationDetail = null;

            testObject.Manipulate(carrierRequest.Object);

            WebAuthenticationDetail detail = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(carrierRequest.Object);

            WebAuthenticationDetail detail = ((PostalCodeInquiryRequest)carrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }
    }
}
