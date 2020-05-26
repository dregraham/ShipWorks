﻿using System;
using System.Linq;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Ebay
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public partial class EbayOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly EbayStoreEntity store;
        private Mock<IEbayWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly EbayOnlineUpdateCommandCreator commandCreator;
        private Mock<IUserInteraction> userInteraction;

        public EbayOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IEbayWebClient>();
                userInteraction = mock.Override<IUserInteraction>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Ebay) as EbayOnlineUpdateCommandCreator;
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

            store = Create.Store<EbayStoreEntity>(StoreTypeCode.Ebay).Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer).Save();

            Create.Shipment(dummyOrder)
                .AsOther(o => o.Set(x => x.Carrier, "Bad").Set(x => x.Service, "Bad"))
                .Set(x => x.TrackingNumber, "track-999")
                .Set(x => x.Processed, true)
                .Save();
        }

        /// <summary>
        /// Create a normal, non-combined order
        /// </summary>
        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual, params int[] roots)
        {
            var order = roots.Aggregate(
                Create.Order<EbayOrderEntity>(store, context.Customer),
                (o, root) => AddItem(o, root, manual, false))
                .Set(x => x.EbayOrderID, orderRoot * 10000)
                .Set(x => x.EbayBuyerID, (orderRoot * 100000).ToString())
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
            var order = Create.Order<EbayOrderEntity>(store, context.Customer)
                .Set(x => x.EbayOrderID, orderRoot * 10000)
                .Set(x => x.EbayBuyerID, (orderRoot * 10000).ToString())
                .Set(x => x.SellingManagerRecord, orderRoot * 10000)
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
                    Create.Entity<EbayOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Set(x => x.EbayOrderID, idRoot * 10000)
                        .Set(x => x.EbayBuyerID, (idRoot * 100000).ToString())
                        .Set(x => x.SellingManagerRecord, orderRoot * 10000)
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
        private OrderEntityBuilder<EbayOrderEntity> AddItem(OrderEntityBuilder<EbayOrderEntity> builder, int root, bool isManual, bool setOriginalOrderID)
        {
            if (isManual)
            {
                return builder.WithItem<OrderItemEntity>(i => CreateItem(i, root, setOriginalOrderID));
            }

            return builder.WithItem<EbayOrderItemEntity>(i =>
                CreateItem(i, root, setOriginalOrderID)
                .Set(x => x.FeedbackLeftType, (int) EbayFeedbackType.None)
                .Set(x => x.EbayItemID, root * 100L)
                .Set(x => x.EbayTransactionID, root * 1000));
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