using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Rakuten;
using ShipWorks.Stores.Platforms.Rakuten.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Rakuten
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper output;
        private readonly RakutenStoreEntity store;
        private Mock<IRakutenWebClient> webClient;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;
        private readonly RakutenOnlineUpdateCommandCreator commandCreator;
        private readonly DateTime processedDate = DateTime.Parse("2017-09-01 12:00:00");

        public OnlineUpdateCommandCreatorTest(DatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                webClient = mock.Override<IRakutenWebClient>();
                mock.Override<IMessageHelper>();
            });

            menuContext = context.Mock.Mock<IMenuCommandExecutionContext>();
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
            commandCreator = context.Mock.Container.ResolveKeyed<IOnlineUpdateCommandCreator>(StoreTypeCode.Rakuten) as RakutenOnlineUpdateCommandCreator;
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

            store = Create.Store<RakutenStoreEntity>(StoreTypeCode.Rakuten).Save();
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            ShipmentEntity shipment = CreateNormalShipment(1, "track-123", false);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { shipment.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.ConfirmShipping(store, shipment, It.IsAny<RakutenUploadDetails>()));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            ShipmentEntity manualShipment = CreateNormalShipment(2, "track-456", true);
            ShipmentEntity shipment = CreateNormalShipment(1, "track-123", false);


            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualShipment.OrderID, shipment.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.ConfirmShipping(new RakutenStoreEntity(), new ShipmentEntity(), new RakutenUploadDetails()), Times.Never);
            webClient.Verify(x => x.ConfirmShipping(store, shipment, It.IsAny<RakutenUploadDetails>()));
        }

        [Fact]
        public async Task UploadDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            ShipmentEntity shipment = CreateCombinedShipment(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { shipment.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.ConfirmShipping(store, shipment, It.IsAny<RakutenUploadDetails>()));
            webClient.Verify(x => x.ConfirmShipping(store, shipment, It.IsAny<RakutenUploadDetails>()));
        }

        [Fact]
        public async Task UploadDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            ShipmentEntity shipment = CreateCombinedShipment(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { shipment.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.ConfirmShipping(new RakutenStoreEntity(), new ShipmentEntity(), new RakutenUploadDetails()), Times.Never);
            webClient.Verify(x => x.ConfirmShipping(store, shipment, It.IsAny<RakutenUploadDetails>()));
        }

        [Fact]
        public async Task UploadDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            ShipmentEntity normalShipment = CreateNormalShipment(1, "track-123", false);
            ShipmentEntity combinedShipment = CreateCombinedShipment(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));


            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalShipment.OrderID, combinedShipment.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.ConfirmShipping(store, normalShipment, It.IsAny<RakutenUploadDetails>()));
            webClient.Verify(x => x.ConfirmShipping(store, combinedShipment, It.IsAny<RakutenUploadDetails>()));
            webClient.Verify(x => x.ConfirmShipping(store, combinedShipment, It.IsAny<RakutenUploadDetails>()));
        }

        [Fact]
        public async Task UploadDetails_ContinuesUploading_WhenFailuresOccur()
        {
            ShipmentEntity normalShipment = CreateNormalShipment(1, "track-123", false);
            ShipmentEntity combinedShipment = CreateCombinedShipment(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            ShipmentEntity normalShipment2 = CreateNormalShipment(7, "track-789", false);

            webClient.Setup(x => x.ConfirmShipping(It.IsAny<RakutenStoreEntity>(), It.IsAny<ShipmentEntity>(), It.IsAny<RakutenUploadDetails>()))
                .Throws(new WebException("foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalShipment.OrderID, combinedShipment.OrderID, normalShipment2.OrderID });

            await commandCreator.OnUploadDetails(menuContext.Object, store);

            webClient.Verify(x => x.ConfirmShipping(store, normalShipment, It.IsAny<RakutenUploadDetails>()));
            webClient.Verify(x => x.ConfirmShipping(store, combinedShipment, It.IsAny<RakutenUploadDetails>()));
        }

        private ShipmentEntity CreateNormalShipment(int orderRoot, string trackingNumber, bool manual)
        {
            RakutenOrderEntity order = Create.Order<RakutenOrderEntity>(store, context.Customer)
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.None)
                .Set(x => x.IsManual, manual)
                .Save();

            ShipmentEntity shipment = Create.Shipment(order)
                .AsOther(o =>
                {
                    o.Set(x => x.Carrier, "Foo")
                    .Set(x => x.Service, "Bar");
                })
                .Set(x => x.TrackingNumber, trackingNumber)
                .Set(x => x.Processed, true)
                .Set(x => x.ProcessedDate, processedDate)
                .Save();

            return shipment;
        }

        private ShipmentEntity CreateCombinedShipment(int orderRoot, string trackingNumber, params Tuple<int, bool>[] combinedOrderDetails)
        {
            RakutenOrderEntity order = Create.Order<RakutenOrderEntity>(store, context.Customer)
                .Set(x => x.OrderNumber, orderRoot * 10)
                .Set(x => x.CombineSplitStatus, CombineSplitStatusType.Combined)
                .Save();

            foreach (Tuple<int, bool> details in combinedOrderDetails)
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
                    Create.Entity<RakutenOrderSearchEntity>()
                        .Set(x => x.OrderID, order.OrderID)
                        .Set(x => x.OriginalOrderID, idRoot * -1006)
                        .Save();
                }
            }

            ShipmentEntity shipment = Create.Shipment(order)
                .AsOther(o =>
                {
                    o.Set(x => x.Carrier, "Foo")
                    .Set(x => x.Service, "Bar");
                })
                .Set(x => x.TrackingNumber, trackingNumber)
                .Set(x => x.Processed, true)
                .Set(x => x.ProcessedDate, processedDate)
                .Save();

            return shipment;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}