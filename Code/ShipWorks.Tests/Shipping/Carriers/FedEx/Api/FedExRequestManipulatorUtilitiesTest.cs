using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    [TestClass]
    public class FedExRequestManipulatorUtilitiesTest
    {
        private Mock<CarrierRequest> carrierRequest;
        private ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.DropoffType = (int) FedExDropoffType.RegularPickup;

            nativeRequest = new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void FedExGetShipServiceRequestedShipment_ThrowsCarrierException_WhenNativeRequestIsNotValidShipmentRequest_Test()
        {
            // Setup to pass a shipment entity as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipmentEntity());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void FedExGetShipServiceRequestedShipment_ThrowsCarrierException_WhenNativeRequestIsCancelPendingShipmentRequest_Test()
        {
            // Setup to pass a CancelPendingShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.CancelPendingShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void FedExGetShipServiceRequestedShipment_ThrowsCarrierException_WhenNativeRequestIsDeleteShipmentRequest_Test()
        {
            // Setup to pass a DeleteShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.DeleteShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);
        }

        [TestMethod]
        public void FedExGetShipServiceRequestedShipment_ReturnsProcessShipmentRequest_Test()
        {
            // Setup to pass a ProcessShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity,
                                                                      new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

            Assert.IsInstanceOfType(carrierRequest.Object.NativeRequest, typeof(ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest));
        }

        [TestMethod]
        public void FedExGetShipServiceRequestedShipment_ReturnsCreatePendingShipmentRequest_Test()
        {
            // Setup to pass a CreatePendingShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.CreatePendingShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

            Assert.IsInstanceOfType(carrierRequest.Object.NativeRequest, typeof(ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.CreatePendingShipmentRequest));
        }

        [TestMethod]
        public void FedExGetShipServiceRequestedShipment_ReturnsCreateValidateShipmentRequest_Test()
        {
            // Setup to pass a ValidateShipmentRequest as the native request
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ValidateShipmentRequest());

            FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(carrierRequest.Object);

            Assert.IsInstanceOfType(carrierRequest.Object.NativeRequest, typeof(ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ValidateShipmentRequest));
        }

        [TestMethod]
        public void FedExGetShipmentDropoffType_ReturnsCreateValidateShipmentRequest_Test()
        {
            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.DropoffType dropOffType = FedExRequestManipulatorUtilities.GetShipmentDropoffType((FedExDropoffType)shipmentEntity.FedEx.DropoffType);

            Assert.AreEqual(ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.DropoffType.REGULAR_PICKUP, dropOffType);
        }


        #region Shipping Web Authentication Tests

        [TestMethod]
        public void CreateShippingWebAuthenticationDetails_UsesCspCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity {FedExPassword = "password", FedExUsername = "username"});
            
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.CspCredential.Key, settings.CspCredentialKey);
        }

        [TestMethod]
        public void CreateShippingWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.CspCredential.Password, settings.CspCredentialPassword);
        }

        [TestMethod]
        public void CreateShippingWebAuthenticationDetails_UsesUserCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.UserCredential.Key, settings.UserCredentialsKey);
        }

        [TestMethod]
        public void CreateShippingWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.UserCredential.Password, settings.UserCredentialsPassword);
        }

        #endregion Shipping Web Authentication Tests


        #region Registration Web Authentication Tests

        [TestMethod]
        public void CreateRegistrationWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.UserCredential.Password, settings.UserCredentialsPassword);
        }
        
        [TestMethod]
        public void CreateRegistrationWebAuthenticationDetails_UsesCspCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.CspCredential.Key, settings.CspCredentialKey);
        }

        [TestMethod]
        public void CreateRegistrationWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.CspCredential.Password, settings.CspCredentialPassword);
        }

        [TestMethod]
        public void CreateRegistrationWebAuthenticationDetails_UsesUserCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.UserCredential.Key, settings.UserCredentialsKey);
        }

        #endregion Registration Web Authentication Tests


        #region Package Movement Web Authentication Tests

        [TestMethod]
        public void CreatePackageMovementWebAuthenticationDetails_UsesUserCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.UserCredential.Password, settings.UserCredentialsPassword);
        }

        [TestMethod]
        public void CreatePackageMovementWebAuthenticationDetails_UsesCspCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.CspCredential.Key, settings.CspCredentialKey);
        }

        [TestMethod]
        public void CreatePackageMovementWebAuthenticationDetails_UsesCspCredentialPasswordFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.CspCredential.Password, settings.CspCredentialPassword);
        }

        [TestMethod]
        public void CreatePackageMovementWebAuthenticationDetails_UsesUserCredentialKeyFromSettings_Test()
        {
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetShippingSettings()).Returns(new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" });

            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.WebAuthenticationDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementWebAuthenticationDetail(settings);

            Assert.AreEqual(detail.UserCredential.Key, settings.UserCredentialsKey);
        }

        #endregion Package Movement Web Authentication Tests


        #region Shipping Client Detail Tests

        [TestMethod]
        public void CreateShippingClientDetail_UsesAccountNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity {AccountNumber = "123-456-789"};
            
            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account, settings);

            Assert.AreEqual(account.AccountNumber, detail.AccountNumber);
        }

        [TestMethod]
        public void CreateShippingClientDetail_UsesMeterNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account, settings);

            Assert.AreEqual(account.MeterNumber, detail.MeterNumber);
        }

        [TestMethod]
        public void CreateShippingClientDetail_UsesClientProductIdFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account, settings);

            Assert.AreEqual(settings.ClientProductId, detail.ClientProductId);
        }

        [TestMethod]
        public void CreateShippingClientDetail_UsesClientProductVersionFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ClientDetail detail = FedExRequestManipulatorUtilities.CreateShippingClientDetail(account, settings);

            Assert.AreEqual(settings.ClientProductVersion, detail.ClientProductVersion);
        }

        #endregion Shipping Client Detail Tests


        #region Registration Client Detail Tests

        [TestMethod]
        public void CreateRegistrationClientDetail_UsesAccountNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "123-456-789" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, settings);

            Assert.AreEqual(account.AccountNumber, detail.AccountNumber);
        }

        [TestMethod]
        public void CreateRegistrationClientDetail_UsesMeterNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, settings);

            Assert.AreEqual(account.MeterNumber, detail.MeterNumber);
        }

        [TestMethod]
        public void CreateRegistrationClientDetail_UsesClientProductIdFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, settings);

            Assert.AreEqual(settings.ClientProductId, detail.ClientProductId);
        }

        [TestMethod]
        public void CreateRegistrationClientDetail_UsesClientProductVersionFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration.ClientDetail detail = FedExRequestManipulatorUtilities.CreateRegistrationClientDetail(account, settings);

            Assert.AreEqual(settings.ClientProductVersion, detail.ClientProductVersion);
        }

        #endregion Registration Client Detail Tests


        #region Package Movement Client Detail Tests

        [TestMethod]
        public void CreatePackageMovementClientDetail_UsesAccountNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "123-456-789" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.AreEqual(account.AccountNumber, detail.AccountNumber);
        }

        [TestMethod]
        public void CreatePackageMovementClientDetail_UsesMeterNumberFromAccount_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.AreEqual(account.MeterNumber, detail.MeterNumber);
        }

        [TestMethod]
        public void CreatePackageMovementClientDetail_UsesClientProductIdFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.AreEqual(settings.ClientProductId, detail.ClientProductId);
        }

        [TestMethod]
        public void CreatePackageMovementClientDetail_UsesClientProductVersionFromSettings_Test()
        {
            FedExAccountEntity account = new FedExAccountEntity { MeterNumber = "987654321" };

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();
            FedExSettings settings = new FedExSettings(settingsRepository.Object);

            ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement.ClientDetail detail = FedExRequestManipulatorUtilities.CreatePackageMovementClientDetail(account, settings);

            Assert.AreEqual(settings.ClientProductVersion, detail.ClientProductVersion);
        }

        #endregion Package Movement Client Detail Tests
    }
}
