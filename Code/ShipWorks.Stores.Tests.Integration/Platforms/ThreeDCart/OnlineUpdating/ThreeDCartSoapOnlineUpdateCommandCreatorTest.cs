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
using ShipWorks.Stores.Platforms.ThreeDCart;
using ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating;
using ShipWorks.Stores.Platforms.ThreeDCart.Enums;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Stores.Tests.Integration.Platforms.ThreeDCart.OnlineUpdating
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "CombinedOrderUpdates")]
    public class ThreeDCartSoapOnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly ThreeDCartStoreEntity store;
        private Mock<IThreeDCartSoapWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly ThreeDCartOnlineUpdateCommandCreator commandCreator;
        private Mock<Func<ThreeDCartStoreEntity, IThreeDCartSoapWebClient>> webClientFactory;
        private readonly string statusNewText;
        private readonly string statusProcessingText;
        private readonly int statusProcessing;

        public ThreeDCartSoapOnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            statusNewText = EnumHelper.GetApiValue(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.New);
            statusProcessingText = EnumHelper.GetApiValue(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing);
            statusProcessing = (int) ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing;

            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IThreeDCartSoapWebClient>();
                webClient.Setup(wc => wc.UpdateOrderStatus(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>())).Returns(Result.FromSuccess());
                webClient.Setup(wc => wc.UploadOrderShipmentDetails(It.IsAny<ThreeDCartOnlineUpdatingOrderDetail>(), It.IsAny<long>(), It.IsAny<string>())).Returns(Result.FromSuccess());

                webClientFactory = mock.Override<Func<ThreeDCartStoreEntity, IThreeDCartSoapWebClient>>();
                webClientFactory.Setup(x => x(It.IsAny<ThreeDCartStoreEntity>())).Returns(webClient.Object);
                
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.ThreeDCart) as ThreeDCartOnlineUpdateCommandCreator;

            store = Create.Store<ThreeDCartStoreEntity>(StoreTypeCode.ThreeDCart)
                .Set(x => x.StoreUrl, "https://shipworks.3dcartstores.com")
                .Set(x => x.ApiUserKey, "12288569944166522122885699441665")
                .Set(x => x.TimeZoneID, "Central Standard Time")
                .Set(x => x.StatusCodes, @"<StatusCodes><StatusCode><Code>1</Code><Name>New and Fresh</Name></StatusCode></StatusCodes>")
                .Set(x => x.DownloadModifiedNumberOfDaysBack, 7)
                .Set(x => x.RestUser, false)
                .Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer)
                .WithItem<ThreeDCartOrderItemEntity>(i => i.Set(x => x.ThreeDCartShipmentID, 900L))
                .Set(o => o.OnlineStatus, statusNewText)
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

            ThreeDCartOrderEntity order = CreateNormalOrder(1, item1Root, item2Root, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                            .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(order.OrderNumber, order.OrderNumberComplete, statusProcessing), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesOneWebRequest_WhenOneOfTwoNotCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(4, 5, 6, "track-456", true);
            OrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                            .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(order.OrderNumber, order.OrderNumberComplete, statusProcessing), Times.Once);
        }
        
        [Fact]
        public async Task UpdateOrderStatus_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                            .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(2));
            webClient.Verify(x => x.UpdateOrderStatus(20, "20", statusProcessing), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(30, "30", statusProcessing), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                            .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(30, "30", statusProcessing), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatus_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            ThreeDCartOrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            ThreeDCartOrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                                .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(3));
            webClient.Verify(x => x.UpdateOrderStatus(10, "", statusProcessing), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(50, "50", statusProcessing), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(60, "60", statusProcessing), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            ThreeDCartOrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                            .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);
            
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.IsAny<ThreeDCartOnlineUpdatingOrderDetail>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od => IsMatchingShipmentUpload(od, order)), 200L, "track-123"), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNotCombinedOrdersIsManual()
        {
            ThreeDCartOrderEntity manualOrder = CreateNormalOrder(4, 5, 6, "track-456", true);
            ThreeDCartOrderEntity order = CreateNormalOrder(1, 2, 3, "track-123", false);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                                .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(It.IsAny<ThreeDCartOnlineUpdatingOrderDetail>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od => IsMatchingShipmentUpload(od, order)), 200L, "track-123"), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            ThreeDCartOrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                            .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(It.IsAny<ThreeDCartOnlineUpdatingOrderDetail>(), It.IsAny<long>(), It.IsAny<string>()), Times.Exactly(2));
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od => 
                                        IsMatchingShipmentUpload(od, 20, order.ThreeDCartOrderID, order.IsManual)), 
                                        2000L, 
                                        "track-123"), Times.Once);
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od => 
                                        IsMatchingShipmentUpload(od, 30, order.ThreeDCartOrderID, order.IsManual)), 
                                        3000L, 
                                        "track-123"), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            ThreeDCartOrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                                .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(It.IsAny<ThreeDCartOnlineUpdatingOrderDetail>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od =>
                                        IsMatchingShipmentUpload(od, 30, order.ThreeDCartOrderID, order.IsManual)),
                                        3000L,
                                        "track-123"), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            ThreeDCartOrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            ThreeDCartOrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                                .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(It.IsAny<ThreeDCartOnlineUpdatingOrderDetail>(), It.IsAny<long>(), It.IsAny<string>()), Times.Exactly(3));
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.IsAny<ThreeDCartOnlineUpdatingOrderDetail>(), It.IsAny<long>(), It.IsAny<string>()), Times.Exactly(3));
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od =>
                                        IsMatchingShipmentUpload(od, 10, normalOrder.ThreeDCartOrderID, normalOrder.IsManual)),
                                        200L,
                                        "track-123"), Times.Once);
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od =>
                                        IsMatchingShipmentUpload(od, 50, combinedOrder.ThreeDCartOrderID, combinedOrder.IsManual)),
                                        5000L,
                                        "track-456"), Times.Once);
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od =>
                                        IsMatchingShipmentUpload(od, 60, combinedOrder.ThreeDCartOrderID, combinedOrder.IsManual)),
                                        6000L,
                                        "track-456"), Times.Once);
        }

        [Fact]
        public async Task SetOnlineStatus_ContinuesUploading_WhenFailuresOccur()
        {
            ThreeDCartOrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            ThreeDCartOrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            ThreeDCartOrderEntity normalOrder2 = CreateNormalOrder(7, 8, 9, "track-123", false);

            webClient.Setup(x => x.UpdateOrderStatus(normalOrder.OrderNumber, normalOrder.OrderNumberComplete, statusProcessing)).Returns(Result.FromError(new ThreeDCartException()));
            webClient.Setup(x => x.UpdateOrderStatus(normalOrder2.OrderNumber, normalOrder2.OrderNumberComplete, statusProcessing)).Returns(Result.FromError(new ThreeDCartException()));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                                .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store);

            webClient.Verify(x => x.UpdateOrderStatus(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(4));
            webClient.Verify(x => x.UpdateOrderStatus(10, "", statusProcessing), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(50, "50", statusProcessing), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(60, "60", statusProcessing), Times.Once);
            webClient.Verify(x => x.UpdateOrderStatus(70, "", statusProcessing), Times.Once);
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            ThreeDCartOrderEntity normalOrder = CreateNormalOrder(1, 2, 3, "track-123", false);
            ThreeDCartOrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            ThreeDCartOrderEntity normalOrder2 = CreateNormalOrder(7, 8, 9, "track-123", false);

            webClient.Setup(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od => IsMatchingShipmentUpload(od, 10, normalOrder.ThreeDCartOrderID, normalOrder.IsManual)), 200, "track-123")).Returns(Result.FromError(new ThreeDCartException()));
            webClient.Setup(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od => IsMatchingShipmentUpload(od, 10, normalOrder.ThreeDCartOrderID, normalOrder.IsManual)), 5000, "track-123")).Returns(Result.FromError(new ThreeDCartException()));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag)
                                .Returns(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.Processing)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadOrderShipmentDetails(It.IsAny<ThreeDCartOnlineUpdatingOrderDetail>(), It.IsAny<long>(), It.IsAny<string>()), Times.Exactly(4));
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od =>
                                        IsMatchingShipmentUpload(od, 10, normalOrder.ThreeDCartOrderID, normalOrder.IsManual)),
                                        200L,
                                        "track-123"), Times.Once);
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od =>
                                        IsMatchingShipmentUpload(od, 50, combinedOrder.ThreeDCartOrderID, combinedOrder.IsManual)),
                                        5000L,
                                        "track-456"), Times.Once);
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od =>
                                        IsMatchingShipmentUpload(od, 60, combinedOrder.ThreeDCartOrderID, combinedOrder.IsManual)),
                                        6000L,
                                        "track-456"), Times.Once);
            webClient.Verify(x => x.UploadOrderShipmentDetails(It.Is<ThreeDCartOnlineUpdatingOrderDetail>(od =>
                                        IsMatchingShipmentUpload(od, 70, normalOrder2.ThreeDCartOrderID, combinedOrder.IsManual)),
                                        800L,
                                        "track-123"), Times.Once);
        }

        private bool IsMatchingShipmentUpload(ThreeDCartOnlineUpdatingOrderDetail orderDetail, ThreeDCartOrderEntity order)
        {
            return orderDetail.OrderNumber == order.OrderNumber && orderDetail.ThreeDCartOrderID == order.ThreeDCartOrderID && orderDetail.IsManual == order.IsManual;
        }
        private bool IsMatchingShipmentUpload(ThreeDCartOnlineUpdatingOrderDetail orderDetail, long orderNumber, long threeDCartOrderID, bool isManual)
        {
            return orderDetail.OrderNumber == orderNumber && orderDetail.ThreeDCartOrderID == threeDCartOrderID && orderDetail.IsManual == isManual;
        }

        private ThreeDCartOrderEntity CreateNormalOrder(int orderRoot, int item1Root, int item2Root, string trackingNumber, bool manual)
        {
            var order = Create.Order<ThreeDCartOrderEntity>(store, context.Customer)
                .WithItem<ThreeDCartOrderItemEntity>(i => i.Set(x => x.ThreeDCartShipmentID, item1Root * 100L)
                    .Set(x => x.Quantity, item1Root))
                .WithItem<ThreeDCartOrderItemEntity>(i => i.Set(x => x.ThreeDCartShipmentID, item2Root * 100L)
                    .Set(x => x.Quantity, item2Root))
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.None)
                .Set(x => x.IsManual, manual)
                .Set(x => x.ThreeDCartOrderID, orderRoot * 10000)
                .Set(o => o.OnlineStatus, EnumHelper.GetApiValue(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.New))
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

        private ThreeDCartOrderEntity CreateCombinedOrder(int orderRoot, string trackingNumber, params Tuple<int, bool>[] combinedOrderDetails)
        {
            var order = Create.Order<ThreeDCartOrderEntity>(store, context.Customer)
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
                .Set(o => o.OnlineStatus, EnumHelper.GetApiValue(ShipWorks.Stores.Platforms.ThreeDCart.Enums.ThreeDCartOrderStatus.New))
                .Set(x => x.ThreeDCartOrderID, orderRoot * 10000)
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

                Create.Entity<ThreeDCartOrderSearchEntity>()
                    .Set(x => x.OrderID, order.OrderID)
                    .Set(x => x.ThreeDCartOrderID, order.ThreeDCartOrderID)
                    .Set(x => x.OriginalOrderID, idRoot * -1006)
                    .Save();
            }

            combinedOrderDetails.Aggregate(
                Modify.Order(order),
                (o, d) => o.WithItem<ThreeDCartOrderItemEntity>(i => i
                    .Set(x => x.ThreeDCartShipmentID, d.Item1 * 100L)
                    .Set(x => x.OriginalOrderID, d.Item1 * -1006)
                    .Set(x => x.ThreeDCartShipmentID, d.Item1 * 1000)
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