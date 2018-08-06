using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.OnlineUpdating;
using ShipWorks.Stores.Tests.Integration.Helpers;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Ebay
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombineSplit")]
    public class EbayCombineAndSplitTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> combineInteraction;
        private Mock<IOrderSplitUserInteraction> splitInteraction;
        private Mock<IAsyncMessageHelper> asyncMessageHelper;
        private readonly EbayStoreEntity store;
        private readonly EbayOrderSearchEntityComparer comparer;
        private OrderEntity orderA;
        private OrderEntity orderB;
        private readonly OrderEntity orderD;
        private readonly EbayOrderSearchEntity expectedOrderSearchA;
        private readonly EbayOrderSearchEntity expectedOrderSearchB;
        private readonly EbayOrderSearchEntity expectedOrderSearchD;
        private readonly CombineSplitHelpers combineSplitHelpers;

        private Mock<IEbayWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly EbayOnlineUpdateCommandCreator commandCreator;

        public EbayCombineAndSplitTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                combineInteraction = mock.Override<IOrderCombinationUserInteraction>();
                splitInteraction = mock.Override<IOrderSplitUserInteraction>();
                mock.Override<IMessageHelper>();
                asyncMessageHelper = mock.Override<IAsyncMessageHelper>();
                webClient = mock.Override<IEbayWebClient>();
            });

            combineSplitHelpers = new CombineSplitHelpers(context, splitInteraction, combineInteraction);

            asyncMessageHelper.Setup(x => x.ShowProgressDialog(AnyString, AnyString))
                .ReturnsAsync(context.Mock.Build<ISingleItemProgressDialog>());

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Ebay) as EbayOnlineUpdateCommandCreator;

            store = Create.Store<EbayStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.Ebay)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching aL orders later
            Create.Order(store, context.Customer).Save();

            orderA = CreateEbayOrder(10L, 1000L, "1000L", 1000, 10);
            orderB = CreateEbayOrder(20L, 2000L, "2000L", 2000, 20);
            orderD = CreateEbayOrder(30L, 3000L, "3000L", 3000, 30);

            expectedOrderSearchA = CreateEbayOrderSearch(orderA);
            expectedOrderSearchB = CreateEbayOrderSearch(orderB);
            expectedOrderSearchD = CreateEbayOrderSearch(orderD);

            comparer = new EbayOrderSearchEntityComparer();
        }

        [Fact]
        public async Task Split_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var (orderA_2, orderA_3) = await combineSplitHelpers.PerformSplit(orderA_0, 2, 2);
            var (orderA_4, orderA_5) = await combineSplitHelpers.PerformSplit(orderA_1, 2, 2);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();
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

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_0.OrderID, orderA_1.OrderID, orderA_2.OrderID, orderA_3.OrderID, orderA_4.OrderID, orderA_5.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10", "UPS"), Times.Exactly(2));
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10-1", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10-1-1", "UPS"), Times.Once);
        }

        [Fact]
        public async Task CombineSplitCombine_WithOrderNumbers()
        {
            OrderEntity orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA, orderB);
            var (orderA_C_O, orderA_C_1) = await combineSplitHelpers.PerformSplit(orderA_C, 2, 2);
            var orderD_C = await combineSplitHelpers.PerformCombine("D-C", orderD, orderA_C_O);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();
            var identities_A_C_1 = await identityProvider.GetOrderIdentifiers(orderA_C_1);
            var identities_D_C = await identityProvider.GetOrderIdentifiers(orderD_C);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_C_1, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB, expectedOrderSearchD }, identities_D_C, comparer);

            CreateShipment(orderA_C_1);
            CreateShipment(orderD_C);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_C_1.OrderID, orderD_C.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-A-C-1", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 2000 && t.TransactionID == 2000), (bool?) null, true, "tracking-A-C-1", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-D-C", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 2000 && t.TransactionID == 2000), (bool?) null, true, "tracking-D-C", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 3000 && t.TransactionID == 3000), (bool?) null, true, "tracking-D-C", "UPS"), Times.Once);
        }

        [Fact]
        public async Task CombineTwoManualOrders_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);
            orderB = Create.CreateManualOrder(store, context.Customer, 20);

            var orderA_M_C = await combineSplitHelpers.PerformCombine("10A-M-C", orderA, orderB);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();

            var identities_A_M_C = await identityProvider.GetOrderIdentifiers(orderA_M_C);

            Assert.Equal(0, identities_A_M_C.Count());

            CreateShipment(orderA_M_C);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_M_C.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayTransactionDetails>(), (bool?) null, true, AnyString, AnyString), Times.Never);
        }

        [Fact]
        public async Task SplitManualCombineWithNotManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            var orderA_M_1_C = await combineSplitHelpers.PerformCombine("10A-M-1-C", orderA_1, orderB);

            CreateShipment(orderA_0);
            CreateShipment(orderA_M_1_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();

            var identities_A_M_1_C = await identityProvider.GetOrderIdentifiers(orderA_M_1_C);
            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);

            Assert.Equal(new[] { expectedOrderSearchB }, identities_A_M_1_C, comparer);
            Assert.Equal(0, identities_A_0.Count());

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_0.OrderID, orderA_M_1_C.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, AnyString, AnyString), Times.Never);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 2000 && t.TransactionID == 2000), (bool?) null, true, "tracking-10A-M-1-C", "UPS"), Times.Once);
        }

        [Fact]
        public async Task SplitManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();

            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            CreateShipment(orderA_0);
            CreateShipment(orderA_1);

            Assert.Equal(0, identities_A_0.Count());
            Assert.Equal(0, identities_A_1.Count());

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_0.OrderID, orderA_1.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayTransactionDetails>(), (bool?) null, true, AnyString, AnyString), Times.Never);
        }

        [Fact]
        public async Task CombineMixManualSplit_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C, 2, 2);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            CreateShipment(orderB_0);
            CreateShipment(orderB_1);

            Assert.Equal(new[] { expectedOrderSearchB }, identities_B_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchB }, identities_B_1, comparer);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderB_0.OrderID, orderB_1.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, AnyString, AnyString), Times.Never);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 2000 && t.TransactionID == 2000), (bool?) null, true, "tracking-10B-1-C", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 2000 && t.TransactionID == 2000), (bool?) null, true, "tracking-10B-1-C-1", "UPS"), Times.Once);
        }

        [Fact]
        public async Task SplitThenCombineOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            var orderA_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_0, orderB);

            CreateShipment(orderA_C);
            CreateShipment(orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();

            var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_C, comparer);
            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_1, comparer);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_C.OrderID, orderA_1.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10-1", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10A-1-C", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 2000 && t.TransactionID == 2000), (bool?) null, true, "tracking-10A-1-C", "UPS"), Times.Once);
        }

        [Fact]
        public async Task CombineSplitWithBSurviving_WithOrderNumbers()
        {
            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C, 2, 2);

            CreateShipment(orderB_0);
            CreateShipment(orderB_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_B_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_B_1, comparer);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderB_0.OrderID, orderB_1.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10B-1-C", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 2000 && t.TransactionID == 2000), (bool?) null, true, "tracking-10B-1-C", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10B-1-C-1", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 2000 && t.TransactionID == 2000), (bool?) null, true, "tracking-10B-1-C-1", "UPS"), Times.Once);
        }

        [Fact]
        public async Task CombineSplitWithASurviving_WithOrderNumbers()
        {
            var orderA_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderA, orderB);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA_1_C, 2, 2);

            CreateShipment(orderA_0);
            CreateShipment(orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();

            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_0, comparer);
            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_1, comparer);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_0.OrderID, orderA_1.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10B-1-C", "UPS"), Times.Once);
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10B-1-C-1", "UPS"), Times.Once);
        }

        [Fact]
        public async Task SplitCombine_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var orderA_C = await combineSplitHelpers.PerformCombine("A-C", orderA_0, orderA_1);

            CreateShipment(orderA_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();
            var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_C, comparer);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_C.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-A-C", "UPS"), Times.Exactly(2));
        }

        [Fact]
        public async Task SplitCombine_SplitSurvivingOrder_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("A-1-C", orderA_0, orderA_1);

            CreateShipment(orderA_1_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();
            var identities_A_1_C = await identityProvider.GetOrderIdentifiers(orderA_1_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_A_1_C, comparer);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_1_C.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-A-1-C", "UPS"), Times.Exactly(2));
        }

        [Fact]
        public async Task SplitACombineBCombineRemainingTwo_WithOrderNumbers()
        {
            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("10A-1-C", orderA_1, orderB);
            var orderA_1_C_1 = await combineSplitHelpers.PerformCombine("10A-1-C-1", orderA_1_C, orderA_0);

            CreateShipment(orderA_1_C_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();
            var identities_A_1_C_1 = await identityProvider.GetOrderIdentifiers(orderA_1_C_1);

            Assert.Equal(new[] { expectedOrderSearchA, expectedOrderSearchB }, identities_A_1_C_1, comparer);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderA_1_C_1.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10A-1-C-1", "UPS"), Times.Exactly(2));
            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 2000 && t.TransactionID == 2000), (bool?) null, true, "tracking-10A-1-C-1", "UPS"), Times.Once);
        }

        [Fact]
        public async Task SplitCombineWithManualOrder_WithOrderNumbers()
        {
            orderB = Create.CreateManualOrder(store, context.Customer, 20);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            var orderB_M_C = await combineSplitHelpers.PerformCombine("10A-M-C", orderB, orderA_1);

            CreateShipment(orderB_M_C);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<EbayCombineOrderSearchProvider>();

            var identities_B_M_C = await identityProvider.GetOrderIdentifiers(orderB_M_C);

            Assert.Equal(new[] { expectedOrderSearchA }, identities_B_M_C, comparer);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { orderB_M_C.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.Is<EbayTransactionDetails>(t => t.ItemID == 1000 && t.TransactionID == 1000), (bool?) null, true, "tracking-10A-M-C", "UPS"), Times.Once);
        }

        private EbayOrderEntity CreateEbayOrder(long orderNumber, long ebayOrderID, string ebayBuyerID, int sellerManagerRecord, double itemQuantity)
        {
            return Create.Order<EbayOrderEntity>(store, context.Customer)
                .WithItem<EbayOrderItemEntity>(i => i
                    .Set(e => e.EbayTransactionID, orderNumber * 100)
                    .Set(e => e.EbayItemID, orderNumber * 100)
                    .Set(e => e.SellingManagerRecord, sellerManagerRecord)
                    .Set(n => n.Quantity, itemQuantity))
                .Set(x => x.EbayOrderID, ebayOrderID)
                .Set(x => x.EbayBuyerID, ebayBuyerID)
                .Set(x => x.SellingManagerRecord, sellerManagerRecord)
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Save();
        }

        private EbayOrderSearchEntity CreateEbayOrderSearch(OrderEntity order)
        {
            EbayOrderEntity ebayOrder = order as EbayOrderEntity;
            return new EbayOrderSearchEntity()
            {
                EbayOrderID = ebayOrder.EbayOrderID,
                EbayBuyerID = ebayOrder.EbayBuyerID,
                SellingManagerRecord = ebayOrder.SellingManagerRecord
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