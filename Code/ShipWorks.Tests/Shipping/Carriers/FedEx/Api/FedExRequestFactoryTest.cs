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
        public void CreateShipRequest_ThrowsArgumentNullException_WhenShipmentEntityIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.CreateShipRequest(null));
        }

        [Fact]
        public void CreateShipRequest_ReturnsFedexShipRequest()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment);

            Assert.IsAssignableFrom<FedExShipRequest>(request);
        }

        [Fact]
        public void CreateShipRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(36, request.Manipulators.Count());
        }

        [Fact]
        public void CreateShipRequest_AddsShipperManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShipperManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsRecipientManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRecipientManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsShipmentSpecialServiceTypeManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShipmentSpecialServiceTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsRateTypeManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsLabelSpecificationManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExLabelSpecificationManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsTotalWeightManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExTotalWeightManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsTotalInsuredValueManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExTotalInsuredValueManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsFedExShippingChargesManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingChargesManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_AddsFedExCertificationManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCertificationManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPackagingTypeManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackagingTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPickupManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPickupManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExServiceTypeManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExServiceTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPackageSpecialServicesManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageSpecialServicesManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_WebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExShippingClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExShippingVersionManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExShippingVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExReferenceManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExReferenceManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPackageDetailsManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageDetailsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExEmailNotificationsManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExEmailNotificationsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExDryIceManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExDryIceManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExPriorityAlertManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPriorityAlertManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExMasterTrackingManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExMasterTrackingManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExCodOptionsManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCodOptionsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExCustomsManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCustomsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExAdmissibilityManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExAdmissibilityManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExBrokerManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExBrokerManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExCommercialInvoiceManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCommercialInvoiceManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExHomeDeliveryManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExHomeDeliveryManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExHoldAtLocationManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExHoldAtLocationManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExFreightManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExFreightManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExDangerousGoodsManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExDangerousGoodsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExReturnsManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExReturnsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExTrafficInArmsManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExTrafficInArmsManipulator)) == 1);
        }

        [Fact]
        public void CreateShipRequest_FedExOneRateManipulator()
        {
            CarrierRequest request = testObject.CreateShipRequest(fedExShipment) as FedExShipRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExOneRateManipulator)) == 1);
        }

        #endregion CreateShipRequest Tests

        #region CreateVersionCaptureRequest Tests

        [Fact]
        public void CreateVersionCaptureRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(3, request.Manipulators.Count());
        }

        [Fact]
        public void CreateVersionCaptureRequest_FedExRegistrationWebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateVersionCaptureRequest_FedExRegistrationClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateVersionCaptureRequest_FedExRegistrationVersionManipulator()
        {
            CarrierRequest request = testObject.CreateVersionCaptureRequest(fedExShipment, string.Empty, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        #endregion CreateVersionCaptureRequest Tests

        #region CreatePackageMovementRequest Tests

        [Fact]
        public void CreatePackageMovementRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(fedExShipment, new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(3, request.Manipulators.Count());
        }

        [Fact]
        public void CreatePackageMovementRequest_WebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(fedExShipment, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreatePackageMovementRequest_FedExPackageMovementClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(fedExShipment, new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreatePackageMovementRequest_FedExPackageMovementVersionManipulator()
        {
            CarrierRequest request = testObject.CreatePackageMovementRequest(new ShipmentEntity(), new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPackageMovementVersionManipulator)) == 1);
        }

        #endregion CreateVersionCaptureRequest Tests

        #region CreateGroundCloseRequest Tests

        [Fact]
        public void CreateGroundCloseRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateGroundCloseRequest_WebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateGroundCloseRequest_FedExPackageMovementClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateGroundCloseRequest_FedExPackageMovementVersionManipulator()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateGroundCloseRequest_FedExCloseDateManipulator()
        {
            CarrierRequest request = testObject.CreateGroundCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseDateManipulator)) == 1);
        }

        #endregion CreateCloseRequest Tests

        #region CreateSmartPostCloseRequest Tests

        [Fact]
        public void CreateSmartPostCloseRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateSmartPostCloseRequest_WebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSmartPostCloseRequest_FedExPackageMovementClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSmartPostCloseRequest_FedExPackageMovementVersionManipulator()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCloseVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateSmartPostCloseRequest_FedExCloseDateManipulator()
        {
            CarrierRequest request = testObject.CreateSmartPostCloseRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExPickupCarrierManipulator)) == 1);
        }

        #endregion CreateSmartPostCloseRequest Tests

        #region CreateRegisterCspUserRequest Tests

        [Fact]
        public void CreateRegisterCspUserRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExRegistrationWebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExRegistrationClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExRegistrationVersionManipulator()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateRegisterCspUserRequest_FedExCspContactManipulator()
        {
            CarrierRequest request = testObject.CreateRegisterCspUserRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExCspContactManipulator)) == 1);
        }

        #endregion CreateRegisterCspUserRequest Tests

        #region CreateSubscriptionRequest Tests

        [Fact]
        public void CreateSubscriptionRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(4, request.Manipulators.Count());
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExRegistrationWebAuthenticationDetailManipulator()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationWebAuthenticationDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExRegistrationClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExRegistrationVersionManipulator()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRegistrationVersionManipulator)) == 1);
        }

        [Fact]
        public void CreateSubscriptionRequest_FedExSubscriberManipulator()
        {
            CarrierRequest request = testObject.CreateSubscriptionRequest(new FedExAccountEntity());

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExSubscriberManipulator)) == 1);
        }

        #endregion CreateSubscriptionRequest Tests

        #region CreateRateRequest Tests

        [Fact]
        public void CreateRateRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.Equal(19, request.Manipulators.Count());
        }

        [Fact]
        public void CreateRateRequest_PopulatesManipulators_WithSpecializedManipulators()
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
        public void CreateRateRequest_PopulatesManipulators_WhenSpecializedManipulatorsIsEmpty()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), new List<ICarrierRequestManipulator>());

            Assert.Equal(19, request.Manipulators.Count());
        }

        [Fact]
        public void CreateRateRequest_ReturnsFedExRateRequest()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.IsAssignableFrom<FedExRateRequest>(request);
        }

        [Fact]
        public void CreateRateRequest_FedExReturnTransitManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateReturnTransitManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExShipperManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateShipperManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRecipientManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateRecipientManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExShipmentSpecialServiceTypeManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateShipmentSpecialServiceTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExTotalInsuredValueManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateTotalInsuredValueManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExTotalWeightManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateTotalWeightManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRateTypeManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateRateTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExPickupManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePickupManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExPackageDetailsManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePackageDetailsManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExPackageSpecialServicesManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePackageSpecialServicesManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExPackagingTypeManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRatePackagingTypeManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRateClientDetailManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateClientDetailManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRateWebAuthenticationManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateWebAuthenticationManipulator)) == 1);
        }

        [Fact]
        public void CreateRateRequest_FedExRateVersionManipulator()
        {
            CarrierRequest request = testObject.CreateRateRequest(new ShipmentEntity(), null);

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(FedExRateVersionManipulator)) == 1);
        }

        #endregion CreateRateRequest Tests

        #region CreateCertificateRequest Tests

        [Fact]
        public void CreateCertificateRequest_ReturnsCertficateReqeust()
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