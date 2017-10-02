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
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using System.Diagnostics.CodeAnalysis;
using ShipWorks.Stores.Platforms.GenericModule.OnlineUpdating;
using ShipWorks.ApplicationCore.Logging;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.ClickCartPro
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombinedOrderUpdates")]
    public class ClickCartProOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly GenericModuleStoreEntity store;
        private Mock<IGenericStoreWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly GenericModuleOnlineUpdateCommandCreator commandCreator;
        private Mock<IGenericStoreWebClientFactory> webClientFactory;

        private static int NewStatus = 1;
        private static int ProcessingStatus = 2;

        public ClickCartProOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IGenericStoreWebClient>();
                webClient.Setup(wc => wc.UpdateOrderStatus(It.IsAny<OrderEntity>(), AnyString, AnyObject, AnyString)).Returns(Task.FromResult((IResult) Result.FromSuccess()));
                webClient.Setup(wc => wc.UploadShipmentDetails(It.IsAny<OrderEntity>(), AnyString, It.IsAny<ShipmentEntity>())).Returns(Task.FromResult((IResult) Result.FromSuccess()));

                webClientFactory = mock.Override<IGenericStoreWebClientFactory>();
                webClientFactory.Setup(wcf => wcf.CreateWebClient(It.IsAny<long>())).Returns(webClient.Object);

                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.GenericModule) as GenericModuleOnlineUpdateCommandCreator;

            store = Create.Store<GenericModuleStoreEntity>(StoreTypeCode.GenericModule)
                .Set(x => x.StoreTypeCode, StoreTypeCode.ClickCartPro)
                .Set(x => x.TypeCode, (int) StoreTypeCode.ClickCartPro)
                .Set(x => x.ModuleUrl, "https://shipworks.ClickCartPro.com")
                .Set(x => x.ModulePassword, "12288569944166522122885699441665")
                .Set(x => x.ModuleDownloadStrategy, (int) GenericStoreDownloadStrategy.ByModifiedTime)
                .Set(x => x.ModuleStatusCodes, $@"<StatusCodes><StatusCode><Code>{NewStatus}</Code><Name>NewStatus</Name></StatusCode><StatusCode><Code>{ProcessingStatus}</Code><Name>ProcessingStatus</Name></StatusCode></StatusCodes>")
                .Set(x => x.ModuleOnlineStatusSupport, (int) GenericOnlineStatusSupport.StatusWithComment)
                .Set(x => x.ModuleOnlineShipmentDetails, true)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer)
                .WithItem<OrderItemEntity>(i => i.Set(x => x.SKU, "900"))
                .Set(o => o.OnlineStatus, NewStatus.ToString())
                .Save();

            Create.Shipment(dummyOrder)
                .AsOther(o => o.Set(x => x.Carrier, "Bad").Set(x => x.Service, "Bad"))
                .Set(x => x.TrackingNumber, "track-999")
                .Set(x => x.Processed, true)
                .Save();
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            int item1Root = 2;
            int item2Root = 3;

            OrderEntity order = CreateNormalOrder(1, item1Root, item2Root, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), AnyString, ProcessingStatus, ""), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "10", ProcessingStatus, ""), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesZeroWebRequest_WhenOrderIsNotCombinedAndIsManual()
        {
            int item1Root = 2;
            int item2Root = 3;

            OrderEntity order = CreateNormalOrder(1, item1Root, item2Root, "track-123", true);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), AnyString, ProcessingStatus, ""), Times.Never);
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesOneWebRequest_WhenOneOfTwoNotCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(4, 5, 6, "track-456", true);
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), AnyString, ProcessingStatus, ""), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "10", ProcessingStatus, ""), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), AnyString, ProcessingStatus, ""), Times.Exactly(2));
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "20", ProcessingStatus, ""), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "30", ProcessingStatus, ""), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), AnyString, ProcessingStatus, ""), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "30", ProcessingStatus, ""), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), AnyString, ProcessingStatus, ""), Times.Exactly(3));
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "10", ProcessingStatus, ""), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "50", ProcessingStatus, ""), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "60", ProcessingStatus, ""), Times.Once);
        }

        [Fact]
        public async Task SetOnlineStatus_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, 8, 9, "track-123", false);

            webClient.Setup(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "10", ProcessingStatus, "")).Returns(Task.FromResult((IResult) Result.FromError(new GenericStoreException())));
            webClient.Setup(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "50", ProcessingStatus, "")).Returns(Task.FromResult((IResult) Result.FromError(new GenericStoreException())));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), AnyString, ProcessingStatus, ""), Times.Exactly(4));
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "10", ProcessingStatus, ""), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "50", ProcessingStatus, ""), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "60", ProcessingStatus, ""), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<OrderEntity>(), "70", ProcessingStatus, ""), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), AnyString, It.IsAny<ShipmentEntity>()), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "10", It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesZeroWebRequest_WhenOrderIsNotCombinedAndIsManual()
        {
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", true);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), AnyString, It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNotCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(4, 5, 6, "track-456", true);
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), AnyString, It.IsAny<ShipmentEntity>()), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "10", It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(ProcessingStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), AnyString, It.IsAny<ShipmentEntity>()), Times.Exactly(2));
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "20", It.IsAny<ShipmentEntity>()), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "30", It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(NewStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), AnyString, It.IsAny<ShipmentEntity>()), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "30", It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(NewStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), AnyString, It.IsAny<ShipmentEntity>()), Times.Exactly(3));
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "10", It.IsAny<ShipmentEntity>()), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "50", It.IsAny<ShipmentEntity>()), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "60", It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, 8, 9, "track-123", false);

            webClient.Setup(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "10", It.IsAny<ShipmentEntity>())).Returns(Task.FromResult((IResult) Result.FromError(new GenericStoreException())));
            webClient.Setup(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "50", It.IsAny<ShipmentEntity>())).Returns(Task.FromResult((IResult) Result.FromError(new GenericStoreException())));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(NewStatus)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), AnyString, It.IsAny<ShipmentEntity>()), Times.Exactly(4));
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "10", It.IsAny<ShipmentEntity>()), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "50", It.IsAny<ShipmentEntity>()), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "60", It.IsAny<ShipmentEntity>()), Times.Once);
            webClient.Verify(x => x.UploadShipmentDetails(It.IsAny<OrderEntity>(), "70", It.IsAny<ShipmentEntity>()), Times.Once);
        }

        private OrderEntity CreateNormalOrder(int orderRoot, int item1Root, int item2Root, string trackingNumber, bool manual)
        {
            long orderNumber = orderRoot * 10;

            var order = Create.Order<ClickCartProOrderEntity>(store, context.Customer)
                .WithItem<OrderItemEntity>(i => i.Set(x => x.SKU, (item1Root * 100L).ToString())
                    .Set(x => x.Quantity, item1Root))
                .WithItem<OrderItemEntity>(i => i.Set(x => x.SKU, (item2Root * 100L).ToString())
                    .Set(x => x.Quantity, item2Root))
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.None)
                .Set(x => x.IsManual, manual)
                .Set(o => o.OnlineStatus, NewStatus.ToString())
                .Set(x => x.ClickCartProOrderID, orderNumber.ToString())
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
            long orderNumber = orderRoot * 10;

            var order = Create.Order<ClickCartProOrderEntity>(store, context.Customer)
                .Set(x => x.OrderNumber, orderNumber)
                .Set(x => x.OrderNumberComplete, orderNumber.ToString())
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
                .Set(x => x.ClickCartProOrderID, orderNumber.ToString())
                .Set(o => o.OnlineStatus, NewStatus.ToString())
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
                    Create.Entity<ClickCartProOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.ClickCartProOrderID, (idRoot * 10).ToString())
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Save();
                }
            }

            combinedOrderDetails.Aggregate(
                Modify.Order(order),
                (o, d) => o.WithItem<OrderItemEntity>(i => i
                    .Set(x => x.SKU, (d.Item1 * 100L).ToString())
                    .Set(x => x.OriginalOrderID, d.Item1 * -1006)
                    .Set(x => x.SKU, (d.Item1 * 1000).ToString())
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