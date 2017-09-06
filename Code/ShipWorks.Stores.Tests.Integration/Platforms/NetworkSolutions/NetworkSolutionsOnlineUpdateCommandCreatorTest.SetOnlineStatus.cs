using System;
using System.Threading.Tasks;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.NetworkSolutions;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.NetworkSolutions
{
    public partial class NetworkSolutionsOnlineUpdateCommandCreatorTest
    {
        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 10000, ShipmentWithTrackingNumber("track-123")));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 20000, AnyShipment), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(store, 10000, ShipmentWithTrackingNumber("track-123")));
        }

        [Fact]
        public async Task UpdateShipment_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 20000, ShipmentWithTrackingNumber("track-123")));
            webClient.Verify(x => x.UploadShipmentDetails(store, 30000, ShipmentWithTrackingNumber("track-123")));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 20000, AnyShipment), Times.Never);
            webClient.Verify(x => x.UploadShipmentDetails(store, 30000, ShipmentWithTrackingNumber("track-123")));
        }

        [Fact]
        public async Task UpdateShipment_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 10000, ShipmentWithTrackingNumber("track-123")));
            webClient.Verify(x => x.UploadShipmentDetails(store, 50000, ShipmentWithTrackingNumber("track-456")));
            webClient.Verify(x => x.UploadShipmentDetails(store, 60000, ShipmentWithTrackingNumber("track-456")));
        }

        [Fact]
        public async Task UpdateShipment_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UploadShipmentDetails(store, 10000, AnyShipment))
                .Throws(new NetworkSolutionsException("Foo"));
            webClient.Setup(x => x.UploadShipmentDetails(store, 50000, AnyShipment))
                .Throws(new NetworkSolutionsException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUploadShipmentDetails(store, menuContext.Object);

            webClient.Verify(x => x.UploadShipmentDetails(store, 60000, ShipmentWithTrackingNumber("track-456")));
            webClient.Verify(x => x.UploadShipmentDetails(store, 70000, ShipmentWithTrackingNumber("track-789")));
        }
    }
}