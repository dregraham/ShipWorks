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
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.OnlineUpdating;
using ShipWorks.Stores.Tests.Integration.Helpers;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Newegg
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombineSplit")]
    public class NeweggCombineAndSplitTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private Mock<IOrderCombinationUserInteraction> combineInteraction;
        private Mock<IOrderSplitUserInteraction> splitInteraction;
        private Mock<IAsyncMessageHelper> asyncMessageHelper;
        private readonly NeweggStoreEntity store;
        private OrderEntity orderA;
        private OrderEntity orderB;
        private OrderEntity orderD;
        private readonly Dictionary<long, OrderEntity> orders;
        private readonly CombineSplitHelpers combineSplitHelpers;
        private readonly IOrderSplitGateway orderSplitGateway;
        private IDataAccess dataAccess;
        private (string, double)[] emptyResults = Enumerable.Empty<(string, double)>().ToArray();

        public NeweggCombineAndSplitTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                combineInteraction = mock.Override<IOrderCombinationUserInteraction>();
                splitInteraction = mock.Override<IOrderSplitUserInteraction>();
                mock.Override<IMessageHelper>();
                asyncMessageHelper = mock.Override<IAsyncMessageHelper>();
            });

            orderSplitGateway = context.Mock.Container.Resolve<IOrderSplitGateway>();
            dataAccess = context.Mock.Container.Resolve<IDataAccess>();
            combineSplitHelpers = new CombineSplitHelpers(context, splitInteraction, combineInteraction);

            asyncMessageHelper.Setup(x => x.ShowProgressDialog(AnyString, AnyString))
                .ReturnsAsync(context.Mock.Build<ISingleItemProgressDialog>());

            store = Create.Store<NeweggStoreEntity>()
                .Set(x => x.StoreTypeCode, StoreTypeCode.NeweggMarketplace)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            Create.Order(store, context.Customer).Save();

            orders = new Dictionary<long, OrderEntity> { { 1, orderA }, { 2, orderB }, { 3, orderD } };
        }

        private void CreateDefaultOrders()
        {
            orderA = CreateNeweggOrder(10, 1000, 10);
            orderB = CreateNeweggOrder(20, 2000, 20);
            orderD = CreateNeweggOrder(30, 3000, 30);
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

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();
            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);
            var identities_A_2 = await identityProvider.GetOrderIdentifiers(orderA_2);
            var identities_A_3 = await identityProvider.GetOrderIdentifiers(orderA_3);
            var identities_A_4 = await identityProvider.GetOrderIdentifiers(orderA_4);
            var identities_A_5 = await identityProvider.GetOrderIdentifiers(orderA_5);

            Assert.True(identities_A_0.All(i => i.OrderNumber == 10));
            Assert.True(identities_A_1.All(i => i.OrderNumber == 10));
            Assert.True(identities_A_2.All(i => i.OrderNumber == 10));
            Assert.True(identities_A_3.All(i => i.OrderNumber == 10));
            Assert.True(identities_A_4.All(i => i.OrderNumber == 10));
            Assert.True(identities_A_5.All(i => i.OrderNumber == 10));

            var expectedItems = Enumerable.Empty<(string, double)>().ToArray();

            await ValidateResults(orderA_0, new[] { ("spn-10", 6.0) }, identityProvider, dataAccess);

            await ValidateResults(orderA_1, emptyResults, identityProvider, dataAccess);

            await ValidateResults(orderA_2, new[] { ("spn-10", 6.0) }, identityProvider, dataAccess);

            await ValidateResults(orderA_3, new[] { ("spn-10", 2.0) }, identityProvider, dataAccess);

            await ValidateResults(orderA_4, emptyResults, identityProvider, dataAccess);

            await ValidateResults(orderA_5, new[] { ("spn-10", 2.0) }, identityProvider, dataAccess);
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

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();
            var identities_A_C_1 = await identityProvider.GetOrderIdentifiers(orderA_C_1);
            var identities_D_C = await identityProvider.GetOrderIdentifiers(orderD_C);

            Assert.Equal(new[] { 10L, 20L }, identities_A_C_1.Select(i => i.OrderNumber));
            Assert.Equal(new[] { 10L, 20L, 30L }, identities_D_C.Select(i => i.OrderNumber));

            await ValidateResults(orderA_C_1, new[] { ("spn-10", 2.0), ("spn-20", 2.0) }, identityProvider, dataAccess);

            await ValidateResults(orderD_C, new[] { ("spn-10", 8.0), ("spn-20", 18.0), ("spn-30", 30.0) }, identityProvider, dataAccess);
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

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();

            var identities_A_M_C = await identityProvider.GetOrderIdentifiers(orderA_M_C);

            Assert.Equal(0, identities_A_M_C.Count());

            await ValidateResults(orderA_M_C, emptyResults, identityProvider, dataAccess);
        }

        [Fact]
        public async Task SplitManualCombineWithNotManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);
            orderB = CreateNeweggOrder(20, 2000, 20);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            var orderA_M_1_C = await combineSplitHelpers.PerformCombine("10A-M-1-C", orderA_1, orderB);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();

            var identities_A_M_1_C = await identityProvider.GetOrderIdentifiers(orderA_M_1_C);
            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);

            Assert.Equal(new[] { 20L }, identities_A_M_1_C.Select(i => i.OrderNumber));
            Assert.Equal(0, identities_A_0.Count());

            await ValidateResults(orderA_M_1_C, new[] { ("spn-20", 20.0) }, identityProvider, dataAccess);

            await ValidateResults(orderA_0, emptyResults, identityProvider, dataAccess);
        }

        [Fact]
        public async Task SplitManualOrder_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();

            await ValidateResults(orderA_0, emptyResults, identityProvider, dataAccess);

            await ValidateResults(orderA_1, emptyResults, identityProvider, dataAccess);
        }

        [Fact]
        public async Task CombineMixManualSplit_WithOrderNumbers()
        {
            orderA = Create.CreateManualOrder(store, context.Customer, 10);
            orderB = CreateNeweggOrder(20, 2000, 20);

            var orderB_1_C = await combineSplitHelpers.PerformCombine("10B-1-C", orderB, orderA);

            var (orderB_0, orderB_1) = await combineSplitHelpers.PerformSplit(orderB_1_C, 2, 2);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            Assert.Equal(new[] { 20L }, identities_B_0.Select(i => i.OrderNumber));
            Assert.Equal(new[] { 20L }, identities_B_1.Select(i => i.OrderNumber));

            await ValidateResults(orderB_0, new[] { ("spn-20", 18.0) }, identityProvider, dataAccess);

            await ValidateResults(orderB_1, new[] { ("spn-20", 2.0) }, identityProvider, dataAccess);
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

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();

            var identities_A = await identityProvider.GetOrderIdentifiers(orderA);
            var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            // Combined order
            Assert.Equal(new[] { 10L, 20L }, identities_A_C.Select(i => i.OrderNumber));
            await ValidateResults(orderA_C, new[] { ("spn-10", 8.0), ("spn-20", 20.0) }, identityProvider, dataAccess);

            // Split order
            Assert.Equal(new[] { 10L }, identities_A_1.Select(i => i.OrderNumber));
            await ValidateResults(orderA_1, new[] { ("spn-10", 2.0) }, identityProvider, dataAccess);
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

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();

            var identities_B_0 = await identityProvider.GetOrderIdentifiers(orderB_0);
            var identities_B_1 = await identityProvider.GetOrderIdentifiers(orderB_1);

            // Combined order
            Assert.Equal(new[] { 10L, 20L }, identities_B_0.Select(i => i.OrderNumber));
            await ValidateResults(orderB_0, new[] { ("spn-10", 8.0), ("spn-20", 18.0) }, identityProvider, dataAccess);

            Assert.Equal(new[] { 10L, 20L }, identities_B_1.Select(i => i.OrderNumber));
            await ValidateResults(orderB_1, new[] { ("spn-10", 2.0), ("spn-20", 2.0) }, identityProvider, dataAccess);
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

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();

            var identities_A_0 = await identityProvider.GetOrderIdentifiers(orderA_0);
            var identities_A_1 = await identityProvider.GetOrderIdentifiers(orderA_1);

            // Combined order
            Assert.Equal(new[] { 10L, 20L }, identities_A_0.Select(i => i.OrderNumber));
            await ValidateResults(orderA_0, new[] { ("spn-10", 8.0), ("spn-20", 18.0) }, identityProvider, dataAccess);

            Assert.Equal(new[] { 10L, 20L }, identities_A_1.Select(i => i.OrderNumber));
            await ValidateResults(orderA_1, new[] { ("spn-10", 2.0), ("spn-20", 2.0) }, identityProvider, dataAccess);
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

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();
            var identities_A_C = await identityProvider.GetOrderIdentifiers(orderA_C);
            
            Assert.Equal(new[] { 10L }, identities_A_C.Select(i => i.OrderNumber).Distinct());
            await ValidateResults(orderA_C, new[] { ("spn-10", 8.0), ("spn-10", 2.0) }, identityProvider, dataAccess);
        }

        [Fact]
        public async Task SplitCombine_SplitSurvivingOrder_WithOrderNumbers()
        {
            CreateDefaultOrders();

            var (orderA_0, orderA_1) = await combineSplitHelpers.PerformSplit(orderA, 2, 2);
            var orderA_1_C = await combineSplitHelpers.PerformCombine("A-1-C", orderA_0, orderA_1);

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();
            var identities_A_1_C = await identityProvider.GetOrderIdentifiers(orderA_1_C);

            Assert.Equal(new[] { 10L }, identities_A_1_C.Select(i => i.OrderNumber).Distinct());
            await ValidateResults(orderA_1_C, new[] { ("spn-10", 8.0), ("spn-10", 2.0) }, identityProvider, dataAccess);
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

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();
            var identities_D = await identityProvider.GetOrderIdentifiers(orderD);
            var identities_A_1_C_1 = await identityProvider.GetOrderIdentifiers(orderA_1_C_1);

            // Unchanged orderD
            await ValidateResults(orderD, new[] { ("spn-30", 30.0) }, identityProvider, dataAccess);

            // Combined order
            await ValidateResults(orderA_1_C_1, new[] { ("spn-10", 8.0), ("spn-10", 2.0), ("spn-20", 20.0) }, identityProvider, dataAccess);
        }


        [Fact]
        public async Task SplitCombineWithManualOrder_WithOrderNumbers()
        {
            orderA = CreateNeweggOrder(10, 1000, 10);
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

            // Get online identities
            var identityProvider = context.Mock.Container.Resolve<INeweggCombineOrderSearchProvider>();

            var identities_B_M_C = await identityProvider.GetOrderIdentifiers(orderB_M_C);
            Assert.Equal(new[] { 10L }, identities_B_M_C.Select(i => i.OrderNumber));

            // Split orders
            await ValidateResults(orderA_0, new[] { ("spn-10", 8.0) }, identityProvider, dataAccess);

            // Combined order
            await ValidateResults(orderB_M_C, new[] { ("spn-10", 2.0) }, identityProvider, dataAccess);
        }
        

        private static async Task ValidateResults(OrderEntity order, ValueTuple<string, double>[] expectedItems,
            INeweggCombineOrderSearchProvider identityProvider, IDataAccess dataAccess)
        {
            var identities = await identityProvider.GetOrderIdentifiers(order);
            var actualItems = await GetShipmentItems(order, dataAccess, identities, $"tracking-{DateTime.Now:T}");
            Assert.Equal(expectedItems, actualItems.Select(i => (i.SellerPartNumber, i.Quantity)));
        }

        private NeweggOrderEntity CreateNeweggOrder(long orderNumber, long invoiceNumber, double itemQuantity)
        {
            return Create.Order<NeweggOrderEntity>(store, context.Customer)
                .WithItem<NeweggOrderItemEntity>(i => i.Set(n => n.SellerPartNumber, $"spn-{orderNumber}").Set(n => n.Quantity, itemQuantity))
                .Set(x => x.InvoiceNumber, invoiceNumber)
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Save();
        }

        public static async Task<IEnumerable<ItemDetails>> GetShipmentItems(OrderEntity order, IDataAccess dataAccess,
            IEnumerable<OrderUploadDetail> orderIdentities, string tranckingNumber)
        {
            var shipment = Create.Shipment(order)
                .AsOther(o => o.Set(x => x.Carrier, "UPS").Set(x => x.Service, "Ground"))
                .Set(x => x.TrackingNumber, tranckingNumber)
                .Set(x => x.Processed, true)
                .Save();

            ShipmentUploadDetails shipmentUploadDetails = await dataAccess.LoadShipmentDetailsAsync(shipment);

            return orderIdentities.SelectMany(oi => shipmentUploadDetails.GetItemsFor(oi));
        }

        public void Dispose() => context.Dispose();
    }
}