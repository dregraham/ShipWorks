using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
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
        private readonly ThreeDCartRestDownloader testObject;
        private readonly Mock<IThreeDCartRestWebClient> webClient = new Mock<IThreeDCartRestWebClient>();
        private readonly Mock<ISqlAdapterRetry> sqlAdapter = new Mock<ISqlAdapterRetry>();
        private readonly Mock<ILog> log = new Mock<ILog>();
        readonly OrderEntity orderEntity;
        private readonly string orderJsonNoAttributes;
        private readonly string orderJsonOneAttribute;
        private readonly string orderJsonTwoAttributes;
        private readonly string orderJsonWithKitItem;
        private List<ThreeDCartOrder> orders;

        public ThreeDCartDownloaderTest()
        {
            orderJsonNoAttributes =
                EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetOrderResponseItemHasNoAttributes.json");

            orderJsonOneAttribute =
                EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetOrderResponseItemHasOneAttribute.json");

            orderJsonTwoAttributes =
                EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetOrderResponseItemHasTwoAttributes.json");

            orderJsonWithKitItem =
                EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetOrderResponseWithKitItem.json");

            orders = JsonConvert.DeserializeObject<List<ThreeDCartOrder>>(orderJsonNoAttributes);

            string productJson =
                EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetProductResponse.json");

            ThreeDCartProduct product =
                JsonConvert.DeserializeObject<List<ThreeDCartProduct>>(productJson).FirstOrDefault();

            webClient.Setup(x => x.GetOrders(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(orders);
            webClient.Setup(x => x.GetProduct(It.IsAny<int>())).Returns(product);

            sqlAdapter.Setup((r => r.ExecuteWithRetry(It.IsAny<Action>()))).Callback((Action x) => x.Invoke());
            ThreeDCartStoreEntity store = new ThreeDCartStoreEntity
            {
                TypeCode = (int) StoreTypeCode.ThreeDCart,
                StoreTimeZone = TimeZoneInfo.Utc
            };

            testObject = new ThreeDCartRestDownloader(store, webClient.Object, sqlAdapter.Object, log.Object);

            orderEntity = testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);
        }

        [Fact]
        public void LoadOrder_LoadsOrderStatus()
        {
            Assert.Equal("New", orderEntity.OnlineStatus);
        }

        [Fact]
        public void LoadOrder_LoadsLastModifiedDate()
        {
            Assert.Equal(new DateTime(2016, 3, 16, 22, 16, 9, DateTimeKind.Utc), orderEntity.OnlineLastModified);
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
            Assert.Equal("Custom Cap 2", orderEntity.OrderItems.FirstOrDefault()?.Name);
        }

        [Fact]
        public void LoadOrder_LoadsItemNameAndAttribute_WhenDescriptionIsNameAndOneAttribute()
        {
            List<ThreeDCartOrder> orders = JsonConvert.DeserializeObject<List<ThreeDCartOrder>>(orderJsonOneAttribute);

            OrderEntity order = testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            OrderItemAttributeEntity actualAttribute = order.OrderItems.FirstOrDefault()?.OrderItemAttributes?.FirstOrDefault();

            Assert.Equal("Custom Cap 2", order.OrderItems.FirstOrDefault()?.Name);

            Assert.Equal("CustCap: Size", actualAttribute.Name);
            Assert.Equal("Extra: Small", actualAttribute.Description);
            Assert.Equal(2, actualAttribute.UnitPrice);
        }

        [Fact]
        public void LoadOrder_LoadsItemNameAndAttributes_WhenDescriptionIsNameAndMultipleAttributes()
        {
            List<ThreeDCartOrder> orders = JsonConvert.DeserializeObject<List<ThreeDCartOrder>>(orderJsonTwoAttributes);

            OrderEntity order = testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            OrderItemAttributeEntity actualAttribute1 = order.OrderItems.FirstOrDefault()?.OrderItemAttributes?.
                FirstOrDefault(a => a.UnitPrice == 2);
            OrderItemAttributeEntity actualAttribute2 = order.OrderItems.FirstOrDefault()?.OrderItemAttributes?.
                FirstOrDefault(a => a.UnitPrice == 3);

            Assert.Equal("Custom Cap 2", order.OrderItems.FirstOrDefault()?.Name);

            Assert.Equal("CustCap: Size", actualAttribute1.Name);
            Assert.Equal("Extra: Small", actualAttribute1.Description);
            Assert.Equal(2, actualAttribute1.UnitPrice);

            Assert.Equal("CustCap: Color", actualAttribute2.Name);
            Assert.Equal("CustCap: Blue", actualAttribute2.Description);
            Assert.Equal(3, actualAttribute2.UnitPrice);
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
            List<ThreeDCartOrder> orders = JsonConvert.DeserializeObject<List<ThreeDCartOrder>>(orderJsonWithKitItem);

            OrderEntity order = testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            OrderChargeEntity actualCharge =
                order.OrderCharges.FirstOrDefault(c => c.Description == "Kit Adjustment");

            Assert.Equal("KIT ADJUSTMENT", actualCharge.Type);
            Assert.Equal("Kit Adjustment", actualCharge.Description);
            Assert.Equal(11m, actualCharge.Amount);
        }
    }
}