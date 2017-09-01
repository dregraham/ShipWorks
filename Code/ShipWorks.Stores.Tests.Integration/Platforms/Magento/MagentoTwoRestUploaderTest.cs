using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Startup;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Stores.Platforms.Magento.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Magento
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Magento")]
    public class MagentoTwoRestUploaderTest : IDisposable
    {
        private readonly DataContext context;
        private readonly IProgressReporter progress;
        private readonly MagentoStoreEntity magentoStore;
        private Mock<Func<MagentoStoreEntity, IMagentoTwoRestClient>> webClientFactory;
        private Mock<ICombineOrderSearchProvider<MagentoOrderSearchEntity>> combineOrderSearchProvider;
        private Mock<IMagentoTwoRestClient> restClient;

        public MagentoTwoRestUploaderTest(DatabaseFixture db)
        {
            DateTime utcNow = new DateTime(2017, 4, 23, 12, 0, 0, DateTimeKind.Utc);

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IDateTimeProvider>().SetupGet(x => x.UtcNow).Returns(utcNow);
                mock.Override<ILogEntryFactory>();

                restClient = new Mock<IMagentoTwoRestClient>();

                webClientFactory = mock.CreateMock<Func<MagentoStoreEntity, IMagentoTwoRestClient>>();
                webClientFactory.Setup(f => f(It.IsAny<MagentoStoreEntity>()))
                    .Returns(restClient.Object);
                mock.Provide(webClientFactory.Object);
            });

            magentoStore = Create.Store<MagentoStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A Magento Test Store" + Guid.NewGuid().ToString("N"))
                .Set(x => x.StoreTypeCode = StoreTypeCode.Magento)
                .Set(x => x.ModuleUrl, "https://www.com")
                .Save();

            context.Mock.Provide<StoreEntity>(magentoStore);

            var statusCreator = Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID, magentoStore.StoreID)
                .Set(x => x.StatusText, "status")
                .Set(x => x.IsDefault, true);

            statusCreator.Set(x => x.StatusTarget, 0).Save();
            statusCreator.Set(x => x.StatusTarget, 1).Save();

            StatusPresetManager.CheckForChanges();

            progress = context.Mock.Mock<IProgressReporter>().Object;
            StatusPresetManager.CheckForChanges();
            LogSession.Initialize();
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesNotCallWebClientFactoryOrSearchProvider_WhenManualNonCombinedOrder()
        {
            combineOrderSearchProvider = context.Mock.CreateMock<ICombineOrderSearchProvider<MagentoOrderSearchEntity>>();
            context.Mock.Provide(combineOrderSearchProvider.Object);

            Mock<IIndex<StoreTypeCode, ICombineOrderSearchProvider<MagentoOrderSearchEntity>>> searchRepo =
                context.Mock.MockRepository.Create<IIndex<StoreTypeCode, ICombineOrderSearchProvider<MagentoOrderSearchEntity>>>();
            searchRepo.Setup(x => x[StoreTypeCode.Magento]).Returns(combineOrderSearchProvider.Object);
            context.Mock.Provide(searchRepo.Object);

            var testObject = context.Mock.Create<MagentoTwoRestOnlineUpdater>(TypedParameter.From(magentoStore));

            MagentoOrderEntity order = CreateOrder(1234, CombineSplitStatusType.None, true);

            foreach (var magentoCommand in EnumHelper.GetEnumList<MagentoUploadCommand>())
            {
                using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
                {
                    using (DbConnection con = SqlSession.Current.OpenConnection())
                    {

                        await testObject.UploadShipmentDetails(order.OrderID, MagentoUploadCommand.Complete, "", false).ConfigureAwait(false);
                    }
                }

                webClientFactory.Verify(wf => wf(magentoStore), Times.Never);
                combineOrderSearchProvider.Verify(c => c.GetOrderIdentifiers(order), Times.Never);
            }
        }

        [Fact]
        public async Task UploadShipmentDetails_CallsWebClientFactoryAndOrderSearchProvider_WhenNonManualNonCombinedOrder()
        {
            combineOrderSearchProvider = context.Mock.CreateMock<ICombineOrderSearchProvider<MagentoOrderSearchEntity>>();
            context.Mock.Provide(combineOrderSearchProvider.Object);

            Mock<IIndex<StoreTypeCode, ICombineOrderSearchProvider<MagentoOrderSearchEntity>>> searchRepo =
                context.Mock.MockRepository.Create<IIndex<StoreTypeCode, ICombineOrderSearchProvider<MagentoOrderSearchEntity>>>();
            searchRepo.Setup(x => x[StoreTypeCode.Magento]).Returns(combineOrderSearchProvider.Object);
            context.Mock.Provide(searchRepo.Object);

            var testObject = context.Mock.Create<MagentoTwoRestOnlineUpdater>(TypedParameter.From(magentoStore));

            MagentoOrderEntity order = CreateOrder(1234, CombineSplitStatusType.None, false);
            Order magentoClientOrder = CreateMagentoOrder(1234, order);
            restClient.Setup(c => c.GetOrder(order.MagentoOrderID)).Returns(magentoClientOrder);

            foreach (var magentoCommand in EnumHelper.GetEnumList<MagentoUploadCommand>())
            {
                using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
                {
                    using (DbConnection con = SqlSession.Current.OpenConnection())
                    {

                        await testObject.UploadShipmentDetails(order.OrderID, MagentoUploadCommand.Complete, "", false).ConfigureAwait(false);
                    }
                }

                webClientFactory.Verify(wf => wf(magentoStore), Times.Once);
                combineOrderSearchProvider.Verify(c => c.GetOrderIdentifiers(It.IsAny<IOrderEntity>()), Times.Once);

                webClientFactory.Reset();
                combineOrderSearchProvider.Reset();
            }
        }

        [Fact]
        public async Task UploadShipmentDetails_CallsWebClientCorrectNumberOfTimes_WhenNonManualCombinedOrder()
        {
            MagentoOrderEntity order = CreateOrder(1234, CombineSplitStatusType.Combined, false);
            Order magentoClientOrder = CreateMagentoOrder(1234, order);
            restClient.Setup(c => c.GetOrder(order.MagentoOrderID)).Returns(magentoClientOrder);

            MagentoOrderEntity secondOrder = CreateOrder(4567, CombineSplitStatusType.None, false);
            Order secondMagentoClientOrder = CreateMagentoOrder(4567, secondOrder);
            restClient.Setup(c => c.GetOrder(secondOrder.MagentoOrderID)).Returns(secondMagentoClientOrder);

            Create.Shipment(order)
                .AsOther()
                .Save();

            Create.Entity<MagentoOrderSearchEntity>()
                .Set(os => os.OrderID, order.OrderID)
                .Set(os => os.MagentoOrderID, secondOrder.MagentoOrderID)
                .Set(os => os.OriginalOrderID, secondOrder.OrderID)
                .Save();

            Create.Entity<MagentoOrderSearchEntity>()
                .Set(os => os.OrderID, order.OrderID)
                .Set(os => os.MagentoOrderID, order.MagentoOrderID)
                .Set(os => os.OriginalOrderID, order.OrderID)
                .Save();

            var testObject = context.Mock.Create<MagentoTwoRestOnlineUpdater>(TypedParameter.From(magentoStore));

            using (TrackedDurationEvent trackedDurationEvent = new TrackedDurationEvent("Store.Order.Download"))
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {

                    await testObject.UploadShipmentDetails(order.OrderID, MagentoUploadCommand.Complete, "", false).ConfigureAwait(false);
                }
            }

            webClientFactory.Verify(wf => wf(magentoStore), Times.Once);
            restClient.Verify(rc => rc.UploadShipmentDetails(It.IsAny<string>(), It.IsAny<string>(), order.MagentoOrderID), Times.Exactly(1));
            restClient.Verify(rc => rc.UploadShipmentDetails(It.IsAny<string>(), It.IsAny<string>(), secondOrder.MagentoOrderID), Times.Exactly(1));

            webClientFactory.Reset();
        }

        private MagentoOrderEntity CreateOrder(int orderNumber, CombineSplitStatusType combineSplitStatusType, bool isManual)
        {
            MagentoOrderEntity order = Create.Order<MagentoOrderEntity>(context.Store, context.Customer)
                .WithOrderNumber(orderNumber)
                .Set(x => x.MagentoOrderID, orderNumber)
                .Set(x => x.CombineSplitStatus, combineSplitStatusType)
                .Set(x => x.IsManual, isManual)
                .Save();

            Modify.Order(order)
                .WithItem(i => i.Set(x => x.Name, "Foo")
                    .Set(x => x.Code, "1")
                    .Set(x => x.Quantity, 2)
                    .Set(x => x.SKU, "Sku1"));

            return order;
        }

        private Order CreateMagentoOrder(int orderNumber, MagentoOrderEntity order)
        {
            Order magentoOrder = new Order()
            {
                EntityId = orderNumber,
                IncrementId = orderNumber.ToString(),
                Items =
                    order.OrderItems.Select(oi => new Item()
                    {
                        ItemId = int.Parse(oi.Code),
                        Name = oi.Name,
                        QtyOrdered = oi.Quantity,
                        Sku = oi.SKU,
                    })
            };

            return magentoOrder;
        }

        public void Dispose() => context.Dispose();
    }
}
