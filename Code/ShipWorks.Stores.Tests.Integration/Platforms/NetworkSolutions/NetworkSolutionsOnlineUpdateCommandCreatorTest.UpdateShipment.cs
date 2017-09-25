using System;
using System.Threading.Tasks;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.NetworkSolutions;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.NetworkSolutions
{
    public partial class NetworkSolutionsOnlineUpdateCommandCreatorTest
    {
        private void Setup_OnlineStatus()
        {
            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(1L)));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            Setup_OnlineStatus();
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UpdateOrderStatus(store, 10000, 3, 1, string.Empty));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            Setup_OnlineStatus();
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UpdateOrderStatus(store, 20000, 3, 1, AnyString), Times.Never);
            webClient.Verify(x => x.UpdateOrderStatus(store, 10000, 3, 1, string.Empty));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            Setup_OnlineStatus();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UpdateOrderStatus(store, 20000, 3, 1, string.Empty));
            webClient.Verify(x => x.UpdateOrderStatus(store, 30000, 3, 1, string.Empty));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            Setup_OnlineStatus();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UpdateOrderStatus(store, 20000, 3, 1, AnyString), Times.Never);
            webClient.Verify(x => x.UpdateOrderStatus(store, 30000, 3, 1, string.Empty));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            Setup_OnlineStatus();
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UpdateOrderStatus(store, 10000, 3, 1, string.Empty));
            webClient.Verify(x => x.UpdateOrderStatus(store, 50000, 3, 1, string.Empty));
            webClient.Verify(x => x.UpdateOrderStatus(store, 60000, 3, 1, string.Empty));
        }

        [Fact]
        public async Task SetOnlineStatus_ContinuesUploading_WhenFailuresOccur()
        {
            Setup_OnlineStatus();
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UpdateOrderStatus(store, 10000, 3, 1, AnyString))
                .Throws(new NetworkSolutionsException("Foo"));
            webClient.Setup(x => x.UpdateOrderStatus(store, 50000, 3, 1, AnyString))
                .Throws(new NetworkSolutionsException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UpdateOrderStatus(store, 60000, 3, 1, string.Empty));
            webClient.Verify(x => x.UpdateOrderStatus(store, 70000, 3, 1, string.Empty));
        }
    }
}