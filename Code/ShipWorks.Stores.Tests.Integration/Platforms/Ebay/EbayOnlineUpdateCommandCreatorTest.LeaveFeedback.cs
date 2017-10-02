using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Ebay
{
    public partial class EbayOnlineUpdateCommandCreatorTest
    {
        [Fact]
        public async Task LeaveFeedback_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            SetupFeedbackDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnLeaveFeedback(menuContext.Object);

            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 200, 2000, "100000", CommentTypeCodeType.Positive, "Foo"));
            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 300, 3000, "100000", CommentTypeCodeType.Positive, "Foo"));
        }

        [Fact]
        public async Task LeaveFeedback_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            SetupFeedbackDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnLeaveFeedback(menuContext.Object);

            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 400, 4000, "200000", CommentTypeCodeType.Positive, "Foo"),
                Times.Never);
            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 300, 3000, "100000", CommentTypeCodeType.Positive, "Foo"));
        }

        [Fact]
        public async Task LeaveFeedback_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            SetupFeedbackDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnLeaveFeedback(menuContext.Object);

            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 200, 2000, "200000", CommentTypeCodeType.Positive, "Foo"));
            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 300, 3000, "300000", CommentTypeCodeType.Positive, "Foo"));
        }

        [Fact]
        public async Task LeaveFeedback_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            SetupFeedbackDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnLeaveFeedback(menuContext.Object);

            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 200, 2000, "200000", CommentTypeCodeType.Positive, "Foo"),
                Times.Never);
            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 300, 3000, "300000", CommentTypeCodeType.Positive, "Foo"));
        }

        [Fact]
        public async Task LeaveFeedback_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            SetupFeedbackDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnLeaveFeedback(menuContext.Object);

            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 200, 2000, "100000", CommentTypeCodeType.Positive, "Foo"));
            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 300, 3000, "100000", CommentTypeCodeType.Positive, "Foo"));
            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 500, 5000, "500000", CommentTypeCodeType.Positive, "Foo"));
            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 600, 6000, "600000", CommentTypeCodeType.Positive, "Foo"));
        }

        [Fact]
        public async Task LeaveFeedback_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 200, It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>()))
                .Throws(new EbayException("Foo"));
            webClient.Setup(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 500, It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CommentTypeCodeType>(), It.IsAny<string>()))
                .Throws(new EbayException("Foo"));

            SetupFeedbackDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnLeaveFeedback(menuContext.Object);

            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 600, 6000, "600000", CommentTypeCodeType.Positive, "Foo"));
            webClient.Verify(x =>
                x.LeaveFeedback(It.IsAny<EbayToken>(), 800, 8000, "700000", CommentTypeCodeType.Positive, "Foo"));
        }

        private void SetupFeedbackDetails()
        {
            var details = new EbayFeedbackDetails
            {
                FeedbackType = CommentTypeCodeType.Positive,
                Comments = "Foo"
            };

            userInteraction.Setup(x => x.GetFeedbackDetails(It.IsAny<IWin32Window>(), It.IsAny<IEnumerable<long>>()))
                .Returns(GenericResult.FromSuccess(details));
        }
    }
}