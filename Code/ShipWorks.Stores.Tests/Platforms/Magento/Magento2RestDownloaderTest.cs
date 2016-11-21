using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class Magento2RestDownloaderTest
    {
        private readonly Mock<IMagentoTwoRestClient> webClient;
        private readonly Mock<ISqlAdapterRetry> sqlAdapter;

        private readonly string magentoOrder;
        private readonly Order order;
        private readonly Magento2RestDownloader testObject;
        private readonly MagentoOrderEntity orderEntity;

        public Magento2RestDownloaderTest()
        {
             magentoOrder = EmbeddedResourceHelper.GetEmbeddedResourceString(
                    "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.MagentoOrder.json");

            order = JsonConvert.DeserializeObject<Order>(magentoOrder);
            var response = new OrdersResponse();
            response.Orders = new List<Order>() {order};
            webClient = new Mock<IMagentoTwoRestClient>();
            webClient.Setup(w => w.GetToken(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<string>())).Returns("token");
            webClient.Setup(w => w.GetOrders(It.IsAny<DateTime>(), It.IsAny<Uri>(), It.IsAny<string>())).Returns(response);

            sqlAdapter = new Mock<ISqlAdapterRetry>();
            sqlAdapter.Setup((r => r.ExecuteWithRetry(It.IsAny<Action>()))).Callback((Action x) => x.Invoke());

            MagentoStoreEntity store = new MagentoStoreEntity()
            {
                ModuleUrl = "https://www.url.com/",
                ModuleUsername = "dude",
                ModulePassword = "sweet"
            };

            orderEntity = new MagentoOrderEntity();

            testObject = new Magento2RestDownloader(store, webClient.Object, sqlAdapter.Object);
            testObject.LoadOrder(orderEntity, order);
        }

        [Fact]
        public void LoadOrder_LoadsOnlineLastModified()
        {
            var date = new DateTime(2016, 10, 18, 19, 22, 1).ToUniversalTime();
            Assert.Equal(date, orderEntity.OnlineLastModified);
        }

        [Fact]
        public void LoadOrder_LoadsOrderStatus()
        {
            Assert.Equal("pending", orderEntity.OnlineStatus);
        }

        [Fact]
        public void LoadOrder_LoadsRequestedShipping()
        {
            Assert.Equal("Flat Rate - Fixed", orderEntity.RequestedShipping);
        }

        [Fact]
        public void LoadOrder_LoadsBillingAddress()
        {
            Assert.Equal("16204 Bay Harbour Ct", orderEntity.BillStreet1);
        }

        [Fact]
        public void LoadOrder_LoadsShippingAddress()
        {
            Assert.Equal("Mirza", orderEntity.ShipFirstName);
        }

        [Fact]
        public void LoadOrder_LoadsOrderDate()
        {
            var date = new DateTime(2016, 10, 18, 19, 22, 0).ToUniversalTime();
            Assert.Equal(date, orderEntity.OrderDate);
        }

        [Fact]
        public void LoadOrder_LoadOrderNumber()
        {
            Assert.Equal(1, orderEntity.OrderNumber);
        }

        [Fact]
        public void LoadOrder_LoadsOrderTotal()
        {
            Assert.Equal(109.55m, orderEntity.OrderTotal);
        }

        [Fact]
        public void LoadOrder_LoadsItems()
        {
            Assert.Equal(1, orderEntity.OrderItems.FirstOrDefault()?.Quantity);
        }

        [Fact]
        public void LoadOrder_LoadsOrderCharges()
        {
            Assert.Equal(-18.45m, orderEntity.OrderCharges.FirstOrDefault(c => c.Type=="DISCOUNT")?.Amount);
        }
    }
}