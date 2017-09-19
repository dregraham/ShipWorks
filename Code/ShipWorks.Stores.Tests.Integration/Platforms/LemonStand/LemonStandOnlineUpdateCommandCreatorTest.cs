using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Stores.Platforms.LemonStand.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Tests.Integration.Platforms.LemonStand
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public partial class LemonStandOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly LemonStandStoreEntity store;
        private Mock<ILemonStandWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly LemonStandOnlineUpdateCommandCreator commandCreator;

        private static int NewStatus = 1;
        private static int ProcessingStatus = 2;
        private static string NewStatusText = "NewStatus";
        private static string ProcessingStatusText = "ProcessingStatus";

        public LemonStandOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<ILemonStandWebClient>();
                webClient.Setup(wc => wc.GetOrderInvoice(AnyString)).Returns(CreateOrderInvoice("1"));
                webClient.Setup(wc => wc.GetShipment(AnyString)).Returns(CreateShipment("9"));
                mock.Override<IMessageHelper>();
            });
            
            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.LemonStand) as LemonStandOnlineUpdateCommandCreator;

            store = Create.Store<LemonStandStoreEntity>(StoreTypeCode.LemonStand)
                .Set(x => x.Token, "999999")
                .Set(x => x.StoreURL, "http://www.lemonstand.com")
                .Set(x => x.StatusCodes, $@"<StatusCodes><StatusCode><Code>{NewStatus}</Code><Name>{NewStatusText}</Name></StatusCode><StatusCode><Code>{ProcessingStatus}</Code><Name>{ProcessingStatusText}</Name></StatusCode></StatusCodes>")
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer).Save();

            Create.Shipment(dummyOrder)
                .AsOther(o => o.Set(x => x.Carrier, "Bad").Set(x => x.Service, "Bad"))
                .Set(x => x.TrackingNumber, "track-999")
                .Set(x => x.Processed, true)
                .Save();
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyString), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("track-123", "10000"));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyString), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("track-123", "10000"));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyString), Times.Exactly(2));
            webClient.Verify(x => x.UploadShipmentDetails("track-123", "20000"));
            webClient.Verify(x => x.UploadShipmentDetails("track-123", "30000"));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            LemonStandOrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyString), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("track-123", "30000"));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);
            
            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyString), Times.Exactly(3));
            webClient.Verify(x => x.UploadShipmentDetails("track-123", "10000"));
            webClient.Verify(x => x.UploadShipmentDetails("track-456", "50000"));
            webClient.Verify(x => x.UploadShipmentDetails("track-456", "60000"));
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UploadShipmentDetails("track-123", "10000"))
                .Throws(new LemonStandException("Foo"));
            webClient.Setup(x => x.UploadShipmentDetails("track-456", "50000"))
                .Throws(new LemonStandException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyString), Times.Exactly(4));
            webClient.Verify(x => x.UploadShipmentDetails("track-123", "10000"), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("track-456", "50000"), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("track-456", "60000"), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("track-789", "70000"), Times.Once);
        }
        
        [Fact]
        public async Task UpdateOrderStatus_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(AnyString, AnyString), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus("10000", ProcessingStatusText));
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            LemonStandOrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            LemonStandOrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(AnyString, AnyString), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus("10000", ProcessingStatusText));
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            LemonStandOrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(AnyString, AnyString), Times.Exactly(2));
            webClient.Verify(x => x.UpdateOrderStatus("20000", ProcessingStatusText));
            webClient.Verify(x => x.UpdateOrderStatus("30000", ProcessingStatusText));
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            LemonStandOrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(AnyString, AnyString), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus("30000", ProcessingStatusText));
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(AnyString, AnyString), Times.Exactly(3));
            webClient.Verify(x => x.UpdateOrderStatus("10000", ProcessingStatusText));
            webClient.Verify(x => x.UpdateOrderStatus("50000", ProcessingStatusText));
            webClient.Verify(x => x.UpdateOrderStatus("60000", ProcessingStatusText));
        }

        [Fact]
        public async Task UpdateOrderStatus_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UpdateOrderStatus("10000", ProcessingStatusText))
                .Throws(new LemonStandException("Foo"));
            webClient.Setup(x => x.UpdateOrderStatus("50000", ProcessingStatusText))
                .Throws(new LemonStandException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(AnyString, AnyString), Times.Exactly(4));
            webClient.Verify(x => x.UpdateOrderStatus("10000", ProcessingStatusText), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus("50000", ProcessingStatusText), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus("60000", ProcessingStatusText), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus("70000", ProcessingStatusText), Times.Once);
        }

        private JToken CreateOrderInvoice(string id)
        {
            return JToken.Parse("{\"data\": {\"invoices\": {\"data\": [{\"id\": " + id + "}]}}}");
        }

        private JToken CreateShipment(string id)
        {
            return JToken.Parse("{\"data\": {\"shipments\": {\"data\": [{\"id\": " + id + "}]}}}");
        }

        /// <summary>
        /// Create a normal, non-combined order
        /// </summary>
        private LemonStandOrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual, params int[] roots)
        {
            LemonStandOrderEntity order = roots.Aggregate(
                Create.Order<LemonStandOrderEntity>(store, context.Customer),
                (o, root) => AddItem(o, root, false))
                .Set(x => x.LemonStandOrderID, (orderRoot * 10000).ToString())
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.None)
                .Set(x => x.IsManual, manual)
                .Set(x => x.OnlineStatusCode, NewStatus)
                .Set(x => x.OnlineStatus, "NewStatus")
                .Save();

            webClient.Setup(wc => wc.GetOrderInvoice(order.LemonStandOrderID)).Returns(CreateOrderInvoice(order.LemonStandOrderID));
            webClient.Setup(wc => wc.GetShipment(order.LemonStandOrderID)).Returns(CreateShipment(order.LemonStandOrderID));

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

        /// <summary>
        /// Create a combined order
        /// </summary>
        private LemonStandOrderEntity CreateCombinedOrder(int orderRoot, string trackingNumber, params Tuple<int, bool>[] combinedOrderDetails)
        {
            var order = Create.Order<LemonStandOrderEntity>(store, context.Customer)
                .Set(x => x.LemonStandOrderID, (orderRoot * 10000).ToString())
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
                .Set(x => x.OnlineStatusCode, NewStatus)
                .Set(x => x.OnlineStatus, "NewStatus")
                .Save();

            foreach (var details in combinedOrderDetails)
            {
                int idRoot = details.Item1;
                bool manual = details.Item2;

                Create.Entity<OrderSearchEntity>()
                    .Set(x => x.OrderID, order.OrderID)
                    .Set(x => x.StoreID, store.StoreID)
                    .Set(x => x.IsManual, manual)
                    .Set(x => x.OriginalOrderID, idRoot * -1006)
                    .Set(x => x.OrderNumber, idRoot * 10)
                    .Set(x => x.OrderNumberComplete, (idRoot * 10).ToString())
                    .Save();

                if (!manual)
                {
                    LemonStandOrderSearchEntity orderSearch = Create.Entity<LemonStandOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Set(x => x.LemonStandOrderID, (idRoot * 10000).ToString())
                        .Save();

                    webClient.Setup(wc => wc.GetOrderInvoice(orderSearch.LemonStandOrderID)).Returns(CreateOrderInvoice(orderSearch.LemonStandOrderID));
                    webClient.Setup(wc => wc.GetShipment(orderSearch.LemonStandOrderID)).Returns(CreateShipment(orderSearch.LemonStandOrderID));
                }
            }

            combinedOrderDetails.Aggregate(
                Modify.Order(order),
                (o, root) => AddItem(o, root.Item1, true))
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

        /// <summary>
        /// Add an item to the order
        /// </summary>
        private OrderEntityBuilder<LemonStandOrderEntity> AddItem(OrderEntityBuilder<LemonStandOrderEntity> builder, int root, bool setOriginalOrderID) =>
            builder.WithItem<OrderItemEntity>(i => CreateItem(i, root, setOriginalOrderID));

        /// <summary>
        /// Create an item
        /// </summary>
        private void CreateItem<T>(OrderItemEntityBuilder<T> builder, int root, bool setOriginalOrderID) where T : OrderItemEntity, new()
        {
            builder.Set(x => x.Quantity, root);

            if (setOriginalOrderID)
            {
                builder.Set(x => x.OriginalOrderID, root * -1006);
            }
        }

        public void Dispose() => context.Dispose();
    }
}