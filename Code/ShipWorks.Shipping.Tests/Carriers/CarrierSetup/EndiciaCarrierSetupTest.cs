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
    public class EndiciaCarrierSetupTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>> carrierAccountRepository;
        private readonly CarrierConfiguration payload;

        private readonly Guid carrierId = new Guid("117CD221-EC30-41EB-BBB3-58E6097F45CC");

        public EndiciaCarrierSetupTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            payload = new CarrierConfiguration
            {
                AdditionalData = JObject.Parse("{endicia: {accountNumber: \"account\", passphrase: \"passphrase\"}}"),
                HubVersion = 2,
                HubCarrierID = carrierId,
                RequestedLabelFormat = ThermalLanguage.None,
                Address = new Warehouse.Configuration.DTO.ConfigurationAddress()
            };

            carrierAccountRepository = mock.Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>>();
            mock.Mock<IShipmentTypeSetupActivity>();
            mock.Mock<IShippingSettings>();
            mock.Mock<IShipmentPrintHelper>();
        }

        [Fact]
        public async Task Setup_Returns_WhenCarrierIDMatches_AndHubVersionIsEqual()
        {
            var accounts = new List<EndiciaAccountEntity>
            {
                new EndiciaAccountEntity
                {
                    AccountNumber = "foo",
                    HubCarrierId = carrierId,
                    HubVersion = 2
                }
            };

            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);
            await mock.Create<EndiciaCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<EndiciaAccountEntity>()), Times.Never);
        }

        [Fact]
        public async Task Setup_ReturnsExistingAccount_WhenCarriedIdMatches()
        {
            var accounts = new List<EndiciaAccountEntity>
            {
                new EndiciaAccountEntity
                {
                    AccountNumber = "foo",
                    HubCarrierId = carrierId,
                    HubVersion = 1,
                    IsNew = false
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            await mock.Create<EndiciaCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.Is<EndiciaAccountEntity>(y => y.AccountNumber == "foo")), Times.Once);
        }

        [Fact]
        public async Task Setup_SavesEncryptedPassphrase()
        {
            var accounts = new List<EndiciaAccountEntity>
            {
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            await mock.Create<EndiciaCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.Is<EndiciaAccountEntity>(y => SecureText.Decrypt(y.ApiUserPassword, "Endicia") == "passphrase")), Times.Once);
        }
    }
}