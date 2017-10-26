using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Ebay
{
    public partial class EbayOnlineUpdateCommandCreatorTest
    {
        [Fact]
        public async Task SendMessage_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            SetupMessagingDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSendMessage(menuContext.Object);

            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 300 && t.BuyerID == "100000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 300 && t.BuyerID == "100000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
        }

        [Fact]
        public async Task SendMessage_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            SetupMessagingDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnSendMessage(menuContext.Object);

            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 400 && t.BuyerID == "200000"), QuestionTypeCodeType.General, "Foo", "Bar", true),
                Times.Never);
            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 300 && t.BuyerID == "100000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
        }

        [Fact]
        public async Task SendMessage_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            SetupMessagingDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSendMessage(menuContext.Object);

            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 200 && t.BuyerID == "200000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 300 && t.BuyerID == "300000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
        }

        [Fact]
        public async Task SendMessage_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            SetupMessagingDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnSendMessage(menuContext.Object);

            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 200 && t.BuyerID == "200000"), QuestionTypeCodeType.General, "Foo", "Bar", true),
                Times.Never);
            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 300 && t.BuyerID == "300000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
        }

        [Fact]
        public async Task SendMessage_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            SetupMessagingDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnSendMessage(menuContext.Object);

            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 200 && t.BuyerID == "100000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 300 && t.BuyerID == "100000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 500 && t.BuyerID == "500000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 600 && t.BuyerID == "600000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
        }

        [Fact]
        public async Task SendMessage_ContinuesUploading_WhenFailuresOccur()
        {
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClient.Setup(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 200), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Throws(new EbayException("Foo"));
            webClient.Setup(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 500), It.IsAny<QuestionTypeCodeType>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Throws(new EbayException("Foo"));

            SetupMessagingDetails();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnSendMessage(menuContext.Object);

            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 600 && t.BuyerID == "600000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
            webClient.Verify(x =>
                x.SendMessage(It.Is<EbayTransactionDetails>(t => t.ItemID == 800 && t.BuyerID == "700000"), QuestionTypeCodeType.General, "Foo", "Bar", true));
        }

        private void SetupMessagingDetails()
        {
            var details = new EbayMessagingDetails
            {
                MessageType = EbaySendMessageType.General,
                Subject = "Foo",
                Message = "Bar",
                CopyMe = true
            };

            userInteraction.Setup(x => x.GetMessagingDetails(It.IsAny<IWin32Window>(), It.IsAny<IEnumerable<long>>()))
                .Returns(GenericResult.FromSuccess(details));
        }
    }
}