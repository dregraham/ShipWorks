using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Loading;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    public class ShipmentLoaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderEntity order;
        private readonly ShipmentLoader testObject;

        public ShipmentLoaderTest(DatabaseFixture db)
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            ContainerInitializer.Initialize(mock.Container);

            db.CreateDataContext(mock);

            var store = Create.Store<GenericModuleStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A Test Store")
                .Save();

            var customer = Create.Entity<CustomerEntity>().Save();

            order = Create.Order(store, customer)
                .WithOrderNumber(12345)
                .WithShipAddress("1 Memorial Dr.", "Suite 2000", "St. Louis", "MO", "63102", "US")
                .Save();

            // Reset the static fields before each test
            StoreManager.CheckForChanges();

            testObject = mock.Create<ShipmentLoader>();
        }

        [Fact]
        public async Task Load_ReturnsOrder_WhenOrderHasNoShipments()
        {
            var loadedOrder = await testObject.Load(order.OrderID);

            Assert.Equal(order.OrderID, loadedOrder.Order.OrderID);
        }

        [Fact]
        public async Task Load_ReturnsOrder_WhenOrderHasShipment()
        {
            Create.Shipment(order).AsUps().Save();

            var loadedOrder = await testObject.Load(order.OrderID);

            Assert.Equal(order.OrderID, loadedOrder.Order.OrderID);
        }

        [Fact]
        public async Task Load_ReturnsShipmentAdapters_FromLoadedShipments()
        {
            Create.Shipment(order)
                .AsPostal(x => x.AsUsps())
                .Save();

            var loadedOrder = await testObject.Load(order.OrderID);

            Assert.IsAssignableFrom<UspsShipmentAdapter>(loadedOrder.ShipmentAdapters.Single());
        }

        [Fact]
        public async Task Load_CreatesShipment_WhenOrderHasNoShipments()
        {
            Mock<IShippingConfiguration> configuration = mock.Override<IShippingConfiguration>();
            configuration.Setup(x => x.ShouldAutoCreateShipment(It.IsAny<OrderEntity>())).Returns(true);

            var loadedOrder = await mock.Create<ShipmentLoader>().Load(order.OrderID);

            Assert.Equal(1, loadedOrder.Order.Shipments.Count);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
