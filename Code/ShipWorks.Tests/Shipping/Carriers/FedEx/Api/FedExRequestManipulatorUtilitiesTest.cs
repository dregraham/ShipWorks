using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class FedExRequestManipulatorUtilitiesTest
    {
        private Mock<CarrierRequest> carrierRequest;
        private ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRequestManipulatorUtilitiesTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.DropoffType = (int)FedExDropoffType.RegularPickup;

            nativeRequest = new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ThrowsCarrierException_WhenNativeRequestIsNotValidShipmentRequest()
        {
            // Setup to pass a shipment entity as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipmentEntity());

            Assert.Throws<CarrierException>(() => FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object));
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ThrowsCarrierException_WhenNativeRequestIsObject()
        {
            // Setup to pass a object as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object));
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ThrowsCarrierException_WhenNativeRequestIsDeleteShipmentRequest()
        {
            // Setup to pass a DeleteShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.DeleteShipmentRequest());

            Assert.Throws<CarrierException>(() => FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object));
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ReturnsProcessShipmentRequest()
        {
            // Setup to pass a ProcessShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity,
                                                                      new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

            Assert.IsAssignableFrom<ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest>(carrierRequest.Object.NativeRequest);
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ReturnsCreateValidateShipmentRequest()
        {
            // Setup to pass a ValidateShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ValidateShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

            Assert.IsAssignableFrom<ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ValidateShipmentRequest>(carrierRequest.Object.NativeRequest);
        }

        [Fact]
        public void FedExGetShipmentDropoffType_ReturnsCreateValidateShipmentRequest()
        {
            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.DropoffType dropOffType = FedExRequestManipulatorUtilities.GetShipmentDropoffType((FedExDropoffType)shipmentEntity.FedEx.DropoffType);

            Assert.Equal(ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.DropoffType.REGULAR_PICKUP, dropOffType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsPriorityOvernight_WhenServiceTypeIsOneRatePriorityOvernight()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRatePriorityOvernight);

            Assert.Equal(ServiceType.PRIORITY_OVERNIGHT, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsStandardOvernight_WhenServiceTypeIsOneRateStandardOvernight()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRateStandardOvernight);

            Assert.Equal(ServiceType.STANDARD_OVERNIGHT, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsFedEx2Day_WhenServiceTypeIsOneRate2Day()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRate2Day);

            Assert.Equal(ServiceType.FEDEX_2_DAY, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsFedEx2DayAM_WhenServiceTypeIsOneRate2DayAM()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRate2DayAM);

            Assert.Equal(ServiceType.FEDEX_2_DAY_AM, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsFirstOvernight_WhenServiceTypeIsOneRateFirstOvernight()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRateFirstOvernight);

            Assert.Equal(ServiceType.FIRST_OVERNIGHT, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnExpressSaver_WhenServiceTypeIsOneRateExpressSaver()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRateExpressSaver);

            Assert.Equal(ServiceType.FEDEX_EXPRESS_SAVER, serviceType);
        }

        #region Shipping Web Authentication Tests

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesCspCredentialKeyFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.ParentCredential.Key, settings.CspCredentialKey);
        }

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.ParentCredential.Password, settings.CspCredentialPassword);
        }

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesUserCredentialKeyFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Key, settings.UserCredentialsKey);
        }

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Password, settings.UserCredentialsPassword);
        }

        #endregion Shipping Web Authentication Tests


        #region Registration Web Authentication Tests

        [Fact]
        public void CreateRegistrationWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Password, settings.UserCredentialsPassword);
        }

        [Fact]
        public void CreateRegistrationWebAuthenticationDetails_UsesCspCredentialKeyFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.Equal(detail.ParentCredential.Key, settings.CspCredentialKey);
        }

        [Fact]
        public void CreateRegistrationWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.Equal(detail.ParentCredential.Password, settings.CspCredentialPassword);
        }

        [Fact]
        public void CreateRegistrationWebAuthenticationDetails_UsesUserCredentialKeyFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Key, settings.UserCredentialsKey);
        }

        #endregion Registration Web Authentication Tests


        #region Package Movement Web Authentication Tests

        [Fact]
        public void CreatePackageMovementWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Password, settings.UserCredentialsPassword);
        }

        [Fact]
        public void CreatePackageMovementWebAuthenticationDetails_UsesCspCredentialKeyFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.Equal(detail.CspCredential.Key, settings.CspCredentialKey);
        }

        [Fact]
        public void CreatePackageMovementWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.Equal(detail.CspCredential.Password, settings.CspCredentialPassword);
        }

        [Fact]
        public void CreatePackageMovementWebAuthenticationDetails_UsesUserCredentialKeyFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Key, settings.UserCredentialsKey);
        }

        #endregion Package Movement Web Authentication Tests


        #region Shipping Client Detail Tests

        [Fact]
        public void CreateShippingClientDetail_UsesAccountNumberFromAccount()
        {
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "123-456-789" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account);

            Assert.Equal(account.AccountNumber, detail.AccountNumber);
        }

        [Fact]
        public void CreateShippingClientDetail_UsesMeterNumberFromAccount()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account);

            Assert.Equal(account.MeterNumber, detail.MeterNumber);
        }

        #endregion Shipping Client Detail Tests


        #region Registration Client Detail Tests

        [Fact]
        public void CreateRegistrationClientDetail_UsesAccountNumberFromAccount()
        {
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "123-456-789" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account);

            Assert.Equal(account.AccountNumber, detail.AccountNumber);
        }

        [Fact]
        public void CreateRegistrationClientDetail_UsesMeterNumberFromAccount()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account);

            Assert.Equal(account.MeterNumber, detail.MeterNumber);
        }

        #endregion Registration Client Detail Tests


        #region Package Movement Client Detail Tests

        [Fact]
        public void CreatePackageMovementClientDetail_UsesAccountNumberFromAccount()
        {
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "123-456-789" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.Equal(account.AccountNumber, detail.AccountNumber);
        }

        [Fact]
        public void CreatePackageMovementClientDetail_UsesMeterNumberFromAccount()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.Equal(account.MeterNumber, detail.MeterNumber);
        }

        [Fact]
        public void CreatePackageMovementClientDetail_UsesClientProductIdFromSettings()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.Equal(settings.ClientProductId, detail.ClientProductId);
        }

        [Fact]
        public void CreatePackageMovementClientDetail_UsesClientProductVersionFromSettings()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.Equal(settings.ClientProductVersion, detail.ClientProductVersion);
        }

        #endregion Package Movement Client Detail Tests
    }
}
