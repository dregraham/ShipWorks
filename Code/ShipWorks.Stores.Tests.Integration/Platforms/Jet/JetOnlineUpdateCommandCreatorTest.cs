﻿using System;
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
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Jet
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public partial class JetOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly JetStoreEntity store;
        private Mock<IJetWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly JetUpdateOnlineCommandCreator commandCreator;

        public JetOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IJetWebClient>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Jet) as JetUpdateOnlineCommandCreator;
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

            store = Create.Store<JetStoreEntity>(StoreTypeCode.Jet)
                .Set(x => x.ApiUser, "apiuser")
                .Set(x => x.Secret, "secret")
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

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails("10000", ShipmentWithTrackingNumber("track-123"), store));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyShipment, store), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("10000", ShipmentWithTrackingNumber("track-123"), store), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyShipment, store), Times.Exactly(2));
            webClient.Verify(x => x.UploadShipmentDetails("20000", ShipmentWithTrackingNumber("track-123"), store), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("30000", ShipmentWithTrackingNumber("track-123"), store), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyShipment, store), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("20000", ShipmentWithTrackingNumber("track-123"), store), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails("30000", ShipmentWithTrackingNumber("track-123"), store), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyShipment, store), Times.Exactly(3));
            webClient.Verify(x => x.UploadShipmentDetails("10000", ShipmentWithTrackingNumber("track-123"), store), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("50000", ShipmentWithTrackingNumber("track-456"), store), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("60000", ShipmentWithTrackingNumber("track-456"), store), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UploadShipmentDetails("10000", ShipmentWithTrackingNumber("track-123"), store))
                .Throws(new JetException("Foo"));
            webClient.Setup(x => x.UploadShipmentDetails("50000", ShipmentWithTrackingNumber("track-456"), store))
                .Throws(new JetException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(AnyString, AnyShipment, store), Times.Exactly(4));
            webClient.Verify(x => x.UploadShipmentDetails("10000", ShipmentWithTrackingNumber("track-123"), store), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("50000", ShipmentWithTrackingNumber("track-456"), store), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("60000", ShipmentWithTrackingNumber("track-456"), store), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails("70000", ShipmentWithTrackingNumber("track-789"), store), Times.Once);
        }

        /// <summary>
        /// Create a normal, non-combined order
        /// </summary>
        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual, params int[] roots)
        {
            var order = roots.Aggregate(
                Create.Order<JetOrderEntity>(store, context.Customer),
                (o, root) => AddItem(o, root, false))
                .Set(x => x.MerchantOrderId, (orderRoot * 10000).ToString())
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
            var order = Create.Order<JetOrderEntity>(store, context.Customer)
                .Set(x => x.MerchantOrderId, (orderRoot * 10000).ToString())
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
                    Create.Entity<JetOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Set(x => x.MerchantOrderID, (idRoot * 10000).ToString())
                        .Save();
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
        private OrderEntityBuilder<JetOrderEntity> AddItem(OrderEntityBuilder<JetOrderEntity> builder, int root, bool setOriginalOrderID) =>
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