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
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.BigCommerce
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class BigCommerceOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly BigCommerceStoreEntity store;
        private Mock<IBigCommerceWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly BigCommerceOnlineUpdateCommandCreator commandCreator;

        public BigCommerceOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IBigCommerceWebClient>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.BigCommerce) as BigCommerceOnlineUpdateCommandCreator;

            store = Create.Store<BigCommerceStoreEntity>(StoreTypeCode.BigCommerce)
                .Set(x => x.ApiUrl, "http://www.example.com")
                .Set(x => x.OauthClientId, "l7ksaksgrfgtvgh96hjjwleyf2uld75")
                .Set(x => x.OauthToken, "n4itbtdl0eiztunnr05qtosttfn5476")
                .Set(x => x.BigCommerceAuthentication, BigCommerceAuthenticationType.Oauth)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer)
                .WithItem<BigCommerceOrderItemEntity>(i => i.Set(x => x.OrderAddressID, 900L))
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
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOneOfTwoNotCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(4, 5, 6, "track-456", true);
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(99)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

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
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(99)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

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
        public async Task SetOnlineStatus_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, 8, 9, "track-789", false);

            webClient.Setup(x => x.UpdateOrderStatus(10, 99)).Throws<BigCommerceException>();
            webClient.Setup(x => x.UpdateOrderStatus(50, 99)).Throws<BigCommerceException>();

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(99)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(60, 99));
            webClient.Verify(x => x.UpdateOrderStatus(70, 99));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(10L, 200L, "track-123", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a =>
                    a.Any(i => i.order_product_id == 2000 && i.quantity == 2) &&
                    a.Any(i => i.order_product_id == 3000 && i.quantity == 3))));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNotCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(4, 5, 6, "track-456", true);
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(10L, 200L, "track-123", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a =>
                    a.Any(i => i.order_product_id == 2000 && i.quantity == 2) &&
                    a.Any(i => i.order_product_id == 3000 && i.quantity == 3))));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(20L, 200L, "track-123", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a => a.Any(i => i.order_product_id == 2000 && i.quantity == 2))));
            webClient.Verify(x => x.UploadOrderShipmentDetails(30L, 300L, "track-123", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a => a.Any(i => i.order_product_id == 3000 && i.quantity == 3))));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(30L, 300L, "track-123", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a => a.Any(i => i.order_product_id == 3000 && i.quantity == 3))));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(10L, 200L, "track-123", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a =>
                    a.Any(i => i.order_product_id == 2000 && i.quantity == 2) &&
                    a.Any(i => i.order_product_id == 3000 && i.quantity == 3))));
            webClient.Verify(x => x.UploadOrderShipmentDetails(50L, 500L, "track-456", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a => a.Any(i => i.order_product_id == 5000 && i.quantity == 5))));
            webClient.Verify(x => x.UploadOrderShipmentDetails(60L, 600L, "track-456", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a => a.Any(i => i.order_product_id == 6000 && i.quantity == 6))));
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, 8, 9, "track-789", false);

            webClient.Setup(x => x.UploadOrderShipmentDetails(10L, It.IsAny<long>(), It.IsAny<string>(),
                    It.IsAny<Tuple<string, string>>(), It.IsAny<List<BigCommerceItem>>()))
                .Throws<BigCommerceException>();
            webClient.Setup(x => x.UploadOrderShipmentDetails(50L, It.IsAny<long>(), It.IsAny<string>(),
                    It.IsAny<Tuple<string, string>>(), It.IsAny<List<BigCommerceItem>>()))
                .Throws<BigCommerceException>();

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(60L, 600L, "track-456", Tuple.Create(string.Empty, "Foo Bar"),
                It.IsAny<List<BigCommerceItem>>()));

            webClient.Verify(x => x.UploadOrderShipmentDetails(70L, 800L, "track-789", Tuple.Create(string.Empty, "Foo Bar"),
                It.IsAny<List<BigCommerceItem>>()));
        }

        private OrderEntity CreateNormalOrder(int orderRoot, int item1Root, int item2Root, string trackingNumber, bool manual)
        {
            var order = Create.Order(store, context.Customer)
                .WithItem<BigCommerceOrderItemEntity>(i => i.Set(x => x.OrderAddressID, item1Root * 100L)
                    .Set(x => x.OrderProductID, item1Root * 1000).Set(x => x.Quantity, item1Root))
                .WithItem<BigCommerceOrderItemEntity>(i => i.Set(x => x.OrderAddressID, item2Root * 100L)
                    .Set(x => x.OrderProductID, item2Root * 1000).Set(x => x.Quantity, item2Root))
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

            combinedOrderDetails.Aggregate(
                Modify.Order(order),
                (o, d) => o.WithItem<BigCommerceOrderItemEntity>(i => i
                    .Set(x => x.OrderAddressID, d.Item1 * 100L)
                    .Set(x => x.OriginalOrderID, d.Item1 * -1006)
                    .Set(x => x.OrderProductID, d.Item1 * 1000)
                    .Set(x => x.Quantity, d.Item1)))
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