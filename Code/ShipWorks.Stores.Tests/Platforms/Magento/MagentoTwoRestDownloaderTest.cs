using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class MagentoTwoRestDownloaderTest : IDisposable
    {
        private MagentoOrderEntity orderEntity;
        private readonly AutoMock mock;
        private readonly string goodMagentoOrder;
        private readonly string magentoOrderWithConfigurableProduct;
        private readonly string weirdMagentoOrder;
        private readonly string badMagentoOrder;
        private readonly string magentoOrderWithAttributes;
        private readonly string magentoItemWithOptions;
        private readonly string magentoItem;
        private readonly string magentoProduct;

        public MagentoTwoRestDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            goodMagentoOrder = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.MagentoOrder.json");

            magentoOrderWithConfigurableProduct = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.MagentoOrderWithConfigurableProduct.json");

            weirdMagentoOrder = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.WeirdMagentoOrder.json");

            badMagentoOrder = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.BadMagentoOrder.json");

            magentoItemWithOptions = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.MagentoItemWithOptions.json");

            magentoOrderWithAttributes = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.MagentoOrderWithItemAttributes.json");

            magentoItem = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.MagentoItem.json");

            magentoProduct = EmbeddedResourceHelper.GetEmbeddedResourceString(
               "ShipWorks.Stores.Tests.Platforms.Magento.Artifacts.MagentoProduct.json");
        }

        private void LoadOrder(string magentoOrder)
        {
            var order = JsonConvert.DeserializeObject<Order>(magentoOrder);
            var item = JsonConvert.DeserializeObject<Item>(magentoItem);
            var itemWithOptions = JsonConvert.DeserializeObject<Item>(magentoItemWithOptions);
            var product = JsonConvert.DeserializeObject<Product>(magentoProduct);

            var response = new OrdersResponse()
            {
                Orders = new List<Order> {order}
            };

            var sqlAdapter = mock.Mock<ISqlAdapterRetry>();
            sqlAdapter.Setup((r => r.ExecuteWithRetry(It.IsAny<Action>()))).Callback((Action x) => x.Invoke());

            MagentoStoreEntity store = new MagentoStoreEntity()
            {
                ModuleUrl = "https://www.url.com/",
                ModuleUsername = "dude",
                ModulePassword = "sweet"
            };

            var progressReport = mock.Mock<IProgressReporter>();

            var webClient = mock.MockRepository.Create<IMagentoTwoRestClient>();
            webClient.Setup(w => w.GetToken()).Returns("token");
            webClient.Setup(w => w.GetOrders(It.IsAny<DateTime?>(), It.IsAny<int>())).Returns(response);
            webClient.Setup(w => w.GetItem(It.IsAny<long>())).Returns(item);
            webClient.Setup(w => w.GetItem(15)).Returns(itemWithOptions);
            webClient.Setup(w => w.GetItem(14)).Returns(itemWithOptions);
            webClient.Setup(w => w.GetProduct(It.IsAny<string>())).Returns(product);

            mock.MockFunc<MagentoStoreEntity, IMagentoTwoRestClient>(webClient);

            orderEntity = new MagentoOrderEntity();

            var testObject = mock.Create<MagentoTwoRestDownloader>(new TypedParameter(typeof(StoreEntity), store));
            testObject.LoadOrder(orderEntity, order, progressReport.Object);
        }

        [Fact]
        public void LoadOrder_LoadsOnlineLastModified()
        {
            LoadOrder(goodMagentoOrder);

            var date = new DateTime(2016, 10, 18, 19, 22, 1, DateTimeKind.Utc);

            Assert.Equal(date, orderEntity.OnlineLastModified);
        }

        [Fact]
        public void LoadOrder_LoadsOrderStatus()
        {
            LoadOrder(goodMagentoOrder);

            Assert.Equal("pending", orderEntity.OnlineStatus);
        }

        [Fact]
        public void LoadOrder_LoadsRequestedShipping()
        {
            LoadOrder(goodMagentoOrder);

            Assert.Equal("Flat Rate - Fixed", orderEntity.RequestedShipping);
        }

        [Fact]
        public void LoadOrder_LoadsBillingAddress()
        {
            LoadOrder(goodMagentoOrder);

            Assert.Equal("16204 Bay Harbour Ct", orderEntity.BillStreet1);
        }

        [Fact]
        public void LoadOrder_LoadsShippingAddress()
        {
            LoadOrder(goodMagentoOrder);

            Assert.Equal("Mirza", orderEntity.ShipFirstName);
        }

        [Fact]
        public void LoadOrder_LoadsOrderDate()
        {
            LoadOrder(goodMagentoOrder);

            var date = new DateTime(2016, 10, 18, 19, 22, 0, DateTimeKind.Utc);

            Assert.Equal(date, orderEntity.OrderDate);
        }

        [Fact]
        public void LoadOrder_LoadOrderNumber()
        {
            LoadOrder(goodMagentoOrder);

            Assert.Equal(1, orderEntity.OrderNumber);
        }

        [Fact]
        public void LoadOrder_LoadsOrderTotal()
        {
            LoadOrder(goodMagentoOrder);

            Assert.Equal(109.55m, orderEntity.OrderTotal);
        }

        [Fact]
        public void LoadOrder_LoadsItems()
        {
            LoadOrder(goodMagentoOrder);

            Assert.Equal(1, orderEntity.OrderItems.FirstOrDefault()?.Quantity);
        }

        [Fact]
        public void LoadOrder_LoadsItemsAttributes()
        {
            LoadOrder(magentoOrderWithAttributes);

            Assert.Equal(2, orderEntity.OrderItems.FirstOrDefault().OrderItemAttributes.Count);
        }

        [Fact]
        public void LoadOrder_LoadsOrderCharges()
        {
            LoadOrder(goodMagentoOrder);

            Assert.Equal(-18.45m, orderEntity.OrderCharges.FirstOrDefault(c => c.Type == "DISCOUNT")?.Amount);
        }

        [Fact]
        public void LoadOrder_LoadsNotes()
        {
            LoadOrder(goodMagentoOrder);

            Assert.Equal("here are some comments", orderEntity.Notes.FirstOrDefault().Text);
        }

        [Fact]
        public void LoadOrder_LoadsPricesForSimpleItemsWithAssociatedConfigurableProduct()
        {
            LoadOrder(magentoOrderWithConfigurableProduct);

            Assert.Equal(11.49M, orderEntity.OrderItems.Single().UnitPrice);
        }

        [Fact]
        public void LoadOrder_LoadsOrderWithNoItemsWithNoErrors()
        {
            LoadOrder(weirdMagentoOrder);

            Assert.Empty(orderEntity.OrderItems);
        }

        [Fact]
        public void LoadOrder_LoadsMultipleOrderCharges()
        {
            LoadOrder(weirdMagentoOrder);

            Assert.Equal(3, orderEntity.OrderCharges.Count);
        }

        [Fact]
        public void LoadOrder_LoadsOrderWithMissingFields()
        {
            LoadOrder(badMagentoOrder);

            Assert.Equal(1, orderEntity.OrderItems.Count);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}