using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateWebAuthenticationManipulatorTest
    {
        private FedExRateWebAuthenticationManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;
        private FedExSettings settings;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest rateRequest;

        public FedExRateWebAuthenticationManipulatorTest()
        {
            shippingSettings = new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            settings = new FedExSettings(settingsRepository.Object);

            rateRequest = new RateRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), rateRequest);

            testObject = new FedExRateWebAuthenticationManipulator(settings);
        }

        [Fact]
        public void Shouldapply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(null, FedExRateRequestOptions.None));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenRateRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null));
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull()
        {
            // Only setup is  to set the detail to null value
            rateRequest.WebAuthenticationDetail = null;

            testObject.Manipulate(null, rateRequest);

            WebAuthenticationDetail detail = ((RateRequest) carrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
            Assert.Equal(settings.UserCredentialsPassword, detail.UserCredential.Password);
            Assert.Equal(settings.UserCredentialsKey, detail.UserCredential.Key);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(null, rateRequest);

            WebAuthenticationDetail detail = ((RateRequest) carrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }
    }
}
