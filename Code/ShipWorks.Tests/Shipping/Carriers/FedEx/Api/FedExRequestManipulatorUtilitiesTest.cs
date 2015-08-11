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
        public void FedExGetShipServiceRequestedShipment_ThrowsCarrierException_WhenNativeRequestIsNotValidShipmentRequest_Test()
        {
            // Setup to pass a shipment entity as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipmentEntity());

            Assert.Throws<CarrierException>(() => FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object));
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ThrowsCarrierException_WhenNativeRequestIsObject_Test()
        {
            // Setup to pass a object as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object));
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ThrowsCarrierException_WhenNativeRequestIsDeleteShipmentRequest_Test()
        {
            // Setup to pass a DeleteShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.DeleteShipmentRequest());

            Assert.Throws<CarrierException>(() => FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object));
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ReturnsProcessShipmentRequest_Test()
        {
            // Setup to pass a ProcessShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity,
                                                                      new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

            Assert.IsAssignableFrom<ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest>(carrierRequest.Object.NativeRequest);
        }

        //[Fact]
        //public void FedExGetShipServiceRequestedShipment_ReturnsCreatePendingShipmentRequest_Test()
        //{
        //    // Setup to pass a CreatePendingShipmentRequest as the native request
        //    carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.CreatePendingShipmentRequest());

        //    FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

        //    Assert.IsAssignableFrom<ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.CreatePendingShipmentRequest>(carrierRequest.Object.NativeRequest);
        //}

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ReturnsCreateValidateShipmentRequest_Test()
        {
            // Setup to pass a ValidateShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ValidateShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

            Assert.IsAssignableFrom<ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ValidateShipmentRequest>(carrierRequest.Object.NativeRequest);
        }

        [Fact]
        public void FedExGetShipmentDropoffType_ReturnsCreateValidateShipmentRequest_Test()
        {
            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.DropoffType dropOffType = FedExRequestManipulatorUtilities.GetShipmentDropoffType((FedExDropoffType)shipmentEntity.FedEx.DropoffType);

            Assert.Equal(ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.DropoffType.REGULAR_PICKUP, dropOffType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsPriorityOvernight_WhenServiceTypeIsOneRatePriorityOvernight_Test()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRatePriorityOvernight);

            Assert.Equal(ServiceType.PRIORITY_OVERNIGHT, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsStandardOvernight_WhenServiceTypeIsOneRateStandardOvernight_Test()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRateStandardOvernight);

            Assert.Equal(ServiceType.STANDARD_OVERNIGHT, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsFedEx2Day_WhenServiceTypeIsOneRate2Day_Test()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRate2Day);

            Assert.Equal(ServiceType.FEDEX_2_DAY, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsFedEx2DayAM_WhenServiceTypeIsOneRate2DayAM_Test()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRate2DayAM);

            Assert.Equal(ServiceType.FEDEX_2_DAY_AM, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnsFirstOvernight_WhenServiceTypeIsOneRateFirstOvernight_Test()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRateFirstOvernight);

            Assert.Equal(ServiceType.FIRST_OVERNIGHT, serviceType);
        }

        [Fact]
        public void GetApiServiceType_ReturnExpressSaver_WhenServiceTypeIsOneRateExpressSaver_Test()
        {
            ServiceType serviceType = FedExRequestManipulatorUtilities.GetApiServiceType(FedExServiceType.OneRateExpressSaver);

            Assert.Equal(ServiceType.FEDEX_EXPRESS_SAVER, serviceType);
        }

        #region Shipping Web Authentication Tests

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesCspCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.CspCredential.Key, settings.CspCredentialKey);
        }

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.CspCredential.Password, settings.CspCredentialPassword);
        }

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesUserCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Key, settings.UserCredentialsKey);
        }

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings_Test()
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
        public void CreateRegistrationWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Password, settings.UserCredentialsPassword);
        }

        [Fact]
        public void CreateRegistrationWebAuthenticationDetails_UsesCspCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.Equal(detail.CspCredential.Key, settings.CspCredentialKey);
        }

        [Fact]
        public void CreateRegistrationWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.Equal(detail.CspCredential.Password, settings.CspCredentialPassword);
        }

        [Fact]
        public void CreateRegistrationWebAuthenticationDetails_UsesUserCredentialKeyFromSettings_Test()
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
        public void CreatePackageMovementWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Password, settings.UserCredentialsPassword);
        }

        [Fact]
        public void CreatePackageMovementWebAuthenticationDetails_UsesCspCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.Equal(detail.CspCredential.Key, settings.CspCredentialKey);
        }

        [Fact]
        public void CreatePackageMovementWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.Equal(detail.CspCredential.Password, settings.CspCredentialPassword);
        }

        [Fact]
        public void CreatePackageMovementWebAuthenticationDetails_UsesUserCredentialKeyFromSettings_Test()
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
        public void CreateShippingClientDetail_UsesAccountNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "123-456-789" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account, settings);

            Assert.Equal(account.AccountNumber, detail.AccountNumber);
        }

        [Fact]
        public void CreateShippingClientDetail_UsesMeterNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account, settings);

            Assert.Equal(account.MeterNumber, detail.MeterNumber);
        }

        [Fact]
        public void CreateShippingClientDetail_UsesClientProductIdFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account, settings);

            Assert.Equal(settings.ClientProductId, detail.ClientProductId);
        }

        [Fact]
        public void CreateShippingClientDetail_UsesClientProductVersionFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account, settings);

            Assert.Equal(settings.ClientProductVersion, detail.ClientProductVersion);
        }

        #endregion Shipping Client Detail Tests


        #region Registration Client Detail Tests

        [Fact]
        public void CreateRegistrationClientDetail_UsesAccountNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "123-456-789" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, settings);

            Assert.Equal(account.AccountNumber, detail.AccountNumber);
        }

        [Fact]
        public void CreateRegistrationClientDetail_UsesMeterNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, settings);

            Assert.Equal(account.MeterNumber, detail.MeterNumber);
        }

        [Fact]
        public void CreateRegistrationClientDetail_UsesClientProductIdFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, settings);

            Assert.Equal(settings.ClientProductId, detail.ClientProductId);
        }

        [Fact]
        public void CreateRegistrationClientDetail_UsesClientProductVersionFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, settings);

            Assert.Equal(settings.ClientProductVersion, detail.ClientProductVersion);
        }

        #endregion Registration Client Detail Tests


        #region Package Movement Client Detail Tests

        [Fact]
        public void CreatePackageMovementClientDetail_UsesAccountNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "123-456-789" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.Equal(account.AccountNumber, detail.AccountNumber);
        }

        [Fact]
        public void CreatePackageMovementClientDetail_UsesMeterNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.Equal(account.MeterNumber, detail.MeterNumber);
        }

        [Fact]
        public void CreatePackageMovementClientDetail_UsesClientProductIdFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.Equal(settings.ClientProductId, detail.ClientProductId);
        }

        [Fact]
        public void CreatePackageMovementClientDetail_UsesClientProductVersionFromSettings_Test()
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
