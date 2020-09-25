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
using ShipWorks.Shipping.Carriers.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.CarrierSetup
{
    public class UpsCarrierSetupTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> carrierAccountRepository;
        private readonly Mock<IShipmentTypeSetupActivity> shipmentTypeSetupActivity;
        private readonly Mock<IShippingSettings> shippingSettings;
        private readonly Mock<IShipmentPrintHelper> printHelper;
        private readonly CarrierConfiguration payload;

        private readonly Guid carrierID = new Guid("117CD221-EC30-41EB-BBB3-58E6097F45CC");

        public UpsCarrierSetupTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            this.payload = new CarrierConfiguration
            {
                AdditionalData = JObject.Parse("{ups: {accessToken: \"token\", accountNumber: \"account\", rateType: \"0\" } }"),
                HubVersion = 1,
                HubCarrierID = carrierID,
                RequestedLabelFormat = ThermalLanguage.None,
                Address = new Warehouse.Configuration.DTO.ConfigurationAddress
                {
                    FirstName = "Test",
                    MiddleName = "Reginald",
                    LastName = "Jones",
                    Company = "ShipWorks",
                    Street1 = "1 Memorial Drive",
                    Street2 = "Suite 2000",
                    City = "St. Louis",
                    State = "MO",
                    Zip = "63102"
                },
            };

            this.carrierAccountRepository = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            this.shipmentTypeSetupActivity = mock.Mock<IShipmentTypeSetupActivity>();
            this.shippingSettings = mock.Mock<IShippingSettings>();
            this.printHelper = mock.Mock<IShipmentPrintHelper>();
        }


        [Fact]
        public void Setup_CreatesNewAccount_WhenNoPreviousUPSAccountsExist()
        {
            var testObject = mock.Create<UpsCarrierSetup>();
            testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<UpsAccountEntity>()), Times.Once);
        }

        [Fact]
        public void Setup_Returns_WhenCarrierIDMatches_AndHubVersionIsEqual()
        {
            var accounts = new List<UpsAccountEntity>
            {
                new UpsAccountEntity
                {
                    HubCarrierId = carrierID,
                    HubVersion = 1
                }
            };

            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UpsCarrierSetup>();
            testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<UpsAccountEntity>()), Times.Never);
        }

        [Fact]
        public void Setup_ReturnsExistingAccount_WhenCarrierIDMatches()
        {
            var accounts = new List<UpsAccountEntity>
            {
                new UpsAccountEntity
                {
                    AccountNumber = "user",
                    FirstName = "blah",
                    IsNew = false,
                    HubCarrierId = carrierID
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UpsCarrierSetup>();
            testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.Is<UpsAccountEntity>(y => y.AccountNumber == "user")), Times.Once);
        }

        [Fact]
        public void Setup_CallsInitializationMethods_WhenNoPreviousUPSAccountsExist()
        {
            var testObject = mock.Create<UpsCarrierSetup>();
            testObject.Setup(payload);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(ShipmentTypeCode.UpsOnLineTools, ShipmentOriginSource.Account, false, ThermalLanguage.None), Times.Once);
            shippingSettings.Verify(x => x.MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools), Times.Once);
            printHelper.Verify(x => x.InstallDefaultRules(ShipmentTypeCode.UpsOnLineTools), Times.Once);
        }

        [Fact]
        public void Setup_DoesNotCallInitilizationMethods_WhenPreviousUPSAccountsExist()
        {
            var accounts = new List<UpsAccountEntity>
            {
                new UpsAccountEntity
                {
                    AccountNumber = "user",
                    FirstName = "blah",
                    IsNew = false
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            var testObject = mock.Create<UpsCarrierSetup>();
            testObject.Setup(payload);

            shipmentTypeSetupActivity
                .Verify(x => x.InitializeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentOriginSource>(), It.IsAny<bool>(), It.IsAny<ThermalLanguage>()), Times.Never);
            shippingSettings.Verify(x => x.MarkAsConfigured(It.IsAny<ShipmentTypeCode>()), Times.Never);
            printHelper.Verify(x => x.InstallDefaultRules(It.IsAny<ShipmentTypeCode>()), Times.Never);
        }
    }
}
