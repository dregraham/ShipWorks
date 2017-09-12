using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.BuyDotCom
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class DataAccessTest : IDisposable
    {
        private readonly DataContext context;
        private readonly BuyDotComStoreEntity store;
        private readonly OrderEntity order;
        private readonly ShipmentEntity shipment;
        private readonly DataAccess testObject;

        public DataAccessTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            testObject = context.Mock.Create<DataAccess>();

            store = Create.Store<BuyDotComStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.BuyDotCom)
                .Save();

            order = Create.Order(store, context.Customer)
                .WithItem<BuyDotComOrderItemEntity>(x => x.Set(y => y.ListingID, 5150))
                .Set(x => x.IsManual, true)
                .Set(x => x.OrderNumber, 9911)
                .Set(x => x.OrderNumberComplete, "A-9911")
                .Save();

            shipment = Create.Shipment(order).Save();
        }

        [Fact]
        public async Task GetShipmentDataAsync_ReturnsDetails_WhenOrderExists()
        {
            var shipmentDetails = await testObject.GetShipmentDataAsync(new[] { order.OrderID });

            Assert.Equal(1, shipmentDetails.Count());

            var shipmentDetail = shipmentDetails.Single();
            Assert.Equal(shipment.ShipmentID, shipmentDetail.Shipment.ShipmentID);

            Assert.Equal(1, shipmentDetail.Orders.Count());

            var orderDetail = shipmentDetail.Orders.Single();
            Assert.Equal(order.OrderNumberComplete, orderDetail.OrderNumberComplete);
            Assert.Equal(order.IsManual, orderDetail.IsManual);

            Assert.Equal(1, orderDetail.Items.Count());

            var itemDetail = orderDetail.Items.Single();
            Assert.Equal(order.OrderItems.Single().OrderItemID, itemDetail.OrderItemID);
        }

        [Fact]
        public async Task GetShipmentDataAsync_ReturnsCombinedDetails_WhenOrderIsCombined()
        {
            Modify.Order(order)
                .WithItem<BuyDotComOrderItemEntity>(i => i.Set(x => x.OriginalOrderID, 4006))
                .WithItem<BuyDotComOrderItemEntity>(i => i.Set(x => x.OriginalOrderID, 4006))
                .WithItem<BuyDotComOrderItemEntity>(i => i.Set(x => x.OriginalOrderID, 5006))
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
                .Save();

            Create.Entity<OrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.StoreID, store.StoreID)
                .Set(x => x.OriginalOrderID, 4006)
                .Set(x => x.IsManual, true)
                .Set(x => x.OrderNumberComplete, "456")
                .Save();

            Create.Entity<OrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.StoreID, store.StoreID)
                .Set(x => x.OriginalOrderID, 5006)
                .Set(x => x.IsManual, false)
                .Set(x => x.OrderNumberComplete, "789")
                .Save();

            var shipmentDetails = await testObject.GetShipmentDataAsync(new[] { order.OrderID });

            var shipmentDetail = shipmentDetails.Single();

            Assert.Equal(2, shipmentDetail.Orders.Count());

            var firstOrder = shipmentDetail.Orders.FirstOrDefault(x => x.OrderNumberComplete == "456");
            Assert.NotNull(firstOrder);
            Assert.True(firstOrder.IsManual);
            Assert.Equal(2, firstOrder.Items.Count());

            var secondOrder = shipmentDetail.Orders.FirstOrDefault(x => x.OrderNumberComplete == "789");
            Assert.NotNull(secondOrder);
            Assert.False(secondOrder.IsManual);
            Assert.Equal(1, secondOrder.Items.Count());
        }

        [Fact]
        public async Task GetShipmentDataAsync_ExtraShipmentDataIsLoaded()
        {
            Modify.Shipment(shipment)
                .AsPostal(p => p.AsUsps(u => u.Set(x => x.UspsAccountID, 123)))
                .Save();

            var shipmentDetails = await testObject.GetShipmentDataAsync(new[] { order.OrderID });

            var shipmentDetail = shipmentDetails.Single();
            Assert.NotNull(shipmentDetail.Shipment.Postal);
            Assert.NotNull(shipmentDetail.Shipment.Postal.Usps);
            Assert.Equal(123, shipmentDetail.Shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public async Task GetShipmentDataAsync_ReturnsEmptyDetails_WhenOrderHasNoShipments()
        {
            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                await adapter.DeleteEntityAsync(shipment);
            }

            var shipmentDetails = await testObject.GetShipmentDataAsync(new[] { order.OrderID });

            Assert.Empty(shipmentDetails);
        }

        [Fact]
        public async Task GetShipmentDataAsync_ReturnsEmptyDetails_WhenOrderIsNotFound()
        {
            var shipmentDetails = await testObject.GetShipmentDataAsync(new[] { -1006L });

            Assert.Empty(shipmentDetails);
        }

        public void Dispose() => context.Dispose();
    }
}