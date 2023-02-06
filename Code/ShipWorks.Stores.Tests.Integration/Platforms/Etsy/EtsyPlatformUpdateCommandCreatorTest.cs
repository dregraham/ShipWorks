using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Etsy;
using ShipWorks.Stores.Platforms.Etsy.OnlineUpdating;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Etsy
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class EtsyPlatformUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly EtsyStoreEntity store;
        private Mock<IEtsyWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly PlatformUpdateCommandCreator commandCreator;

        public EtsyPlatformUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IEtsyWebClient>();
                mock.Override<IMessageHelper>();
                mock.Override<IEtsyUserInteraction>()
                    .Setup(x => x.GetComment(It.IsAny<IWin32Window>()))
                    .Returns(GenericResult.FromSuccess("Foo"));
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Etsy) as PlatformUpdateCommandCreator;
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

            store = Create.Store<EtsyStoreEntity>(StoreTypeCode.Etsy)
                .Set(x => x.EtsyShopID, 999)
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
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(999, 10, "track-123", "other"));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNotCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(4, "track-456", true);
            OrderEntity order = CreateNormalOrder(1, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(AnyLong, 40, AnyString, AnyString), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(999, 10, "track-123", "other"));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(999, 20, "track-123", "other"));
            webClient.Verify(x => x.UploadShipmentDetails(999, 30, "track-123", "other"));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(AnyLong, 20, AnyString, AnyString), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(999, 30, "track-123", "other"));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(999, 10, "track-123", "other"));
            webClient.Verify(x => x.UploadShipmentDetails(999, 50, "track-456", "other"));
            webClient.Verify(x => x.UploadShipmentDetails(999, 60, "track-456", "other"));
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false);

            webClient.Setup(x => x.UploadShipmentDetails(AnyLong, 10, AnyString, AnyString))
                .Throws(new EtsyException("Foo"));
            webClient.Setup(x => x.UploadShipmentDetails(AnyLong, 50, AnyString, AnyString))
                .Throws(new EtsyException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(999, 60, "track-456", "other"));
            webClient.Verify(x => x.UploadShipmentDetails(999, 70, "track-789", "other"));
        }

        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual)
        {
            var order = Create.Order<EtsyOrderEntity>(store, context.Customer)
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
            var order = Create.Order<EtsyOrderEntity>(store, context.Customer)
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