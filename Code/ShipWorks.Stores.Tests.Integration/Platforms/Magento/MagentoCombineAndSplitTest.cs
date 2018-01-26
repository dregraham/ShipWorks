using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Stores.Platforms.Magento.OnlineUpdating;
using ShipWorks.Stores.Tests.Integration.Helpers;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Tests.Shared.ExtensionMethods;
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
        private OrderEntity orderA;
        private OrderEntity orderB;
        private OrderEntity orderD;
        private readonly MagentoOrderSearchEntity expectedOrderSearchA;
        private readonly MagentoOrderSearchEntity expectedOrderSearchB;
        private readonly MagentoOrderSearchEntity expectedOrderSearchD;
        private readonly MagentoCombineOrderSearchProviderComparer comparer;
        private readonly CombineSplitHelpers combineSplitHelpers;
        private Mock<IMagentoTwoRestClient> webClientRest;
        private Mock<IMagentoTwoWebClient> webClientModule;
        private readonly MagentoTwoRestOnlineUpdater restOnlineUpdater;

        public MagentoCombineAndSplitTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                combineInteraction = mock.Override<IOrderCombinationUserInteraction>();
                splitInteraction = mock.Override<IOrderSplitUserInteraction>();
                mock.Override<IMessageHelper>();
                asyncMessageHelper = mock.Override<IAsyncMessageHelper>();
                webClientRest = mock.Override<IMagentoTwoRestClient>();
                webClientModule = mock.Override<IMagentoTwoWebClient>();
            });

            restOnlineUpdater = context.Mock.Container.Resolve<MagentoTwoRestOnlineUpdater>();

            combineSplitHelpers = new CombineSplitHelpers(context, splitInteraction, combineInteraction);

            asyncMessageHelper.Setup(x => x.ShowProgressDialog(AnyString, AnyString))
                .ReturnsAsync(context.Mock.Build<ISingleItemProgressDialog>());

            store = Create.Store<MagentoStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.Magento)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching aL orders later
            Create.Order(store, context.Customer).Save();

            orderA = CreateMagentoOrder(10L, 1000L, 10, 100);
            orderB = CreateMagentoOrder(20L, 2000L, 20, 200);
            orderD = CreateMagentoOrder(30L, 3000L, 30, 300);

            expectedOrderSearchA = CreateMagentoOrderSearch(orderA);
            expectedOrderSearchB = CreateMagentoOrderSearch(orderB);
            expectedOrderSearchD = CreateMagentoOrderSearch(orderD);

            comparer = new MagentoCombineOrderSearchProviderComparer();
        }

        [Fact]
        public async Task Split_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var (orderA_2, orderA_3) = await combineSplitHelpers.PerformSplit(orderA_0, 2, 2);
            var (orderA_4, orderA_5) = await combineSplitHelpers.PerformSplit(orderA_1, 1, 1);

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

            CreateShipment(orderA_0);
            CreateShipment(orderA_1);
            CreateShipment(orderA_3);
            CreateShipment(orderA_5);

            await restOnlineUpdater.UploadShipmentDetails(orderA_0.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderA_1.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderA_2.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderA_3.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderA_4.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderA_5.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(96.0) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(96)),
                1000), Times.Exactly(2));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(1.0) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(1.0)),
                1000), Times.Exactly(2));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2.0) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2.0)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(1.0) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10-1-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(1.0)),
                1000), Times.Exactly(1));
        }

        [Fact]
        public async Task CombineSplitCombine_WithOrderNumbers()
        {
            OrderEntity orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA, orderB);
            var (orderA_C_O, orderA_C_1) = await combineSplitHelpers.PerformSplit(orderA_C, 2, 2);
            var orderD_C = await combineSplitHelpers.PerformCombine("D-C", orderD, orderA_C_O);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();
            var identities_A_C_1 = await identityProvider.GetOrderIdentifiers(orderA_C_1);
            var identities_D_C = await identityProvider.GetOrderIdentifiers(orderD_C);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_C_1, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB, expectedOrderSearchD }, identities_D_C, comparer);

            CreateShipment(orderA_C_1);
            CreateShipment(orderD_C);

            await restOnlineUpdater.UploadShipmentDetails(orderA_C_1.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderD_C.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-A-C-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2.0) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-A-C-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2)),
                2000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(98) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-D-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(98)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(198) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-D-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(198)),
                2000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 30 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(300) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-D-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 3000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(300)),
                3000), Times.Exactly(1));
        }

        [Fact]
        public async Task CombineTwoManualOrders_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);
            orderB = Create.CreateManualOrder(store, context.Customer, 20);

            var orderA_M_C = await combineSplitHelpers.PerformCombine("10A-M-C", orderA, orderB);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_A_M_C = await identityProvider.GetOrderIdentifiers(orderA_M_C);

            Assert.Equal(0, identities_A_M_C.Count());

            CreateShipment(orderA_M_C);

            await restOnlineUpdater.UploadShipmentDetails(orderA_M_C.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Never());
        }

        [Fact]
        public async Task SplitManualCombineWithNotManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            var orderA_M_1_C = await combineSplitHelpers.PerformCombine("10A-M-1-C", orderA_1, orderB);

            CreateShipment(orderA_M_1_C);
            CreateShipment(orderA_0);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_A_M_1_C = await identityProvider.GetOrderIdentifiers(orderA_M_1_C);
            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);

            Assert.Equal(new[] { 2000L }, identities_A_M_1_C.Select(i => i.MagentoOrderID));
            Assert.Equal(new[] { expectedOrderSearchB }, identities_A_M_1_C, comparer);
            Assert.Equal(0, identities_A_0.Count());

            await restOnlineUpdater.UploadShipmentDetails(orderA_M_1_C.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderA_0.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Once);
            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, 1000), Times.Never());

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(200) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10A-M-1-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(200)),
                2000), Times.Exactly(1));
        }

        [Fact]
        public async Task SplitManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            CreateShipment(orderA_0);
            CreateShipment(orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(0, identities_A_0.Count());
            Assert.Equal(0, identities_A_1.Count());

            await restOnlineUpdater.UploadShipmentDetails(orderA_0.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderA_1.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Never());
        }

        [Fact]
        public async Task CombineMixManualSplit_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C, 2, 2);

            CreateShipment(orderB_0);
            CreateShipment(orderB_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            Assert.Equal(new[] { expectedOrderSearchB }, identities_B_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchB }, identities_B_1, comparer);

            await restOnlineUpdater.UploadShipmentDetails(orderB_0.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderB_1.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Exactly(2));
            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, 1000), Times.Never());

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(198) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(198)),
                2000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2)),
                2000), Times.Exactly(1));
        }

        [Fact]
        public async Task SplitThenCombineOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            var orderA_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_0, orderB);

            CreateShipment(orderA_C);
            CreateShipment(orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_C, comparer);
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_1, comparer);

            await restOnlineUpdater.UploadShipmentDetails(orderA_C.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderA_1.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Exactly(3));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(98) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10A-1-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(98)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(200) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10A-1-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(200)),
                2000), Times.Exactly(1));
        }

        [Fact]
        public async Task CombineSplitWithBSurviving_WithOrderNumbers()
        {
            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C, 2, 2);

            CreateShipment(orderB_0);
            CreateShipment(orderB_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_B_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_B_1, comparer);

            await restOnlineUpdater.UploadShipmentDetails(orderB_0.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderB_1.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Exactly(4));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(98) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(98)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(198) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(198)),
                2000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2)),
                2000), Times.Exactly(1));
        }

        [Fact]
        public async Task CombineSplitWithASurviving_WithOrderNumbers()
        {
            var orderA_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderA, orderB);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA_1_C, 2, 2);

            CreateShipment(orderA_0);
            CreateShipment(orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_1, comparer);

            await restOnlineUpdater.UploadShipmentDetails(orderA_0.OrderID, MagentoUploadCommand.Complete, "", false);
            await restOnlineUpdater.UploadShipmentDetails(orderA_1.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Exactly(4));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(98) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(98)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(198) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(198)),
                2000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10B-1-C-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2)),
                2000), Times.Exactly(1));
        }

        [Fact]
        public async Task SplitCombine_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA_0, orderA_1);

            CreateShipment(orderA_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();
            var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_C, comparer);

            await restOnlineUpdater.UploadShipmentDetails(orderA_C.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(98) &&
                    s["items"][1]["orderItemId"].Value<long>() == 10 &&
                    s["items"][1]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-A-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(100)),
                1000), Times.Exactly(1));
        }

        [Fact]
        public async Task SplitCombine_SplitSurvivingOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("A-1-C", orderA_0, orderA_1);

            CreateShipment(orderA_1_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();
            var identities_A_1_C = await identityProvider.GetOrderIdentifiers(orderA_1_C);
            
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_1_C, comparer);

            await restOnlineUpdater.UploadShipmentDetails(orderA_1_C.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(98) &&
                    s["items"][1]["orderItemId"].Value<long>() == 10 &&
                    s["items"][1]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-A-1-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(100)),
                1000), Times.Exactly(1));
        }

        [Fact]
        public async Task SplitACombineBCombineRemainingTwo_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_1, orderB);
            var orderA_1_C_1 = await combineSplitHelpers.PerformCombine("10A-1-C-1", orderA_1_C, orderA_0);

            CreateShipment(orderA_1_C_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();
            var identities_A_1_C_1 = await identityProvider.GetOrderIdentifiers(orderA_1_C_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_1_C_1, comparer);

            await restOnlineUpdater.UploadShipmentDetails(orderA_1_C_1.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Exactly(2));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(98) &&
                    s["items"][1]["orderItemId"].Value<long>() == 10 &&
                    s["items"][1]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10A-1-C-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(100)),
                1000), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 20 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(200) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10A-1-C-1" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 2000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(200)),
                2000), Times.Exactly(1));
        }

        [Fact]
        public async Task SplitCombineWithManualOrder_WithOrderNumbers()
        {
            orderB = Create.CreateManualOrder(store, context.Customer, 20);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            var orderB_M_C = await combineSplitHelpers.PerformCombine("10A-M-C", orderB, orderA_1);

            CreateShipment(orderB_M_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<MagentoCombineOrderSearchProvider>();

            var identities_B_M_C = await identityProvider.GetOrderIdentifiers(orderB_M_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_B_M_C, comparer);

            await restOnlineUpdater.UploadShipmentDetails(orderB_M_C.OrderID, MagentoUploadCommand.Complete, "", false);

            webClientRest.Verify(x => x.UploadShipmentDetails(AnyString, AnyString, AnyLong), Times.Exactly(1));

            webClientRest.Verify(x => x.UploadShipmentDetails(
                ItIs.Json(s =>
                    s["items"][0]["orderItemId"].Value<long>() == 10 &&
                    s["items"][0]["qty"].Value<double>().IsEquivalentTo(2) &&
                    s["tracks"][0]["trackNumber"].Value<string>() == "tracking-10A-M-C" &&
                    s["tracks"][0]["title"].Value<string>() == "Ground" &&
                    s["tracks"][0]["carrierCode"].Value<string>() == "other"),
                ItIs.Json(s =>
                    s["entity"]["orderId"].Value<long>() == 1000 &&
                    s["entity"]["totalQty"].Value<double>().IsEquivalentTo(2)),
                1000), Times.Exactly(1));
        }

        private MagentoOrderEntity CreateMagentoOrder(long orderNumber, long magentoOrderID, int magentoItemID, double quantity)
        {
            SetupWebClientOrderRequest(magentoOrderID, magentoItemID, quantity);

            return Create.Order<MagentoOrderEntity>(store, context.Customer)
                .WithItem<OrderItemEntity>(i => i
                    .Set(n => n.Quantity, quantity)
                    .Set(n => n.Code, magentoItemID.ToString()))
                .Set(x => x.MagentoOrderID, magentoOrderID)
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Save();
        }

        private void SetupWebClientOrderRequest(long magentoOrderID, int magentoItemID, double quantity)
        {
            Item item = new Item
            {
                ItemId = magentoItemID,
                QtyOrdered = quantity
            };

            var webOrder = new Order { Items = new [] { item } };

            webClientRest.Setup(x => x.GetOrder(magentoOrderID))
                .Returns(webOrder);
        }

        private MagentoOrderSearchEntity CreateMagentoOrderSearch(OrderEntity order)
        {
            return new MagentoOrderSearchEntity()
            {
                MagentoOrderID = (order as MagentoOrderEntity).MagentoOrderID,
            };
        }

        private void CreateShipment(OrderEntity order)
        {
            Create.Shipment(order)
                .AsOther(o =>
                {
                    o.Set(x => x.Carrier, "UPS")
                        .Set(x => x.Service, "Ground");
                })
                .Set(x => x.TrackingNumber, $"tracking-{order.OrderNumberComplete}")
                .Set(x => x.Processed, true)
                .Save();
        }

        public void Dispose() => context.Dispose();
    }
}