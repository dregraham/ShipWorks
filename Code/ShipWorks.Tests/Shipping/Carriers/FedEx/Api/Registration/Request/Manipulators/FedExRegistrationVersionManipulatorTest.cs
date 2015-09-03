using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    public class FedExRegistrationVersionManipulatorTest
    {
        private FedExRegistrationVersionManipulator testObject;

        private Mock<CarrierRequest> versionCaptureCarrierRequest;
        private VersionCaptureRequest nativeVersionCapture;

        private Mock<CarrierRequest> registerCarrierRequest;
        private RegisterWebCspUserRequest nativeRegister;

        private Mock<CarrierRequest> subscriptionCarrierRequest;
        private SubscriptionRequest nativeSubscription;


        public FedExRegistrationVersionManipulatorTest()
        {
            nativeVersionCapture = new VersionCaptureRequest { Version = new VersionId() };
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeVersionCapture);

            nativeRegister = new RegisterWebCspUserRequest { ClientDetail = new ClientDetail() };
            registerCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRegister);

            nativeSubscription = new SubscriptionRequest { ClientDetail = new ClientDetail() };
            subscriptionCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeSubscription);

            testObject = new FedExRegistrationVersionManipulator();
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
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(versionCaptureCarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVersionCaptureRequest_AndNotRegisterWebCspUserRequest_AndNotSubscriptionRequest_Test()
        {
            // Setup the native request to be an unexpected type
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SubscriptionReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(versionCaptureCarrierRequest.Object));
        }


        #region Version Capture Request Tests

        [Fact]
        public void Manipulate_SetsServiceIdToFcas_AndRequestIsVersionCapture_Test()
        {
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            VersionId version = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal("fcas", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo3_AndRequestIsVersionCapture_Test()
        {
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            VersionId version = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(6, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_AndRequestIsVersionCapture_Test()
        {
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            VersionId version = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo1_AndRequestIsVersionCapture_Test()
        {
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            VersionId version = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(1, version.Intermediate);
        }

        #endregion Version Capture Request Tests


        #region Registration Request Tests

        [Fact]
        public void Manipulate_SetsServiceIdToFcas_AndRequestIsRegistration_Test()
        {
            testObject.Manipulate(registerCarrierRequest.Object);

            VersionId version = ((RegisterWebCspUserRequest)registerCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal("fcas", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo3_AndRequestIsRegistration_Test()
        {
            testObject.Manipulate(registerCarrierRequest.Object);

            VersionId version = ((RegisterWebCspUserRequest)registerCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(6, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_AndRequestIsRegistration_Test()
        {
            testObject.Manipulate(registerCarrierRequest.Object);

            VersionId version = ((RegisterWebCspUserRequest)registerCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo1_AndRequestIsRegistration_Test()
        {
            testObject.Manipulate(registerCarrierRequest.Object);

            VersionId version = ((RegisterWebCspUserRequest)registerCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(1, version.Intermediate);
        }

        #endregion Registration Capture Request Tests



        #region Registration Request Tests

        [Fact]
        public void Manipulate_SetsServiceIdToFcas_AndRequestIsSubscription_Test()
        {
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            VersionId version = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal("fcas", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo3_AndRequestIsSubscription_Test()
        {
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            VersionId version = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(6, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0_AndRequestIsSubscription_Test()
        {
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            VersionId version = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo1_AndRequestIsSubscription_Test()
        {
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            VersionId version = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).Version;
            Assert.Equal(1, version.Intermediate);
        }

        #endregion Registration Capture Request Tests


    }
}
