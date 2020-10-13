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
using ShipWorks.Shipping.Carriers.CarrierSetup;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.Configuration.DTO.ShippingSettings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.CarrierSetup
{
    public class OnTracCarrierSetupTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity>> carrierAccountRepository;
        private readonly CarrierConfiguration payload;

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
            mock.Mock<IShipmentTypeSetupActivity>();
            mock.Mock<IShippingSettings>();
            mock.Mock<IShipmentPrintHelper>();
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
                    AccountNumber = 123,
                    HubCarrierId = carrierId,
                    HubVersion = 1
                }
            };

            carrierAccountRepository.Setup(x =>
                x.Accounts).Returns(accounts);

            carrierAccountRepository.Setup(x =>
                x.AccountsReadOnly).Returns(accounts);

            await mock.Create<OnTracCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x =>
                x.Save(It.Is<OnTracAccountEntity>(y => y.AccountNumber == 123)), Times.Once);
        }

        [Fact]
        public async Task Setup_SavesEncryptedPassword()
        {
            var accounts = new List<OnTracAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x =>
                x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x =>
                x.AccountsReadOnly).Returns(accounts);

            await mock.Create<OnTracCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x =>
                x.Save(It.Is<OnTracAccountEntity>(y =>
                    SecureText.Decrypt(y.Password, y.AccountNumber.ToString()) == "password")), Times.Once);
        }
    }
}