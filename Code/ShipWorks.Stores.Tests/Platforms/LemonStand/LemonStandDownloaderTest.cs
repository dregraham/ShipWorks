using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.LemonStand
{
    /// <summary>
    /// Performs unit testing on the LemonStandDownloader class.
    /// This test class actually tests a FakeLemonStandDownloader that mocks the inherited dependencies on InstantiateOrder and SaveDownloadedOrders
    /// </summary>
    public class LemonStandDownloaderTest
    {
        readonly StoreEntity store = new LemonStandStoreEntity();

        private readonly string lemonStandOrders;
        private readonly string singleOrder;
        private readonly string shipment;
        private readonly string customer;
        private readonly string product;
        private readonly string invoice;
        private readonly string badDataOrder;
        private readonly string missingDataOrder;
        private readonly string missingItems;
        private readonly AutoMock mock;

        public LemonStandDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            lemonStandOrders = GetEmbeddedResourceJson("LemonStandJsonOrderResponse");
            singleOrder = GetEmbeddedResourceJson("LemonStandSingleOrderJson");
            invoice = GetEmbeddedResourceJson("LemonStandInvoiceJson");
            shipment = GetEmbeddedResourceJson("LemonStandShipmentJson");
            customer = GetEmbeddedResourceJson("LemonStandCustomerJson");
            product = GetEmbeddedResourceJson("LemonStandProductJson");
            badDataOrder = GetEmbeddedResourceJson("LemonStandOrderWithBadOrderStatusAndItemQuantity");
            missingDataOrder = GetEmbeddedResourceJson("LemonStandOrderWithMissingData");
            missingItems = GetEmbeddedResourceJson("OrderMissingItems");

            var client = mock.Mock<ILemonStandWebClient>();
            client.Setup(w => w.GetOrders(1, DateTime.UtcNow.ToString())).Returns(JObject.Parse(lemonStandOrders));
            client.Setup(w => w.GetShipment("36")).Returns(JObject.Parse(invoice));
            client.Setup(w => w.GetShippingAddress("36")).Returns(JObject.Parse(shipment));
            client.Setup(w => w.GetBillingAddress("34")).Returns(JObject.Parse(customer));
            client.Setup(w => w.GetProduct("1")).Returns(JObject.Parse(product));

            store.StoreTypeCode = StoreTypeCode.LemonStand;

            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(It.IsAny<StoreEntity>()))
                .Returns(new LemonStandStoreType(store));
        }

        private string GetEmbeddedResourceJson(string embeddedResourceName) =>
            GetType().Assembly.GetEmbeddedResourceString($"Platforms.LemonStand.Artifacts.{embeddedResourceName}.js");

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
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            var order = await testObject.PrepareOrder(JObject.Parse(singleOrder));

            Assert.Equal(36, order.Value.OrderNumber);
        }

        [Fact]
        public async Task PrepareOrders_LoadsIntoLemonStandShipmentDto_WhenGivenJsonShipment()
        {
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            var order = await testObject.PrepareOrder(JObject.Parse(singleOrder));

            Assert.Equal("Priority Mail", order.Value.RequestedShipping);
        }

        [Fact]
        public async Task PrepareOrders_LoadsIntoLemonStandShippingAddressDto_WhenGivenJsonShipment()
        {
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            var order = await testObject.PrepareOrder(JObject.Parse(singleOrder));

            Assert.Equal("204 S Friedline Dr", order.Value.ShipStreet1);
        }

        [Fact]
        public async Task PrepareOrders_LoadsIntoLemonStandCustomerDto_WhenGivenJsonCustomer()
        {
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            var order = await testObject.PrepareOrder(JObject.Parse(singleOrder));

            Assert.Equal("Stan", order.Value.BillFirstName);
        }

        [Fact]
        public async Task PrepareOrders_LoadsIntoLemonStandBillingAddressDto_WhenGivenJsonCustomer()
        {
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            var order = await testObject.PrepareOrder(JObject.Parse(singleOrder));

            Assert.Equal("Carbondale", order.Value.BillCity);
        }

        [Fact]
        public async Task LoadItems_LoadsIntoLemonStandItemDto_WhenGivenJsonItem()
        {
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            var order = await testObject.PrepareOrder(JObject.Parse(singleOrder));

            Assert.Equal("Baseball cap", order.Value.OrderItems.First().Name);
        }

        [Fact]
        public async Task PrepareOrders_GetShipmentIsOnlyCalledOnce_WhenGivenSingleJsonOrder()
        {
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            await testObject.PrepareOrder(JObject.Parse(singleOrder));

            mock.Mock<ILemonStandWebClient>().Verify(client => client.GetShipment("36"));
        }

        [Fact]
        public async Task LoadOrder_SqlAdapterIsCalledOnce_WhenOrderIsLoaded()
        {
            var adapterRetry = mock.FromFactory<ISqlAdapterRetryFactory>()
                .Mock(x => x.Create<SqlException>(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            await testObject.LoadOrder(JObject.Parse(singleOrder));

            adapterRetry.Verify(a => a.ExecuteWithRetryAsync(It.IsAny<Func<Task>>()), Times.Exactly(1));
        }

        [Fact]
        public async Task PrepareOrder_ThrowsLemonStandException_WhenItemQuantityIsEmptyInResponse()
        {
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            await Assert.ThrowsAsync<LemonStandException>(async () => await testObject.PrepareOrder(JObject.Parse(badDataOrder)));
        }

        [Fact]
        public async Task PrepareOrder_ThrowsLemonStandException_WhenOrderIsMissingOrderNumber()
        {
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            await Assert.ThrowsAsync<LemonStandException>(async () => await testObject.PrepareOrder(JObject.Parse(missingDataOrder)));
        }

        [Fact]
        public async Task PrepareOrder_ThrowsLemonStandException_WhenItemsAreMissingFromOrderResponse()
        {
            var testObject = mock.Create<LemonStandDownloader>(TypedParameter.From(store));

            await Assert.ThrowsAsync<LemonStandException>(async () => await testObject.PrepareOrder(JObject.Parse(missingItems)));
        }
    }
}
