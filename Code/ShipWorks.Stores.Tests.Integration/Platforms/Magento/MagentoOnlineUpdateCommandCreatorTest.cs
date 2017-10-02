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
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Stores.Platforms.Magento.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Magento
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public partial class MagentoOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private MagentoStoreEntity store;
        private Mock<IMagentoTwoRestClient> webClientRest;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly MagentoOnlineUpdateCommandCreator commandCreator;
        private Mock<IUserInteraction> userInteraction;
        private Mock<IMagentoTwoWebClient> webClientModule;

        public MagentoOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClientRest = mock.Override<IMagentoTwoRestClient>();
                webClientModule = mock.Override<IMagentoTwoWebClient>();
                userInteraction = mock.Override<IUserInteraction>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Magento) as MagentoOnlineUpdateCommandCreator;

            store = Create.Store<MagentoStoreEntity>(StoreTypeCode.Magento)
                .Set(x => x.MagentoVersion, (int) MagentoVersion.MagentoTwoREST)
                .Save();

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
                Create.Order<MagentoOrderEntity>(store, context.Customer),
                (o, root) => AddItem(o, root, false))
                .Set(x => x.MagentoOrderID, orderRoot * 10000)
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

            SetupWebClientOrderRequest(orderRoot * 10000, roots);

            return order;
        }

        /// <summary>
        /// Create a combined order
        /// </summary>
        private OrderEntity CreateCombinedOrder(int orderRoot, string trackingNumber, params Tuple<int, bool>[] combinedOrderDetails)
        {
            var order = Create.Order<MagentoOrderEntity>(store, context.Customer)
                .Set(x => x.MagentoOrderID, orderRoot * 10000)
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
                    Create.Entity<MagentoOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Set(x => x.MagentoOrderID, idRoot * 10000)
                        .Save();

                    SetupWebClientOrderRequest(idRoot * 10000, idRoot);
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
        private OrderEntityBuilder<MagentoOrderEntity> AddItem(OrderEntityBuilder<MagentoOrderEntity> builder, int root, bool setOriginalOrderID)
        {
            return builder.WithItem<OrderItemEntity>(i => CreateItem(i, root, setOriginalOrderID));
        }

        /// <summary>
        /// Create an item
        /// </summary>
        private void CreateItem<T>(OrderItemEntityBuilder<T> builder, int root, bool setOriginalOrderID) where T : OrderItemEntity, new()
        {
            builder.Set(x => x.Quantity, root)
                .Set(x => x.Code, (root * 1000000).ToString());

            if (setOriginalOrderID)
            {
                builder.Set(x => x.OriginalOrderID, root * -1006);
            }
        }

        private void SetupWebClientOrderRequest(int magentoOrderID, params int[] roots)
        {
            var webOrder = new Order
            {
                Items = roots
                    .Select(x => new Item { ItemId = x * 10000, QtyOrdered = x })
                    .ToList()
            };

            webClientRest.Setup(x => x.GetOrder(magentoOrderID))
                .Returns(webOrder);
        }

        public void Dispose() => context.Dispose();
    }
}