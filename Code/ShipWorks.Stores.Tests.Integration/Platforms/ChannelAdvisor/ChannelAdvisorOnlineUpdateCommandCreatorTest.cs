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
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.ChannelAdvisor
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly ChannelAdvisorStoreEntity store;
        private Mock<IChannelAdvisorSoapClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly ChannelAdvisorOnlineUpdateCommandCreator commandCreator;
        private readonly DateTime processedDate = DateTime.Parse("2017-09-01 12:00:00");

        public OnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IChannelAdvisorSoapClient>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.ChannelAdvisor) as ChannelAdvisorOnlineUpdateCommandCreator;

            store = Create.Store<ChannelAdvisorStoreEntity>(StoreTypeCode.ChannelAdvisor).Save();

            // Create a dummy order that serves as a guarantee that we're not just fetching all orders later
            var dummyOrder = Create.Order(store, context.Customer).Save();

            Create.Shipment(dummyOrder)
                .AsOther(o => o.Set(x => x.Carrier, "Bad").Set(x => x.Service, "Bad"))
                .Set(x => x.TrackingNumber, "track-999")
                .Set(x => x.Processed, true)
                .Set(x => x.ProcessedDate, processedDate)
                .Save();
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(10, processedDate, "Foo", "Bar", "track-123"));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true);
            OrderEntity order = CreateNormalOrder(1, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(20, AnyDate, AnyString, AnyString, AnyString), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(10, processedDate, "Foo", "Bar", "track-123"));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(20, processedDate, "Foo", "Bar", "track-123"));
            webClient.Verify(x => x.UploadShipmentDetails(30, processedDate, "Foo", "Bar", "track-123"));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(20, AnyDate, AnyString, AnyString, AnyString), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(30, processedDate, "Foo", "Bar", "track-123"));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(10, processedDate, "Foo", "Bar", "track-123"));
            webClient.Verify(x => x.UploadShipmentDetails(50, processedDate, "Foo", "Bar", "track-456"));
            webClient.Verify(x => x.UploadShipmentDetails(60, processedDate, "Foo", "Bar", "track-456"));
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false);

            webClient.Setup(x => x.UploadShipmentDetails(10, AnyDate, AnyString, AnyString, AnyString))
                .Throws(new ChannelAdvisorException("foo"));
            webClient.Setup(x => x.UploadShipmentDetails(50, AnyDate, AnyString, AnyString, AnyString))
                .Throws(new ChannelAdvisorException("foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.UploadShipmentDetails(60, processedDate, "Foo", "Bar", "track-456"));
            webClient.Verify(x => x.UploadShipmentDetails(70, processedDate, "Foo", "Bar", "track-789"));
        }

        private OrderEntity CreateNormalOrder(int orderRoot, string trackingNumber, bool manual)
        {
            var order = Create.Order<ChannelAdvisorOrderEntity>(store, context.Customer)
                .Set(x => x.CustomOrderIdentifier, (orderRoot * 10000).ToString())
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
                .Set(x => x.ProcessedDate, processedDate)
                .Save();

            return order;
        }

        private OrderEntity CreateCombinedOrder(int orderRoot, string trackingNumber, params Tuple<int, bool>[] combinedOrderDetails)
        {
            var order = Create.Order<ChannelAdvisorOrderEntity>(store, context.Customer)
                .Set(x => x.CustomOrderIdentifier, (orderRoot * 10000).ToString())
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
                    Create.Entity<ChannelAdvisorOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Set(x => x.CustomOrderIdentifier, (idRoot * 10000).ToString())
                        .Save();
                }
            }

            Create.Shipment(order)
                .AsOther(o =>
                {
                    o.Set(x => x.Carrier, "Foo")
                    .Set(x => x.Service, "Bar");
                })
                .Set(x => x.TrackingNumber, trackingNumber)
                .Set(x => x.Processed, true)
                .Set(x => x.ProcessedDate, processedDate)
                .Save();

            return order;
        }

        public void Dispose() => context.Dispose();
    }
}