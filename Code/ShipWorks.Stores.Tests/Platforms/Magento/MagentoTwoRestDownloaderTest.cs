using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using Newtonsoft.Json;
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

        public MagentoTwoRestDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        private T DeserializeFromResource<T>(string name)
        {
            var json = GetType().Assembly.GetEmbeddedResourceString($"Platforms.Magento.Artifacts.{name}.json");
            return JsonConvert.DeserializeObject<T>(json);
        }

        private async Task LoadOrder(string magentoOrder)
        {
            var order = DeserializeFromResource<Order>(magentoOrder);
            var item = DeserializeFromResource<Item>("MagentoItem");
            var itemWithOptions = DeserializeFromResource<Item>("MagentoItemWithOptions");
            var product = DeserializeFromResource<Product>("MagentoProduct");

            var response = new OrdersResponse()
            {
                Orders = new[] { order }
            };

            MagentoStoreEntity store = new MagentoStoreEntity()
            {
                ModuleUrl = "https://www.url.com/",
                ModuleUsername = "dude",
                ModulePassword = "sweet",
                StoreTypeCode = StoreTypeCode.Magento
            };

            var webClient = mock.Mock<IMagentoTwoRestClient>();
            webClient.Setup(w => w.GetToken()).Returns("token");
            webClient.Setup(w => w.GetOrders(It.IsAny<DateTime?>(), It.IsAny<int>())).Returns(response);
            webClient.Setup(w => w.GetItem(It.IsAny<long>())).Returns(item);
            webClient.Setup(w => w.GetItem(15)).Returns(itemWithOptions);
            webClient.Setup(w => w.GetItem(14)).Returns(itemWithOptions);
            webClient.Setup(w => w.GetProduct(It.IsAny<string>())).Returns(product);

            orderEntity = new MagentoOrderEntity();

            var testObject = mock.Create<MagentoTwoRestDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject.LoadOrder(orderEntity, order, mock.Mock<IProgressReporter>().Object);
        }

        [Fact]
        public async Task LoadOrder_LoadsOnlineLastModified()
        {
            await LoadOrder("MagentoOrder");

            var date = new DateTime(2016, 10, 18, 19, 22, 1, DateTimeKind.Utc);

            Assert.Equal(date, orderEntity.OnlineLastModified);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderStatus()
        {
            await LoadOrder("MagentoOrder");

            Assert.Equal("pending", orderEntity.OnlineStatus);
        }

        [Fact]
        public async Task LoadOrder_LoadsRequestedShipping()
        {
            await LoadOrder("MagentoOrder");

            Assert.Equal("Flat Rate - Fixed", orderEntity.RequestedShipping);
        }

        [Fact]
        public async Task LoadOrder_LoadsBillingAddress()
        {
            await LoadOrder("MagentoOrder");

            Assert.Equal("16204 Bay Harbour Ct", orderEntity.BillStreet1);
        }

        [Fact]
        public async Task LoadOrder_LoadsShippingAddress()
        {
            await LoadOrder("MagentoOrder");

            Assert.Equal("Mirza", orderEntity.ShipFirstName);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderDate()
        {
            await LoadOrder("MagentoOrder");

            var date = new DateTime(2016, 10, 18, 19, 22, 0, DateTimeKind.Utc);

            Assert.Equal(date, orderEntity.OrderDate);
        }

        [Fact]
        public async Task LoadOrder_LoadOrderNumber()
        {
            await LoadOrder("MagentoOrder");

            Assert.Equal(1, orderEntity.OrderNumber);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderTotal()
        {
            await LoadOrder("MagentoOrder");

            Assert.Equal(109.55m, orderEntity.OrderTotal);
        }

        [Fact]
        public async Task LoadOrder_LoadsItems()
        {
            await LoadOrder("MagentoOrder");

            Assert.Equal(1, orderEntity.OrderItems.FirstOrDefault()?.Quantity);
        }

        [Fact]
        public async Task LoadOrder_LoadsItemsAttributes()
        {
            await LoadOrder("MagentoOrderWithItemAttributes");

            Assert.Equal(2, orderEntity.OrderItems.FirstOrDefault().OrderItemAttributes.Count);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderCharges()
        {
            await LoadOrder("MagentoOrder");

            Assert.Equal(-18.45m, orderEntity.OrderCharges.FirstOrDefault(c => c.Type == "DISCOUNT")?.Amount);
        }

        [Fact]
        public async Task LoadOrder_LoadsNotes()
        {
            await LoadOrder("MagentoOrder");

            Assert.Equal("here are some comments", orderEntity.Notes.FirstOrDefault().Text);
        }

        [Fact]
        public async Task LoadOrder_LoadsPricesForSimpleItemsWithAssociatedConfigurableProduct()
        {
            await LoadOrder("MagentoOrderWithConfigurableProduct");

            Assert.Equal(11.49M, orderEntity.OrderItems.Single().UnitPrice);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderWithNoItemsWithNoErrors()
        {
            await LoadOrder("WeirdMagentoOrder");

            Assert.Empty(orderEntity.OrderItems);
        }

        [Fact]
        public async Task LoadOrder_LoadsMultipleOrderCharges()
        {
            await LoadOrder("WeirdMagentoOrder");

            Assert.Equal(3, orderEntity.OrderCharges.Count);
        }

        [Fact]
        public async Task LoadOrder_LoadsOrderWithMissingFields()
        {
            await LoadOrder("BadMagentoOrder");

            Assert.Equal(1, orderEntity.OrderItems.Count);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}