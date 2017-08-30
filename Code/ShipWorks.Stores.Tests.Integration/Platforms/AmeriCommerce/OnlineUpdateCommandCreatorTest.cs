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
using ShipWorks.Stores.Platforms.AmeriCommerce;
using ShipWorks.Stores.Platforms.AmeriCommerce.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.AmeriCommerce
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly AmeriCommerceStoreEntity store;
        private Mock<IAmeriCommerceWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly AmeriCommerceCommandCreator commandCreator;

        public OnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IAmeriCommerceWebClient>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.AmeriCommerce) as AmeriCommerceCommandCreator;

            store = Create.Store<AmeriCommerceStoreEntity>(StoreTypeCode.AmeriCommerce)
                .Set(x => x.StoreUrl, "http://www.example.com")
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer)
                .Save();

            Create.Shipment(dummyOrder)
                .AsOther(o => o.Set(x => x.Carrier, "Bad").Set(x => x.Service, "Bad"))
                .Set(x => x.TrackingNumber, "track-999")
                .Set(x => x.Processed, true)
                .Save();
        }

        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(99)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(10, 99));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(99)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(20, 99));
            webClient.Verify(x => x.UpdateOrderStatus(30, 99));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(99)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(10, 99));
            webClient.Verify(x => x.UpdateOrderStatus(50, 99));
            webClient.Verify(x => x.UpdateOrderStatus(60, 99));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(10L, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123")));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(20L, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123")));
            webClient.Verify(x => x.UploadShipmentDetails(30L, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123")));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(10L, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-123")));
            webClient.Verify(x => x.UploadShipmentDetails(50L, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-456")));
            webClient.Verify(x => x.UploadShipmentDetails(60L, It.Is<ShipmentEntity>(s => s.TrackingNumber == "track-456")));
        }

        private OrderEntity CreateNormalOrder(int orderRoot, int item1Root, int item2Root, string trackingNumber, bool manual)
        {
            var order = Create.Order(store, context.Customer)
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
            var order = Create.Order(store, context.Customer)
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
                    .Set(x => x.OriginalOrderID, idRoot * -1006)
                    .Set(x => x.OrderNumber, idRoot * 10)
                    .Set(x => x.OrderNumberComplete, (idRoot * 10).ToString())
                    .Set(x => x.IsManual, manual)
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