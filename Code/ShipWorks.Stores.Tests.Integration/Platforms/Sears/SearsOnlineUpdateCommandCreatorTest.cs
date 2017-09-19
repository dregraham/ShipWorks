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
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Sears;
using ShipWorks.Stores.Platforms.Sears.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Sears
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public partial class SearsOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly SearsStoreEntity store;
        private Mock<ISearsWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly SearsOnlineUpdateCommandCreator commandCreator;

        public SearsOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<ISearsWebClient>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Sears) as SearsOnlineUpdateCommandCreator;

            store = Create.Store<SearsStoreEntity>(StoreTypeCode.Sears).Save();

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

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "10000"),
                It.Is<IEnumerable<SearsTracking>>(t =>
                    HasTracking(t, "10000", "track-123", 2) &&
                    HasTracking(t, "10000", "track-123", 3))));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "20000"),
                It.IsAny<IEnumerable<SearsTracking>>()), Times.Never);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "10000"),
                It.Is<IEnumerable<SearsTracking>>(t => HasTracking(t, "10000", "track-123", 3))));
        }

        [Fact]
        public async Task UpdateShipment_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "20000"),
                It.Is<IEnumerable<SearsTracking>>(t => HasTracking(t, "20000", "track-123", 2))));
            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "30000"),
                It.Is<IEnumerable<SearsTracking>>(t => HasTracking(t, "30000", "track-123", 3))));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "20000"),
                It.IsAny<IEnumerable<SearsTracking>>()), Times.Never);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "30000"),
                It.Is<IEnumerable<SearsTracking>>(t => HasTracking(t, "30000", "track-123", 3))));
        }

        [Fact]
        public async Task UpdateShipment_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "10000"),
                It.Is<IEnumerable<SearsTracking>>(t =>
                    HasTracking(t, "10000", "track-123", 2) &&
                    HasTracking(t, "10000", "track-123", 3))));

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "50000"),
                It.Is<IEnumerable<SearsTracking>>(t => HasTracking(t, "50000", "track-456", 5))));

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "60000"),
                It.Is<IEnumerable<SearsTracking>>(t => HasTracking(t, "60000", "track-456", 6))));
        }

        [Fact]
        public async Task UpdateShipment_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UploadShipmentDetails(store,
                    It.Is<SearsOrderDetail>(o => o.PoNumber == "20000"),
                    It.IsAny<IEnumerable<SearsTracking>>()))
                .Throws(new SearsException("Foo"));

            webClient.Setup(x => x.UploadShipmentDetails(store,
                    It.Is<SearsOrderDetail>(o => o.PoNumber == "20000"),
                    It.IsAny<IEnumerable<SearsTracking>>()))
                .Throws(new SearsException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "60000"),
                It.Is<IEnumerable<SearsTracking>>(t => HasTracking(t, "60000", "track-456", 6))));

            webClient.Verify(x => x.UploadShipmentDetails(store,
                It.Is<SearsOrderDetail>(o => o.PoNumber == "70000"),
                It.Is<IEnumerable<SearsTracking>>(t => HasTracking(t, "70000", "track-789", 8))));
        }

        /// <summary>
        /// Check for a tracking entry
        /// </summary>
        private bool HasTracking(IEnumerable<SearsTracking> trackingEntry, string PoNumber, string trackingNumber, int itemRoot) =>
            trackingEntry.Any(z =>
                z.TrackingNumber == trackingNumber &&
                z.ItemID == (itemRoot * 100).ToString() &&
                z.LineNumber == itemRoot * 1000 &&
                z.PoNumber == PoNumber &&
                z.Carrier == "OTH");

        /// <summary>
        /// Create a normal, non-combined order
        /// </summary>
        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual, params int[] roots)
        {
            var order = roots.Aggregate(
                    Create.Order<SearsOrderEntity>(store, context.Customer),
                    (o, root) => AddItem(o, root, manual, false))
                .Set(x => x.PoNumber, (orderRoot * 10000).ToString())
                .Set(x => x.OrderNumber, orderRoot * 10)
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

        /// <summary>
        /// Create a combined order
        /// </summary>
        private OrderEntity CreateCombinedOrder(int orderRoot, string trackingNumber, params Tuple<int, bool>[] combinedOrderDetails)
        {
            var order = Create.Order<SearsOrderEntity>(store, context.Customer)
                .Set(x => x.PoNumber, (orderRoot * 10000).ToString())
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
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
                    Create.Entity<SearsOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Set(x => x.PoNumber, (idRoot * 10000).ToString())
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
        private OrderEntityBuilder<SearsOrderEntity> AddItem(OrderEntityBuilder<SearsOrderEntity> builder, int root, bool isManual, bool setOriginalOrderID)
        {
            if (isManual)
            {
                return builder.WithItem<OrderItemEntity>(i => CreateItem(i, root, setOriginalOrderID));
            }

            return builder.WithItem<SearsOrderItemEntity>(i =>
                CreateItem(i, root, setOriginalOrderID)
                .Set(x => x.ItemID, (root * 100).ToString())
                .Set(x => x.LineNumber, root * 1000));
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