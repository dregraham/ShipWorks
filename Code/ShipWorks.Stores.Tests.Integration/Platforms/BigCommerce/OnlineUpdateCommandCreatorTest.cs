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
    public class OnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly BigCommerceStoreEntity store;
        private Mock<IBigCommerceWebClient> webClient;

        public OnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IBigCommerceWebClient>();
                mock.Override<IMessageHelper>();
            });

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
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123");

            var menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(99)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            var commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.BigCommerce) as CommandCreator;
            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(10, 99));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, 2, 3, "track-123");

            var menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(99)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            var commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.BigCommerce) as CommandCreator;
            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(20, 99));
            webClient.Verify(x => x.UpdateOrderStatus(30, 99));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123");
            OrderEntity combinedOrder = CreateCombinedOrder(4, 5, 6, "track-456");

            var menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(99)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            var commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.BigCommerce) as CommandCreator;
            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(10, 99));
            webClient.Verify(x => x.UpdateOrderStatus(50, 99));
            webClient.Verify(x => x.UpdateOrderStatus(60, 99));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123");

            var menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            var commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.BigCommerce) as CommandCreator;
            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(10L, 200L, "track-123", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a =>
                    a.Any(i => i.order_product_id == 2000 && i.quantity == 2) &&
                    a.Any(i => i.order_product_id == 3000 && i.quantity == 3))));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, 2, 3, "track-123");

            var menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            var commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.BigCommerce) as CommandCreator;
            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(20L, 200L, "track-123", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a => a.Any(i => i.order_product_id == 2000 && i.quantity == 2))));
            webClient.Verify(x => x.UploadOrderShipmentDetails(30L, 300L, "track-123", Tuple.Create(string.Empty, "Foo Bar"),
                It.Is<List<BigCommerceItem>>(a => a.Any(i => i.order_product_id == 3000 && i.quantity == 3))));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123");
            OrderEntity combinedOrder = CreateCombinedOrder(4, 5, 6, "track-456");

            var menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            var commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.BigCommerce) as CommandCreator;
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

        private OrderEntity CreateNormalOrder(int orderRoot, int item1Root, int item2Root, string trackingNumber)
        {
            var order = Create.Order(store, context.Customer)
                .WithItem<BigCommerceOrderItemEntity>(i => i.Set(x => x.OrderAddressID, item1Root * 100L)
                    .Set(x => x.OrderProductID, item1Root * 1000).Set(x => x.Quantity, item1Root))
                .WithItem<BigCommerceOrderItemEntity>(i => i.Set(x => x.OrderAddressID, item2Root * 100L)
                    .Set(x => x.OrderProductID, item2Root * 1000).Set(x => x.Quantity, item2Root))
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
            var order = Create.Order(store, context.Customer)
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

            Create.Entity<OrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.StoreID, store.StoreID)
                .Set(x => x.OriginalOrderID, item2Root * -1006)
                .Set(x => x.OrderNumber, item2Root * 10)
                .Set(x => x.OrderNumberComplete, (item2Root * 10).ToString())
                .Save();

            order = Modify.Order(order)
                .WithItem<BigCommerceOrderItemEntity>(i => i.Set(x => x.OrderAddressID, item1Root * 100L)
                    .Set(x => x.OriginalOrderID, item1Root * -1006).Set(x => x.OrderProductID, item1Root * 1000).Set(x => x.Quantity, item1Root))
                .WithItem<BigCommerceOrderItemEntity>(i => i.Set(x => x.OrderAddressID, item2Root * 100L)
                    .Set(x => x.OriginalOrderID, item2Root * -1006).Set(x => x.OrderProductID, item2Root * 1000).Set(x => x.Quantity, item2Root))
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