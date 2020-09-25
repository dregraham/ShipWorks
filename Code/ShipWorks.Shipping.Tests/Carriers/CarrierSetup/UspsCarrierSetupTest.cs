﻿using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
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
                HubVersion = 1,
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
        public void Setup_CreatesNewAccount_WhenNoPreviousUSPSAccountsExist()
        {
            var testObject = mock.Create<UspsCarrierSetup>();
            testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<UspsAccountEntity>()), Times.Once);
        }

        [Fact]
        public void Setup_Returns_WhenCarrierIDMatches_AndHubVersionIsEqual()
        {
            var accounts = new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    HubCarrierId = carrierID,
                    HubVersion = 1
                }
            };

            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UspsCarrierSetup>();
            testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<UspsAccountEntity>()), Times.Never);
        }

        [Fact]
        public void Setup_ReturnsExistingAccount_WhenCarrierIDMatches()
        {
            var accounts = new List<UspsAccountEntity>
            {
                new UspsAccountEntity
                {
                    Username = "user",
                    FirstName = "blah",
                    IsNew = false,
                    HubCarrierId = carrierID
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UspsCarrierSetup>();
            testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.Is<UspsAccountEntity>(y => y.Username == "user" && y.FirstName == "blah")), Times.Once);
        }

        [Fact]
        public void Setup_CallsInitializationMethods_WhenNoPreviousUSPSAccountsExist()
        {
            var testObject = mock.Create<UspsCarrierSetup>();
            testObject.Setup(payload);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(ShipmentTypeCode.Usps, ShipmentOriginSource.Account, false, ThermalLanguage.None), Times.Once);
            shippingSettings.Verify(x => x.MarkAsConfigured(ShipmentTypeCode.Usps), Times.Once);
            printHelper.Verify(x => x.InstallDefaultRules(ShipmentTypeCode.Usps), Times.Once);
        }

        [Fact]
        public void Setup_DoesNotCallInitilizationMethods_WhenPreviousUSPSAccountsExist()
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
            testObject.Setup(payload);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentOriginSource>(), It.IsAny<bool>(), It.IsAny<ThermalLanguage>()), Times.Never);
            shippingSettings.Verify(x => x.MarkAsConfigured(It.IsAny<ShipmentTypeCode>()), Times.Never);
            printHelper.Verify(x => x.InstallDefaultRules(It.IsAny<ShipmentTypeCode>()), Times.Never);
        }
    }
}
