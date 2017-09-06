using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using ShipWorks.Stores.Platforms.Walmart.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Walmart
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public partial class WalmartOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly WalmartStoreEntity store;
        private Mock<IWalmartWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly WalmartOnlineUpdateInstanceCommands commandCreator;

        public WalmartOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IWalmartWebClient>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Walmart) as WalmartOnlineUpdateInstanceCommands;

            webClient.Setup(x => x.UpdateShipmentDetails(It.IsAny<IWalmartStoreEntity>(), It.IsAny<orderShipment>(), AnyString))
                .Returns(new Order
                {
                    orderLines = new orderLineType[0]
                });

            store = Create.Store<WalmartStoreEntity>(StoreTypeCode.Walmart).Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer).Save();

            Create.Shipment(dummyOrder)
                .AsOther(o => o.Set(x => x.Carrier, "Bad").Set(x => x.Service, "Bad"))
                .Set(x => x.TrackingNumber, "track-999")
                .Set(x => x.Processed, true)
                .Save();
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o =>
                    MatchesItem(o.orderLines, 2, "track-123") &&
                    MatchesItem(o.orderLines, 3, "track-123")),
                "10000"));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UpdateShipmentDetails(store, It.IsAny<orderShipment>(), "20000"), Times.Never);
            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o => MatchesItem(o.orderLines, 3, "track-123")),
                "10000"));
        }

        [Fact]
        public async Task UpdateShipment_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o => MatchesItem(o.orderLines, 2, "track-123")),
                "20000"));
            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o => MatchesItem(o.orderLines, 3, "track-123")),
                "30000"));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UpdateShipmentDetails(store, It.IsAny<orderShipment>(), "20000"), Times.Never);
            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o => MatchesItem(o.orderLines, 3, "track-123")),
                "30000"));
        }

        [Fact]
        public async Task UpdateShipment_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o =>
                    MatchesItem(o.orderLines, 2, "track-123") &&
                    MatchesItem(o.orderLines, 3, "track-123")),
                "10000"));
            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o => MatchesItem(o.orderLines, 5, "track-456")),
                "50000"));
            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o => MatchesItem(o.orderLines, 6, "track-456")),
                "60000"));
        }

        [Fact]
        public async Task UpdateShipment_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UpdateShipmentDetails(store, It.IsAny<orderShipment>(), "10000"))
                .Throws(new WalmartException("Foo"));
            webClient.Setup(x => x.UpdateShipmentDetails(store, It.IsAny<orderShipment>(), "50000"))
                .Throws(new WalmartException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o => MatchesItem(o.orderLines, 6, "track-456")),
                "60000"));
            webClient.Verify(x => x.UpdateShipmentDetails(store,
                It.Is<orderShipment>(o => MatchesItem(o.orderLines, 8, "track-789")),
                "70000"));
        }

        private bool MatchesItem(IEnumerable<shippingLineType> shippingLineTypes, int root, string trackingNumber)
        {
            return shippingLineTypes.Any(x =>
                {
                    var lineStatus = x.orderLineStatuses[0];

                    return x.lineNumber == (root * 100).ToString() &&
                        lineStatus.status == orderLineStatusValueType.Shipped &&
                        lineStatus.statusQuantity.amount == root.ToString() &&
                        lineStatus.trackingInfo.trackingNumber == trackingNumber &&
                        lineStatus.trackingInfo.methodCode == shippingMethodCodeType.Standard;
                });

        }

        /// <summary>
        /// Create a normal, non-combined order
        /// </summary>
        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual, params int[] roots)
        {
            var order = roots.Aggregate(
                Create.Order<WalmartOrderEntity>(store, context.Customer),
                (o, root) => AddItem(o, root, manual, false))
                .Set(x => x.PurchaseOrderID, (orderRoot * 10000).ToString())
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.None)
                .Set(x => x.IsManual, manual)
                .Set(x => x.RequestedShippingMethodCode, "Standard")
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
        /// Create a combined order
        /// </summary>
        private OrderEntity CreateCombinedOrder(int orderRoot, string trackingNumber, params Tuple<int, bool>[] combinedOrderDetails)
        {
            var order = Create.Order<WalmartOrderEntity>(store, context.Customer)
                .Set(x => x.PurchaseOrderID, (orderRoot * 10000).ToString())
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
                .Set(x => x.RequestedShippingMethodCode, "Standard")
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
                    Create.Entity<WalmartOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Set(x => x.PurchaseOrderID, (idRoot * 10000).ToString())
                        .Save();
                }
            }

            combinedOrderDetails.Aggregate(
                Modify.Order(order),
                (o, root) => AddItem(o, root.Item1, root.Item2, true))
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
        private OrderEntityBuilder<WalmartOrderEntity> AddItem(OrderEntityBuilder<WalmartOrderEntity> builder, int root, bool isManual, bool setOriginalOrderID)
        {
            if (isManual)
            {
                return builder.WithItem<OrderItemEntity>(i => CreateItem(i, root, setOriginalOrderID));
            }

            return builder.WithItem<WalmartOrderItemEntity>(i =>
                CreateItem(i, root, setOriginalOrderID)
                .Set(x => x.OnlineStatus, orderLineStatusValueType.Acknowledged.ToString())
                .Set(x => x.LineNumber, (root * 100).ToString()));
        }

        /// <summary>
        /// Create an item
        /// </summary>
        private OrderItemEntityBuilder<T> CreateItem<T>(OrderItemEntityBuilder<T> builder, int root, bool setOriginalOrderID) where T : OrderItemEntity, new()
        {
            builder.Set(x => x.Quantity, root);

            if (setOriginalOrderID)
            {
                builder.Set(x => x.OriginalOrderID, root * -1006);
            }

            return builder;
        }

        public void Dispose() => context.Dispose();
    }
}