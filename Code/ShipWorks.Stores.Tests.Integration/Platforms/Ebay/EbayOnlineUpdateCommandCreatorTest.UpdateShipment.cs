using System;
using System.Threading.Tasks;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.OnlineUpdating;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Ebay
{
    public partial class EbayOnlineUpdateCommandCreatorTest
    {
        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 200, 2000, (bool?) null, true, "track-123", "Other"));
            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 300, 3000, (bool?) null, true, "track-123", "Other"));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 400, 4000, (bool?) null, true, It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 300, 3000, (bool?) null, true, "track-123", "Other"));
        }

        [Fact]
        public async Task UpdateShipment_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 200, 2000, (bool?) null, true, "track-123", "Other"));
            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 300, 3000, (bool?) null, true, "track-123", "Other"));
        }

        [Fact]
        public async Task UpdateShipment_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 200, 2000, (bool?) null, true, It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 300, 3000, (bool?) null, true, "track-123", "Other"));
        }

        [Fact]
        public async Task UpdateShipment_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 200, 2000, (bool?) null, true, "track-123", "Other"));
            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 300, 3000, (bool?) null, true, "track-123", "Other"));
            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 500, 5000, (bool?) null, true, "track-456", "Other"));
            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 600, 6000, (bool?) null, true, "track-456", "Other"));
        }

        [Fact]
        public async Task UpdateShipment_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x => x.CompleteSale(It.IsAny<EbayToken>(), 200, 2000, (bool?) null, true, It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new EbayException("Foo"));
            webClient.Setup(x => x.CompleteSale(It.IsAny<EbayToken>(), 300, 3000, (bool?) null, true, It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new EbayException("Foo"));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(EbayOnlineAction.Shipped)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnUpdateShipment(menuContext.Object);

            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 600, 6000, (bool?) null, true, "track-456", "Other"));
            webClient.Verify(x => x.CompleteSale(It.IsAny<EbayToken>(), 800, 8000, (bool?) null, true, "track-789", "Other"));
        }
    }
}