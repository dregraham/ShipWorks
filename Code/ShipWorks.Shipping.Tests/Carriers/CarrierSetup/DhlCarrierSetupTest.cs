using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
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
        private readonly ICarrierAccountDescription accountDescription;

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

            mock.Mock<IShipmentTypeSetupActivity>();
            mock.Mock<IShippingSettings>();
            mock.Mock<IShipmentPrintHelper>();
        }

        [Fact]
        public void Setup_Returns_WhenCarrierIdMatches_AndHubVersionIsEqual()
        {
            var carrierDescription = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ICarrierAccountDescription>>();

            carrierDescription.Setup(x => x[ShipmentTypeCode.DhlExpress])
                .Returns(new DhlExpressAccountDescription());
            mock.Provide(carrierDescription.Object);
            
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

            mock.Create<DhlCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x => x.Save(It.IsAny<DhlExpressAccountEntity>()), Times.Never);
        }

        [Fact]
        public void Setup_ReturnsExistingAccount_WhenCarriedIdMatches()
        {
            var carrierDescription = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ICarrierAccountDescription>>();

            carrierDescription.Setup(x => x[ShipmentTypeCode.DhlExpress])
                .Returns(new DhlExpressAccountDescription());
            mock.Provide(carrierDescription.Object);

            var accounts = new List<DhlExpressAccountEntity>
            {
                new DhlExpressAccountEntity
                {
                    AccountNumber = 123,
                    HubCarrierId = carrierId,
                    HubVersion = 1
                }
            };

            carrierAccountRepository.Setup(x => x.Accounts).Returns(accounts);
            carrierAccountRepository.Setup(x => x.AccountsReadOnly).Returns(accounts);

            mock.Create<DhlCarrierSetup>().Setup(payload);

            carrierAccountRepository.Verify(x => 
                x.Save(It.Is<DhlExpressAccountEntity>(y => y.AccountNumber == 123)), Times.Once);

        }
    }
}