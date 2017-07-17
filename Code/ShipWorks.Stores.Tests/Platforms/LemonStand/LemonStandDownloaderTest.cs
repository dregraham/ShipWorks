using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Stores.LemonStand;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.LemonStand
{
    /// <summary>
    /// Performs unit testing on the LemonStandDownloader class.
    /// This test class actually tests a FakeLemonStandDownloader that mocks the inherited dependencies on InstantiateOrder and SaveDownloadedOrders
    /// </summary>
    public class LemonStandDownloaderTest
    {
        readonly Mock<ILemonStandWebClient> webClient = new Mock<ILemonStandWebClient>();
        readonly Mock<ISqlAdapterRetry> adapter = new Mock<ISqlAdapterRetry>();
        readonly LemonStandStoreEntity store = new LemonStandStoreEntity();

        private FakeLemonStandDownloader testObject;
        private readonly string lemonStandOrders;
        private readonly string singleOrder;
        private readonly string shipment;
        private readonly string customer;
        private readonly string product;
        private readonly string invoice;
        private readonly string badDataOrder;
        private readonly string missingDataOrder;
        private readonly string missingItems;

        public LemonStandDownloaderTest()
        {
            lemonStandOrders = GetEmbeddedResourceJson("Platforms.LemonStand.Artifacts.LemonStandJsonOrderResponse.js");
            singleOrder = GetEmbeddedResourceJson("Platforms.LemonStand.Artifacts.LemonStandSingleOrderJson.js");
            invoice = GetEmbeddedResourceJson("Platforms.LemonStand.Artifacts.LemonStandInvoiceJson.js");
            shipment = GetEmbeddedResourceJson("Platforms.LemonStand.Artifacts.LemonStandShipmentJson.js");
            customer = GetEmbeddedResourceJson("Platforms.LemonStand.Artifacts.LemonStandCustomerJson.js");
            product = GetEmbeddedResourceJson("Platforms.LemonStand.Artifacts.LemonStandProductJson.js");
            badDataOrder = GetEmbeddedResourceJson("Platforms.LemonStand.Artifacts.LemonStandOrderWithBadOrderStatusAndItemQuantity.js");
            missingDataOrder = GetEmbeddedResourceJson("Platforms.LemonStand.Artifacts.LemonStandOrderWithMissingData.js");
            missingItems = GetEmbeddedResourceJson("Platforms.LemonStand.Artifacts.OrderMissingItems.js");

            webClient.Setup(w => w.GetOrders(1, DateTime.UtcNow.ToString())).Returns(JObject.Parse(lemonStandOrders));
            webClient.Setup(w => w.GetShipment("36")).Returns(JObject.Parse(invoice));
            webClient.Setup(w => w.GetShippingAddress("36")).Returns(JObject.Parse(shipment));
            webClient.Setup(w => w.GetBillingAddress("34")).Returns(JObject.Parse(customer));
            webClient.Setup(w => w.GetProduct("1")).Returns(JObject.Parse(product));

            adapter.Setup(retry => retry.ExecuteWithRetryAsync(It.IsAny<Func<Task>>())).Callback((Func<Task> x) => x.Invoke()).Returns(Task.CompletedTask);

            store.TypeCode = 68;
        }

        private string GetEmbeddedResourceJson(string embeddedResourceName) =>
            Assembly.GetExecutingAssembly().GetEmbeddedResourceString(embeddedResourceName);

        private async Task<FakeLemonStandDownloader> SetupPrepareOrderTests()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object, new LemonStandStoreType(store));
            JObject jsonOrder = JObject.Parse(singleOrder);
            testObject.Order = await testObject.PrepareOrder(jsonOrder);

            return testObject;
        }

        private async Task<FakeLemonStandDownloader> SetupLoadOrderTests()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object, new LemonStandStoreType(store));
            JObject jsonOrder = JObject.Parse(singleOrder);
            await testObject.LoadOrder(jsonOrder);

            return testObject;
        }

        [Fact]
        public void GetDate_ReturnsCorrectDateTimeInUTC_WhenGivenLemonStandTimeFormat()
        {
            Assert.Equal(new DateTimeOffset(2015, 9, 10, 13, 59, 6, new TimeSpan(0, -5, 0, 0)), LemonStandDownloader.GetDate("2015-09-10T13:59:06-05:00"));
        }

        [Fact]
        public void GetDate_ThrowsLemonStandException_WhenGivenInvalidDate()
        {
            Assert.Throws<LemonStandException>(() => LemonStandDownloader.GetDate("A Bad Date"));
        }

        [Fact]
        public async Task PrepareOrders_LoadsIntoLemonStandOrderDto_WhenGivenJsonOrder()
        {
            testObject = await SetupPrepareOrderTests();
            Assert.Equal(36, testObject.Order.OrderNumber);
        }

        [Fact]
        public async Task PrepareOrders_LoadsIntoLemonStandShipmentDto_WhenGivenJsonShipment()
        {
            testObject = await SetupPrepareOrderTests();
            Assert.Equal("Priority Mail", testObject.Order.RequestedShipping);
        }

        [Fact]
        public async Task PrepareOrders_LoadsIntoLemonStandShippingAddressDto_WhenGivenJsonShipment()
        {
            testObject = await SetupPrepareOrderTests();
            Assert.Equal("204 S Friedline Dr", testObject.Order.ShipStreet1);
        }

        [Fact]
        public async Task PrepareOrders_LoadsIntoLemonStandCustomerDto_WhenGivenJsonCustomer()
        {
            testObject = await SetupPrepareOrderTests();
            Assert.Equal("Stan", testObject.Order.BillFirstName);
        }

        [Fact]
        public async Task PrepareOrders_LoadsIntoLemonStandBillingAddressDto_WhenGivenJsonCustomer()
        {
            testObject = await SetupPrepareOrderTests();
            Assert.Equal("Carbondale", testObject.Order.BillCity);
        }

        [Fact]
        public async Task LoadItems_LoadsIntoLemonStandItemDto_WhenGivenJsonItem()
        {
            testObject = await SetupPrepareOrderTests();
            Assert.Equal("Baseball cap", testObject.Order.OrderItems.First().Name);
        }

        [Fact]
        public async Task PrepareOrders_GetShipmentIsOnlyCalledOnce_WhenGivenSingleJsonOrder()
        {
            await SetupPrepareOrderTests();
            webClient.Verify(client => client.GetShipment("36"), Times.Exactly(1));
        }

        [Fact]
        public async Task LoadOrder_SqlAdapterIsCalledOnce_WhenOrderIsLoaded()
        {
            testObject = await SetupLoadOrderTests();
            adapter.Verify(a => a.ExecuteWithRetryAsync(It.IsAny<Func<Task>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task LoadOrder_SqlAdapterSavesOrder_WhenGivenJsonOrder()
        {
            testObject = await SetupLoadOrderTests();
            Assert.Equal(testObject.SavedOrder, testObject.Order);
        }

        [Fact]
        public void LoadOrder_ThrowsLemonStandException_WhenItemQuantityIsEmptyInResponse()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object, new LemonStandStoreType(store));
            JObject jsonOrder = JObject.Parse(badDataOrder);

            Assert.ThrowsAsync<LemonStandException>(() => testObject.PrepareOrder(jsonOrder));
        }

        [Fact]
        public void LoadOrder_ThrowsLemonStandException_WhenOrderIsMissingOrderNumber()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object, new LemonStandStoreType(store));
            JObject jsonOrder = JObject.Parse(missingDataOrder);

            Assert.ThrowsAsync<LemonStandException>(() => testObject.PrepareOrder(jsonOrder));
        }

        [Fact]
        public void LoadItem_ThrowsLemonStandException_WhenItemsAreMissingFromOrderResponse()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object, new LemonStandStoreType(store));
            JObject jsonOrder = JObject.Parse(missingItems);

            Assert.ThrowsAsync<LemonStandException>(() => testObject.PrepareOrder(jsonOrder));
        }
    }
}
