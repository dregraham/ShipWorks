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
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Magento
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombineSplit")]
    public class MagentoCombineAndSplitTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> combineInteraction;
        private Mock<IOrderSplitUserInteraction> splitInteraction;
        private Mock<IAsyncMessageHelper> asyncMessageHelper;
        private readonly MagentoStoreEntity store;
        private readonly MagentoOrderEntity orderA;
        private readonly MagentoOrderEntity orderB;
        private readonly MagentoOrderEntity orderD;
        private readonly MagentoOrderSearchEntity expectedOrderSearchA;
        private readonly MagentoOrderSearchEntity expectedOrderSearchB;
        private readonly MagentoOrderSearchEntity expectedOrderSearchD;
        private readonly MagentoCombineOrderSearchProviderComparer comparer;

        public MagentoCombineAndSplitTest(DatabaseFixture db, ITestOutputHelper output)
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

            store = Create.Store<MagentoStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.Magento)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching aL orders later
            Create.Order(store, context.Customer).Save();

            orderA = CreateMagentoOrder(10L, 1000L, "1000L", 1000);
            orderB = CreateMagentoOrder(20L, 2000L, "2000L", 2000);
            orderD = CreateMagentoOrder(30L, 3000L, "3000L", 3000);

            expectedOrderSearchA = CreateMagentoOrderSearch(orderA);
            expectedOrderSearchB = CreateMagentoOrderSearch(orderB);
            expectedOrderSearchD = CreateMagentoOrderSearch(orderD);

            comparer = new MagentoCombineOrderSearchProviderComparer();
        }

        [Fact]
        public async Task Split_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await PerformSplit(orderA);
            var (orderA_2, orderA_3) = await PerformSplit(orderA_0);
            var (orderA_4, orderA_5) = await PerformSplit(orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();
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
            OrderEntity orderA_C = await PerformCombine("A-C", orderA, orderB);
            var (orderA_C_O, orderA_C_1) = await PerformSplit(orderA_C);
            var orderD_C = await PerformCombine("D-C", orderD, orderA_C_O);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();
            var identities_A_C_1 = await identityProvider.GetOrderIdentifiers(orderA_C_1);
            var identities_D_C = await identityProvider.GetOrderIdentifiers(orderD_C);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_C_1, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB, expectedOrderSearchD }, identities_D_C, comparer);
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
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

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
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_A_M_1_C = await identityProvider.GetOrderIdentifiers(orderA_M_1_C);
            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);

            Assert.Equal(new[] { 2000L }, identities_A_M_1_C.Select(i => i.MagentoOrderID));

            Assert.Equal(new[] { expectedOrderSearchB }, identities_A_M_1_C, comparer);
            Assert.Equal(0, identities_A_0.Count());
        }

        [Fact]
        public async Task SplitManualOrder_WithOrderNumbers()
        {
            orderA.IsManual = true;
            Modify.Order(orderA).Save();

            var (orderA_0, orderA_1) = await PerformSplit(orderA);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

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
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            Assert.Equal(new[] { expectedOrderSearchB }, identities_B_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchB }, identities_B_1, comparer);
        }

        [Fact]
        public async Task SplitThenCombineOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await PerformSplit(orderA);

            var orderA_C = await PerformCombine("10A-1-C", orderA_0, orderB);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_C, comparer);
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_1, comparer);
        }

        [Fact]
        public async Task CombineSplitWithBSurviving_WithOrderNumbers()
        {
            var orderB_1_C = await PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await PerformSplit(orderB_1_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_B_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_B_1, comparer);
        }

        [Fact]
        public async Task CombineSplitWithASurviving_WithOrderNumbers()
        {
            var orderA_1_C = await PerformCombine("10B-1-C", orderA, orderB);

            var (orderA_0, orderA_1) = await PerformSplit(orderA_1_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_1, comparer);
        }

        [Fact]
        public async Task SplitCombine_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await PerformSplit(orderA);
            var orderA_C = await PerformCombine("A-C", orderA_0, orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();
            var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_C, comparer);
        }

        [Fact]
        public async Task SplitCombine_SplitSurvivingOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await PerformSplit(orderA);
            var orderA_1_C = await PerformCombine("A-1-C", orderA_0, orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();
            var identities_A_1_C = await identityProvider.GetOrderIdentifiers(orderA_1_C);
            
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_1_C, comparer);
        }

        [Fact]
        public async Task SplitACombineBCombineRemainingTwo_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await PerformSplit(orderA);
            var orderA_1_C = await PerformCombine("10A-1-C", orderA_1, orderB);
            var orderA_1_C_1 = await PerformCombine("10A-1-C-1", orderA_1_C, orderA_0);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();
            var identities_A_1_C_1 = await identityProvider.GetOrderIdentifiers(orderA_1_C_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_1_C_1, comparer);
        }

        [Fact]
        public async Task SplitCombineWithManualOrder_WithOrderNumbers()
        {
            orderB.IsManual = true;
            Modify.Order(orderB).Save();

            var (orderA_0, orderA_1) = await PerformSplit(orderA);

            var orderB_M_C = await PerformCombine("10A-M-C", orderB, orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_B_M_C = await identityProvider.GetOrderIdentifiers(orderB_M_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_B_M_C, comparer);
        }

        private MagentoOrderEntity CreateMagentoOrder(long orderNumber, long magentoOrderID, string magentoBuyerID, int sellerManagerRecord)
        {
            return Create.Order<MagentoOrderEntity>(store, context.Customer)
                .Set(x => x.MagentoOrderID, magentoOrderID)
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Save();
        }

        private MagentoOrderSearchEntity CreateMagentoOrderSearch(MagentoOrderEntity order)
        {
            return new MagentoOrderSearchEntity()
            {
                MagentoOrderID = order.MagentoOrderID,
            };
        }

        /// <summary>
        /// Perform a split of the given order
        /// </summary>
        private async Task<(MagentoOrderEntity original, MagentoOrderEntity split)> PerformSplit(OrderEntity order)
        {
            var dataProvider = context.Mock.Container.Resolve<IDataProvider>();
            var orchestrator = context.Mock.Container.Resolve<IOrderSplitOrchestrator>();

            splitInteraction.Setup(x => x.GetSplitDetailsFromUser(AnyOrder, AnyString))
                .ReturnsAsync(new OrderSplitDefinition(order, new Dictionary<long, decimal>(), new Dictionary<long, decimal>(), order.OrderNumberComplete + "-1"));

            var result = await orchestrator.Split(order.OrderID);
            var original = await dataProvider.GetEntityAsync<MagentoOrderEntity>(result.First());
            var split = await dataProvider.GetEntityAsync<MagentoOrderEntity>(result.Last());

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

        public void Dispose() => context.Dispose();
    }

    ///// <summary>
    ///// IEqualityComparer for MagentoOrderSearchEntity
    ///// </summary>
    //public class MagentoOrderSearchEntityComparer : IEqualityComparer<MagentoOrderSearchEntity>
    //{
    //    /// <summary>
    //    /// Compare 2 MagentoOrderSearchEntities
    //    /// </summary>
    //    public bool Equals(MagentoOrderSearchEntity x, MagentoOrderSearchEntity y)
    //    {
    //        return x.MagentoOrderID.Equals(y.MagentoOrderID);
    //    }

    //    /// <summary>
    //    /// Get hash code for an MagentoOrderSearchEntity
    //    /// </summary>
    //    public int GetHashCode(MagentoOrderSearchEntity MagentoOrderSearchEntity)
    //    {
    //        return MagentoOrderSearchEntity.MagentoOrderID.GetHashCode();
    //    }
    //}
}