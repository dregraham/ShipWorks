using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    [TestClass]
    public class FedExRegistrationVersionManipulatorTest
    {
        private FedExRegistrationVersionManipulator testObject;

        private Mock<CarrierRequest> versionCaptureCarrierRequest;
        private VersionCaptureRequest nativeVersionCapture;

        private Mock<CarrierRequest> registerCarrierRequest;
        private RegisterWebUserRequest nativeRegister;

        private Mock<CarrierRequest> subscriptionCarrierRequest;
        private SubscriptionRequest nativeSubscription;


        [TestInitialize]
        public void Initialize()
        {
            nativeVersionCapture = new VersionCaptureRequest { Version = new VersionId() };
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeVersionCapture);

            nativeRegister = new RegisterWebUserRequest { ClientDetail = new ClientDetail() };
            registerCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRegister);

            nativeSubscription = new SubscriptionRequest { ClientDetail = new ClientDetail() };
            subscriptionCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeSubscription);

            testObject = new FedExRegistrationVersionManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVersionCaptureRequest_AndNotRegisterWebCspUserRequest_AndNotSubscriptionRequest_Test()
        {
            // Setup the native request to be an unexpected type
            versionCaptureCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SubscriptionReply());

            testObject.Manipulate(versionCaptureCarrierRequest.Object);
        }


        #region Version Capture Request Tests

        [TestMethod]
        public void Manipulate_SetsServiceIdToFcas_AndRequestIsVersionCapture_Test()
        {
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            VersionId version = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("fcas", version.ServiceId);
        }

        [TestMethod]
        public void Manipulate_SetsMajorTo7_AndRequestIsVersionCapture_Test()
        {
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            VersionId version = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(7, version.Major);
        }

        [TestMethod]
        public void Manipulate_SetsMinorTo0_AndRequestIsVersionCapture_Test()
        {
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            VersionId version = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [TestMethod]
        public void Manipulate_SetsIntermediateTo0_AndRequestIsVersionCapture_Test()
        {
            testObject.Manipulate(versionCaptureCarrierRequest.Object);

            VersionId version = ((VersionCaptureRequest)versionCaptureCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }

        #endregion Version Capture Request Tests


        #region Registration Request Tests

        [TestMethod]
        public void Manipulate_SetsServiceIdToFcas_AndRequestIsRegistration_Test()
        {
            testObject.Manipulate(registerCarrierRequest.Object);

            VersionId version = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("fcas", version.ServiceId);
        }

        [TestMethod]
        public void Manipulate_SetsMajorTo7_AndRequestIsRegistration_Test()
        {
            testObject.Manipulate(registerCarrierRequest.Object);

            VersionId version = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(7, version.Major);
        }

        [TestMethod]
        public void Manipulate_SetsMinorTo0_AndRequestIsRegistration_Test()
        {
            testObject.Manipulate(registerCarrierRequest.Object);

            VersionId version = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [TestMethod]
        public void Manipulate_SetsIntermediateTo0_AndRequestIsRegistration_Test()
        {
            testObject.Manipulate(registerCarrierRequest.Object);

            VersionId version = ((RegisterWebUserRequest)registerCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }

        #endregion Registration Capture Request Tests



        #region Registration Request Tests

        [TestMethod]
        public void Manipulate_SetsServiceIdToFcas_AndRequestIsSubscription_Test()
        {
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            VersionId version = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual("fcas", version.ServiceId);
        }

        [TestMethod]
        public void Manipulate_SetsMajorTo7_AndRequestIsSubscription_Test()
        {
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            VersionId version = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(7, version.Major);
        }

        [TestMethod]
        public void Manipulate_SetsMinorTo0_AndRequestIsSubscription_Test()
        {
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            VersionId version = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Minor);
        }

        [TestMethod]
        public void Manipulate_SetsIntermediateTo0_AndRequestIsSubscription_Test()
        {
            testObject.Manipulate(subscriptionCarrierRequest.Object);

            VersionId version = ((SubscriptionRequest)subscriptionCarrierRequest.Object.NativeRequest).Version;
            Assert.AreEqual(0, version.Intermediate);
        }

        #endregion Registration Capture Request Tests


    }
}
