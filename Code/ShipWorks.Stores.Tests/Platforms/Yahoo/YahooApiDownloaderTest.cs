using System;
using System.Linq;
using Moq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class YahooApiDownloaderTest
    {
        private readonly YahooApiDownloader testObject;
        private readonly Mock<IYahooApiWebClient> webClient = new Mock<IYahooApiWebClient>();
        private readonly Mock<ISqlAdapterRetry> sqlAdapter = new Mock<ISqlAdapterRetry>();
        readonly YahooOrderEntity orderEntity;

        public YahooApiDownloaderTest()
        {
            string orderXml = EmbeddedResourceHelper.GetEmbeddedResourceXml("ShipWorks.Stores.Tests.Platforms.Yahoo.Artifacts.YahooGetOrderResponse.xml");
            YahooResponse orderResponse = YahooApiWebClient.DeserializeResponse<YahooResponse>(orderXml);

            string itemXml = EmbeddedResourceHelper.GetEmbeddedResourceXml("ShipWorks.Stores.Tests.Platforms.Yahoo.Artifacts.YahooGetItemResponse.xml");
            YahooResponse itemResponse = YahooApiWebClient.DeserializeResponse<YahooResponse>(itemXml);

            webClient.Setup(x => x.GetOrder(1001)).Returns(orderResponse);
            webClient.Setup(x => x.GetItem("hubbabubbagum")).Returns(itemResponse);

            sqlAdapter.Setup(retry => retry.ExecuteWithRetry(It.IsAny<Action>())).Callback((Action x) => x.Invoke());

            YahooStoreEntity storeEntity = new YahooStoreEntity
            {
                TypeCode = (int) StoreTypeCode.Yahoo,
                YahooStoreID = "yhst-12345"
            };

            testObject = new YahooApiDownloader(storeEntity, webClient.Object, sqlAdapter.Object);

            orderEntity = new YahooOrderEntity(1001)
            {
                YahooOrderID = "1001"
            };

            orderEntity = testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), orderEntity);
        }

        [Fact]
        public void LoadOrder_LoadsOrderDetails_WhenGivenYahooResponse()
        {
            Assert.Equal("Tracked", orderEntity.OnlineStatus);
        }

        [Fact]
        public void LoadOrder_LoadsShippingAddress_WhenGivenYahooResponse()
        {
            Assert.Equal("chris", orderEntity.ShipFirstName);
        }

        [Fact]
        public void LoadOrder_LoadsBillingAddress_WhenGivenYahooResponse()
        {
            Assert.Equal("hicks", orderEntity.BillLastName);
        }

        [Fact]
        public void LoadOrder_LoadsItems_WhenGivenYahooResponse()
        {
            Assert.Equal("hubbabubbagum", ((YahooOrderItemEntity)orderEntity.OrderItems.FirstOrDefault()).YahooProductID);
        }

        [Fact]
        public void LoadOrder_LoadsItemAttributes_WhenGivenYahooResponse()
        {
            Assert.Equal("Large", orderEntity.OrderItems.FirstOrDefault().OrderItemAttributes.FirstOrDefault().Description);
        }

        [Fact]
        public void LoadOrder_LoadsOrderTotals_WhenGivenYahooResponse()
        {
            Assert.Equal(.2m, orderEntity.OrderTotal);
        }

        [Fact]
        public void LoadOrder_LoadsOrderCharges_WhenGivenYahooResponse()
        {
            Assert.Equal(0,
                orderEntity.OrderCharges.Where(charge => charge.Description == "Shipping")
                    .Select(charge => charge.Amount).First());
        }

        [Fact]
        public void LoadOrder_LoadsGiftMessages_WhenGivenYahooResponse()
        {
            Assert.Equal("Enjoy", orderEntity.OrderItems.LastOrDefault().OrderItemAttributes.FirstOrDefault().Description);
        }

        [Fact]
        public void LoadOrder_LoadsPrivateNotes_WhenGivenYahooResponse()
        {
            EntityCollection<NoteEntity> notes = orderEntity.Notes;
            string note = notes.Where(x => x.Visibility == (int)NoteVisibility.Internal).Select(x => x.Text).FirstOrDefault();

            Assert.Equal("> 2007 Aug 22 03:16: This will be a private note", note);
        }

        [Fact]
        public void LoadOrder_LoadsPublicNotes_WhenGivenYahooResponse()
        {
            EntityCollection<NoteEntity> notes = orderEntity.Notes;
            string note = notes.Where(x => x.Visibility == (int)NoteVisibility.Public).Select(x => x.Text).FirstOrDefault();

            Assert.Equal("This will be a public note", note);
        }

        [Fact]
        public void LoadOrder_LoadsPaymentDetails_WhenGivenYahooResponse()
        {
            Assert.Equal("Purchase Order", orderEntity.OrderPaymentDetails.FirstOrDefault().Value);
        }

        [Fact]
        public void ParseYahooDateTime_ReturnsCorrectDateTime_WhenGivenValidYahooDateString()
        {
            DateTime expectedDateTime = new DateTime(2013, 9, 10, 9, 33, 14, DateTimeKind.Utc).ToUniversalTime();

            DateTime actualDateTime = testObject.ParseYahooDateTime("Tue Sep 10 09:33:14 2013 GMT");

            Assert.Equal(expectedDateTime, actualDateTime);
        }

        [Fact]
        public void ParseYahooDateTime_ThrowsYahooException_WhenGivenInvalidYahooDateString()
        {
            Assert.Throws<YahooException>(() => testObject.ParseYahooDateTime("This is clearly not a valid date string"));
        }
    }
}
