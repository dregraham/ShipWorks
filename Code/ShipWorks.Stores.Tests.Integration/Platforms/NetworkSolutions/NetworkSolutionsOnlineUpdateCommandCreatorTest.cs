using System;
using System.Linq;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.NetworkSolutions;
using ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.NetworkSolutions
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public partial class NetworkSolutionsOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly NetworkSolutionsStoreEntity store;
        private Mock<INetworkSolutionsWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly NetworkSolutionsOnlineUpdateCommandCreator commandCreator;
        private Mock<INetworkSolutionsUserInteraction> userInteraction;

        public NetworkSolutionsOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<INetworkSolutionsWebClient>();
                userInteraction = mock.Override<INetworkSolutionsUserInteraction>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.NetworkSolutions) as NetworkSolutionsOnlineUpdateCommandCreator;

            store = Create.Store<NetworkSolutionsStoreEntity>(StoreTypeCode.NetworkSolutions).Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer).Save();

            Create.Shipment(dummyOrder)
                .AsOther(o => o.Set(x => x.Carrier, "Bad").Set(x => x.Service, "Bad"))
                .Set(x => x.TrackingNumber, "track-999")
                .Set(x => x.Processed, true)
                .Save();
        }

        /// <summary>
        /// Checks a parameter for a shipment with given tracking number
        /// </summary>
        private ShipmentEntity AnyShipment => It.IsAny<ShipmentEntity>();

        /// <summary>
        /// Checks a parameter for a shipment with given tracking number
        /// </summary>
        private ShipmentEntity ShipmentWithTrackingNumber(string trackingNumber) =>
            It.Is<ShipmentEntity>(s => s.TrackingNumber == trackingNumber);

        /// <summary>
        /// Create a normal, non-combined order
        /// </summary>
        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual, params int[] roots)
        {
            var order = roots.Aggregate(
                Create.Order<NetworkSolutionsOrderEntity>(store, context.Customer),
                (o, root) => AddItem(o, root, false))
                .Set(x => x.OnlineStatusCode, 3L)
                .Set(x => x.NetworkSolutionsOrderID, orderRoot * 10000)
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
            var order = Create.Order<NetworkSolutionsOrderEntity>(store, context.Customer)
                .Set(x => x.OnlineStatusCode, 3L)
                .Set(x => x.NetworkSolutionsOrderID, orderRoot * 10000)
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
                    Create.Entity<NetworkSolutionsOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Set(x => x.NetworkSolutionsOrderID, idRoot * 10000)
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
        private OrderEntityBuilder<NetworkSolutionsOrderEntity> AddItem(OrderEntityBuilder<NetworkSolutionsOrderEntity> builder, int root, bool setOriginalOrderID) =>
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