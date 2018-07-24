using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Stores.Content
{
    public class OrderManagerTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<ISqlAdapter> sqlAdapter;
        private OrderManager testObject;

        public OrderManagerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create());

            testObject = mock.Create<OrderManager>();
        }

        [Fact]
        public void GetItems_DoesNotGoToDatabase_WhenOrderHasItems()
        {
            var order = Create.Order(new StoreEntity(), new CustomerEntity())
                .WithItem()
                .Build();

            testObject.GetItems(order);

            sqlAdapter.Verify(x => x.FetchQuery(It.IsAny<EntityQuery<OrderItemEntity>>()), Times.Never);
        }

        [Fact]
        public void GetItems_ReturnsExistingOrders_WhenOrderHasItems()
        {
            var item1 = new OrderItemEntity();
            var item2 = new OrderItemEntity();

            var order = new OrderEntity();
            order.OrderItems.AddRange(new[] { item1, item2 });

            var items = testObject.GetItems(order);

            Assert.Contains(item1, items);
            Assert.Contains(item2, items);
        }

        [Fact]
        public void GetItems_QueriesDatabase_WhenOrderHasNoItems()
        {
            var order = Create.Order(new StoreEntity(), new CustomerEntity()).Build();

            testObject.GetItems(order);

            sqlAdapter.Verify(x => x.FetchQuery(It.IsAny<EntityQuery<OrderItemEntity>>()));
        }

        [Fact]
        public void GetItems_ReturnsRetrievedOrders_WhenOrderHasNoItems()
        {
            var item1 = new OrderItemEntity();
            var item2 = new OrderItemEntity();

            sqlAdapter.Setup(x => x.FetchQuery(It.IsAny<EntityQuery<OrderItemEntity>>()))
                .Returns(new[] { item1, item2 }.ToEntityCollection());

            var order = new OrderEntity();

            var items = testObject.GetItems(order);

            Assert.Contains(item1, items);
            Assert.Contains(item2, items);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
