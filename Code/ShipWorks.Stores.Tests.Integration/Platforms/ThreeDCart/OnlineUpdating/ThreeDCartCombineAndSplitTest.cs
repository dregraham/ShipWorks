﻿using System;
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
using ShipWorks.Stores.Orders.Combine;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.ThreeDCart.OnlineUpdating
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombineSplit")]
    public class ThreeDCartCombineAndSplitTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> combineInteraction;
        private Mock<IOrderSplitUserInteraction> splitInteraction;
        private Mock<IAsyncMessageHelper> asyncMessageHelper;
        private readonly ThreeDCartStoreEntity store;
        private readonly OrderEntity orderA;
        private readonly OrderEntity orderB;
        private readonly OrderEntity orderD;
        private readonly Dictionary<long, OrderEntity> orders;

        public ThreeDCartCombineAndSplitTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                combineInteraction = mock.Override<IOrderCombinationUserInteraction>();
                splitInteraction = mock.Override<IOrderSplitUserInteraction>();
                mock.Override<IMessageHelper>();
                asyncMessageHelper = mock.Override<IAsyncMessageHelper>();
            });

            asyncMessageHelper.Setup(x => x.ShowProgressDialog(AnyString, AnyString))
                .ReturnsAsync(context.Mock.Create<ISingleItemProgressDialog>());

            store = Create.Store<ThreeDCartStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.ThreeDCart)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            Create.Order(store, context.Customer).Save();

            orderA = Create.Order<ThreeDCartOrderEntity>(store, context.Customer)
                .WithItem<ThreeDCartOrderItemEntity>(i => i.Set(x => x.ThreeDCartShipmentID, 1000L))
                .Set(x => x.ThreeDCartOrderID, 1000)
                .Set(x => x.OrderNumber, 10)
                .Set(x => x.OrderNumberComplete, "10")
                .Save();
            orderB = Create.Order<ThreeDCartOrderEntity>(store, context.Customer)
                .WithItem<ThreeDCartOrderItemEntity>(i => i.Set(x => x.ThreeDCartShipmentID, 2000L))
                .Set(x => x.ThreeDCartOrderID, 2000)
                .Set(x => x.OrderNumber, 20)
                .Set(x => x.OrderNumberComplete, "20")
                .Save();
            orderD = Create.Order<ThreeDCartOrderEntity>(store, context.Customer)
                .WithItem<ThreeDCartOrderItemEntity>(i => i.Set(x => x.ThreeDCartShipmentID, 3000L))
                .Set(x => x.ThreeDCartOrderID, 3000)
                .Set(x => x.OrderNumber, 30)
                .Set(x => x.OrderNumberComplete, "30")
                .Save();

            orders = new Dictionary<long, OrderEntity> { { 1, orderA }, { 2, orderB }, { 3, orderD } };
        }

        [Fact]
        public async Task Split_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await PerformSplit(orderA);
            var (orderA_2, orderA_3) = await PerformSplit(orderA_0);
            var (orderA_4, orderA_5) = await PerformSplit(orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();
            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);
            var identities_A_2 = await identityProvider.GetOrderIdentifiers(orderA_2);
            var identities_A_3 = await identityProvider.GetOrderIdentifiers(orderA_3);
            var identities_A_4 = await identityProvider.GetOrderIdentifiers(orderA_4);
            var identities_A_5 = await identityProvider.GetOrderIdentifiers(orderA_5);

            Assert.True(identities_A_0.Select(i => IsMatchingShipmentUpload(i, 10, 1000, orderA_0.IsManual)).All(r => r));
            Assert.True(identities_A_1.Select(i => IsMatchingShipmentUpload(i, 10, 1000, orderA_1.IsManual)).All(r => r));
            Assert.True(identities_A_2.Select(i => IsMatchingShipmentUpload(i, 10, 1000, orderA_2.IsManual)).All(r => r));
            Assert.True(identities_A_3.Select(i => IsMatchingShipmentUpload(i, 10, 1000, orderA_3.IsManual)).All(r => r));
            Assert.True(identities_A_4.Select(i => IsMatchingShipmentUpload(i, 10, 1000, orderA_4.IsManual)).All(r => r));
            Assert.True(identities_A_5.Select(i => IsMatchingShipmentUpload(i, 10, 1000, orderA_5.IsManual)).All(r => r));
        }

        [Fact]
        public async Task CombineSplitCombine_WithOrderNumbers()
        {
            OrderEntity orderA_C = await PerformCombine("A-C", orderA, orderB);
            var (orderA_C_O, orderA_C_1) = await PerformSplit(orderA_C);
            var orderD_C = await PerformCombine("D-C", orderD, orderA_C_O);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();
            var identities_A_C_1 = await identityProvider.GetOrderIdentifiers(orderA_C_1);
            var identities_D_C = await identityProvider.GetOrderIdentifiers(orderD_C);

            Assert.True(identities_A_C_1.Any(i => IsMatchingShipmentUpload(i, 10, 1000, orderA_C_1.IsManual)));
            Assert.True(identities_A_C_1.Any(i => IsMatchingShipmentUpload(i, 20, 2000, orderA_C_1.IsManual)));

            Assert.True(identities_D_C.Any(i => IsMatchingShipmentUpload(i, 10, 1000, orderD_C.IsManual)));
            Assert.True(identities_D_C.Any(i => IsMatchingShipmentUpload(i, 20, 2000, orderD_C.IsManual)));
            Assert.True(identities_D_C.Any(i => IsMatchingShipmentUpload(i, 30, 3000, orderD_C.IsManual)));
        }

        [Fact]
        public async Task CombineTwoManualOrders_WithOrderNumbers()
        {
            orderA.IsManual = true;
            Modify.Order(orderA).Save();
            orderB.IsManual = true;
            Modify.Order(orderB).Save();

            var orderA_M_C = await PerformCombine("10A-M-C", orderA, orderB);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();

            var identities_A_M_C = await identityProvider.GetOrderIdentifiers(orderA_M_C);

            Assert.Equal(0, identities_A_M_C.Count());
        }

        [Fact]
        public async Task SplitManualCombineWithNotManualOrder_WithOrderNumbers()
        {
            orderA.IsManual = true;
            Modify.Order(orderA).Save();

            var (orderA_0, orderA_1) = await PerformSplit(orderA);

            var orderA_M_1_C = await PerformCombine("10A-M-1-C", orderA_1, orderB);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();

            var identities_A_M_1_C = await identityProvider.GetOrderIdentifiers(orderA_M_1_C);
            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);

            Assert.True(identities_A_M_1_C.Any(i => IsMatchingShipmentUpload(i, 20, 2000, orderA_M_1_C.IsManual)));
            Assert.Equal(0, identities_A_0.Count());
        }

        [Fact]
        public async Task SplitManualOrder_WithOrderNumbers()
        {
            orderA.IsManual = true;
            Modify.Order(orderA).Save();

            var (orderA_0, orderA_1) = await PerformSplit(orderA);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();

            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(0, identities_A_0.Count());
            Assert.Equal(0, identities_A_1.Count());
        }

        [Fact]
        public async Task CombineMixManualSplit_WithOrderNumbers()
        {
            orderA.IsManual = true;
            Modify.Order(orderA).Save();

            var orderB_1_C = await PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await PerformSplit(orderB_1_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            Assert.True(identities_B_0.Any(i => IsMatchingShipmentUpload(i, 20, 2000, orderB_0.IsManual)));
            Assert.True(identities_B_1.Any(i => IsMatchingShipmentUpload(i, 20, 2000, orderB_0.IsManual)));

            //Assert.Equal(new[] { "20" }, identities_B_0);
            //Assert.Equal(new[] { "20" }, identities_B_1);
        }

        //[Fact]
        //public async Task SplitThenCombineOrder_WithOrderNumbers()
        //{
        //    var (orderA_0, orderA_1) = await PerformSplit(orderA);

        //    var orderA_C = await PerformCombine("10A-1-C", orderA_0, orderB);

        //    // Get online identities
        //    var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();

        //    var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);
        //    var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

        //    Assert.Equal(new[] { "10", "20" }, identities_A_C);
        //    Assert.Equal(new[] { "10" }, identities_A_1);
        //}

        //[Fact]
        //public async Task CombineSplitWithBSurviving_WithOrderNumbers()
        //{
        //    var orderB_1_C = await PerformCombine("10B-1-C", orderB, orderA);

        //    var (orderB_0, orderB_1) = await PerformSplit(orderB_1_C);

        //    // Get online identities
        //    var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();

        //    var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
        //    var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

        //    Assert.Equal(new[] { "10", "20" }, identities_B_0);
        //    Assert.Equal(new[] { "10", "20" }, identities_B_1);
        //}

        //[Fact]
        //public async Task CombineSplitWithASurviving_WithOrderNumbers()
        //{
        //    var orderA_1_C = await PerformCombine("10B-1-C", orderA, orderB);

        //    var (orderA_0, orderA_1) = await PerformSplit(orderA_1_C);

        //    // Get online identities
        //    var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();

        //    var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
        //    var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

        //    Assert.Equal(new[] { "10", "20" }, identities_A_0);
        //    Assert.Equal(new[] { "10", "20" }, identities_A_1);
        //}

        //[Fact]
        //public async Task SplitCombine_WithOrderNumbers()
        //{
        //    var (orderA_0, orderA_1) = await PerformSplit(orderA);
        //    var orderA_C = await PerformCombine("A-C", orderA_0, orderA_1);

        //    // Get online identities
        //    var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();
        //    var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);

        //    Assert.Equal(new[] { "10" }, identities_A_C);
        //}

        //[Fact]
        //public async Task SplitCombine_SplitSurvivingOrder_WithOrderNumbers()
        //{
        //    var (orderA_0, orderA_1) = await PerformSplit(orderA);
        //    var orderA_1_C = await PerformCombine("A-1-C", orderA_0, orderA_1);

        //    // Get online identities
        //    var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();
        //    var identities_A_1_C = await identityProvider.GetOrderIdentifiers(orderA_1_C);

        //    Assert.Equal(new[] { "10" }, identities_A_1_C);
        //}

        //[Fact]
        //public async Task SplitACombineBCombineRemainingTwo_WithOrderNumbers()
        //{
        //    var (orderA_0, orderA_1) = await PerformSplit(orderA);
        //    var orderA_1_C = await PerformCombine("10A-1-C", orderA_1, orderB);
        //    var orderA_1_C_1 = await PerformCombine("10A-1-C-1", orderA_1_C, orderA_0);

        //    // Get online identities
        //    var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();
        //    var identities_A_1_C_1 = await identityProvider.GetOrderIdentifiers(orderA_1_C_1);

        //    Assert.Equal(new[] { "10", "20" }, identities_A_1_C_1);
        //}

        //[Fact]
        //public async Task SplitCombineWithManualOrder_WithOrderNumbers()
        //{
        //    orderB.IsManual = true;
        //    Modify.Order(orderB).Save();

        //    var (orderA_0, orderA_1) = await PerformSplit(orderA);

        //    var orderB_M_C = await PerformCombine("10A-M-C", orderB, orderA_1);

        //    // Get online identities
        //    var identityProvider = context.Mock.Container.Resolve<IThreeDCartCombineOrderSearchProvider>();

        //    var identities_B_M_C = await identityProvider.GetOrderIdentifiers(orderB_M_C);

        //    Assert.Equal(new[] { "10" }, identities_B_M_C);
        //}

        /// <summary>
        /// Perform a split of the given order
        /// </summary>
        private async Task<(ThreeDCartOrderEntity original, ThreeDCartOrderEntity split)> PerformSplit(OrderEntity order)
        {
            var dataProvider = context.Mock.Container.Resolve<IDataProvider>();
            var orchestrator = context.Mock.Container.Resolve<IOrderSplitOrchestrator>();

            splitInteraction.Setup(x => x.GetSplitDetailsFromUser(AnyOrder, AnyString))
                .ReturnsAsync(new OrderSplitDefinition(order, new Dictionary<long, decimal>(), new Dictionary<long, decimal>(), order.OrderNumberComplete + "-1"));

            var result = await orchestrator.Split(order.OrderID);
            var original = await dataProvider.GetEntityAsync<ThreeDCartOrderEntity>(result.First());
            var split = await dataProvider.GetEntityAsync<ThreeDCartOrderEntity>(result.Last());

            return (original, split);
        }

        /// <summary>
        /// Combine the given orders, using the first order in the params as the surviving order
        /// </summary>
        private async Task<OrderEntity> PerformCombine(string orderNumber, params OrderEntity[] ordersToCombine)
        {
            var dataProvider = context.Mock.Container.Resolve<IDataProvider>();
            var combineOrchestrator = context.Mock.Container.Resolve<ICombineOrderOrchestrator>();

            combineInteraction.Setup(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()))
                .Returns(Tuple.Create(ordersToCombine.First().OrderID, orderNumber));

            var result = await combineOrchestrator.Combine(ordersToCombine.Select(x => x.OrderID));
            return await dataProvider.GetEntityAsync<OrderEntity>(result.Value);
        }
        
        private bool IsMatchingShipmentUpload(ThreeDCartOnlineUpdatingOrderDetail orderDetail, long orderNumber, long threeDCartOrderID, bool isManual)
        {
            return orderDetail.OrderNumber == orderNumber && orderDetail.ThreeDCartOrderID == threeDCartOrderID && orderDetail.IsManual == isManual;
        }

        public void Dispose() => context.Dispose();
    }
}