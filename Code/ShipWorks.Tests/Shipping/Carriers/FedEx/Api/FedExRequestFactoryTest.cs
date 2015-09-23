using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Net;
using Xunit;
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
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class FedExRequestFactoryTest
    {
        private Mock<IFedExServiceGateway> fedExService;
        private Mock<IFedExServiceGateway> fedExOpenShipService;
        private Mock<ICarrierResponseFactory> responseFactory;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private FedExRequestFactory testObject;
        private Mock<IFedExShipmentTokenProcessor> tokenProcessor;
        private ShipmentEntity fedExShipment;

        public FedExRequestFactoryTest()
        {
            fedExService = new Mock<IFedExServiceGateway>();
            fedExOpenShipService = new Mock<IFedExServiceGateway>();
            responseFactory = new Mock<ICarrierResponseFactory>();

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity());

            tokenProcessor = new Mock<IFedExShipmentTokenProcessor>();

            // Use the "testing version" of the constructor
            testObject = new FedExRequestFactory(fedExService.Object, fedExOpenShipService.Object, settingsRepository.Object, tokenProcessor.Object, responseFactory.Object);

            fedExShipment = new ShipmentEntity() { FedEx = new FedExShipmentEntity() };
        }

        #region CreateShipRequest Tests

        [Fact]
        public void CreateShipRequest_ThrowsArgumentNullException_WhenShipmentEntityIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.CreateShipRequest(null));
        }

        [Fact]
        public void CreateShipRequest_ReturnsFedexShipRequest_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment);

            Assert.IsAssignableFrom<FedExShipRequest>(request);
        }

        [Fact]
        public void CreateShipRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(36, request.Manipulators.Count());
        }

        [Fact]
        public void CreateShipRequest_AddsShipperManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShipperManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsRecipientManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRecipientManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsShipmentSpecialServiceTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShipmentSpecialServiceTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsRateTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsLabelSpecificationManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExLabelSpecificationManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsTotalWeightManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExTotalWeightManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsTotalInsuredValueManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExTotalInsuredValueManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsFedExShippingChargesManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingChargesManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsFedExCertificationManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCertificationManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPackagingTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackagingTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPickupManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPickupManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExServiceTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExServiceTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPackageSpecialServicesManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageSpecialServicesManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_WebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExShippingClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExShippingVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExReferenceManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExReferenceManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPackageDetailsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageDetailsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExEmailNotificationsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExEmailNotificationsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExDryIceManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExDryIceManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPriorityAlertManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPriorityAlertManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExMasterTrackingManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExMasterTrackingManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExCodOptionsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCodOptionsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExCustomsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCustomsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExAdmissibilityManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExAdmissibilityManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExBrokerManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExBrokerManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExCommercialInvoiceManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCommercialInvoiceManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExHomeDeliveryManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExHomeDeliveryManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExHoldAtLocationManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExHoldAtLocationManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExFreightManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExFreightManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExDangerousGoodsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExDangerousGoodsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExReturnsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExReturnsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExTrafficInArmsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExTrafficInArmsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExOneRateManipulator_Test()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExOneRateManipulator)) == 1);
        }

        #endregion CreateShipRequest Tests

        #region CreateVersionCaptureRequest Tests

        [Fact]
        public void CreateVersionCaptureRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(3, request.Manipulators.Count());
        }

        [Fact]
        public void CreateVersionCaptureRequest_FedExRegistrationWebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateVersionCaptureRequest_FedExRegistrationClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateVersionCaptureRequest_FedExRegistrationVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        #endregion CreateVersionCaptureRequest Tests

        #region CreatePackageMovementRequest Tests

        [Fact]
        public void CreatePackageMovementRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(fedExShipment, new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(3, request.Manipulators.Count());
        }

        [Fact]
        public void CreatePackageMovementRequest_WebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(fedExShipment, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreatePackageMovementRequest_FedExPackageMovementClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(fedExShipment, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreatePackageMovementRequest_FedExPackageMovementVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(new ShipmentEntity(), new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementVersionManipulator)) == 1);
        }

        #endregion CreateVersionCaptureRequest Tests

        #region CreateGroundCloseRequest Tests

        [Fact]
        public void CreateGroundCloseRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateGroundCloseRequest_WebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateGroundCloseRequest_FedExPackageMovementClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateGroundCloseRequest_FedExPackageMovementVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateGroundCloseRequest_FedExCloseDateManipulator_Test()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseDateManipulator)) == 1);
        }

        #endregion CreateCloseRequest Tests

        #region CreateSmartPostCloseRequest Tests

        [Fact]
        public void CreateSmartPostCloseRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateSmartPostCloseRequest_WebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSmartPostCloseRequest_FedExPackageMovementClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSmartPostCloseRequest_FedExPackageMovementVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateSmartPostCloseRequest_FedExCloseDateManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPickupCarrierManipulator)) == 1);
        }

        #endregion CreateSmartPostCloseRequest Tests

        #region CreateRegisterCspUserRequest Tests

        [Fact]
        public void CreateRegisterCspUserRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExRegistrationWebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExRegistrationClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExRegistrationVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExCspContactManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCspContactManipulator)) == 1);
        }

        #endregion CreateRegisterCspUserRequest Tests

        #region CreateSubscriptionRequest Tests

        [Fact]
        public void CreateSubscriptionRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExRegistrationWebAuthenticationDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExRegistrationClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExRegistrationVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExSubscriberManipulator_Test()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExSubscriberManipulator)) == 1);
        }

        #endregion CreateSubscriptionRequest Tests

        #region CreateRateRequest Tests

        [Fact]
        public void CreateRateRequest_PopulatesManipulators_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.Equal(19, request.Manipulators.Count());
        }

        [Fact]
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

            Assert.Equal(21, request.Manipulators.Count());
        }

        [Fact]
        public void CreateRateRequest_PopulatesManipulators_WhenSpecializedManipulatorsIsEmpty_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), new List<ICarrierRequestManipulator>());

            Assert.Equal(19, request.Manipulators.Count());
        }

        [Fact]
        public void CreateRateRequest_ReturnsFedExRateRequest_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsAssignableFrom<FedExRateRequest>(request);
        }

        [Fact]
        public void CreateRateRequest_FedExReturnTransitManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateReturnTransitManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExShipperManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateShipperManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRecipientManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateRecipientManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExShipmentSpecialServiceTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateShipmentSpecialServiceTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExTotalInsuredValueManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateTotalInsuredValueManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExTotalWeightManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateTotalWeightManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRateTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateRateTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExPickupManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePickupManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExPackageDetailsManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePackageDetailsManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExPackageSpecialServicesManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePackageSpecialServicesManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExPackagingTypeManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePackagingTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRateClientDetailManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRateWebAuthenticationManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateWebAuthenticationManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRateVersionManipulator_Test()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateVersionManipulator)) == 1);
        }

        #endregion CreateRateRequest Tests

        #region CreateCertificateRequest Tests

        [Fact]
        public void CreateCertificateRequest_ReturnsCertficateReqeust_Test()
        {
            Mock<ICertificateInspector> inspector = new Mock<ICertificateInspector>();

            ICertificateRequest request = testObject.CreateCertificateRequest(inspector.Object);

            Assert.IsAssignableFrom<CertificateRequest>(request);
        }
        #endregion

        #region ChooseFedExServiceGateway Tests

        [Fact]
        public void ChooseFedExServiceGateway_ReturnsShipGateway_PrintReturn()
        {
            fedExShipment.ReturnShipment = true;
            fedExShipment.FedEx.ReturnType = (int)FedExReturnType.PrintReturnLabel;

            IFedExServiceGateway chosenGateway = testObject.ChooseFedExServiceGateway(fedExShipment);

            Assert.Equal(fedExService.Object, chosenGateway);
        }

        [Fact]
        public void ChooseFedExServiceGateway_ReturnsShipGateway_NotReturnButEmailReturnLabelReturnType()
        {
            fedExShipment.ReturnShipment = false;
            fedExShipment.FedEx.ReturnType = (int)FedExReturnType.EmailReturnLabel;

            IFedExServiceGateway chosenGateway = testObject.ChooseFedExServiceGateway(fedExShipment);

            Assert.Equal(fedExService.Object, chosenGateway);
        }

        [Fact]
        public void ChooseFedExServiceGateway_ReturnsOpenShipGateway_EmailReturn()
        {
            fedExShipment.ReturnShipment = true;
            fedExShipment.FedEx.ReturnType = (int)FedExReturnType.EmailReturnLabel;

            IFedExServiceGateway chosenGateway = testObject.ChooseFedExServiceGateway(fedExShipment);

            Assert.Equal(fedExOpenShipService.Object, chosenGateway);
        }

        #endregion ChooseFedExServiceGateway Tests
    }
}