using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating;
using ShipWorks.Stores.Tests.Integration.Helpers;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.BuyDotCom
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombineSplit")]
    public class BuyDotComCombineAndSplitTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> combineInteraction;
        private Mock<IOrderSplitUserInteraction> splitInteraction;
        private Mock<IAsyncMessageHelper> asyncMessageHelper;
        private readonly BuyDotComStoreEntity store;
        private readonly OrderEntity orderA;
        private OrderEntity orderB;
        private readonly OrderEntity orderD;
        private readonly OrderEntity orderM;
        private readonly Dictionary<long, OrderEntity> orders;
        private readonly CombineSplitHelpers combineSplitHelpers;

        public BuyDotComCombineAndSplitTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                combineInteraction = mock.Override<IOrderCombinationUserInteraction>();
                splitInteraction = mock.Override<IOrderSplitUserInteraction>();
                mock.Override<IMessageHelper>();
                asyncMessageHelper = mock.Override<IAsyncMessageHelper>();
            });

            combineSplitHelpers = new CombineSplitHelpers(context, splitInteraction, combineInteraction);

            asyncMessageHelper.Setup(x => x.ShowProgressDialog(AnyString, AnyString))
                .ReturnsAsync(context.Mock.Build<ISingleItemProgressDialog>());

            store = Create.Store<BuyDotComStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.BuyDotCom)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            Create.Order(store, context.Customer).Save();

            orderA = Create.Order<OrderEntity>(store, context.Customer)
                .WithItem<BuyDotComOrderItemEntity>(i => i.Set(x => x.ReceiptItemID, "100").Set(x => x.Quantity, 3))
                .Set(x => x.OrderNumber, 10)
                .Set(x => x.OrderNumberComplete, "10")
                .Save();
            orderB = Create.Order<OrderEntity>(store, context.Customer)
                .WithItem<BuyDotComOrderItemEntity>(i => i.Set(x => x.ReceiptItemID, "200").Set(x => x.Quantity, 3))
                .Set(x => x.OrderNumber, 20)
                .Set(x => x.OrderNumberComplete, "20")
                .Save();
            orderD = Create.Order<OrderEntity>(store, context.Customer)
                .WithItem<BuyDotComOrderItemEntity>(i => i.Set(x => x.ReceiptItemID, "300").Set(x => x.Quantity, 3))
                .Set(x => x.OrderNumber, 30)
                .Set(x => x.OrderNumberComplete, "30")
                .Save();
            orderM = Create.Order(store, context.Customer)
                .WithItem(i => i.Set(x => x.Quantity, 3))
                .Set(x => x.IsManual, true)
                .Set(x => x.OrderNumber, 40)
                .Set(x => x.OrderNumberComplete, "40")
                .Save();

            orders = new Dictionary<long, OrderEntity> { { 1, orderA }, { 2, orderB }, { 3, orderD } };
        }

        [Fact]
        public async Task Split_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);
            var (orderA_2, orderA_3) = await combineSplitHelpers.PerformSplit(orderA_0);
            var (orderA_4, orderA_5) = await combineSplitHelpers.PerformSplit(orderA_1);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_2, orderA_3, orderA_4, orderA_5);

            Assert.Equal(4, shipmentData.SelectMany(x => x.Orders).Where(x => x.OrderNumberComplete == "10").Count());
        }

        [Fact]
        public async Task CombineSplitCombine_WithOrderNumbers()
        {
            OrderEntity orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA, orderB);
            var (orderA_C_O, orderA_C_1) = await combineSplitHelpers.PerformSplit(orderA_C);
            var orderD_C = await combineSplitHelpers.PerformCombine("D-C", orderD, orderA_C_O);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_C_1, orderD_C);

            var orderLists = shipmentData.Select(x => x.Orders.Select(o => o.OrderNumberComplete));
            Assert.Contains(new[] { "10", "20" }, orderLists);
            Assert.Contains(new[] { "10", "20", "30" }, orderLists);
        }

        [Fact]
        public async Task CombineTwoManualOrders_WithOrderNumbers()
        {
            orderB = Create.CreateManualOrder(store, context.Customer, 20);

            var orderA_M_C = await combineSplitHelpers.PerformCombine("40A-M-C", orderM, orderB);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_M_C);

            var orderLists = shipmentData.Select(x => x.Orders.Select(o => o.OrderNumberComplete));
            Assert.Contains(new[] { "40", "20" }, orderLists);
            Assert.True(shipmentData.SelectMany(x => x.Orders).All(x => x.IsManual));
        }

        [Fact]
        public async Task SplitManualCombineWithNotManualOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderM);
            var orderA_M_1_C = await combineSplitHelpers.PerformCombine("40A-M-1-C", orderA_1, orderB);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_M_1_C, orderA_0);

            var dataA_0 = shipmentData.Single(x => x.Shipment.OrderID == orderA_0.OrderID);
            Assert.Equal(new[] { "40" }, dataA_0.Orders.Select(x => x.OrderNumberComplete));
            Assert.True(dataA_0.Orders.All(x => x.IsManual));

            var dataA_M_1_C = shipmentData.Single(x => x.Shipment.OrderID == orderA_M_1_C.OrderID);
            Assert.True(dataA_M_1_C.Orders.Single(x => x.OrderNumberComplete == "40").IsManual);
            Assert.False(dataA_M_1_C.Orders.Single(x => x.OrderNumberComplete == "20").IsManual);
        }

        [Fact]
        public async Task SplitManualOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderM);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_0, orderA_1);

            var dataA_0 = shipmentData.Single(x => x.Shipment.OrderID == orderA_0.OrderID);
            Assert.True(dataA_0.Orders.Single(x => x.OrderNumberComplete == "40").IsManual);

            var dataA_1 = shipmentData.Single(x => x.Shipment.OrderID == orderA_1.OrderID);
            Assert.True(dataA_1.Orders.Single(x => x.OrderNumberComplete == "40").IsManual);
        }

        [Fact]
        public async Task CombineMixManualSplit_WithOrderNumbers()
        {
            var manualOrder = Create.CreateManualOrder(store, context.Customer, 10);

            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, manualOrder);
            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C);

            // Get online identities
            var shipmentData = await GetShipmentData(orderB_0, orderB_1);

            var dataB_0 = shipmentData.Single(x => x.Shipment.OrderID == orderB_0.OrderID);
            Assert.True(dataB_0.Orders.Single(x => x.OrderNumberComplete == "10").IsManual);
            Assert.False(dataB_0.Orders.Single(x => x.OrderNumberComplete == "20").IsManual);

            var dataB_1 = shipmentData.Single(x => x.Shipment.OrderID == orderB_1.OrderID);
            Assert.True(dataB_1.Orders.Single(x => x.OrderNumberComplete == "10").IsManual);
            Assert.False(dataB_0.Orders.Single(x => x.OrderNumberComplete == "20").IsManual);
        }

        [Theory]
        [InlineData(0, 3, null)]
        [InlineData(1, 2, 1)]
        [InlineData(3, null, 3)]
        public async Task SplitThenCombineOrder_WithOrderNumbers(decimal splitCount, double? expectedOriginalQuantity, double? expectedSplitQuantity)
        {
            var itemQuantities = new Dictionary<long, decimal> { { orderA.OrderItems.Single().OrderItemID, splitCount } };

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, itemQuantities);
            var orderA_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_0, orderB);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_C, orderA_1);

            var dataA_C = shipmentData.Single(x => x.Shipment.OrderID == orderA_C.OrderID);
            Assert.Equal(new[] { "10", "20" }, dataA_C.Orders.Select(x => x.OrderNumberComplete));
            Assert.Equal(expectedOriginalQuantity, QuantityForItem(dataA_C, "100"));
            Assert.Equal(3, QuantityForItem(dataA_C, "200"));

            var dataA_1 = shipmentData.Single(x => x.Shipment.OrderID == orderA_1.OrderID);
            Assert.Equal(new[] { "10" }, dataA_1.Orders.Select(x => x.OrderNumberComplete));
            Assert.Equal(expectedSplitQuantity, QuantityForItem(dataA_1, "100"));
        }

        [Theory]
        [InlineData(0, 3, null)]
        [InlineData(1, 2, 1)]
        [InlineData(3, null, 3)]
        public async Task CombineSplitWithBSurviving_WithOrderNumbers(decimal splitCount, double? expectedOriginalQuantity, double? expectedSplitQuantity)
        {
            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);

            var itemQuantities = new Dictionary<long, decimal>
            {
                { orderB_1_C.OrderItems.OfType<BuyDotComOrderItemEntity>().Single(x => x.ReceiptItemID == "100").OrderItemID, splitCount },
                { orderB_1_C.OrderItems.OfType<BuyDotComOrderItemEntity>().Single(x => x.ReceiptItemID == "200").OrderItemID, splitCount }
            };
            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C, itemQuantities);

            // Get online identities
            var shipmentData = await GetShipmentData(orderB_0, orderB_1);

            var dataB_0 = shipmentData.Single(x => x.Shipment.OrderID == orderB_0.OrderID);
            Assert.Equal(new[] { "10", "20" }, dataB_0.Orders.Select(x => x.OrderNumberComplete).OrderBy(x => x));
            Assert.Equal(expectedOriginalQuantity, QuantityForItem(dataB_0, "100"));

            var dataB_1 = shipmentData.Single(x => x.Shipment.OrderID == orderB_1.OrderID);
            Assert.Equal(new[] { "10", "20" }, dataB_1.Orders.Select(x => x.OrderNumberComplete).OrderBy(x => x));
            Assert.Equal(expectedSplitQuantity, QuantityForItem(dataB_1, "100"));
        }

        [Fact]
        public async Task CombineSplitWithASurviving_WithOrderNumbers()
        {
            var orderA_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderA, orderB);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA_1_C);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_0, orderA_1);

            var dataA_0 = shipmentData.Single(x => x.Shipment.OrderID == orderA_0.OrderID);
            Assert.Equal(new[] { "10", "20" }, dataA_0.Orders.Select(x => x.OrderNumberComplete).OrderBy(x => x));

            var dataA_1 = shipmentData.Single(x => x.Shipment.OrderID == orderA_1.OrderID);
            Assert.Equal(new[] { "10", "20" }, dataA_1.Orders.Select(x => x.OrderNumberComplete).OrderBy(x => x));
        }

        [Fact]
        public async Task SplitCombine_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);
            var orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA_0, orderA_1);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_C);

            var dataA_C = shipmentData.Single(x => x.Shipment.OrderID == orderA_C.OrderID);
            Assert.Equal(new[] { "10" }, dataA_C.Orders.Select(x => x.OrderNumberComplete).OrderBy(x => x));
        }

        [Fact]
        public async Task SplitCombine_SplitSurvivingOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("A-1-C", orderA_0, orderA_1);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_1_C);

            var dataA_1_C = shipmentData.Single(x => x.Shipment.OrderID == orderA_1_C.OrderID);
            Assert.Equal(new[] { "10" }, dataA_1_C.Orders.Select(x => x.OrderNumberComplete).OrderBy(x => x));
        }

        [Fact]
        public async Task SplitACombineBCombineRemainingTwo_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_1, orderB);
            var orderA_1_C_1 = await combineSplitHelpers.PerformCombine("10A-1-C-1", orderA_1_C, orderA_0);

            // Get online identities
            var shipmentData = await GetShipmentData(orderA_1_C_1);

            var dataA_1_C_1 = shipmentData.Single(x => x.Shipment.OrderID == orderA_1_C_1.OrderID);
            Assert.Equal(new[] { "10", "20" }, dataA_1_C_1.Orders.Select(x => x.OrderNumberComplete).OrderBy(x => x));
        }

        [Fact]
        public async Task SplitCombineWithManualOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);
            var orderM_A_1 = await combineSplitHelpers.PerformCombine("40A-M-C", orderM, orderA_1);

            // Get online identities
            var shipmentData = await GetShipmentData(orderM_A_1);

            var dataM_A_1 = shipmentData.Single(x => x.Shipment.OrderID == orderM_A_1.OrderID);
            Assert.False(dataM_A_1.Orders.Single(x => x.OrderNumberComplete == "10").IsManual);
            Assert.True(dataM_A_1.Orders.Single(x => x.OrderNumberComplete == "40").IsManual);
        }

        private Task<IEnumerable<BuyDotComShipmentUpload>> GetShipmentData(params OrderEntity[] orders)
        {
            foreach (var order in orders)
            {
                Create.Shipment(order).Save();
            }

            var identityProvider = context.Mock.Container.Resolve<IBuyDotComDataAccess>();

            return identityProvider.GetShipmentDataByOrderAsync(orders.Select(x => x.OrderID));
        }

        private static double? QuantityForItem(BuyDotComShipmentUpload data, string receiptItemID) =>
            data.Orders.SelectMany(x => x.Items).Where(x => x.ReceiptItemID == receiptItemID).SingleOrDefault()?.Quantity;

        public void Dispose() => context.Dispose();
    }
}