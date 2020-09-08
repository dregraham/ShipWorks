using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.ApplicationCore.Licensing.Activation;
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
        private readonly UspsCarrierSetup testObject;
        private readonly CarrierConfigurationPayload payload;

        private readonly Guid carrierID = new Guid("117CD221-EC30-41EB-BBB3-58E6097F45CC");

        public UspsCarrierSetupTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            this.payload = new CarrierConfigurationPayload
            {
                AdditionalData = JObject.Parse("{account: {username: \"user\", password: \"password\" } }"),
                HubVersion = 1,
                HubCarrierId = carrierID
            };

            this.carrierAccountRepository = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            this.shipmentTypeSetupActivity = mock.Mock<IShipmentTypeSetupActivity>();
            this.shippingSettings = mock.Mock<IShippingSettings>();
            this.printHelper = mock.Mock<IShipmentPrintHelper>();

            var encryptionProvider = mock.Mock<IEncryptionProvider>();
            encryptionProvider.Setup(x => x.Decrypt(It.IsAny<string>())).Returns("password");

            mock.Mock<IEncryptionProviderFactory>().Setup(x => x.CreateHubConfigEncryptionProvider()).Returns(encryptionProvider);

            this.testObject = mock.Create<UspsCarrierSetup>();
        }


        [Fact]
        public void Setup_CreatesNewAccount_WhenNoPreviousUSPSAccountsExist()
        {
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

            testObject.Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.Is<UspsAccountEntity>(y => y.Username == "user" && y.FirstName == "blah")), Times.Once);
        }

        [Fact]
        public void Setup_CallsInitializationMethods_WhenNoPreviousUSPSAccountsExist()
        {
            testObject.Setup(payload);

            shipmentTypeSetupActivity.Verify(x => x.InitializeShipmentType(ShipmentTypeCode.Usps, ShipmentOriginSource.Account), Times.Once);
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

            testObject.Setup(payload);

            shipmentTypeSetupActivity.Verify(x => x.InitializeShipmentType(It.IsAny<ShipmentTypeCode>(), It.IsAny<ShipmentOriginSource>()), Times.Never);
            shippingSettings.Verify(x => x.MarkAsConfigured(It.IsAny<ShipmentTypeCode>()), Times.Never);
            printHelper.Verify(x => x.InstallDefaultRules(It.IsAny<ShipmentTypeCode>()), Times.Never);
        }
    }
}
