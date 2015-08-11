using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    public class FedExRegistrationClientDetailManipulatorTest
    {
        private FedExRegistrationClientDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;        

        private Mock<CarrierRequest> versionCaptureCarrierRequest;
        private VersionCaptureRequest nativeVersionCapture;

        private Mock<CarrierRequest> registerCarrierRequest;
        private RegisterWebCspUserRequest nativeRegister;

        private Mock<CarrierRequest> subscriptionCarrierRequest;
        private SubscriptionRequest nativeSubscription;

        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "67890" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            
            nativeVersionCapture = new VersionCaptureRequest { ClientDetail = new ClientDetail()};

            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeVersionCapture);
            versionCaptureCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);


            nativeRegister = new RegisterWebCspUserRequest { ClientDetail = new ClientDetail() };
            registerCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRegister);
            registerCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);


            nativeSubscription = new SubscriptionRequest { ClientDetail = new ClientDetail() };
            subscriptionCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeSubscription);
            subscriptionCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            
            testObject = new FedExRegistrationClientDetailManipulator(settingsRepository.Object);
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
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(versionCaptureCarrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVersionCaptureRequest_AndNotRegisterWebCspUserRequest_AndNotSubscriptionRequest_Test()
        {
            // Setup the native request to be an unexpected type
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SubscriptionReply());

            testObject.Manipulate(versionCaptureCarrierRequest.Object);
        }



        #region Version Capture Request Tests

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount_WhenRequestIsVersionCapture_Test()
        {
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            versionCaptureCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenClientDetailIsNull__AndRequestIsVersionCapture_Test()
        {
            // Only setup is  to set the detail to null value
            nativeVersionCapture.ClientDetail = null;

            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            ClientDetail detail = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenClientDetailIsNotNull_AndRequestIsVersionCapture_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            ClientDetail detail = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        #endregion Version Capture Request Tests



        #region Register Request Tests

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount_WhenRequestIsRegisterCspUser_Test()
        {
            testObject.Manipulate(registerCarrierRequest.Object);

            registerCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenClientDetailIsNull__AndRequestIsRegisterCspUser_Test()
        {
            // Only setup is  to set the detail to null value
            nativeVersionCapture.ClientDetail = null;

            testObject.Manipulate(registerCarrierRequest.Object);

            ClientDetail detail = ((RegisterWebCspUserRequest)registerCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenClientDetailIsNotNull_AndRequestIsRegisterCspUser_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(registerCarrierRequest.Object);

            ClientDetail detail = ((RegisterWebCspUserRequest)registerCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        #endregion Register Request Tests


        #region Subscription Request Tests

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount_WhenRequestIsSubscription_Test()
        {
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            subscriptionCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenClientDetailIsNull__AndRequestIsSubscription_Test()
        {
            // Only setup is  to set the detail to null value
            nativeVersionCapture.ClientDetail = null;

            testObject.Manipulate(subscriptionCarrierRequest.Object);

            ClientDetail detail = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenClientDetailIsNotNull_AndRequestIsSubscription_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            ClientDetail detail = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        #endregion Subscription Request Tests
    }
}
