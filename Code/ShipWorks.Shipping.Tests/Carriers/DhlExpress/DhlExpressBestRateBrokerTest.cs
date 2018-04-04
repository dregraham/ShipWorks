using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressBestRateBrokerTest : IDisposable
    {
        private readonly AutoMock mock;

        public DhlExpressBestRateBrokerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks()
        {
            var testObject = mock.Create<DhlExpressBestRateBroker>();

            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity()));
        }

        [Fact]
        public void GetBestRates_UsesCustomsItemsFromOriginalShipment()
        {
            DhlExpressAccountEntity account = new DhlExpressAccountEntity() { CountryCode = "US" };


            var genericRepositoryMock = mock.Mock<ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity>>();
            genericRepositoryMock.Setup(x => x.Accounts)
                .Returns(new List<DhlExpressAccountEntity> { account });
            genericRepositoryMock.Setup(x => x.DefaultProfileAccount)
                .Returns(account);

            var shipmentTypeMock = mock.Mock<ShipmentType>();
            shipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.DhlExpress);
            shipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                .Callback<ShipmentEntity>(x =>
                {
                    x.DhlExpress = new DhlExpressShipmentEntity();
                    x.DhlExpress.Packages.Add(new DhlExpressPackageEntity());
                    x.CustomsItems.Clear();
                });

            var testShipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.BestRate,
                BestRate = new BestRateShipmentEntity(),
                CustomsItems = { new ShipmentCustomsItemEntity() { Description = "blah" } },
                OriginCountryCode = "US"
            };

            List<BrokerException> brokerExceptions = new List<BrokerException>();

            RateGroup rateGroup = new RateGroup(new List<RateResult>());

            var testObject = mock.Create<DhlExpressBestRateBroker>();

            ShipmentEntity shipmentUsedForRating = null;
            testObject.GetRatesAction = (shipment, type) =>
            {
                shipmentUsedForRating = shipment;
                return rateGroup;
            };

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(ShipmentTypeCode.DhlExpress, shipmentUsedForRating.ShipmentTypeCode);
            Assert.Equal("blah", shipmentUsedForRating.CustomsItems.Single().Description);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}