
using System;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Volusion;
using ShipWorks.Stores.Platforms.Volusion.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Volusion
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly VolusionStoreEntity store;
        private Mock<IVolusionWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly VolusionOnlineUpdateCommandCreator commandCreator;

        public OnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IVolusionWebClient>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Volusion) as VolusionOnlineUpdateCommandCreator;

            store = Create.Store<VolusionStoreEntity>(StoreTypeCode.Volusion).Save();

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
            OrderEntity order = CreateNormalOrder(1, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 10, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123"), true));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true);
            OrderEntity order = CreateNormalOrder(1, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 20, It.IsAny<ShipmentEntity>(), AnyBool), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(store, 10, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123"), true));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 20, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123"), true));
            webClient.Verify(x => x.UploadShipmentDetails(store, 30, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123"), true));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 20, It.IsAny<ShipmentEntity>(), AnyBool), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(store, 30, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123"), true));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 10, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123"), true));
            webClient.Verify(x => x.UploadShipmentDetails(store, 50, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-456"), true));
            webClient.Verify(x => x.UploadShipmentDetails(store, 60, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-456"), true));
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false);

            webClient.Setup(x => x.UploadShipmentDetails(store, 10, It.IsAny<ShipmentEntity>(), AnyBool))
                .Throws(new VolusionException("Foo"));
            webClient.Setup(x => x.UploadShipmentDetails(store, 50, It.IsAny<ShipmentEntity>(), AnyBool))
                .Throws(new VolusionException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 60, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-456"), true));
            webClient.Verify(x => x.UploadShipmentDetails(store, 70, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-789"), true));
        }

        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual)
        {
            var order = Create.Order<OrderEntity>(store, context.Customer)
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

        private OrderEntity CreateCombinedOrder(int orderRoot, string trackingNumber, params Tuple<int, bool>[] combinedOrderDetails)
        {
            var order = Create.Order<OrderEntity>(store, context.Customer)
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