using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    public class FedExRegistrationWebAuthenticationDetailManipulatorTest
    {
        private FedExRegistrationWebAuthenticationDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        private ShippingSettingsEntity shippingSettings;
        private FedExSettings settings;

        private Mock<CarrierRequest> versionCaptureCarrierRequest;
        private VersionCaptureRequest nativeVersionCapture;

        private Mock<CarrierRequest> registerCarrierRequest;
        private RegisterWebUserRequest nativeRegister;

        private Mock<CarrierRequest> subscriptionCarrierRequest;
        private SubscriptionRequest nativeSubscription;

        public FedExRegistrationWebAuthenticationDetailManipulatorTest()
        {
            shippingSettings = new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(shippingSettings);

            settings = new FedExSettings(settingsRepository.Object);

            nativeVersionCapture = new VersionCaptureRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeVersionCapture);

            nativeRegister = new RegisterWebUserRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };
            registerCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRegister);

            nativeSubscription = new SubscriptionRequest { ClientDetail = new ClientDetail() };
            subscriptionCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeSubscription);

            testObject = new FedExRegistrationWebAuthenticationDetailManipulator(settings);
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
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(versionCaptureCarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVersionCaptureRequest_AndRequestIsNotRegisterUserRequest_AndRequestIsNotSubscriptionRequest()
        {
            // Setup the native request to be an unexpected type
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SubscriptionReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(versionCaptureCarrierRequest.Object));
        }



        #region Version Capture Tests

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_AndRequestIsVersionCapture()
        {
            // Only setup is  to set the detail to null value
            nativeVersionCapture.WebAuthenticationDetail = null;

            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            WebAuthenticationDetail detail = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_AndRequestIsVersionCapture()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            WebAuthenticationDetail detail = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        #endregion Version Capture Tests



        #region Register User Tests

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_AndRequestIsRegisterUser()
        {
            // Only setup is  to set the detail to null value
            nativeRegister.WebAuthenticationDetail = null;

            testObject.Manipulate(registerCarrierRequest.Object);

            WebAuthenticationDetail detail = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_AndRequestIsRegisterUser()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(registerCarrierRequest.Object);

            WebAuthenticationDetail detail = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_UserCredentialIsNull_WhenWebAuthenticationDetailsIsNotNull_AndRequestIsRegisterUser()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(registerCarrierRequest.Object);

            WebAuthenticationDetail detail = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.Null(detail.UserCredential);
        }

        #endregion Register User Tests




        #region Subscription Tests

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_AndRequestIsSubscription()
        {
            // Only setup is  to set the detail to null value
            nativeRegister.WebAuthenticationDetail = null;

            testObject.Manipulate(subscriptionCarrierRequest.Object);

            WebAuthenticationDetail detail = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_AndRequestIsSubscription()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            WebAuthenticationDetail detail = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        #endregion Subscription Tests
    }
}
