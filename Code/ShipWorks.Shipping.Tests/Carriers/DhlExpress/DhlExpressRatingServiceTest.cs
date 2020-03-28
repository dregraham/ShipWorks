using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Tests.Shared;
using System;
using Moq;
using ShipWorks.Data.Model.EntityInterfaces;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressRatingServiceTest : IDisposable
    {
        private readonly AutoMock mock;

        public DhlExpressRatingServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetRates_ThrowsShippingException_WhenNoDHLAccounts()
        {
            mock.Mock<IDhlExpressAccountRepository>().Setup(x => x.AccountsReadOnly).Returns(new IDhlExpressAccountEntity[0]);

            var testObject = mock.Create<DhlExpressRatingService>();

            Assert.Throws<ShippingException>(() => testObject.GetRates(new ShipmentEntity()));
        }

        [Fact]
        public void GetRates_UsesStampsRatingClient_WhenAccountDoesNotHaveShipEngineCarrierID()
        {
            DhlExpressAccountEntity account = new DhlExpressAccountEntity
            {
                ShipEngineCarrierId = null
            };

            ShipmentEntity shipment = new ShipmentEntity();

            var accountRepo = mock.Mock<IDhlExpressAccountRepository>();
            accountRepo.Setup(x => x.AccountsReadOnly).Returns(new[] {account});
            accountRepo.Setup(x => x.GetAccountReadOnly(shipment)).Returns(account);

            var stampsClient = mock.Mock<IDhlExpressStampsRatingClient>();

            var testObject = mock.Create<DhlExpressRatingService>();

            testObject.GetRates(shipment);

            stampsClient.Verify(x => x.GetRates(shipment), Times.Once);
        }

        [Fact]
        public void GetRates_UsesShipEngineRatingClient_WhenAccountHasShipEngineCarrierID()
        {
            DhlExpressAccountEntity account = new DhlExpressAccountEntity
            {
                ShipEngineCarrierId = "foo"
            };

            ShipmentEntity shipment = new ShipmentEntity();

            var accountRepo = mock.Mock<IDhlExpressAccountRepository>();
            accountRepo.Setup(x => x.AccountsReadOnly).Returns(new[] {account});
            accountRepo.Setup(x => x.GetAccountReadOnly(shipment)).Returns(account);

            var shipEngineClient = mock.Mock<IDhlExpressShipEngineRatingClient>();

            var testObject = mock.Create<DhlExpressRatingService>();

            testObject.GetRates(shipment);

            shipEngineClient.Verify(x => x.GetRates(shipment), Times.Once);
        }

        [Fact]
        public void GetRates_RethrowsShippingException_WhenExceptionOccurs()
        {
            DhlExpressAccountEntity account = new DhlExpressAccountEntity
            {
                ShipEngineCarrierId = "foo"
            };

            ShipmentEntity shipment = new ShipmentEntity();

            var accountRepo = mock.Mock<IDhlExpressAccountRepository>();
            accountRepo.Setup(x => x.AccountsReadOnly).Returns(new[] {account});
            accountRepo.Setup(x => x.GetAccountReadOnly(shipment)).Throws<Exception>();

            var testObject = mock.Create<DhlExpressRatingService>();

            Assert.Throws<ShippingException>(() => testObject.GetRates(shipment));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
