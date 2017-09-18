using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.BigCommerce
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class DataAccessTest : IDisposable
    {
        private readonly DataContext context;
        private readonly BigCommerceStoreEntity store;
        private readonly OrderEntity order;

        public DataAccessTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            store = Create.Store<BigCommerceStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.BigCommerce)
                .Set(x => x.BigCommerceAuthentication, Interapptive.Shared.Enums.BigCommerceAuthenticationType.Oauth)
                .Save();

            order = Create.Order(store, context.Customer)
                .WithItem<BigCommerceOrderItemEntity>(o => o.Set(x => x.IsDigitalItem, true))
                .WithItem()
                .Set(x => x.IsManual, true)
                .Set(x => x.OrderNumber, 9911)
                .Set(x => x.OrderNumberComplete, "ABC123")
                .Save();
        }

        [Fact]
        public async Task GetOrderDetailsAsync_ReturnsDetails_WhenOrderExists()
        {
            var testObject = context.Mock.Create<BigCommerceDataAccess>();

            var orderDetails = await testObject.GetOrderDetailsAsync(order.OrderID);

            Assert.Equal(order.OrderID, orderDetails.OrderID);
            Assert.Equal(true, orderDetails.AreAllManual);
            Assert.Equal(9911, orderDetails.OrderNumber);
            Assert.Equal("ABC123", orderDetails.OrderNumberComplete);

            Assert.Equal(1, orderDetails.OrdersToUpload.Count());

            var orderToUpload = orderDetails.OrdersToUpload.Single();
            Assert.Equal(order.OrderID, orderToUpload.OrderID);
            Assert.Equal(true, orderToUpload.IsManual);
            Assert.Equal(9911, orderToUpload.OrderNumber);
            Assert.Equal("ABC123", orderToUpload.OrderNumberComplete);
        }

        [Fact]
        public async Task GetOrderDetailsAsync_ReturnsNull_WhenOrderDoesNotExist()
        {
            var testObject = context.Mock.Create<BigCommerceDataAccess>();

            var orderDetails = await testObject.GetOrderDetailsAsync(-100);

            Assert.Null(orderDetails);
        }

        [Fact]
        public async Task GetOrderDetailsAsync_ReturnsCombinedDetails_WhenOrderIsCombined()
        {
            Modify.Order(order)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
                .Save();

            Create.Entity<OrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.StoreID, store.StoreID)
                .Set(x => x.OriginalOrderID, -2006)
                .Set(x => x.IsManual, true)
                .Set(x => x.OrderNumber, 456)
                .Set(x => x.OrderNumberComplete, "456A")
                .Save();

            Create.Entity<OrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.StoreID, store.StoreID)
                .Set(x => x.OriginalOrderID, -3006)
                .Set(x => x.IsManual, false)
                .Set(x => x.OrderNumber, 789)
                .Set(x => x.OrderNumberComplete, "789B")
                .Save();

            var testObject = context.Mock.Create<BigCommerceDataAccess>();

            var orderDetails = await testObject.GetOrderDetailsAsync(order.OrderID);

            Assert.Equal(order.OrderID, orderDetails.OrderID);
            Assert.Equal(false, orderDetails.AreAllManual);
            Assert.Equal(9911, orderDetails.OrderNumber);
            Assert.Equal("ABC123", orderDetails.OrderNumberComplete);

            Assert.Equal(2, orderDetails.OrdersToUpload.Count());

            var firstOrderToUpload = orderDetails.OrdersToUpload.First();
            Assert.Equal(-2006, firstOrderToUpload.OrderID);
            Assert.Equal(true, firstOrderToUpload.IsManual);
            Assert.Equal(456, firstOrderToUpload.OrderNumber);
            Assert.Equal("456A", firstOrderToUpload.OrderNumberComplete);

            var lastOrderToUpload = orderDetails.OrdersToUpload.Last();
            Assert.Equal(-3006, lastOrderToUpload.OrderID);
            Assert.Equal(false, lastOrderToUpload.IsManual);
            Assert.Equal(789, lastOrderToUpload.OrderNumber);
            Assert.Equal("789B", lastOrderToUpload.OrderNumberComplete);
        }

        [Fact]
        public async Task GetOrderItemsAsync_ReturnsItemCollection_WhenItemsExist()
        {
            var testObject = context.Mock.Create<BigCommerceDataAccess>();

            var orderItems = await testObject.GetOrderItemsAsync(order.OrderID);

            Assert.Equal(1, orderItems.Count());

            var bigCommerceOrder = orderItems.SelectMany(x => x.Value).OfType<BigCommerceOrderItemEntity>().Single();
            Assert.True(bigCommerceOrder.IsDigitalItem);
        }

        [Fact]
        public async Task GetOrderItemsAsync_ReturnsEmptyCollection_WhenItemsDoNotExist()
        {
            var testObject = context.Mock.Create<BigCommerceDataAccess>();

            var orderItems = await testObject.GetOrderItemsAsync(-100);

            Assert.Empty(orderItems);
        }

        public void Dispose() => context.Dispose();
    }
}