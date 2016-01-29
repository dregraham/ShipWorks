using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.LemonStand;
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
            lemonStandOrders = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.LemonStand.Artifacts.LemonStandJsonOrderResponse.js");
            singleOrder = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.LemonStand.Artifacts.LemonStandSingleOrderJson.js");
            invoice = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.LemonStand.Artifacts.LemonStandInvoiceJson.js");
            shipment = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.LemonStand.Artifacts.LemonStandShipmentJson.js");
            customer = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.LemonStand.Artifacts.LemonStandCustomerJson.js");
            product = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.LemonStand.Artifacts.LemonStandProductJson.js");
            badDataOrder = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.LemonStand.Artifacts.LemonStandOrderWithBadOrderStatusAndItemQuantity.js");
            missingDataOrder = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.LemonStand.Artifacts.LemonStandOrderWithMissingData.js");
            missingItems = GetEmbeddedResourceJson("ShipWorks.Stores.Tests.Platforms.LemonStand.Artifacts.OrderMissingItems.js");

            webClient.Setup(w => w.GetOrders(1, DateTime.UtcNow.ToString())).Returns(JObject.Parse(lemonStandOrders));
            webClient.Setup(w => w.GetShipment("36")).Returns(JObject.Parse(invoice));
            webClient.Setup(w => w.GetShippingAddress("36")).Returns(JObject.Parse(shipment));
            webClient.Setup(w => w.GetBillingAddress("34")).Returns(JObject.Parse(customer));
            webClient.Setup(w => w.GetProduct("1")).Returns(JObject.Parse(product));

            adapter.Setup(retry => retry.ExecuteWithRetry(It.IsAny<Action>())).Callback((Action x) => x.Invoke());

            store.TypeCode = 68;
        }

        private string GetEmbeddedResourceJson(string embeddedResourceName) {
            string txt = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
            {
                if (stream != null)
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        txt = reader.ReadToEnd();
                    }
            }

            return txt;
        }

        private FakeLemonStandDownloader SetupPrepareOrderTests()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object);
            JObject jsonOrder = JObject.Parse(singleOrder);
            testObject.Order = testObject.PrepareOrder(jsonOrder);
            
            return testObject;
        }

        private FakeLemonStandDownloader SetupLoadOrderTests()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object);
            JObject jsonOrder = JObject.Parse(singleOrder);
            testObject.LoadOrder(jsonOrder);

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
        public void PrepareOrders_LoadsIntoLemonStandOrderDto_WhenGivenJsonOrder()
        {
            testObject = SetupPrepareOrderTests();
            Assert.Equal(36, testObject.Order.OrderNumber);
        }

        [Fact]
        public void PrepareOrders_LoadsIntoLemonStandShipmentDto_WhenGivenJsonShipment()
        {
            testObject = SetupPrepareOrderTests();
            Assert.Equal("Priority Mail", testObject.Order.RequestedShipping);
        }

        [Fact]
        public void PrepareOrders_LoadsIntoLemonStandShippingAddressDto_WhenGivenJsonShipment()
        {
            testObject = SetupPrepareOrderTests();
            Assert.Equal("204 S Friedline Dr", testObject.Order.ShipStreet1);
        }

        [Fact]
        public void PrepareOrders_LoadsIntoLemonStandCustomerDto_WhenGivenJsonCustomer()
        {
            testObject = SetupPrepareOrderTests();
            Assert.Equal("Stan", testObject.Order.BillFirstName);
        }

        [Fact]
        public void PrepareOrders_LoadsIntoLemonStandBillingAddressDto_WhenGivenJsonCustomer()
        {
            testObject = SetupPrepareOrderTests();
            Assert.Equal("Carbondale", testObject.Order.BillCity);
        }

        [Fact]
        public void LoadItems_LoadsIntoLemonStandItemDto_WhenGivenJsonItem()
        {
            testObject = SetupPrepareOrderTests();
            Assert.Equal("Baseball cap", testObject.Order.OrderItems.First().Name);
        }

        [Fact]
        public void PrepareOrders_GetShipmentIsOnlyCalledOnce_WhenGivenSingleJsonOrder()
        {
            SetupPrepareOrderTests();
            webClient.Verify(client => client.GetShipment("36"), Times.Exactly(1));
        }

        [Fact]
        public void LoadOrder_SqlAdapterIsCalledOnce_WhenOrderIsLoaded()
        {
            testObject = SetupLoadOrderTests();
            adapter.Verify(a => a.ExecuteWithRetry(It.IsAny<Action>()), Times.Exactly(1));
        }

        [Fact]
        public void LoadOrder_SqlAdapterSavesOrder_WhenGivenJsonOrder()
        {
            testObject = SetupLoadOrderTests();
            Assert.Equal(testObject.SavedOrder, testObject.Order);
        }

        [Fact]
        public void LoadOrder_ThrowsLemonStandException_WhenItemQuantityIsEmptyInResponse()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object);
            JObject jsonOrder = JObject.Parse(badDataOrder);

            Assert.Throws<LemonStandException>(() => testObject.PrepareOrder(jsonOrder));
        }

        [Fact]
        public void LoadOrder_ThrowsLemonStandException_WhenOrderIsMissingOrderNumber()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object);
            JObject jsonOrder = JObject.Parse(missingDataOrder);

            Assert.Throws<LemonStandException>(() => testObject.PrepareOrder(jsonOrder));
        }

        [Fact]
        public void LoadItem_ThrowsLemonStandException_WhenItemsAreMissingFromOrderResponse()
        {
            testObject = new FakeLemonStandDownloader(store, webClient.Object, adapter.Object);
            JObject jsonOrder = JObject.Parse(missingItems);

            Assert.Throws<LemonStandException>(() => testObject.PrepareOrder(jsonOrder));
        }
    }
}
