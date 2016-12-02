using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.DTO;
using ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class MagentoTwoRestDownloaderTest : IDisposable
    {
        private readonly MagentoOrderEntity orderEntity;
        private readonly AutoMock mock;

        public MagentoTwoRestDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var magentoOrder = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.MagentoOrder.json");

            var order = JsonConvert.DeserializeObject<Order>(magentoOrder);

            var response = new OrdersResponse()
            {
                Orders = new List<Order> { order }
            };


            var sqlAdapter = new Mock<ISqlAdapterRetry>();
            sqlAdapter.Setup((r => r.ExecuteWithRetry(It.IsAny<Action>()))).Callback((Action x) => x.Invoke());

            MagentoStoreEntity store = new MagentoStoreEntity()
            {
                ModuleUrl = "https://www.url.com/",
                ModuleUsername = "dude",
                ModulePassword = "sweet"
            };

            var webClient = mock.MockRepository.Create<IMagentoTwoRestClient>();
            webClient.Setup(w => w.GetToken()).Returns("token");
            webClient.Setup(w => w.GetOrders(It.IsAny<DateTime?>(), It.IsAny<int>())).Returns(response);

            mock.MockFunc<MagentoStoreEntity, IMagentoTwoRestClient>(webClient);

            orderEntity = new MagentoOrderEntity();

            var testObject = mock.Create<MagentoTwoRestDownloader>(new TypedParameter(typeof(StoreEntity), store));
            testObject.LoadOrder(orderEntity, order);
        }

        [Fact]
        public void LoadOrder_LoadsOnlineLastModified()
        {
            var date = new DateTime(2016, 10, 18, 19, 22, 1, DateTimeKind.Utc);

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
            var date = new DateTime(2016, 10, 18, 19, 22, 0, DateTimeKind.Utc);

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

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}