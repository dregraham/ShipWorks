using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Stores.Content
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OrderManagerTest : IDisposable
    {
        private readonly DataContext context;

        public OrderManagerTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            Modify.Order(context.Order)
                .WithOrderNumber(123999)
                .WithItem()
                .WithItem()
                .WithShipment()
                .WithShipment()
                .Save();
        }

        [Fact]
        public void Load_ThrowsArgumentNullException_WhenPrefetchPathIsNull()
        {
            var testObject = context.Mock.Create<OrderManager>();

            Assert.Throws<ArgumentNullException>(() => testObject.LoadOrder(context.Order.OrderID, null));
        }

        [Fact]
        public void Load_LoadsItems_WhenItemsAreInPrefetchPath()
        {
            IPrefetchPath2 prefetchPath = new PrefetchPath2(EntityType.OrderEntity);
            prefetchPath.Add(OrderEntity.PrefetchPathOrderItems);

            var testObject = context.Mock.Create<OrderManager>();
            var loadedOrder = testObject.LoadOrder(context.Order.OrderID, prefetchPath);

            Assert.Equal(2, loadedOrder.OrderItems.Count);
        }

        [Fact]
        public void Load_DoesNotLoadItems_WhenItemsAreNotInPrefetchPath()
        {
            IPrefetchPath2 prefetchPath = new PrefetchPath2(EntityType.OrderEntity);

            var testObject = context.Mock.Create<OrderManager>();
            var loadedOrder = testObject.LoadOrder(context.Order.OrderID, prefetchPath);

            Assert.Equal(0, loadedOrder.OrderItems.Count);
        }

        public void Dispose() => context.Dispose();
    }
}
