using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.CarrierSetup;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.CarrierSetup
{
    public class DhlCarrierSetupTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity>> carrierAccountRepository;
        private readonly CarrierConfiguration payload;
        private readonly Mock<IShipmentTypeSetupActivity> shipmentTypeSetupActivity;
        private readonly Mock<IShippingSettings> shippingSettings;
        private readonly Mock<IShipmentPrintHelper> printHelper;
        private readonly Mock<IShipEngineWebClient> shipEngineWebClient;

        private readonly Guid carrierId = new Guid("117CD221-EC30-41EB-BBB3-58E6097F45CC");

        public DhlCarrierSetupTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            payload = new CarrierConfiguration
            {
                AdditionalData = JObject.Parse("{dhl: {accountNumber: 123, description: \"description\"}}"),
                HubVersion = 2,
                HubCarrierID = carrierId,
                RequestedLabelFormat = ThermalLanguage.None,
                Address = new Warehouse.Configuration.DTO.ConfigurationAddress()
            };

            carrierAccountRepository =
                mock.Mock<ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity>>();

            this.shipmentTypeSetupActivity = mock.Mock<IShipmentTypeSetupActivity>();
            this.shippingSettings = mock.Mock<IShippingSettings>();
            this.printHelper = mock.Mock<IShipmentPrintHelper>();

            var carrierDescription = mock.CreateMock<IIndex<ShipmentTypeCode, ICarrierAccountDescription>>();

            carrierDescription.Setup(x => x[ShipmentTypeCode.DhlExpress])
                .Returns(new DhlExpressAccountDescription());
            mock.Provide(carrierDescription.Object);

            shipEngineWebClient = mock.Mock<IShipEngineWebClient>();
            shipEngineWebClient.Setup(x => x.ConnectDhlAccount(It.IsAny<string>())).Returns(Task.FromResult(GenericResult.FromSuccess<string>("test")));
        }

        [Fact]
        public async Task Setup_Returns_WhenCarrierIdMatches_AndHubVersionIsEqual()
        {
            var accounts = new List<IDhlExpressAccountEntity>
            {
                new DhlExpressAccountEntity
                {
                    AccountNumber = 123,
                    HubCarrierId = carrierId,
                    HubVersion = 2
                }
            };

            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            await mock.Create<DhlCarrierSetup>().Setup(payload, null);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<DhlExpressAccountEntity>()), Times.Never);
        }

        [Fact]
        public async Task Setup_ReturnsExistingAccount_WhenCarriedIdMatches()
        {
            var accounts = new List<DhlExpressAccountEntity>
            {
                new DhlExpressAccountEntity
                {
                    AccountNumber = 123,
                    HubCarrierId = carrierId,
                    HubVersion = 1,
                    IsNew = false
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            await mock.Create<DhlCarrierSetup>().Setup(payload, null);

            carrierAccountRepository.Verify(x =>
                x.Save(It.Is<DhlExpressAccountEntity>(y => y.AccountNumber == 123)), Times.Once);

        }

        [Fact]
        public async Task Setup_CreatesNewAccount_WhenNoPreviousDhlAccountsExist()
        {
            var accounts = new List<DhlExpressAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<DhlCarrierSetup>();
            await testObject.Setup(payload, null);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<DhlExpressAccountEntity>()), Times.Once);
        }

        [Fact]
        public async Task Setup_CallsInitializationMethods_WhenNoPreviousDhlAccountsExist()
        {
            var accounts = new List<DhlExpressAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<DhlCarrierSetup>();
            await testObject.Setup(payload, null);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(ShipmentTypeCode.DhlExpress, ShipmentOriginSource.Account, false, ThermalLanguage.None), Times.Once);
            shippingSettings.Verify(x => x.MarkAsConfigured(ShipmentTypeCode.DhlExpress), Times.Once);
            printHelper.Verify(x => x.InstallDefaultRules(ShipmentTypeCode.DhlExpress), Times.Once);
        }

        [Fact]
        public async Task Setup_DoesNotCallInitilizationMethods_WhenPreviousDhlAccountsExist()
        {
            var accounts = new List<DhlExpressAccountEntity>
            {
                new DhlExpressAccountEntity
                {
                    AccountNumber = 1234,
                    FirstName = "blah",
                    IsNew = false
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<DhlCarrierSetup>();
            await testObject.Setup(payload, null);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentOriginSource>(), It.IsAny<bool>(), It.IsAny<ThermalLanguage>()), Times.Never);
            shippingSettings.Verify(x => x.MarkAsConfigured(It.IsAny<ShipmentTypeCode>()), Times.Never);
            printHelper.Verify(x => x.InstallDefaultRules(It.IsAny<ShipmentTypeCode>()), Times.Never);
        }

        [Fact]
        public async Task Setup_RethrowsShipEngineException_WhenWebClientCallFails()
        {
            shipEngineWebClient.Setup(x => x.ConnectDhlAccount(It.IsAny<string>())).Returns(Task.FromResult(GenericResult.FromError<string>(new WebException(), "test")));

            var testObject = mock.Create<DhlCarrierSetup>();

            await Assert.ThrowsAsync<WebException>(async () => await testObject.Setup(payload, null));
        }

        [Fact]
        public async Task Setup_UpdatesHubVersion()
        {
            var accounts = new List<DhlExpressAccountEntity>
            {
                new DhlExpressAccountEntity
                {
                    IsNew = false,
                    HubVersion = 0,
                    HubCarrierId = carrierId,
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<DhlCarrierSetup>();
            await testObject.Setup(payload, null);

            carrierAccountRepository.Verify(x => x.Save(It.Is<DhlExpressAccountEntity>(y => y.HubVersion == 2)), Times.Once);
        }

        [Fact]
        public async Task Setup_SetsHubCarrierID()
        {
            var accounts = new List<DhlExpressAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<DhlCarrierSetup>();
            await testObject.Setup(payload, null);

            carrierAccountRepository.Verify(x => x.Save(It.Is<DhlExpressAccountEntity>(y => y.HubCarrierId == carrierId)), Times.Once);
        }
    }
}