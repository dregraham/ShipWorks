using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    [TestClass]
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

        [TestInitialize]
        public void Initialize()
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
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(versionCaptureCarrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVersionCaptureRequest_AndRequestIsNotRegisterUserRequest_AndRequestIsNotSubscriptionRequest_Test()
        {
            // Setup the native request to be an unexpected type
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SubscriptionReply());

            testObject.Manipulate(versionCaptureCarrierRequest.Object);
        }

        
        
        #region Version Capture Tests

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_AndRequestIsVersionCapture_Test()
        {
            // Only setup is  to set the detail to null value
            nativeVersionCapture.WebAuthenticationDetail = null;

            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            WebAuthenticationDetail detail = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_AndRequestIsVersionCapture_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            WebAuthenticationDetail detail = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        #endregion Version Capture Tests



        #region Register User Tests

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_AndRequestIsRegisterUser_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRegister.WebAuthenticationDetail = null;

            testObject.Manipulate(registerCarrierRequest.Object);

            WebAuthenticationDetail detail = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_AndRequestIsRegisterUser_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(registerCarrierRequest.Object);

            WebAuthenticationDetail detail = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_UserCredentialIsNull_WhenWebAuthenticationDetailsIsNotNull_AndRequestIsRegisterUser_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(registerCarrierRequest.Object);

            WebAuthenticationDetail detail = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNull(detail.UserCredential);
        }

        #endregion Register User Tests




        #region Subscription Tests

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull_AndRequestIsSubscription_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRegister.WebAuthenticationDetail = null;

            testObject.Manipulate(subscriptionCarrierRequest.Object);

            WebAuthenticationDetail detail = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        [TestMethod]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull_AndRequestIsSubscription_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            WebAuthenticationDetail detail = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).WebAuthenticationDetail;
            Assert.IsNotNull(detail);
        }

        #endregion Subscription Tests
    }
}
