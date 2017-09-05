using System;
using System.Threading.Tasks;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Yahoo
{
    public partial class YahooOnlineUpdateCommandCreatorTest
    {
        private void SetupSetOnlineStatus()
        {
            store = Modify.Store<YahooStoreEntity>(store)
                .Set(x => x.YahooStoreID, "Foo")
                .Save();

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(m => m.SetupGet(x => x.Text).Returns("OnHold")));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            SetupSetOnlineStatus();
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UploadOrderStatus(store, "10000", "OnHold"));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            SetupSetOnlineStatus();
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UploadOrderStatus(store, "20000", AnyString), Times.Never);
            webClient.Verify(x => x.UploadOrderStatus(store, "10000", "OnHold"));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            SetupSetOnlineStatus();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UploadOrderStatus(store, "20000", "OnHold"));
            webClient.Verify(x => x.UploadOrderStatus(store, "30000", "OnHold"));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            SetupSetOnlineStatus();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UploadOrderStatus(store, "20000", AnyString), Times.Never);
            webClient.Verify(x => x.UploadOrderStatus(store, "30000", "OnHold"));
        }

        [Fact]
        public async Task SetOnlineStatus_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            SetupSetOnlineStatus();
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UploadOrderStatus(store, "10000", "OnHold"));
            webClient.Verify(x => x.UploadOrderStatus(store, "50000", "OnHold"));
            webClient.Verify(x => x.UploadOrderStatus(store, "60000", "OnHold"));
        }

        [Fact]
        public async Task SetOnlineStatus_ContinuesUploading_WhenFailuresOccur()
        {
            SetupSetOnlineStatus();
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.UploadOrderStatus(store, "10000", AnyString))
                .Throws(new YahooException("Foo"));
            webClient.Setup(x => x.UploadOrderStatus(store, "50000", AnyString))
                .Throws(new YahooException("Foo"));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnSetOnlineStatus(store, menuContext.Object);

            webClient.Verify(x => x.UploadOrderStatus(store, "60000", "OnHold"));
            webClient.Verify(x => x.UploadOrderStatus(store, "70000", "OnHold"));
        }
    }
}