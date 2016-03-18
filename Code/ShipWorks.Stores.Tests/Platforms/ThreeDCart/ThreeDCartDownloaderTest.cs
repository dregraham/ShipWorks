using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ThreeDCart
{
    public class ThreeDCartDownloaderTest
    {
        private readonly ThreeDCartDownloader testObject;
        private readonly Mock<IThreeDCartRestWebClient> webClient = new Mock<IThreeDCartRestWebClient>();
        private readonly Mock<ISqlAdapterRetry> sqlAdapter = new Mock<ISqlAdapterRetry>();
        readonly OrderEntity orderEntity;

        public ThreeDCartDownloaderTest()
        {
            string orderJson =
                EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetOrderResponse.json");
            List<ThreeDCartOrder> orders = JsonConvert.DeserializeObject<List<ThreeDCartOrder>>(orderJson);

            string productJson =
                EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetProductResponse.json");
            ThreeDCartProduct product = JsonConvert.DeserializeObject<ThreeDCartProduct>(productJson);

            webClient.Setup(x => x.GetOrders(It.IsAny<DateTime>())).Returns(orders);
            webClient.Setup(x => x.GetProduct(It.IsAny<int>())).Returns(product);

            sqlAdapter.Setup((r => r.ExecuteWithRetry(It.IsAny<Action>()))).Callback((Action x) => x.Invoke());

            ThreeDCartStoreEntity storeEntity = new ThreeDCartStoreEntity();
            storeEntity.TypeCode = (int) StoreTypeCode.ThreeDCart;

            testObject = new ThreeDCartDownloader(storeEntity, webClient.Object, sqlAdapter.Object);

            orderEntity = new OrderEntity();

            orderEntity = testObject.LoadOrder(orders.FirstOrDefault(), orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), "", false, false);
        }

        [Fact]
        public void LoadOrder_LoadsOrderDetails()
        {
            Assert.Equal(new DateTime(2016, 3, 16), orderEntity.OnlineLastModified);
        }

        [Fact]
        public void LoadOrder_LoadsBillingAddress()
        {
            Assert.Equal("ShipWorks", orderEntity.BillCompany);
        }

        [Fact]
        public void LoadOrder_LoadsShippingAddress()
        {
            Assert.Equal("Chris", orderEntity.ShipFirstName);
        }

        [Fact]
        public void LoadOrder_LoadsRequestedShipping()
        {
            Assert.Equal("Free Shipping", orderEntity.RequestedShipping);
        }

        [Fact]
        public void LoadOrder_LoadsNotes()
        {
            Assert.Equal("Chris", orderEntity.ShipFirstName);
        }

        [Fact]
        public void LoadOrder_LoadsQuestionAsNotes()
        {
            Assert.True(orderEntity.Notes.Any(x => x.Text == "Is it cool? : The coolest"));
        }

        [Fact]
        public void LoadOrder_LoadsItems()
        {
            Assert.Equal(1, orderEntity.OrderItems.FirstOrDefault()?.Quantity);
        }

        [Fact]
        public void LoadOrder_LoadsItemImages()
        {
            Assert.Equal("assets/images/default/cap_3dcart_2.jpg", orderEntity.OrderItems.FirstOrDefault()?.Image);
        }

        [Fact]
        public void LoadOrder_LoadsItemName_WhenDescriptionIsOnlyName()
        {

        }

        [Fact]
        public void LoadOrder_LoadsItemNameAndAttributes_WhenDescriptionIsNameAndOneAttribute()
        {
        }

        [Fact]
        public void LoadOrder_LoadsItemNameAndAttributes_WhenDescriptionIsNameAndMultipleAttributes()
        {
        }

        [Fact]
        public void LoadOrder_LoadsOrderCharges()
        {
            Assert.True(orderEntity.OrderCharges.Any(c => c.Description == "Tax" && c.Amount == 1.54m));
        }

        [Fact]
        public void LoadOrder_LoadsPaymentDetails()
        {
            Assert.True(orderEntity.OrderPaymentDetails.Any(p => p.Label == "Payment Type" && p.Value == "Money Order"));
        }

        [Fact]
        public void LoadOrder_AddsKitAdjustments_WhenOrderContainsKitItems()
        {

        }
    }
}