using System;
using System.Collections.Generic;
using System.Net;
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
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.CarrierSetup
{
    public class UspsCarrierSetupTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> carrierAccountRepository;
        private readonly Mock<IShipmentTypeSetupActivity> shipmentTypeSetupActivity;
        private readonly Mock<IShippingSettings> shippingSettings;
        private readonly Mock<IShipmentPrintHelper> printHelper;
        private readonly CarrierConfiguration payload;

        private readonly Guid carrierID = new Guid("117CD221-EC30-41EB-BBB3-58E6097F45CC");

        public UspsCarrierSetupTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            this.payload = new CarrierConfiguration
            {
                AdditionalData = JObject.Parse("{usps: {username: \"user\", password: \"password\" } }"),
                HubVersion = 2,
                HubCarrierID = carrierID,
                RequestedLabelFormat = ThermalLanguage.None,
                Address = new Warehouse.Configuration.DTO.ConfigurationAddress(),
            };

            this.carrierAccountRepository = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            this.shipmentTypeSetupActivity = mock.Mock<IShipmentTypeSetupActivity>();
            this.shippingSettings = mock.Mock<IShippingSettings>();
            this.printHelper = mock.Mock<IShipmentPrintHelper>();
        }


        [Fact]
        public async Task Setup_CreatesNewAccount_WhenNoPreviousUSPSAccountsExist()
        {
            var accounts = new List<UspsAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UspsCarrierSetup>();
            await testObject.Setup(payload, null);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<UspsAccountEntity>()), Times.Once);
        }

        [Fact]
        public async Task Setup_Returns_WhenCarrierIDMatches_AndHubVersionIsEqual()
        {
            var accounts = new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    HubCarrierId = carrierID,
                    HubVersion = 2
                }
            };

            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UspsCarrierSetup>();
            await testObject.Setup(payload, null);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<UspsAccountEntity>()), Times.Never);
        }

        [Fact]
        public async Task Setup_ReturnsExistingAccount_WhenCarrierIDMatches()
        {
            var accounts = new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    Username = "username",
                    FirstName = "blah",
                    IsNew = false,
                    HubCarrierId = carrierID
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UspsCarrierSetup>();
            await testObject.Setup(payload, null);

            carrierAccountRepository.Verify(x => x.Save(It.Is<UspsAccountEntity>(y => y.Username == "username" && y.FirstName == "blah")), Times.Once);
        }

        [Fact]
        public async Task Setup_CallsInitializationMethods_WhenNoPreviousUSPSAccountsExist()
        {
            var accounts = new List<UspsAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UspsCarrierSetup>();
            await testObject.Setup(payload, null);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(ShipmentTypeCode.Usps, ShipmentOriginSource.Account, false, ThermalLanguage.None), Times.Once);
            shippingSettings.Verify(x => x.MarkAsConfigured(ShipmentTypeCode.Usps), Times.Once);
            printHelper.Verify(x => x.InstallDefaultRules(ShipmentTypeCode.Usps), Times.Once);
        }

        [Fact]
        public async Task Setup_DoesNotCallInitilizationMethods_WhenPreviousUSPSAccountsExist()
        {
            var accounts = new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    Username = "user",
                    FirstName = "blah",
                    IsNew = false
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UspsCarrierSetup>();
            await testObject.Setup(payload, null);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentOriginSource>(), It.IsAny<bool>(), It.IsAny<ThermalLanguage>()), Times.Never);
            shippingSettings.Verify(x => x.MarkAsConfigured(It.IsAny<ShipmentTypeCode>()), Times.Never);
            printHelper.Verify(x => x.InstallDefaultRules(It.IsAny<ShipmentTypeCode>()), Times.Never);
        }

        [Fact]
        public async Task Setup_SavesEncryptedPassphrase()
        {
            var accounts = new List<UspsAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            await mock.Create<UspsCarrierSetup>().Setup(payload, null);

            carrierAccountRepository.Verify(x =>
                x.Save(It.Is<UspsAccountEntity>(y =>
                    SecureText.Decrypt(y.Password, y.Username) == "password")), Times.Once);
        }

        [Fact]
        public async Task Setup_RethrowsException_WhenWebClientCallFails()
        {
            var accounts = new List<UspsAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(x => x.PopulateUspsAccountEntity(It.IsAny<UspsAccountEntity>())).Throws(new WebException());

            var factory = mock.CreateMock<Func<UspsResellerType, IUspsWebClient>>();
            factory.Setup(x => x(It.IsAny<UspsResellerType>())).Returns(webClient.Object);

            var testObject = mock.Create<UspsCarrierSetup>();
            await Assert.ThrowsAsync<WebException>(async () => await testObject.Setup(payload, null));
        }

        [Fact]
        public async Task Setup_UpdatesHubVersion()
        {
            var accounts = new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    IsNew = false,
                    HubVersion = 0,
                    HubCarrierId = carrierID,
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UspsCarrierSetup>();
            await testObject.Setup(payload, null);

            carrierAccountRepository.Verify(x => x.Save(It.Is<UspsAccountEntity>(y => y.HubVersion == 2)), Times.Once);
        }

        [Fact]
        public async Task Setup_SetsHubCarrierID()
        {
            var accounts = new List<UspsAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UspsCarrierSetup>();
            await testObject.Setup(payload, null);

            carrierAccountRepository.Verify(x => x.Save(It.Is<UspsAccountEntity>(y => y.HubCarrierId == carrierID)), Times.Once);
        }
    }
}
