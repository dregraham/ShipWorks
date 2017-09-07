using System;
using System.Threading.Tasks;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.SparkPay;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.SparkPay
{
    public partial class SparkPayOnlineUpdateCommandCreatorTest : IDisposable
    {
        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store, 3);

            webClient.Verify(x => x.UpdateOrderStatus(store, 10, 3));
            webClient.Verify(x => x.UpdateOrderStatus(store, 10, 3));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store, 3);

            webClient.Verify(x => x.UpdateOrderStatus(store, 20, 3), Times.Never);
            webClient.Verify(x => x.UpdateOrderStatus(store, 10, 3));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store, 3);

            webClient.Verify(x => x.UpdateOrderStatus(store, 20, 3));
            webClient.Verify(x => x.UpdateOrderStatus(store, 30, 3));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store, 3);

            webClient.Verify(x => x.UpdateOrderStatus(store, 20, 3), Times.Never);
            webClient.Verify(x => x.UpdateOrderStatus(store, 30, 3));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store, 3);

            webClient.Verify(x => x.UpdateOrderStatus(store, 10, 3));
            webClient.Verify(x => x.UpdateOrderStatus(store, 50, 3));
            webClient.Verify(x => x.UpdateOrderStatus(store, 60, 3));
        }

        [Fact]
        public async Task SetOnlineStatus_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UpdateOrderStatus(store, 10, 3))
                .Throws(new SparkPayException("Foo"));
            webClient.Setup(x => x.UpdateOrderStatus(store, 50, 3))
                .Throws(new SparkPayException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnSetOnlineStatus(menuContext.Object, store, 3);

            webClient.Verify(x => x.UpdateOrderStatus(store, 60, 3));
            webClient.Verify(x => x.UpdateOrderStatus(store, 70, 3));
        }
    }
}