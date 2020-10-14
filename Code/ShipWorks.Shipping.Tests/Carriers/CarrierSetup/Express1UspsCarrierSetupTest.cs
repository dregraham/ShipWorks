﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Security;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.CarrierSetup
{
    public class Express1UspsCarrierSetupTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> carrierAccountRepository;
        private readonly CarrierConfiguration payload;
        private readonly Mock<IShipmentTypeSetupActivity> shipmentTypeSetupActivity;
        private readonly Mock<IShippingSettings> shippingSettings;
        private readonly Mock<IShipmentPrintHelper> printHelper;

        private readonly Guid carrierId = new Guid("117CD221-EC30-41EB-BBB3-58E6097F45CC");

        public Express1UspsCarrierSetupTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            payload = new CarrierConfiguration
            {
                AdditionalData = JObject.Parse("{express1: {username: \"user\", password: \"password\"}}"),
                HubVersion = 2,
                HubCarrierID = carrierId,
                RequestedLabelFormat = ThermalLanguage.None,
                Address = new Warehouse.Configuration.DTO.ConfigurationAddress()
            };

            carrierAccountRepository = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();

            var factory = mock.CreateMock<IIndex<ShipmentTypeCode, ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>>();
            factory.Setup(x => x[ShipmentTypeCode.Express1Usps]).Returns(carrierAccountRepository.Object);
            mock.Provide(factory.Object);

            this.shipmentTypeSetupActivity = mock.Mock<IShipmentTypeSetupActivity>();
            this.shippingSettings = mock.Mock<IShippingSettings>();
            this.printHelper = mock.Mock<IShipmentPrintHelper>();
        }

        [Fact]
        public async Task Setup_Returns_WhenCarrierIDMatches_AndHubVersionIsEqual()
        {
            var accounts = new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    Username = "username",
                    HubCarrierId = carrierId,
                    HubVersion = 2,
                    UspsReseller = (int) UspsResellerType.Express1
                }
            };

            carrierAccountRepository.Setup(x =>
                x.AccountsReadOnly).Returns(accounts);

            await mock.Create<Express1UspsCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x =>
                x.Save(It.IsAny<UspsAccountEntity>()), Times.Never);
        }

        [Fact]
        public async Task Setup_ReturnsExistingAccount_WhenCarriedIdMatches()
        {
            var accounts = new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    Username = "username",
                    HubCarrierId = carrierId,
                    HubVersion = 1,
                    UspsReseller = (int) UspsResellerType.Express1,
                    IsNew = false,
                }
            };

            carrierAccountRepository.Setup(x =>
                x.Accounts).Returns(accounts);

            carrierAccountRepository.Setup(x =>
                x.AccountsReadOnly).Returns(accounts);

            await mock.Create<Express1UspsCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x =>
                x.Save(It.Is<UspsAccountEntity>(y => y.Username == "username")), Times.Once);
        }

        [Fact]
        public async Task Setup_SavesEncryptedPassword()
        {
            var accounts = new List<UspsAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            await mock.Create<Express1UspsCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x =>
                x.Save(It.Is<UspsAccountEntity>(y =>
                    SecureText.Decrypt(y.Password, y.Username) == "password")), Times.Once);
        }

        [Fact]
        public async Task Setup_CreatesNewAccount_WhenNoPreviousExpress1AccountsExist()
        {
            var accounts = new List<UspsAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<Express1UspsCarrierSetup>();
            await testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<UspsAccountEntity>()), Times.Once);
        }

        [Fact]
        public async Task Setup_CallsInitializationMethods_WhenNoPreviousExpress1AccountsExist()
        {
            var accounts = new List<UspsAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<Express1UspsCarrierSetup>();
            await testObject.Setup(payload);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(ShipmentTypeCode.Express1Usps, ShipmentOriginSource.Account, false, ThermalLanguage.None), Times.Once);
            shippingSettings.Verify(x => x.MarkAsConfigured(ShipmentTypeCode.Express1Usps), Times.Once);
            printHelper.Verify(x => x.InstallDefaultRules(ShipmentTypeCode.Express1Usps), Times.Once);
        }

        [Fact]
        public async Task Setup_DoesNotCallInitilizationMethods_WhenPreviousExpress1AccountsExist()
        {
            var accounts = new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    Username = "username",
                    FirstName = "blah",
                    IsNew = false
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<Express1UspsCarrierSetup>();
            await testObject.Setup(payload);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentOriginSource>(), It.IsAny<bool>(), It.IsAny<ThermalLanguage>()), Times.Never);
            shippingSettings.Verify(x => x.MarkAsConfigured(It.IsAny<ShipmentTypeCode>()), Times.Never);
            printHelper.Verify(x => x.InstallDefaultRules(It.IsAny<ShipmentTypeCode>()), Times.Never);
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
                    HubCarrierId = carrierId,
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<Express1UspsCarrierSetup>();
            await testObject.Setup(payload);

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

            var testObject = mock.Create<Express1UspsCarrierSetup>();
            await testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.Is<UspsAccountEntity>(y => y.HubCarrierId == carrierId)), Times.Once);
        }
    }
}