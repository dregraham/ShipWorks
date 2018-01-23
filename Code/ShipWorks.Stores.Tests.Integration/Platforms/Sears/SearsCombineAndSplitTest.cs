using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Stores.Platforms.Sears;
using ShipWorks.Stores.Platforms.Sears.OnlineUpdating;
using ShipWorks.Stores.Tests.Integration.Helpers;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Sears
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombineSplit")]
    public class SearsCombineAndSplitTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> combineInteraction;
        private Mock<IOrderSplitUserInteraction> splitInteraction;
        private Mock<IAsyncMessageHelper> asyncMessageHelper;
        private Mock<ISearsWebClient> webClient;
        private readonly SearsStoreEntity store;
        private readonly SearsCombineOrderSearchProviderComparer comparer;
        private OrderEntity orderA;
        private OrderEntity orderB;
        private OrderEntity orderD;
        private readonly SearsOrderDetail expectedOrderSearchA;
        private readonly SearsOrderDetail expectedOrderSearchB;
        private readonly SearsOrderDetail expectedOrderSearchD;
        private readonly CombineSplitHelpers combineSplitHelpers;
        private readonly ISearsOnlineUpdater onlineUpdater;


        public SearsCombineAndSplitTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                combineInteraction = mock.Override<IOrderCombinationUserInteraction>();
                splitInteraction = mock.Override<IOrderSplitUserInteraction>();
                mock.Override<IMessageHelper>();
                asyncMessageHelper = mock.Override<IAsyncMessageHelper>();
                webClient = mock.Override<ISearsWebClient>();
            });

            onlineUpdater = context.Mock.Container.Resolve<ISearsOnlineUpdater>();

            combineSplitHelpers = new CombineSplitHelpers(context, splitInteraction, combineInteraction);

            asyncMessageHelper.Setup(x => x.ShowProgressDialog(AnyString, AnyString))
                .ReturnsAsync(context.Mock.Build<ISingleItemProgressDialog>());

            store = Create.Store<SearsStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.Sears)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching aL orders later
            Create.Order(store, context.Customer).Save();

            orderA = CreateSearsOrder(10L, "1000", "100");
            orderB = CreateSearsOrder(20L, "2000", "200");
            orderD = CreateSearsOrder(30L, "3000", "300");

            expectedOrderSearchA = CreateSearsOrderDetail(orderA);
            expectedOrderSearchB = CreateSearsOrderDetail(orderB);
            expectedOrderSearchD = CreateSearsOrderDetail(orderD);

            comparer = new SearsCombineOrderSearchProviderComparer();
        }



        [Fact]
        public async Task Split_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);
            var (orderA_2, orderA_3) = await combineSplitHelpers.PerformSplit(orderA_0);
            var (orderA_4, orderA_5) = await combineSplitHelpers.PerformSplit(orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();
            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);
            var identities_A_2 = await identityProvider.GetOrderIdentifiers(orderA_2);
            var identities_A_3 = await identityProvider.GetOrderIdentifiers(orderA_3);
            var identities_A_4 = await identityProvider.GetOrderIdentifiers(orderA_4);
            var identities_A_5 = await identityProvider.GetOrderIdentifiers(orderA_5);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_1, comparer);
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_2, comparer);
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_3, comparer);
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_4, comparer);
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_5, comparer);
        }

        [Fact]
        public async Task CombineSplitCombine_WithOrderNumbers()
        {
            OrderEntity orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA, orderB);
            var (orderA_C_O, orderA_C_1) = await combineSplitHelpers.PerformSplit(orderA_C);
            var orderD_C = await combineSplitHelpers.PerformCombine("D-C", orderD, orderA_C_O);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();
            var identities_A_C_1 = await identityProvider.GetOrderIdentifiers(orderA_C_1);
            var identities_D_C = await identityProvider.GetOrderIdentifiers(orderD_C);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_C_1, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB, expectedOrderSearchD }, identities_D_C, comparer);
        }

        [Fact]
        public async Task CombineTwoManualOrders_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);
            orderB = Create.CreateManualOrder(store, context.Customer, 20);

            var orderA_M_C = await combineSplitHelpers.PerformCombine("10A-M-C", orderA, orderB);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();

            var identities_A_M_C = await identityProvider.GetOrderIdentifiers(orderA_M_C);

            Assert.Equal(0, identities_A_M_C.Count());
        }

        [Fact]
        public async Task SplitManualCombineWithNotManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);

            var orderA_M_1_C = await combineSplitHelpers.PerformCombine("10A-M-1-C", orderA_1, orderB);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();

            var identities_A_M_1_C = await identityProvider.GetOrderIdentifiers(orderA_M_1_C);
            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);

            Assert.Equal(new[] { expectedOrderSearchB }, identities_A_M_1_C, comparer);
            Assert.Equal(0, identities_A_0.Count());
        }

        [Fact]
        public async Task SplitManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();

            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(0, identities_A_0.Count());
            Assert.Equal(0, identities_A_1.Count());
        }

        [Fact]
        public async Task CombineMixManualSplit_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            Assert.Equal(new[] { expectedOrderSearchB }, identities_B_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchB }, identities_B_1, comparer);
        }

        [Fact]
        public async Task SplitThenCombineOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, new Dictionary<long, decimal>{{ orderA.OrderItems.First().OrderItemID, 2 }});

            var orderA_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_0, orderB);

            var shipmentA_C = Create.Shipment(orderA_C).Set(x => x.TrackingNumber, "track-123").Save();

            await onlineUpdater.UploadShipmentDetails(store, shipmentA_C).ConfigureAwait(false);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "1000"),
                It.Is<IEnumerable<SearsTracking>>(t =>
                    HasTracking(t, "1000", "track-123", "100"))));

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "2000"),
                It.Is<IEnumerable<SearsTracking>>(t =>
                    HasTracking(t, "2000", "track-123", "200"))));

            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<ISearsStoreEntity>(), It.IsAny<SearsOrderDetail>(),
                It.IsAny<IEnumerable<SearsTracking>>()), Times.Exactly(2));

            // Clearing out assertions from orderA_C
            webClient.ResetCalls();

            // Testing orderA_1
            var shipmentA_1 = Create.Shipment(orderA_1).Set(x => x.TrackingNumber, "track-123").Save();

            await onlineUpdater.UploadShipmentDetails(store, shipmentA_1).ConfigureAwait(false);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "1000"),
                It.Is<IEnumerable<SearsTracking>>(t =>
                    HasTracking(t, "1000", "track-123", "100"))));
            
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<ISearsStoreEntity>(), It.IsAny<SearsOrderDetail>(),
                It.IsAny<IEnumerable<SearsTracking>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task CombineSplitWithBSurviving_WithOrderNumbers()
        {
            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_B_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_B_1, comparer);
        }

        [Fact]
        public async Task CombineSplitWithASurviving_WithOrderNumbers()
        {
            var orderA_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderA, orderB);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA_1_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();

            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_1, comparer);
        }

        [Fact]
        public async Task SplitCombine_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);
            var orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA_0, orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();
            var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_C, comparer);
        }

        [Fact]
        public async Task SplitCombine_SplitSurvivingOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("A-1-C", orderA_0, orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();
            var identities_A_1_C = await identityProvider.GetOrderIdentifiers(orderA_1_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_1_C, comparer);
        }

        [Fact]
        public async Task SplitACombineBCombineRemainingTwo_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_1, orderB);
            var orderA_1_C_1 = await combineSplitHelpers.PerformCombine("10A-1-C-1", orderA_1_C, orderA_0);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();
            var identities_A_1_C_1 = await identityProvider.GetOrderIdentifiers(orderA_1_C_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_1_C_1, comparer);
        }

        [Fact]
        public async Task SplitCombineWithManualOrder_WithOrderNumbers()
        {
            orderB = Create.CreateManualOrder(store, context.Customer, 20);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA);

            var orderB_M_C = await combineSplitHelpers.PerformCombine("10A-M-C", orderB, orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();

            var identities_B_M_C = await identityProvider.GetOrderIdentifiers(orderB_M_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_B_M_C, comparer);
        }

        private SearsOrderEntity CreateSearsOrder(long orderNumber, string poNumber, string itemNumber)
        {
            return Create.Order<SearsOrderEntity>(store, context.Customer)
                .WithItem<SearsOrderItemEntity>(i => i.Set(x => x.ItemID, itemNumber).Set(x => x.Quantity, 3))
                .Set(x => x.PoNumber, poNumber)
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Save();
        }

        private SearsOrderDetail CreateSearsOrderDetail(OrderEntity order)
        {
            return new SearsOrderDetail(order.OrderID, (order as SearsOrderEntity).PoNumber, order.OrderDate);
        }

        /// <summary>
        /// Check for a tracking entry
        /// </summary>
        private bool HasTracking(IEnumerable<SearsTracking> trackingEntry, string PoNumber, string trackingNumber,
            string itemID) =>
            trackingEntry.Any(z =>
                z.TrackingNumber == trackingNumber &&
                z.ItemID == itemID &&
                z.PoNumber == PoNumber);

        //private Task<IEnumerable<ShipmentEntity>> GetShipmentData(params OrderEntity[] orders)
        //{
        //    foreach (var order in orders)
        //    {
        //        Create.Shipment(order).Save();
        //    }

        //    var identityProvider = context.Mock.Container.Resolve<SearsCombineOrderSearchProvider>();

        //    return identityProvider.GetOrderIdentifiers(orders.Select(x => x.OrderID));
        //}

        public void Dispose() => context.Dispose();
    }
}