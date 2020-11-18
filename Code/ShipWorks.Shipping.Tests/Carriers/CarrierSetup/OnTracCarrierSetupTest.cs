using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.Configuration.Carriers;
using ShipWorks.Warehouse.Configuration.Carriers.DTO;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.CarrierSetup
{
    public class OnTracCarrierSetupTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity>> carrierAccountRepository;
        private readonly CarrierConfiguration payload;
        private readonly Mock<IShipmentTypeSetupActivity> shipmentTypeSetupActivity;
        private readonly Mock<IShippingSettings> shippingSettings;
        private readonly Mock<IShipmentPrintHelper> printHelper;

        private readonly Guid carrierId = new Guid("117CD221-EC30-41EB-BBB3-58E6097F45CC");

        public OnTracCarrierSetupTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            payload = new CarrierConfiguration
            {
                AdditionalData = JObject.Parse("{ontrac: {accountNumber: 123, password: \"password\"}}"),
                HubVersion = 2,
                HubCarrierID = carrierId,
                RequestedLabelFormat = ThermalLanguage.None,
                Address = new Warehouse.Configuration.DTO.ConfigurationAddress()
            };

            carrierAccountRepository = mock.Mock<ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity>>();

            this.shipmentTypeSetupActivity = mock.Mock<IShipmentTypeSetupActivity>();
            this.shippingSettings = mock.Mock<IShippingSettings>();
            this.printHelper = mock.Mock<IShipmentPrintHelper>();
        }

        [Fact]
        public async Task Setup_Returns_WhenCarrierIDMatches_AndHubVersionIsEqual()
        {
            var accounts = new List<OnTracAccountEntity>
            {
                new OnTracAccountEntity
                {
                    AccountNumber = 123,
                    HubCarrierId = carrierId,
                    HubVersion = 2
                }
            };

            carrierAccountRepository.Setup(x =>
                x.AccountsReadOnly).Returns(accounts);

            await mock.Create<OnTracCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x =>
                x.Save(It.IsAny<OnTracAccountEntity>()), Times.Never);
        }

        [Fact]
        public async Task Setup_ReturnsExistingAccount_WhenCarriedIdMatches()
        {
            var accounts = new List<OnTracAccountEntity>
            {
                new OnTracAccountEntity
                {
                    AccountNumber = 1234,
                    HubCarrierId = carrierId,
                    HubVersion = 1,
                    IsNew = false,
                }
            };

            carrierAccountRepository.Setup(x =>
                x.Accounts).Returns(accounts);

            carrierAccountRepository.Setup(x =>
                x.AccountsReadOnly).Returns(accounts);

            await mock.Create<OnTracCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x =>
                x.Save(It.Is<OnTracAccountEntity>(y => y.AccountNumber == 1234)), Times.Once);
        }

        [Fact]
        public async Task Setup_SavesEncryptedPassword()
        {
            var accounts = new List<OnTracAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            await mock.Create<OnTracCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x =>
                x.Save(It.Is<OnTracAccountEntity>(y =>
                    SecureText.Decrypt(y.Password, y.AccountNumber.ToString()) == "password")), Times.Once);
        }

        [Fact]
        public async Task Setup_CreatesNewAccount_WhenNoPreviousOnTracAccountsExist()
        {
            var accounts = new List<OnTracAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<OnTracCarrierSetup>();
            await testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<OnTracAccountEntity>()), Times.Once);
        }

        [Fact]
        public async Task Setup_CallsInitializationMethods_WhenNoPreviousOnTracAccountsExist()
        {
            var accounts = new List<OnTracAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<OnTracCarrierSetup>();
            await testObject.Setup(payload);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(ShipmentTypeCode.OnTrac, ShipmentOriginSource.Account, false, ThermalLanguage.None), Times.Once);
            shippingSettings.Verify(x => x.MarkAsConfigured(ShipmentTypeCode.OnTrac), Times.Once);
            printHelper.Verify(x => x.InstallDefaultRules(ShipmentTypeCode.OnTrac), Times.Once);
        }

        [Fact]
        public async Task Setup_DoesNotCallInitilizationMethods_WhenPreviousOnTracAccountsExist()
        {
            var accounts = new List<OnTracAccountEntity>
            {
                new OnTracAccountEntity
                {
                    AccountNumber = 123,
                    FirstName = "blah",
                    IsNew = false
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<OnTracCarrierSetup>();
            await testObject.Setup(payload);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentOriginSource>(), It.IsAny<bool>(), It.IsAny<ThermalLanguage>()), Times.Never);
            shippingSettings.Verify(x => x.MarkAsConfigured(It.IsAny<ShipmentTypeCode>()), Times.Never);
            printHelper.Verify(x => x.InstallDefaultRules(It.IsAny<ShipmentTypeCode>()), Times.Never);
        }

        [Fact]
        public async Task Setup_UpdatesHubVersion()
        {
            var accounts = new List<OnTracAccountEntity>
            {
                new OnTracAccountEntity
                {
                    IsNew = false,
                    HubVersion = 0,
                    HubCarrierId = carrierId,
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<OnTracCarrierSetup>();
            await testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.Is<OnTracAccountEntity>(y => y.HubVersion == 2)), Times.Once);
        }

        [Fact]
        public async Task Setup_SetsHubCarrierID()
        {
            var accounts = new List<OnTracAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<OnTracCarrierSetup>();
            await testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.Is<OnTracAccountEntity>(y => y.HubCarrierId == carrierId)), Times.Once);
        }
    }
}