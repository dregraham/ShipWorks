using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Usps;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Tests.Shipping.Carriers.BestRate.Fake;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class BestRateBrokerTest : IDisposable
    {
        private readonly AutoMock mock;

        public BestRateBrokerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetBestRates_AddsBrokerException_WhenExceptionsRateFootnoteFactoryIsReturnedFromBase()
        {
            UspsAccountEntity account = new UspsAccountEntity() { CountryCode = "US" };

            var genericRepositoryMock = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            genericRepositoryMock.Setup(x => x.Accounts)
                                 .Returns(new List<UspsAccountEntity> { account });
            genericRepositoryMock.Setup(x => x.DefaultProfileAccount)
                                 .Returns(account);

            var genericShipmentTypeMock = mock.CreateMock<UspsShipmentType>();
            genericShipmentTypeMock.Setup(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);
            genericShipmentTypeMock.Setup(x => x.ConfigureNewShipment(It.IsAny<ShipmentEntity>()))
                .Callback<ShipmentEntity>(x =>
                {
                    x.Postal = new PostalShipmentEntity()
                    {
                        Usps = new UspsShipmentEntity()
                    };
                });
            mock.Provide(genericShipmentTypeMock);

            var testShipment = new ShipmentEntity
            {
                ShipmentType = (int) ShipmentTypeCode.BestRate,
                ContentWeight = 12.1,
                BestRate = new BestRateShipmentEntity(),
                OriginCountryCode = "US"
            };

            List<BrokerException> brokerExceptions = new List<BrokerException>();

            RateGroup rateGroup = new RateGroup(new List<RateResult>());
            rateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Usps, new Exception("blah")));

            var testObject = new FakeBestRateBroker(genericShipmentTypeMock.Object, genericRepositoryMock.Object, "blah")
            {
                GetRatesAction = (shipment, type) => rateGroup
            };

            testObject.GetBestRates(testShipment, brokerExceptions);

            Assert.Equal(1, brokerExceptions.Count);
            Assert.Equal("blah", brokerExceptions.Single().Message);
        }


        public void Dispose()
        {
            mock.Dispose();
        }

    }
}