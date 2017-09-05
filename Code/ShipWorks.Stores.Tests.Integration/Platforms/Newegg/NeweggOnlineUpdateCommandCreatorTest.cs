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
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Newegg
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public partial class NeweggOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly NeweggStoreEntity store;
        private Mock<INeweggWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly NeweggOnlineUpdateCommandCreator commandCreator;

        public NeweggOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<INeweggWebClient>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.NeweggMarketplace) as NeweggOnlineUpdateCommandCreator;

            store = Create.Store<NeweggStoreEntity>(StoreTypeCode.NeweggMarketplace).Save();

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

            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 10,
                It.Is<IEnumerable<ItemDetails>>(i =>
                    i.Any(a => a.SellerPartNumber == "200000" && a.Quantity.IsEquivalentTo(2)) &&
                    i.Any(a => a.SellerPartNumber == "300000" && a.Quantity.IsEquivalentTo(3)))));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 20, It.IsAny<IEnumerable<ItemDetails>>()), Times.Never);
            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 10,
                It.Is<IEnumerable<ItemDetails>>(i => i.Any(a => a.SellerPartNumber == "300000" && a.Quantity.IsEquivalentTo(3)))));
        }

        [Fact]
        public async Task UpdateShipment_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 20,
                It.Is<IEnumerable<ItemDetails>>(i => i.Any(a => a.SellerPartNumber == "200000" && a.Quantity.IsEquivalentTo(2)))));
            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 30,
                It.Is<IEnumerable<ItemDetails>>(i => i.Any(a => a.SellerPartNumber == "300000" && a.Quantity.IsEquivalentTo(3)))));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 30,
                It.Is<IEnumerable<ItemDetails>>(i =>
                    i.Any(a => a.SellerPartNumber == "300000" && a.Quantity.IsEquivalentTo(3)) &&
                    i.Count() == 1)));
        }

        [Fact]
        public async Task UpdateShipment_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 10,
                It.Is<IEnumerable<ItemDetails>>(i =>
                    i.Any(a => a.SellerPartNumber == "200000" && a.Quantity.IsEquivalentTo(2)) &&
                    i.Any(a => a.SellerPartNumber == "300000" && a.Quantity.IsEquivalentTo(3)))));
            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 50,
                It.Is<IEnumerable<ItemDetails>>(i => i.Any(a => a.SellerPartNumber == "500000" && a.Quantity.IsEquivalentTo(5)))));
            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 60,
                It.Is<IEnumerable<ItemDetails>>(i => i.Any(a => a.SellerPartNumber == "600000" && a.Quantity.IsEquivalentTo(6)))));
        }

        [Fact]
        public async Task UpdateShipment_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 20, It.IsAny<IEnumerable<ItemDetails>>()))
                .Throws(new NeweggException("Foo"));
            webClient.Setup(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 50, It.IsAny<IEnumerable<ItemDetails>>()))
                .Throws(new NeweggException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 60,
                It.Is<IEnumerable<ItemDetails>>(i => i.Any(a => a.SellerPartNumber == "600000" && a.Quantity.IsEquivalentTo(6)))));
            webClient.Verify(x => x.UploadShippingDetails(store, It.IsAny<ShipmentEntity>(), 70,
                It.Is<IEnumerable<ItemDetails>>(i => i.Any(a => a.SellerPartNumber == "800000" && a.Quantity.IsEquivalentTo(8)))));
        }

        /// <summary>
        /// Create a normal, non-combined order
        /// </summary>
        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual, params int[] roots)
        {
            var order = roots.Aggregate(
                Create.Order<NeweggOrderEntity>(store, context.Customer),
                (o, root) => AddItem(o, root, manual, false))
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
            var order = Create.Order<NeweggOrderEntity>(store, context.Customer)
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
        private OrderEntityBuilder<NeweggOrderEntity> AddItem(OrderEntityBuilder<NeweggOrderEntity> builder, int root, bool isManual, bool setOriginalOrderID)
        {
            if (isManual)
            {
                return builder.WithItem<OrderItemEntity>(i => CreateItem(i, root, setOriginalOrderID));
            }

            return builder.WithItem<NeweggOrderItemEntity>(i =>
                CreateItem(i, root, setOriginalOrderID)
                .Set(x => x.SellerPartNumber, (100000 * root).ToString()));
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