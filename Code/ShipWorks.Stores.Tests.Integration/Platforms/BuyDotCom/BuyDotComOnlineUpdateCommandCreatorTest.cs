using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.BuyDotCom;
using ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment;
using ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.BuyDotCom
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombinedOrderUpdates")]
    public class BuyDotComOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly BuyDotComStoreEntity store;
        private Mock<IBuyDotComFtpClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly BuyDotComOnlineUpdateCommandCreator commandCreator;

        public BuyDotComOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IBuyDotComFtpClient>();
                mock.Override<IBuyDotComFtpClientFactory>()
                    .Setup(x => x.LoginAsync(It.IsAny<IBuyDotComStoreEntity>()))
                    .ReturnsAsync(webClient.Object);

                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.BuyDotCom) as BuyDotComOnlineUpdateCommandCreator;

            store = Create.Store<BuyDotComStoreEntity>(StoreTypeCode.BuyDotCom)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer)
                .WithItem<BuyDotComOrderItemEntity>(i => i.Set(x => x.ReceiptItemID, "900"))
                .Save();

            Create.Shipment(dummyOrder)
                .AsOther(o => o.Set(x => x.Carrier, "Bad").Set(x => x.Service, "Bad"))
                .Set(x => x.TrackingNumber, "track-999")
                .Set(x => x.Processed, true)
                .Save();
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-123" && i.ReceiptID == "10" &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(2) && z.ReceiptItemID == "200") &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(3) && z.ReceiptItemID == "300")))));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNotCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(4, 5, 6, "track-456", true);
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-123" && i.ReceiptID == "10" &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(2) && z.ReceiptItemID == "200") &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(3) && z.ReceiptItemID == "300")))));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-123" && i.ReceiptID == "20" &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(2) && z.ReceiptItemID == "200")))));
            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-123" && i.ReceiptID == "30" &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(3) && z.ReceiptItemID == "300")))));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-123" && i.ReceiptID == "30" &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(3) && z.ReceiptItemID == "300")))));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-123" && i.ReceiptID == "10" &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(2) && z.ReceiptItemID == "200") &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(3) && z.ReceiptItemID == "300")))));
            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-456" && i.ReceiptID == "50" &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(5) && z.ReceiptItemID == "500")))));
            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-456" && i.ReceiptID == "60" &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(6) && z.ReceiptItemID == "600")))));
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, 8, 9, "track-789", false);

            webClient.Setup(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c => c.Any(i => i.ReceiptID == "10"))))
                .Throws<BuyDotComException>();
            webClient.Setup(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c => c.Any(i => i.ReceiptID == "50"))))
                .Throws<BuyDotComException>();

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-456" && i.ReceiptID == "60" &&
                    i.OrderLines.Any(z => z.Quantity.IsEquivalentTo(6) && z.ReceiptItemID == "600")))));
            webClient.Verify(x => x.UploadShipConfirmation(It.Is<List<BuyDotComShipConfirmation>>(c =>
                c.Any(i => i.TrackingNumber == "track-789" && i.ReceiptID == "70"))));
        }

        private OrderEntity CreateNormalOrder(int orderRoot, int item1Root, int item2Root, string trackingNumber, bool manual)
        {
            var order = Create.Order(store, context.Customer)
                .WithItem<BuyDotComOrderItemEntity>(i => i.Set(x => x.ReceiptItemID, (item1Root * 100L).ToString())
                    .Set(x => x.ListingID, item1Root * 1000).Set(x => x.Quantity, item1Root))
                .WithItem<BuyDotComOrderItemEntity>(i => i.Set(x => x.ReceiptItemID, (item2Root * 100L).ToString())
                    .Set(x => x.ListingID, item2Root * 1000).Set(x => x.Quantity, item2Root))
                .Set(x => x.OrderNumberComplete, (orderRoot * 10).ToString())
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.None)
                .Set(x => x.IsManual, manual)
                .Save();

            Create.Shipment(order)
                .AsOther(o =>
                {
                    o.Set(x => x.Carrier, "Foo")
                    .Set(x => x.Service, "Bar");
                })
                .Set(x => x.TrackingNumber, trackingNumber)
                .Set(x => x.Processed, true)
                .Save();
            return order;
        }

        private OrderEntity CreateCombinedOrder(int orderRoot, string trackingNumber, params Tuple<int, bool>[] combinedOrderDetails)
        {
            var order = Create.Order(store, context.Customer)
                .Set(x => x.OrderNumberComplete, (orderRoot * 10).ToString())
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
                .Save();

            foreach (var details in combinedOrderDetails)
            {
                int idRoot = details.Item1;
                bool manual = details.Item2;

                Create.Entity<OrderSearchEntity>()
                    .Set(x => x.OrderID, order.OrderID)
                    .Set(x => x.StoreID, store.StoreID)
                    .Set(x => x.OriginalOrderID, idRoot * -1006)
                    .Set(x => x.OrderNumber, idRoot * 10)
                    .Set(x => x.OrderNumberComplete, (idRoot * 10).ToString())
                    .Set(x => x.IsManual, manual)
                    .Save();
            }

            combinedOrderDetails.Aggregate(
                Modify.Order(order),
                (o, d) => o.WithItem<BuyDotComOrderItemEntity>(i => i
                    .Set(x => x.ReceiptItemID, (d.Item1 * 100L).ToString())
                    .Set(x => x.OriginalOrderID, d.Item1 * -1006)
                    .Set(x => x.ListingID, d.Item1 * 1000)
                    .Set(x => x.Quantity, d.Item1)))
                .Save();

            Create.Shipment(order)
                .AsOther(o =>
                {
                    o.Set(x => x.Carrier, "Foo")
                    .Set(x => x.Service, "Bar");
                })
                .Set(x => x.TrackingNumber, trackingNumber)
                .Set(x => x.Processed, true)
                .Save();
            return order;
        }

        public void Dispose() => context.Dispose();
    }
}