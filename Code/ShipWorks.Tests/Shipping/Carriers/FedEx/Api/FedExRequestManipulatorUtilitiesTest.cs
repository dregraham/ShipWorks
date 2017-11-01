using System.Collections.Generic;
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
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class FedExRequestManipulatorUtilitiesTest
    {
        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRequestManipulatorUtilitiesTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.DropoffType = (int) FedExDropoffType.RegularPickup;

            nativeRequest = new ProcessShipmentRequest();
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new DeleteShipmentRequest());

            Assert.Throws<CarrierException>(() => FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object));
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ReturnsProcessShipmentRequest()
        {
            // Setup to pass a ProcessShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity,
                                                                      new ProcessShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

            Assert.IsAssignableFrom<ProcessShipmentRequest>(carrierRequest.Object.NativeRequest);
        }

        [Fact]
        public void FedExGetShipServiceRequestedShipment_ReturnsCreateValidateShipmentRequest()
        {
            // Setup to pass a ValidateShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ValidateShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

            Assert.IsAssignableFrom<ValidateShipmentRequest>(carrierRequest.Object.NativeRequest);
        }

        [Fact]
        public void FedExGetShipmentDropoffType_ReturnsCreateValidateShipmentRequest()
        {
            DropoffType dropOffType = FedExRequestManipulatorUtilities.GetShipmentDropoffType((FedExDropoffType) shipmentEntity.FedEx.DropoffType);

            Assert.Equal(DropoffType.REGULAR_PICKUP, dropOffType);
        }

        [Fact]
        public void GetApiServiceType_ReturnFreight_WhenServiceTypeIsFreight()
        {
            Dictionary<FedExServiceType, ServiceType> serviceList = new Dictionary<FedExServiceType, ServiceType>();

            serviceList.Add(FedExServiceType.PriorityOvernight, ServiceType.PRIORITY_OVERNIGHT);
            serviceList.Add(FedExServiceType.StandardOvernight, ServiceType.STANDARD_OVERNIGHT);
            serviceList.Add(FedExServiceType.FirstOvernight, ServiceType.FIRST_OVERNIGHT);
            serviceList.Add(FedExServiceType.FedEx2Day, ServiceType.FEDEX_2_DAY);
            serviceList.Add(FedExServiceType.FedExExpressSaver, ServiceType.FEDEX_EXPRESS_SAVER);
            serviceList.Add(FedExServiceType.InternationalPriority, ServiceType.INTERNATIONAL_PRIORITY);
            serviceList.Add(FedExServiceType.InternationalEconomy, ServiceType.INTERNATIONAL_ECONOMY);
            serviceList.Add(FedExServiceType.InternationalFirst, ServiceType.INTERNATIONAL_FIRST);
            serviceList.Add(FedExServiceType.FedEx1DayFreight, ServiceType.FEDEX_1_DAY_FREIGHT);
            serviceList.Add(FedExServiceType.FedEx2DayFreight, ServiceType.FEDEX_2_DAY_FREIGHT);
            serviceList.Add(FedExServiceType.FedEx3DayFreight, ServiceType.FEDEX_3_DAY_FREIGHT);
            serviceList.Add(FedExServiceType.FedExGround, ServiceType.FEDEX_GROUND);
            serviceList.Add(FedExServiceType.GroundHomeDelivery, ServiceType.GROUND_HOME_DELIVERY);
            serviceList.Add(FedExServiceType.InternationalPriorityFreight, ServiceType.INTERNATIONAL_PRIORITY_FREIGHT);
            serviceList.Add(FedExServiceType.InternationalEconomyFreight, ServiceType.INTERNATIONAL_ECONOMY_FREIGHT);
            serviceList.Add(FedExServiceType.SmartPost, ServiceType.SMART_POST);
            serviceList.Add(FedExServiceType.FedEx2DayAM, ServiceType.FEDEX_2_DAY_AM);
            serviceList.Add(FedExServiceType.FirstFreight, ServiceType.FEDEX_FIRST_FREIGHT);
            serviceList.Add(FedExServiceType.OneRateFirstOvernight, ServiceType.FIRST_OVERNIGHT);
            serviceList.Add(FedExServiceType.OneRatePriorityOvernight, ServiceType.PRIORITY_OVERNIGHT);
            serviceList.Add(FedExServiceType.OneRateStandardOvernight, ServiceType.STANDARD_OVERNIGHT);
            serviceList.Add(FedExServiceType.OneRate2Day, ServiceType.FEDEX_2_DAY);
            serviceList.Add(FedExServiceType.OneRate2DayAM, ServiceType.FEDEX_2_DAY_AM);
            serviceList.Add(FedExServiceType.OneRateExpressSaver, ServiceType.FEDEX_EXPRESS_SAVER);
            serviceList.Add(FedExServiceType.FedExEconomyCanada, ServiceType.FEDEX_EXPRESS_SAVER);
            serviceList.Add(FedExServiceType.FedExInternationalGround, ServiceType.FEDEX_GROUND);
            serviceList.Add(FedExServiceType.FedExNextDayAfternoon, ServiceType.FEDEX_NEXT_DAY_AFTERNOON);
            serviceList.Add(FedExServiceType.FedExNextDayEarlyMorning, ServiceType.FEDEX_NEXT_DAY_EARLY_MORNING);
            serviceList.Add(FedExServiceType.FedExNextDayMidMorning, ServiceType.FEDEX_NEXT_DAY_MID_MORNING);
            serviceList.Add(FedExServiceType.FedExNextDayEndOfDay, ServiceType.FEDEX_NEXT_DAY_END_OF_DAY);
            serviceList.Add(FedExServiceType.FedExDistanceDeferred, ServiceType.FEDEX_DISTANCE_DEFERRED);
            serviceList.Add(FedExServiceType.FedExNextDayFreight, ServiceType.FEDEX_NEXT_DAY_FREIGHT);
            serviceList.Add(FedExServiceType.InternationalPriorityExpress, ServiceType.INTERNATIONAL_PRIORITY_EXPRESS);
            serviceList.Add(FedExServiceType.FedExFreightEconomy, ServiceType.FEDEX_FREIGHT_ECONOMY);
            serviceList.Add(FedExServiceType.FedExFreightPriority, ServiceType.FEDEX_FREIGHT_PRIORITY);

            foreach (var serviceType in serviceList)
            {
                var testCode = FedExRequestManipulatorUtilities.GetApiServiceType(serviceType.Key);
                Assert.Equal(serviceType.Value, testCode);
            }
        }
        #region Shipping Web Authentication Tests

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesCspCredentialKeyFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.ParentCredential.Key, settings.CspCredentialKey);
        }

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.ParentCredential.Password, settings.CspCredentialPassword);
        }

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesUserCredentialKeyFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.Equal(detail.UserCredential.Key, settings.UserCredentialsKey);
        }

        [Fact]
        public void CreateShippingWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

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

            ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account);

            Assert.Equal(account.AccountNumber, detail.AccountNumber);
        }

        [Fact]
        public void CreateShippingClientDetail_UsesMeterNumberFromAccount()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account);

            Assert.Equal(account.MeterNumber, detail.MeterNumber);
        }

        #endregion Shipping Client Detail Tests

        #region Registration Client Detail Tests

        [Fact]
        public void CreateRegistrationClientDetail_UsesAccountNumberFromAccount()
        {
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "123-456-789" };

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account);

            Assert.Equal(account.AccountNumber, detail.AccountNumber);
        }

        [Fact]
        public void CreateRegistrationClientDetail_UsesMeterNumberFromAccount()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

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
