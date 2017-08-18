using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class YahooApiDownloaderTest
    {
        private readonly YahooOrderEntity inputOrder;
        private readonly YahooResponse orderResponse;
        private readonly AutoMock mock;
        private readonly YahooStoreEntity store;

        public YahooApiDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            string orderXml = EmbeddedResourceHelper.GetEmbeddedResourceString("ShipWorks.Stores.Tests.Platforms.Yahoo.Artifacts.YahooGetOrderResponse.xml");
            orderResponse = YahooApiWebClient.DeserializeResponse<YahooResponse>(orderXml);

            string itemXml = EmbeddedResourceHelper.GetEmbeddedResourceString("ShipWorks.Stores.Tests.Platforms.Yahoo.Artifacts.YahooGetItemResponse.xml");
            YahooResponse itemResponse = YahooApiWebClient.DeserializeResponse<YahooResponse>(itemXml);

            var webClient = mock.Mock<IYahooApiWebClient>();
            webClient.Setup(x => x.GetOrder(1001)).Returns(orderResponse);
            webClient.Setup(x => x.GetItem("hubbabubbagum")).Returns(itemResponse);

            store = new YahooStoreEntity
            {
                TypeCode = (int) StoreTypeCode.Yahoo,
                YahooStoreID = "yhst-12345"
            };

            inputOrder = new YahooOrderEntity(1001)
            {
                YahooOrderID = "1001"
            };
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderDetails_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            Assert.Equal("Tracked", orderEntity.OnlineStatus);
        }

        [Fact]
        public async Task LoadOrder_LoadsShippingAddress_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            Assert.Equal("chris", orderEntity.ShipFirstName);
        }

        [Fact]
        public async Task LoadOrder_LoadsBillingAddress_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            Assert.Equal("hicks", orderEntity.BillLastName);
        }

        [Fact]
        public async Task LoadOrder_LoadsItems_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            Assert.Equal("hubbabubbagum", ((YahooOrderItemEntity) orderEntity.OrderItems.FirstOrDefault()).YahooProductID);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemAttributes_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            Assert.Equal("Large", orderEntity.OrderItems.FirstOrDefault().OrderItemAttributes.FirstOrDefault().Description);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderTotals_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            Assert.Equal(.2M, orderEntity.OrderTotal);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderCharges_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            Assert.Equal(0,
                orderEntity.OrderCharges.Where(charge => charge.Description == "Shipping")
                    .Select(charge => charge.Amount).First());
        }

        [Fact]
        public async Task LoadOrder_LoadsGiftMessages_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            Assert.Equal("Enjoy", orderEntity.OrderItems.LastOrDefault().OrderItemAttributes.FirstOrDefault().Description);
        }

        [Fact]
        public async Task LoadOrder_LoadsPrivateNotes_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            EntityCollection<NoteEntity> notes = orderEntity.Notes;
            string note = notes.Where(x => x.Visibility == (int) NoteVisibility.Internal).Select(x => x.Text).FirstOrDefault();

            Assert.Equal("> 2007 Aug 22 03:16: This will be a private note", note);
        }

        [Fact]
        public async Task LoadOrder_LoadsPublicNotes_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            EntityCollection<NoteEntity> notes = orderEntity.Notes;
            string note = notes.Where(x => x.Visibility == (int) NoteVisibility.Public).Select(x => x.Text).FirstOrDefault();

            Assert.Equal("This will be a public note", note);
        }

        [Fact]
        public async Task LoadOrder_LoadsPaymentDetails_WhenGivenYahooResponse()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            var orderEntity = await testObject.LoadOrder(orderResponse.ResponseResourceList.OrderList.Order.FirstOrDefault(), inputOrder);

            Assert.Equal("Purchase Order", orderEntity.OrderPaymentDetails.FirstOrDefault().Value);
        }

        [Fact]
        public void ParseYahooDateTime_ReturnsCorrectDateTime_WhenGivenValidYahooDateString()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            DateTime expectedDateTime = new DateTime(2013, 9, 10, 9, 33, 14, DateTimeKind.Utc).ToUniversalTime();

            DateTime actualDateTime = testObject.ParseYahooDateTime("Tue Sep 10 09:33:14 2013 GMT");

            Assert.Equal(expectedDateTime, actualDateTime);
        }

        [Fact]
        public void ParseYahooDateTime_ThrowsYahooException_WhenGivenInvalidYahooDateString()
        {
            var testObject = mock.Create<YahooApiDownloader>(TypedParameter.From(store));

            Assert.Throws<YahooException>(() => testObject.ParseYahooDateTime("This is clearly not a valid date string"));
        }
    }
}
