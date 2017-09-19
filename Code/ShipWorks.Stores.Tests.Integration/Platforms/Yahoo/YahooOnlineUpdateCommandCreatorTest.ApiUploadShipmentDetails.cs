using System;
using System.Threading.Tasks;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Yahoo
{
    public partial class YahooOnlineUpdateCommandCreatorTest
    {
        private void SetupApiUploadShipmentDetails()
        {
            store = Modify.Store<YahooStoreEntity>(store)
                .Set(x => x.YahooStoreID, "Foo")
                .Save();
        }

        [Fact]
        public async Task ApiUploadShipmentDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            SetupApiUploadShipmentDetails();
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnApiUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, "10000", "track-123", string.Empty));
        }

        [Fact]
        public async Task ApiUploadShipmentDetails_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            SetupApiUploadShipmentDetails();
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnApiUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, "20000", AnyString, AnyString), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(store, "10000", "track-123", string.Empty));
        }

        [Fact]
        public async Task ApiUploadShipmentDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            SetupApiUploadShipmentDetails();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnApiUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, "20000", "track-123", string.Empty));
            webClient.Verify(x => x.UploadShipmentDetails(store, "30000", "track-123", string.Empty));
        }

        [Fact]
        public async Task ApiUploadShipmentDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            SetupApiUploadShipmentDetails();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnApiUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, "20000", AnyString, AnyString), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(store, "30000", "track-123", string.Empty));
        }

        [Fact]
        public async Task ApiUploadShipmentDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            SetupApiUploadShipmentDetails();
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnApiUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, "10000", "track-123", string.Empty));
            webClient.Verify(x => x.UploadShipmentDetails(store, "50000", "track-456", string.Empty));
            webClient.Verify(x => x.UploadShipmentDetails(store, "60000", "track-456", string.Empty));
        }

        [Fact]
        public async Task ApiUploadShipmentDetails_ContinuesUploading_WhenFailuresOccur()
        {
            SetupApiUploadShipmentDetails();
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UploadShipmentDetails(store, "10000", AnyString, AnyString))
                .Throws(new YahooException("Foo"));
            webClient.Setup(x => x.UploadShipmentDetails(store, "50000", AnyString, AnyString))
                .Throws(new YahooException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnApiUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, "60000", "track-456", string.Empty));
            webClient.Verify(x => x.UploadShipmentDetails(store, "70000", "track-789", string.Empty));
        }
    }
}