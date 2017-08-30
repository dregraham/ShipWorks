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
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores.Platforms.Amazon.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Amazon
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly AmazonStoreEntity store;
        private Mock<IAmazonMwsClient> webClient;

        public OnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IAmazonMwsClient>();
                mock.Override<IMessageHelper>();
            });

            store = Create.Store<AmazonStoreEntity>(StoreTypeCode.Amazon)
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
            OrderEntity order = CreateNormalOrder(1, "track-123");

            var menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            var commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Amazon) as AmazonOnlineUpdateCommandCreator;
            await commandCreator.OnUploadDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(It.Is<List<AmazonOrderUploadDetail>>(o =>
                o.Any(i => i.AmazonOrderID == "10000" && i.Shipment.TrackingNumber == "track-123"))));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, 2, 3, "track-123");

            var menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            var commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Amazon) as AmazonOnlineUpdateCommandCreator;
            await commandCreator.OnUploadDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(It.Is<List<AmazonOrderUploadDetail>>(o =>
                o.Any(i => i.AmazonOrderID == "20000" && i.Shipment.TrackingNumber == "track-123") &&
                o.Any(i => i.AmazonOrderID == "30000" && i.Shipment.TrackingNumber == "track-123"))));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123");
            OrderEntity combinedOrder = CreateCombinedOrder(4, 5, 6, "track-456");

            var menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            var commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Amazon) as AmazonOnlineUpdateCommandCreator;
            await commandCreator.OnUploadDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(It.Is<List<AmazonOrderUploadDetail>>(o =>
                o.Any(i => i.AmazonOrderID == "10000" && i.Shipment.TrackingNumber == "track-123") &&
                o.Any(i => i.AmazonOrderID == "50000" && i.Shipment.TrackingNumber == "track-456") &&
                o.Any(i => i.AmazonOrderID == "60000" && i.Shipment.TrackingNumber == "track-456"))));
        }

        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber)
        {
            var order = Create.Order<AmazonOrderEntity>(store, context.Customer)
                .Set(x => x.AmazonOrderID, (orderRoot * 10000).ToString())
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.None)
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

        private OrderEntity CreateCombinedOrder(int orderRoot, int item1Root, int item2Root, string trackingNumber)
        {
            var order = Create.Order<AmazonOrderEntity>(store, context.Customer)
                .Set(x => x.AmazonOrderID, (orderRoot * 10000).ToString())
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
                .Save();

            Create.Entity<OrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.StoreID, store.StoreID)
                .Set(x => x.OriginalOrderID, item1Root * -1006)
                .Set(x => x.OrderNumber, item1Root * 10)
                .Set(x => x.OrderNumberComplete, (item1Root * 10).ToString())
                .Save();

            Create.Entity<AmazonOrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.OriginalOrderID, item1Root * -1006)
                .Set(x => x.AmazonOrderID, (item1Root * 10000).ToString())
                .Save();

            Create.Entity<OrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.StoreID, store.StoreID)
                .Set(x => x.OriginalOrderID, item2Root * -1006)
                .Set(x => x.OrderNumber, item2Root * 10)
                .Set(x => x.OrderNumberComplete, (item2Root * 10).ToString())
                .Save();

            Create.Entity<AmazonOrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.OriginalOrderID, item2Root * -1006)
                .Set(x => x.AmazonOrderID, (item2Root * 10000).ToString())
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