using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Ebay
{
    public class EbayOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly EbayOrderEntity order;

        public EbayOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new EbayOrderEntity();
        }

        [Fact]
        public void ToString_ReturnsEbayOrderID()
        {
            var testObject = new EbayOrderIdentifier(123, 456, 789);
            Assert.Equal("eBay:456 (123:789)", testObject.ToString());
        }

        [Theory]
        [InlineData("789",     "123", "456", "789", "123", "456")]
        [InlineData("123-456",     "123", "456", "456", "123", "456")]
        [InlineData("123-456-xxx", "123", "456", "456", "123", "456")]
        public void PopulatesFieldsCorrectly_WhenOrderType(
            string orderId, string itemId, string transactionId, 
            string expectedOrderId, string expectedItemId, string expectedTranId)
        {
            OrderType orderType = new OrderType()
            {
                OrderID = orderId,
                TransactionArray = new TransactionType[] {new TransactionType()
                {
                    TransactionID = transactionId,
                    Item = new ItemType() {ItemID = itemId}
                } }
            };

            var testObject = new EbayOrderIdentifier(orderType);

            Assert.Equal(expectedOrderId, testObject.EbayOrderID.ToString());
            Assert.Equal(expectedTranId, testObject.TransactionID.ToString());
            Assert.Equal(expectedItemId, testObject.EbayItemID.ToString());
        }

        [Theory]
        [InlineData("789", "123", "456", "789", "123", "456")]
        [InlineData("123-456", "123", "456", "456", "123", "456")]
        [InlineData("123-456-xxx", "123", "456", "456", "123", "456")]
        public void PopulatesFieldsCorrectly_WhenFeedbackDetailType(
            string orderId, string itemId, string transactionId,
            string expectedOrderId, string expectedItemId, string expectedTranId)
        {
            FeedbackDetailType feedbackDetailType = new FeedbackDetailType()
            {
                OrderLineItemID = orderId,
                TransactionID = transactionId,
                ItemID = itemId
            };

            var testObject = new EbayOrderIdentifier(feedbackDetailType);

            Assert.Equal(expectedOrderId, testObject.EbayOrderID.ToString());
            Assert.Equal(expectedTranId, testObject.TransactionID.ToString());
            Assert.Equal(expectedItemId, testObject.EbayItemID.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
