using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Carriers.Dhl.API;
using ShipWorks.Shipping.Carriers.Dhl.API.ShipEngine;
using ShipWorks.Shipping.Carriers.Dhl.API.Stamps;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress.API
{
    public class DhlExpressLabelClientFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DhlExpressLabelClientFactory testObject;

        public DhlExpressLabelClientFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<DhlExpressLabelClientFactory>();
        }

        [Fact]
        public void Create_ReturnsShipEngineLabelClient_WhenAccountHasShipEngineID()
        {
            var shipment = new ShipmentEntity();
            var account = new DhlExpressAccountEntity
            {
                ShipEngineCarrierId = "foo"
            };
            mock.CreateKeyedMockOf<ICarrierShipmentRequestFactory>().For(ShipmentTypeCode.DhlExpress);
            mock.Mock<IDhlExpressAccountRepository>().Setup(x => x.GetAccount(shipment)).Returns(account);

            var labelClient = testObject.Create(shipment);

            Assert.IsAssignableFrom<DhlExpressShipEngineLabelClient>(labelClient);
        }

        [Fact]
        public void Create_ReturnsStampsLabelClient_WhenAccountDoesNotHaveShipEngineID()
        {
            var shipment = new ShipmentEntity();
            var account = new DhlExpressAccountEntity
            {
                ShipEngineCarrierId = null
            };

            mock.Mock<IDhlExpressAccountRepository>().Setup(x => x.GetAccount(shipment)).Returns(account);

            var labelClient = testObject.Create(shipment);

            Assert.IsAssignableFrom<DhlExpressStampsLabelClient>(labelClient);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
