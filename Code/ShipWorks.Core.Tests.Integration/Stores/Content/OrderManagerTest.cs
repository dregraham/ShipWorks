using System;
using Autofac.Extras.Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Stores.Content
{
    [Collection("Database collection")]
    public class OrderManagerTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderEntity order;

        public OrderManagerTest(DatabaseFixture db)
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            ContainerInitializer.Initialize(mock.Container);

            db.CreateDataContext(mock);

            var store = Create.Entity<GenericModuleStoreEntity>().Save();
            var customer = Create.Entity<CustomerEntity>().Save();

            order = Create.Order(store, customer).WithOrderNumber(123999)
                .WithItem()
                .WithItem()
                .WithShipment()
                .WithShipment()
                .Save();
        }

        [Fact]
        public void Load_ThrowsArgumentNullException_WhenPrefetchPathIsNull()
        {
            var testObject = mock.Create<OrderManager>();

            Assert.Throws<ArgumentNullException>(() => testObject.LoadOrder(order.OrderID, null));
        }

        [Fact]
        public void Load_LoadsItems_WhenItemsAreInPrefetchPath()
        {
            IPrefetchPath2 prefetchPath = new PrefetchPath2(EntityType.OrderEntity);
            prefetchPath.Add(OrderEntity.PrefetchPathOrderItems);

            var testObject = mock.Create<OrderManager>();
            var loadedOrder = testObject.LoadOrder(order.OrderID, prefetchPath);

            Assert.Equal(2, loadedOrder.OrderItems.Count);
        }

        [Fact]
        public void Load_DoesNotLoadItems_WhenItemsAreNotInPrefetchPath()
        {
            IPrefetchPath2 prefetchPath = new PrefetchPath2(EntityType.OrderEntity);

            var testObject = mock.Create<OrderManager>();
            var loadedOrder = testObject.LoadOrder(order.OrderID, prefetchPath);

            Assert.Equal(0, loadedOrder.OrderItems.Count);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
