using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ThreeDCart
{
    [SuppressMessage("SonarLint", "S2259: value is null on at least one execution path",
        Justification = "Any instance where the value is null is a failed test, so it's ok here")]
    public class ThreeDCartDownloaderTest
    {
        private readonly ThreeDCartRestDownloader testObject;
        private readonly Mock<IThreeDCartRestWebClient> webClient = new Mock<IThreeDCartRestWebClient>();
        private readonly Mock<ISqlAdapterRetry> sqlAdapter = new Mock<ISqlAdapterRetry>();
        private readonly Mock<ILog> log = new Mock<ILog>();
        private readonly string orderJsonOneAttribute;
        private readonly string orderJsonTwoAttributes;
        private readonly string orderJsonWithKitItem;
        private readonly string orderJsonOneAttributeWithoutPrice;
        private List<ThreeDCartOrder> orders;

        public ThreeDCartDownloaderTest()
        {
            var orderJsonNoAttributes = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetOrderResponseItemHasNoAttributes.json");

            orderJsonOneAttribute =
                EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetOrderResponseItemHasOneAttribute.json");

            orderJsonOneAttributeWithoutPrice =
                EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.ThreeDCart.Artifacts.GetOrderResponseItemHasOneAttributeWithoutPrice.json");

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
                StoreTimeZone = TimeZoneInfo.Utc,
                StoreUrl = "http://www.shipworks.com"
            };

            testObject = new ThreeDCartRestDownloader(store, webClient.Object, sqlAdapter.Object, log.Object);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderStatus()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);
            Assert.Equal("New", orderEntity.OnlineStatus);
        }

        [Fact]
        public async Task LoadOrder_LoadsLastModifiedDate()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.Equal(new DateTime(2016, 3, 16, 22, 16, 9, DateTimeKind.Utc), orderEntity.OnlineLastModified);
        }

        [Fact]
        public async Task LoadOrder_LoadsBillingAddress()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.Equal("ShipWorks", orderEntity.BillCompany);
        }

        [Fact]
        public async Task LoadOrder_LoadsShippingAddress()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.Equal("Chris", orderEntity.ShipFirstName);
        }

        [Fact]
        public async Task LoadOrder_LoadsRequestedShipping()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.Equal("Free Shipping", orderEntity.RequestedShipping);
        }

        [Fact]
        public async Task LoadOrder_LoadsNotes()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.Equal("Chris", orderEntity.ShipFirstName);
        }

        [Fact]
        public async Task LoadOrder_LoadsQuestionAsNotes()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.True(orderEntity.Notes.Any(x => x.Text == "Is it cool? : The coolest"));
        }

        [Fact]
        public async Task LoadOrder_LoadsItems()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.Equal(1, orderEntity.OrderItems.FirstOrDefault()?.Quantity);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemImages()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.Equal("http://www.shipworks.com/assets/images/default/cap_3dcart_2.jpg", orderEntity.OrderItems.FirstOrDefault()?.Image);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemName_WhenDescriptionIsOnlyName()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.Equal("Custom Cap 2", orderEntity.OrderItems.FirstOrDefault()?.Name);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemNameAndAttribute_WhenDescriptionIsNameAndOneAttribute()
        {
            orders = JsonConvert.DeserializeObject<List<ThreeDCartOrder>>(orderJsonOneAttribute);

            OrderEntity order = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            OrderItemAttributeEntity actualAttribute = order.OrderItems.FirstOrDefault()?.OrderItemAttributes?.FirstOrDefault();

            Assert.Equal("Custom Cap 2", order.OrderItems.FirstOrDefault()?.Name);

            Assert.Equal("CustCap: Size", actualAttribute.Name);
            Assert.Equal("Extra: Small", actualAttribute.Description);
            Assert.Equal(2, actualAttribute.UnitPrice);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemNameAndAttribute_WhenDescriptionIsNameAndOneAttributeWithoutPrice()
        {
            orders = JsonConvert.DeserializeObject<List<ThreeDCartOrder>>(orderJsonOneAttributeWithoutPrice);

            OrderEntity order = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            OrderItemAttributeEntity actualAttribute = order.OrderItems.FirstOrDefault()?.OrderItemAttributes?.FirstOrDefault();

            Assert.Equal("Custom Cap 2", order.OrderItems.FirstOrDefault()?.Name);

            Assert.Equal("CustCap: Size", actualAttribute.Name);
            Assert.Equal("Extra: Small", actualAttribute.Description);
            Assert.Equal(0, actualAttribute.UnitPrice);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemNameAndAttributes_WhenDescriptionIsNameAndMultipleAttributes()
        {
            orders = JsonConvert.DeserializeObject<List<ThreeDCartOrder>>(orderJsonTwoAttributes);

            OrderEntity order = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
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
        public async Task LoadOrder_LoadsOrderCharges()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.True(orderEntity.OrderCharges.Any(c => c.Description == "Tax" && c.Amount == 1.54M));
        }

        [Fact]
        public async Task LoadOrder_LoadsPaymentDetails()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            Assert.True(orderEntity.OrderPaymentDetails.Any(p => p.Label == "Payment Type" && p.Value == "Money Order"));
        }

        [Fact]
        public async Task LoadOrder_AddsKitAdjustments_WhenOrderContainsKitItems()
        {
            orders = JsonConvert.DeserializeObject<List<ThreeDCartOrder>>(orderJsonWithKitItem);

            OrderEntity order = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault(), string.Empty);

            OrderChargeEntity actualCharge =
                order.OrderCharges.FirstOrDefault(c => c.Description == "Kit Adjustment");

            Assert.Equal("KIT ADJUSTMENT", actualCharge.Type);
            Assert.Equal("Kit Adjustment", actualCharge.Description);
            Assert.Equal(11M, actualCharge.Amount);
        }
    }
}