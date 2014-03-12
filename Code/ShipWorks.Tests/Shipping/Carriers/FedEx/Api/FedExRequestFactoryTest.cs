using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    [TestClass]
    public class FedExRequestFactoryTest
    {
        private Mock<IFedExServiceGateway> fedExService;
        private Mock<ICarrierResponseFactory> responseFactory;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private FedExRequestFactory testObject;
        private Mock<IFedExShipmentTokenProcessor> tokenProcessor;

        [TestInitialize]
        public void Initialize()
        {
            fedExService = new Mock<IFedExServiceGateway>();
            responseFactory = new Mock<ICarrierResponseFactory>();

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity());

            tokenProcessor = new Mock<IFedExShipmentTokenProcessor>();

            // Use the "testing version" of the constructor
            testObject = new FedExRequestFactory(fedExService.Object, settingsRepository.Object, tokenProcessor.Object, responseFactory.Object);
        }

        #region CreateShipRequest Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateShipRequest_ThrowsArgumentNullException_WhenShipmentEntityIsNull_Test()
        {
            testObject.CreateShipRequest(null);
        }

        [TestMethod]
        public void CreateShipRequest_ReturnsFedexShipRequest_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity());

            Assert.IsInstanceOfType(request, typeof(FedExShipRequest));
        }

        [TestMethod]
        public void CreateShipRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.AreEqual(35, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateShipRequest_AddsShipperManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExShipperManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_AddsRecipientManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRecipientManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_AddsShipmentSpecialServiceTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExShipmentSpecialServiceTypeManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_AddsRateTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateTypeManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_AddsLabelSpecificationManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExLabelSpecificationManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_AddsTotalWeightManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExTotalWeightManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_AddsTotalInsuredValueManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExTotalInsuredValueManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_AddsFedExShippingChargesManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingChargesManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_AddsFedExCertificationManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCertificationManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExPackagingTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackagingTypeManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExPickupManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExPickupManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExServiceTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExServiceTypeManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExPackageSpecialServicesManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageSpecialServicesManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_WebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingWebAuthenticationDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExShippingClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingClientDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExShippingVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingVersionManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExReferenceManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExReferenceManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExPackageDetailsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageDetailsManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExEmailNotificationsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExEmailNotificationsManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExDryIceManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExDryIceManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExPriorityAlertManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExPriorityAlertManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExMasterTrackingManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExMasterTrackingManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExCodOptionsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCodOptionsManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExCustomsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCustomsManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExAdmissibilityManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExAdmissibilityManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExBrokerManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExBrokerManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExCommercialInvoiceManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCommercialInvoiceManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExHomeDeliveryManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExHomeDeliveryManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExHoldAtLocationManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExHoldAtLocationManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExFreightManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExFreightManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExDangerousGoodsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExDangerousGoodsManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExReturnsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExReturnsManipulator)) == 1);
        }

        [TestMethod]
        public void CreateShipRequest_FedExTrafficInArmsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(new ShipmentEntity()) as FedExShipRequest;

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExTrafficInArmsManipulator)) == 1);
        }

        #endregion CreateShipRequest Tests

        #region CreateVersionCaptureRequest Tests

        [TestMethod]
        public void CreateVersionCaptureRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(new ShipmentEntity(), string.Empty);

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.AreEqual(3, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateVersionCaptureRequest_FedExRegistrationWebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(new ShipmentEntity(), string.Empty);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateVersionCaptureRequest_FedExRegistrationClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(new ShipmentEntity(), string.Empty);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateVersionCaptureRequest_FedExRegistrationVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(new ShipmentEntity(), string.Empty);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        #endregion CreateVersionCaptureRequest Tests

        #region CreatePackageMovementRequest Tests

        [TestMethod]
        public void CreatePackageMovementRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(new ShipmentEntity(), new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.AreEqual(3, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreatePackageMovementRequest_WebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(new ShipmentEntity(), new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementWebAuthenticationDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreatePackageMovementRequest_FedExPackageMovementClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(new ShipmentEntity(), new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementClientDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreatePackageMovementRequest_FedExPackageMovementVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(new ShipmentEntity(), new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementVersionManipulator)) == 1);
        }

        #endregion CreateVersionCaptureRequest Tests

        #region CreateGroundCloseRequest Tests

        [TestMethod]
        public void CreateGroundCloseRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.AreEqual(4, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateGroundCloseRequest_WebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseWebAuthenticationDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateGroundCloseRequest_FedExPackageMovementClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseClientDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateGroundCloseRequest_FedExPackageMovementVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseVersionManipulator)) == 1);
        }

        [TestMethod]
        public void CreateGroundCloseRequest_FedExCloseDateManipulator_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseDateManipulator)) == 1);
        }

        #endregion CreateCloseRequest Tests

        #region CreateSmartPostCloseRequest Tests

        [TestMethod]
        public void CreateSmartPostCloseRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.AreEqual(4, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateSmartPostCloseRequest_WebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseWebAuthenticationDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateSmartPostCloseRequest_FedExPackageMovementClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseClientDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateSmartPostCloseRequest_FedExPackageMovementVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseVersionManipulator)) == 1);
        }

        [TestMethod]
        public void CreateSmartPostCloseRequest_FedExCloseDateManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExPickupCarrierManipulator)) == 1);
        }

        #endregion CreateSmartPostCloseRequest Tests

        #region CreateRegisterCspUserRequest Tests

        [TestMethod]
        public void CreateRegisterCspUserRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.AreEqual(4, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateRegisterCspUserRequest_FedExRegistrationWebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRegisterCspUserRequest_FedExRegistrationClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRegisterCspUserRequest_FedExRegistrationVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRegisterCspUserRequest_FedExCspContactManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExCspContactManipulator)) == 1);
        }

        #endregion CreateRegisterCspUserRequest Tests

        #region CreateSubscriptionRequest Tests

        [TestMethod]
        public void CreateSubscriptionRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.AreEqual(4, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateSubscriptionRequest_FedExRegistrationWebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateSubscriptionRequest_FedExRegistrationClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateSubscriptionRequest_FedExRegistrationVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        [TestMethod]
        public void CreateSubscriptionRequest_FedExSubscriberManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExSubscriberManipulator)) == 1);
        }

        #endregion CreateSubscriptionRequest Tests

        #region CreateRateRequest Tests

        [TestMethod]
        public void CreateRateRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.AreEqual(14, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateRateRequest_PopulatesManipulators_WithSpecializedManipulators_Test()
        {
            var specializedManipulator1 = new Mock<ICarrierRequestManipulator>();
            var specializedManipulator2 = new Mock<ICarrierRequestManipulator>();

            var specializedList = new List<ICarrierRequestManipulator>
            {
                specializedManipulator1.Object,
                specializedManipulator2.Object
            };

            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), specializedList);

            Assert.AreEqual(16, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateRateRequest_PopulatesManipulators_WhenSpecializedManipulatorsIsEmpty_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), new List<ICarrierRequestManipulator>());

            Assert.AreEqual(14, request.Manipulators.Count());
        }

        [TestMethod]
        public void CreateRateRequest_ReturnsFedExRateRequest_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsInstanceOfType(request, typeof(FedExRateRequest));
        }

        [TestMethod]
        public void CreateRateRequest_FedExReturnTransitManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateReturnTransitManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExShipperManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateShipperManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExRecipientManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateRecipientManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExShipmentSpecialServiceTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateShipmentSpecialServiceTypeManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExTotalInsuredValueManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateTotalInsuredValueManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExTotalWeightManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateTotalWeightManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExRateTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateRateTypeManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExPickupManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePickupManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExPackageDetailsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePackageDetailsManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExPackageSpecialServicesManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePackageSpecialServicesManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExPackagingTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePackagingTypeManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExRateClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateClientDetailManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExRateWebAuthenticationManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateWebAuthenticationManipulator)) == 1);
        }

        [TestMethod]
        public void CreateRateRequest_FedExRateVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsTrue(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateVersionManipulator)) == 1);
        }

        #endregion CreateRateRequest Tests

        #region CreateCertificateRequest Tests

        [TestMethod]
        public void CreateCertificateRequest_ReturnsCertficateReqeust_Test()
        {
            Mock<ICertificateInspector> inspector = new Mock<ICertificateInspector>();

            ICertificateRequest request = testObject.CreateCertificateRequest(inspector.Object);

            Assert.IsInstanceOfType(request, typeof(CertificateRequest));
        }
        #endregion
    }
}