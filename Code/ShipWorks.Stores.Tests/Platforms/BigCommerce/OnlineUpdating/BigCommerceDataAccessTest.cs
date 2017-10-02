using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class DataAccessTest : IDisposable
    {
        readonly AutoMock mock;

        public DataAccessTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public async Task GetLatestActiveShipmentAsync_DelegatesToOrderManager()
        {
            var testObject = mock.Create<BigCommerceDataAccess>();

            await testObject.GetLatestActiveShipmentAsync(1006);

            mock.Mock<IOrderManager>()
                .Verify(x => x.GetLatestActiveShipmentAsync(1006));
        }

        [Fact]
        public async Task GetLatestActiveShipmentAsync_ReturnsRetrievedShipment()
        {
            var shipment = new ShipmentEntity();
            mock.Mock<IOrderManager>()
                .Setup(x => x.GetLatestActiveShipmentAsync(It.IsAny<long>()))
                .ReturnsAsync(shipment);

            var testObject = mock.Create<BigCommerceDataAccess>();

            var result = await testObject.GetLatestActiveShipmentAsync(1006);

            Assert.Equal(shipment, result);
        }

        [Fact]
        public void GetOverriddenServiceUsed_DelegatesToShippingManager()
        {
            var shipment = new ShipmentEntity();
            var testObject = mock.Create<BigCommerceDataAccess>();

            testObject.GetOverriddenServiceUsed(shipment);

            mock.Mock<IShippingManager>()
                .Verify(x => x.GetOverriddenServiceUsed(shipment));
        }

        [Fact]
        public void GetOverriddenServiceUsed_ReturnsRetrievedShipment()
        {
            mock.Mock<IShippingManager>()
                .Setup(x => x.GetOverriddenServiceUsed(It.IsAny<ShipmentEntity>()))
                .Returns("Foo");

            var testObject = mock.Create<BigCommerceDataAccess>();

            var result = testObject.GetOverriddenServiceUsed(new ShipmentEntity());

            Assert.Equal("Foo", result);
        }

        [Fact]
        public async Task GetShipmentAsync_DelegatesToShippingManager()
        {
            var testObject = mock.Create<BigCommerceDataAccess>();

            await testObject.GetShipmentAsync(1031);

            mock.Mock<IShippingManager>()
                .Verify(x => x.GetShipmentAsync(1031));
        }

        [Fact]
        public async Task GetShipmentAsync_ReturnsRetrievedShipment()
        {
            var shipment = new ShipmentEntity();
            var adapter = mock.CreateMock<ICarrierShipmentAdapter>();
            adapter.SetupGet(x => x.Shipment).Returns(shipment);

            mock.Mock<IShippingManager>()
                .Setup(x => x.GetShipmentAsync(It.IsAny<long>()))
                .ReturnsAsync(adapter.Object);

            var testObject = mock.Create<BigCommerceDataAccess>();

            var result = await testObject.GetShipmentAsync(1006);

            Assert.Equal(shipment, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
