using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Startup;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;
using ShipWorks.Stores.Tests.Integration.Helpers;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.BigCommerce
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombineSplit")]
    public class BigCommerceCombineAndSplitTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> combineInteraction;
        private Mock<IOrderSplitUserInteraction> splitInteraction;
        private Mock<IBigCommerceWebClient> webClient;
        private Mock<IAsyncMessageHelper> asyncMessageHelper;
        private readonly BigCommerceStoreEntity store;
        private OrderEntity orderA;
        private OrderEntity orderB;
        private OrderEntity orderD;
        private readonly Dictionary<long, OrderEntity> orders;
        private readonly IBigCommerceDataAccess dataAccess;
        private readonly CombineSplitHelpers combineSplitHelpers;
        private readonly IOrderSplitGateway orderSplitGateway;
        private readonly IBigCommerceItemLoader productLoader;
        private readonly (long, int, int)[] emptyResults = Enumerable.Empty<(long, int, int)>().ToArray();

        public BigCommerceCombineAndSplitTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                combineInteraction = mock.Override<IOrderCombinationUserInteraction>();
                splitInteraction = mock.Override<IOrderSplitUserInteraction>();
                webClient = mock.Override<IBigCommerceWebClient>();
                mock.Override<IMessageHelper>();
                asyncMessageHelper = mock.Override<IAsyncMessageHelper>();
            });

            orderSplitGateway = context.Mock.Container.Resolve<IOrderSplitGateway>();

            combineSplitHelpers = new CombineSplitHelpers(context, splitInteraction, combineInteraction);

            asyncMessageHelper.Setup(x => x.ShowProgressDialog(AnyString, AnyString))
                .ReturnsAsync(context.Mock.Build<ISingleItemProgressDialog>());

            dataAccess = context.Mock.Container.Resolve<IBigCommerceDataAccess>();
            productLoader = context.Mock.Container.Resolve<IBigCommerceItemLoader>();

            store = Create.Store<BigCommerceStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.BigCommerce)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            Create.Order(store, context.Customer).Save();

            orders = new Dictionary<long, OrderEntity> { { 1, orderA }, { 2, orderB }, { 3, orderD } };
        }

        private void CreateDefaultOrders()
        {
            orderA = CreateBigCommerceOrder(10, 10);
            orderB = CreateBigCommerceOrder(20, 20);
            orderD = CreateBigCommerceOrder(30, 30);
        }

        [Fact]
        public async Task Split_WithOrderNumbers()
        {
            CreateDefaultOrders();

            // BEFORE:
            // orderA:      [spn-10:10]
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            // BEFORE:
            // orderA_0:    [spn-10:8]
            // orderA_1:    [spn-10:2]
            var (orderA_2, orderA_3) = await combineSplitHelpers.PerformSplit(orderA_0, 2, 2);

            // BEFORE:
            // orderA_0:    [spn-10:6]
            // orderA_1:    [spn-10:2]
            // orderA_3:    [spn-10:2]
            var (orderA_4, orderA_5) = await combineSplitHelpers.PerformSplit(orderA_1, 2, 2);
            // AFTER:
            // orderA_0:    [spn-10:6]
            // orderA_1:    
            // orderA_2:    [spn-10:6]
            // orderA_4:    
            // orderA_3:    [spn-10:2]
            // orderA_5:    [spn-10:2]

            await ValidateResults(orderA_0, new[] { (10L, 10, 6) });
            await ValidateResults(orderA_1, emptyResults);
            await ValidateResults(orderA_2, new[] { (10L, 10, 6) });
            await ValidateResults(orderA_3, new[] { (10L, 10, 2) });
            await ValidateResults(orderA_4, emptyResults);
            await ValidateResults(orderA_5, new[] { (10L, 10, 2) });
        }

        [Fact]
        public async Task CombineSplitCombine_WithOrderNumbers()
        {
            CreateDefaultOrders();

            // BEFORE:
            // orderA:      [spn-10:10]
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]
            OrderEntity orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA, orderB);

            // Result:
            // orderA_C:    [spn-10:10], [spn-20:20]
            // orderD:      [spn-30:30]
            var (orderA_C_O, orderA_C_1) = await combineSplitHelpers.PerformSplit(orderA_C, 2, 2);

            // Result:
            // orderA_C/orderA_C_O:    [spn-10:8], [spn-20:18]
            // orderA_C_1:             [spn-10:2], [spn-20:2]
            // orderD:                 [spn-30:30]
            var orderD_C = await combineSplitHelpers.PerformCombine("D-C", orderD, orderA_C_O);

            // Result:
            // orderD_C/orderA_C/orderA_C_O:    [spn-10:8], [spn-20:18], [spn-30:30]
            // orderA_C_1:                      [spn-10:2], [spn-20:2]

            await ValidateResults(orderA_C_1, new[] { (10L, 10, 2), (10L, 20, 2) });

            await ValidateResults(orderD_C, new[] { (10L, 10, 8), (10L, 20, 18), (10L, 30, 30) });
        }

        [Fact]
        public async Task CombineTwoManualOrders_WithOrderNumbers()
        {
            // BEFORE:
            // orderD:      [spn-30:30]
            orderA = Create.CreateManualOrder(store, context.Customer, 100);

            // Result:
            // orderA:      [spn-100:10]
            // orderD:      [spn-30:30]
            orderB = Create.CreateManualOrder(store, context.Customer, 200);

            // Result:
            // orderA:      [spn-100:10]
            // orderB:      [spn-200:20]
            // orderD:      [spn-30:30]

            var orderA_M_C = await combineSplitHelpers.PerformCombine("100A-M-C", orderA, orderB);

            // Result:
            // orderA_M_C:  [spn-100:10], [spn-200:20]
            // orderD:      [spn-30:30]

            await ValidateResults(orderA_M_C, emptyResults);
        }

        [Fact]
        public async Task SplitManualCombineWithNotManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);
            orderB = CreateBigCommerceOrder(20, 20);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            var orderA_M_1_C = await combineSplitHelpers.PerformCombine("10A-M-1-C", orderA_1, orderB);

            await ValidateResults(orderA_M_1_C, new[] { (20L, 20, 20) });

            await ValidateResults(orderA_0, emptyResults);
        }

        [Fact]
        public async Task SplitManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            await ValidateResults(orderA_0, emptyResults);

            await ValidateResults(orderA_1, emptyResults);
        }

        [Fact]
        public async Task CombineMixManualSplit_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);
            orderB = CreateBigCommerceOrder(20, 20);

            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C, 2, 2);

            await ValidateResults(orderB_0, new[] { (20L, 20, 18) });

            await ValidateResults(orderB_1, new[] { (20L, 20, 2) });
        }

        [Fact]
        public async Task SplitThenCombineOrder_WithOrderNumbers()
        {
            CreateDefaultOrders();

            // BEFORE:
            // orderA:      [spn-10:10]
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            // AFTER:
            // orderA:      [spn-10:8]
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]
            // orderA_0:    [spn-10:8]
            // orderA_1:    [spn-10:2]

            var orderA_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_0, orderB);
            // AFTER:
            // orderD:      [spn-30:30]
            // orderA_1:    [spn-10:2]
            // orderA_C:    [spn-10:8], [spn-20:20]

            // Combined order
            await ValidateResults(orderA_C, new[] { (10L, 10, 8), (10L, 20, 20) });

            // Split order
            await ValidateResults(orderA_1, new[] { (10L, 10, 2) });
        }

        [Fact]
        public async Task CombineSplitWithBSurviving_WithOrderNumbers()
        {
            CreateDefaultOrders();

            // BEFORE:
            // orderA:      [spn-10:10]
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]

            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);
            // AFTER:
            // orderD:      [spn-30:30]
            // orderB_1_C:  [spn-20:20], [spn-10:10]

            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C, 2, 2);
            // AFTER:
            // orderD:      [spn-30:30]
            // orderB_1_C:  [spn-20:18], [spn-10:8]
            // orderB_0:    [spn-20:18], [spn-10:8]
            // orderB_1:    [spn-20:2],  [spn-10:2]

            await ValidateResults(orderB_0, new[] { (10L, 10, 8), (10L, 20, 18) });

            await ValidateResults(orderB_1, new[] { (10L, 10, 2), (10L, 20, 2) });
        }

        [Fact]
        public async Task CombineSplitWithASurviving_WithOrderNumbers()
        {
            CreateDefaultOrders();

            // BEFORE:
            // orderA:      [spn-10:10]
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]

            var orderA_1_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA, orderB);
            // AFTER:
            // orderD:      [spn-30:30]
            // orderA_1_C:  [spn-20:20], [spn-10:10]

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA_1_C, 2, 2);
            // AFTER:
            // orderD:      [spn-30:30]
            // orderA_1_C:  [spn-20:18], [spn-10:8]
            // orderA_0:    [spn-20:18], [spn-10:8]
            // orderA_1:    [spn-20:2],  [spn-10:2]

            await ValidateResults(orderA_0, new[] { (10L, 10, 8), (10L, 20, 18) });

            await ValidateResults(orderA_1, new[] { (10L, 10, 2), (10L, 20, 2) });
        }

        [Fact]
        public async Task SplitCombine_WithOrderNumbers()
        {
            CreateDefaultOrders();

            // BEFORE:
            // orderA:      [spn-10:10]
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            // AFTER:
            // orderA:      [spn-10:8]
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]
            // orderA_0:    [spn-10:8]
            // orderA_1:    [spn-10:2]

            var orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA_0, orderA_1);
            // AFTER:
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]
            // orderA_C:    [spn-10:8], [spn-10:2]

            await ValidateResults(orderA_C, new[] { (10L, 10, 8), (10L, 10, 2) });
        }

        [Fact]
        public async Task SplitCombine_SplitSurvivingOrder_WithOrderNumbers()
        {
            CreateDefaultOrders();

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("A-1-C", orderA_0, orderA_1);

            await ValidateResults(orderA_1_C, new[] { (10L, 10, 8), (10L, 10, 2) });
        }

        [Fact]
        public async Task SplitACombineBCombineRemainingTwo_WithOrderNumbers()
        {
            CreateDefaultOrders();

            // BEFORE:
            // orderA:      [spn-10:10]
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            // AFTER:
            // orderA:      [spn-10:8]
            // orderB:      [spn-20:20]
            // orderD:      [spn-30:30]
            // orderA_0:    [spn-10:8]
            // orderA_1:    [spn-10:2]

            var orderA_1_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_1, orderB);
            // AFTER:
            // orderD:      [spn-30:30]
            // orderA_0:    [spn-10:8]
            // orderA_1:    [spn-10:2]
            // orderA_1_C:  [spn-10:2], [spn-20:20]

            var orderA_1_C_1 = await combineSplitHelpers.PerformCombine("10A-1-C-1", orderA_1_C, orderA_0);
            // AFTER:
            // orderD:      [spn-30:30]
            // orderA_1_C_1:[spn-10:2], [spn-20:20], [spn-10:8]

            // Unchanged orderD
            await ValidateResults(orderD, new[] { (30L, 30, 30) });

            // Combined order
            await ValidateResults(orderA_1_C_1, new[] { (10L, 10, 8), (10L, 10, 2), (10L, 20, 20) });
        }

        [Fact]
        public async Task SplitCombineWithManualOrder_WithOrderNumbers()
        {
            orderA = CreateBigCommerceOrder(10, 10);
            orderB = Create.CreateManualOrder(store, context.Customer, 200);

            // BEFORE:
            // orderA:      [spn-10:10]
            // orderB:      [spn-200:10] (Manual)
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            // RESULT:
            // orderA_0/orderA: [spn-10:8]
            // orderA_1:        [spn-10:2]
            // orderB:          [spn-200:10] (Manual)

            var orderB_M_C = await combineSplitHelpers.PerformCombine("10A-M-C", orderB, orderA_1);
            // RESULT:
            // orderA_0/orderA: [spn-10:8]
            // orderB_M_C:      [spn-200:10] (Manual), [spn-10:2]

            // Split orders
            await ValidateResults(orderA_0, new[] { (10L, 10, 8) });

            // Combined order
            await ValidateResults(orderB_M_C, new[] { (10L, 10, 2) });
        }

        private async Task ValidateResults(OrderEntity order, ValueTuple<long, int, int>[] expectedItems)
        {
            var identities_A_0 = await dataAccess.GetOrderDetailsAsync(order.OrderID);

            IDictionary<long, IEnumerable<IBigCommerceOrderItemEntity>> allItems = await dataAccess.GetOrderItemsAsync(order.OrderID).ConfigureAwait(false);

            var orderItems = allItems.Values.SelectMany(x => x).Cast<IOrderItemEntity>();

            GenericResult<BigCommerceOnlineItems> updateDetailsResult = await productLoader
                .LoadItems(orderItems, order.OrderNumberComplete, order.OrderNumber, webClient.Object)
                .ConfigureAwait(false);

            if (expectedItems.None() && updateDetailsResult.Failure)
            {
                return;
            }

            Assert.Equal(allItems.Keys.OrderBy(k => k), identities_A_0.OrdersToUpload.Where(o => !o.IsManual).Select(o => o.OrderID).Distinct().OrderBy(k => k));
            Assert.True(updateDetailsResult.Success);

            var bigCommerceOnlineItems = updateDetailsResult.Value;

            IEnumerable<(long orderAddressID, int orderProductId, int quantity)> actualItems = bigCommerceOnlineItems
                .Items
                .Select(i => (bigCommerceOnlineItems.OrderAddressID, i.order_product_id, i.quantity));

            Assert.Equal(expectedItems, actualItems);
        }

        private OrderEntity CreateBigCommerceOrder(long orderNumber, double itemQuantity)
        {
            return Create.Order<OrderEntity>(store, context.Customer)
                .WithItem<BigCommerceOrderItemEntity>(i => i
                    .Set(n => n.OrderAddressID, orderNumber)
                    .Set(n => n.OrderProductID, orderNumber)
                    .Set(n => n.Quantity, itemQuantity))
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Save();
        }

        public void Dispose() => context.Dispose();
    }
}