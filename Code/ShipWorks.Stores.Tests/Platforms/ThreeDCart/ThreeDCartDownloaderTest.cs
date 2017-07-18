using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ThreeDCart
{
    public class ThreeDCartDownloaderTest
    {
        private readonly ThreeDCartRestDownloader testObject;
        private IEnumerable<ThreeDCartOrder> orders;
        private readonly AutoMock mock;

        public ThreeDCartDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orders = DeserializeEmbeddedList<ThreeDCartOrder>("GetOrderResponseItemHasNoAttributes");
            var product = DeserializeEmbeddedList<ThreeDCartProduct>("GetProductResponse").FirstOrDefault();

            var webClient = mock.Mock<IThreeDCartRestWebClient>();
            webClient.Setup(x => x.GetOrders(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(() => orders);
            webClient.Setup(x => x.GetProduct(It.IsAny<int>())).Returns(product);

            mock.FromFactory<ISqlAdapterRetryFactory>()
                .Mock(x => x.Create<SqlException>(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Setup((r => r.ExecuteWithRetryAsync(It.IsAny<Func<Task>>()))).Callback((Func<Task> x) => x.Invoke());

            mock.Mock<IOrderManager>()
                .Setup(x => x.CalculateOrderTotal(It.IsAny<OrderEntity>()))
                .Returns((OrderEntity o) => OrderUtility.CalculateTotal(o));

            ThreeDCartStoreEntity store = new ThreeDCartStoreEntity
            {
                TypeCode = (int) StoreTypeCode.ThreeDCart,
                StoreTimeZone = TimeZoneInfo.Utc,
                StoreUrl = "http://www.shipworks.com"
            };

            testObject = mock.Create<ThreeDCartRestDownloader>(TypedParameter.From(store));
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderStatus()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());
            Assert.Equal("New", orderEntity.OnlineStatus);
        }

        [Fact]
        public async Task LoadOrder_LoadsLastModifiedDate()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.Equal(new DateTime(2016, 3, 16, 22, 16, 9, DateTimeKind.Utc), orderEntity.OnlineLastModified);
        }

        [Fact]
        public async Task LoadOrder_LoadsBillingAddress()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.Equal("ShipWorks", orderEntity.BillCompany);
        }

        [Fact]
        public async Task LoadOrder_LoadsShippingAddress()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.Equal("Chris", orderEntity.ShipFirstName);
        }

        [Fact]
        public async Task LoadOrder_LoadsRequestedShipping()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.Equal("Free Shipping", orderEntity.RequestedShipping);
        }

        [Fact]
        public async Task LoadOrder_LoadsNotes()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.Equal("Chris", orderEntity.ShipFirstName);
        }

        [Fact]
        public async Task LoadOrder_LoadsQuestionAsNotes()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.True(orderEntity.Notes.Any(x => x.Text == "Is it cool? : The coolest"));
        }

        [Fact]
        public async Task LoadOrder_LoadsItems()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.Equal(1, orderEntity.OrderItems.FirstOrDefault()?.Quantity);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemImages()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.Equal("http://www.shipworks.com/assets/images/default/cap_3dcart_2.jpg", orderEntity.OrderItems.FirstOrDefault()?.Image);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemName_WhenDescriptionIsOnlyName()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.Equal("Custom Cap 2", orderEntity.OrderItems.FirstOrDefault()?.Name);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemNameAndAttribute_WhenDescriptionIsNameAndOneAttribute()
        {
            orders = DeserializeEmbeddedList<ThreeDCartOrder>("GetOrderResponseItemHasOneAttribute");

            OrderEntity order = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            OrderItemAttributeEntity actualAttribute = order.OrderItems.FirstOrDefault()?.OrderItemAttributes?.FirstOrDefault();

            Assert.Equal("Custom Cap 2", order.OrderItems.FirstOrDefault()?.Name);

            Assert.Equal("CustCap: Size", actualAttribute?.Name);
            Assert.Equal("Extra: Small", actualAttribute?.Description);
            Assert.Equal(2, actualAttribute?.UnitPrice);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemNameAndAttribute_WhenDescriptionIsNameAndOneAttributeWithoutPrice()
        {
            orders = DeserializeEmbeddedList<ThreeDCartOrder>("GetOrderResponseItemHasOneAttributeWithoutPrice");

            OrderEntity order = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            OrderItemAttributeEntity actualAttribute = order.OrderItems.FirstOrDefault()?.OrderItemAttributes?.FirstOrDefault();

            Assert.Equal("Custom Cap 2", order.OrderItems.FirstOrDefault()?.Name);

            Assert.Equal("CustCap: Size", actualAttribute?.Name);
            Assert.Equal("Extra: Small", actualAttribute?.Description);
            Assert.Equal(0, actualAttribute?.UnitPrice);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemNameAndAttributes_WhenDescriptionIsNameAndMultipleAttributes()
        {
            orders = DeserializeEmbeddedList<ThreeDCartOrder>("GetOrderResponseItemHasTwoAttributes");

            OrderEntity order = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            OrderItemAttributeEntity actualAttribute1 = order.OrderItems.FirstOrDefault()?.OrderItemAttributes?.
                FirstOrDefault(a => a.UnitPrice == 2);
            OrderItemAttributeEntity actualAttribute2 = order.OrderItems.FirstOrDefault()?.OrderItemAttributes?.
                FirstOrDefault(a => a.UnitPrice == 3);

            Assert.Equal("Custom Cap 2", order.OrderItems.FirstOrDefault()?.Name);

            Assert.Equal("CustCap: Size", actualAttribute1?.Name);
            Assert.Equal("Extra: Small", actualAttribute1?.Description);
            Assert.Equal(2, actualAttribute1?.UnitPrice);

            Assert.Equal("CustCap: Color", actualAttribute2?.Name);
            Assert.Equal("CustCap: Blue", actualAttribute2?.Description);
            Assert.Equal(3, actualAttribute2?.UnitPrice);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderCharges()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.True(orderEntity.OrderCharges.Any(c => c.Description == "Tax" && c.Amount == 1.54M));
        }

        [Fact]
        public async Task LoadOrder_LoadsPaymentDetails()
        {
            var orderEntity = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            Assert.True(orderEntity.OrderPaymentDetails.Any(p => p.Label == "Payment Type" && p.Value == "Money Order"));
        }

        [Fact]
        public async Task LoadOrder_AddsKitAdjustments_WhenOrderContainsKitItems()
        {
            orders = DeserializeEmbeddedList<ThreeDCartOrder>("GetOrderResponseWithKitItem");

            OrderEntity order = await testObject.LoadOrder(new ThreeDCartOrderEntity(), orders.FirstOrDefault(),
                orders.FirstOrDefault()?.ShipmentList.FirstOrDefault());

            OrderChargeEntity actualCharge =
                order.OrderCharges.FirstOrDefault(c => c.Description == "Kit Adjustment");

            Assert.Equal("KIT ADJUSTMENT", actualCharge.Type);
            Assert.Equal("Kit Adjustment", actualCharge.Description);
            Assert.Equal(11M, actualCharge.Amount);
        }

        /// <summary>
        /// Deserialize an embedded json string into a list of objects
        /// </summary>
        private List<T> DeserializeEmbeddedList<T>(string name)
        {
            var json = GetType().Assembly.GetEmbeddedResourceString($"Platforms.ThreeDCart.Artifacts.{name}.json");
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}