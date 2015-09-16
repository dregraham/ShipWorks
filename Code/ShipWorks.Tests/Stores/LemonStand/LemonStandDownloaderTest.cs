using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Common.Threading;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;


namespace ShipWorks.Tests.Stores.LemonStand
{
    [TestClass]
    public class LemonStandDownloaderTest
    {
        Mock<ILemonStandWebClient> webClient = new Mock<ILemonStandWebClient>();
        Mock<ISqlAdapterRetry> adapter = new Mock<ISqlAdapterRetry>();
        Mock<StoreEntity> store = new Mock<StoreEntity>();
        private string lemonStandOrders;
        private string singleOrder;
        private string shipment;
        private string customer;
        private string product;
        private string invoice;

        [TestInitialize]
        public void Initialize()
        { 
            lemonStandOrders = GetEmbeddedResourceJson("ShipWorks.Tests.Stores.LemonStand.Artifacts.LemonStandJsonOrderResponse.js");
            singleOrder = GetEmbeddedResourceJson("ShipWorks.Tests.Stores.LemonStand.Artifacts.LemonStandSingleOrderJson.js");
            invoice = GetEmbeddedResourceJson("ShipWorks.Tests.Stores.LemonStand.Artifacts.LemonStandInvoiceJson.js");
            shipment = GetEmbeddedResourceJson("ShipWorks.Tests.Stores.LemonStand.Artifacts.LemonStandShipmentJson.js");
            customer = GetEmbeddedResourceJson("ShipWorks.Tests.Stores.LemonStand.Artifacts.LemonStandCustomerJson.js");
            product = GetEmbeddedResourceJson("ShipWorks.Tests.Stores.LemonStand.Artifacts.LemonStandProductJson.js");
            webClient.Setup(w => w.GetOrders()).Returns(JObject.Parse(lemonStandOrders));
            webClient.Setup(w => w.GetShipment("36")).Returns(JObject.Parse(invoice));
            webClient.Setup(w => w.GetShippingAddress("36")).Returns(JObject.Parse(shipment));
            webClient.Setup(w => w.GetBillingAddress("34")).Returns(JObject.Parse(customer));
            webClient.Setup(w => w.GetProduct("1")).Returns(JObject.Parse(product));
            adapter.Setup(retry => retry.ExecuteWithRetry(It.IsAny<Action>())).Callback((Action x) => x.Invoke());
        }

        private string GetEmbeddedResourceJson(string embeddedResourceName) {
            string txt = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    txt = reader.ReadToEnd();
                }
            }

            return txt;
        }

        private FakeLemonStandDownloader SetupPrepareOrderTests()
        {
            FakeLemonStandDownloader testObject = new FakeLemonStandDownloader(store.Object, webClient.Object, adapter.Object);
            JObject jsonOrder = JObject.Parse(singleOrder);
            testObject.Order = testObject.PrepareOrder(jsonOrder);
            return testObject;
        }

        [TestMethod]
        public void GetDate_ReturnsCorrectDateTimeInUTC_WhenGivenLemonStandTimeFormat_Test()
        {
            FakeLemonStandDownloader testObject = new FakeLemonStandDownloader(store.Object, webClient.Object, adapter.Object);
            Assert.AreEqual(new DateTimeOffset(2015, 9, 10, 13, 59, 6, new TimeSpan(0, -5, 0, 0)), testObject.GetDate("2015-09-10T13:59:06-05:00"));
        }

        [TestMethod]
        public void PrepareOrders_LoadsIntoLemonStandOrderDto_WhenGivenJsonOrder_Test()
        {
            FakeLemonStandDownloader testObject = SetupPrepareOrderTests();
            Assert.AreEqual(36, testObject.Order.OrderNumber);
        }

        [TestMethod]
        public void PrepareOrders_LoadsIntoLemonStandShipmentDto_WhenGivenJsonShipment_Test()
        {
            FakeLemonStandDownloader testObject = SetupPrepareOrderTests();
            Assert.AreEqual("Priority Mail", testObject.Order.RequestedShipping);
        }

        [TestMethod]
        public void PrepareOrders_LoadsIntoLemonStandShippingAddressDto_WhenGivenJsonShipment_Test()
        {
            FakeLemonStandDownloader testObject = SetupPrepareOrderTests();
            Assert.AreEqual("204 S Friedline Dr", testObject.Order.ShipStreet1);
        }

        [TestMethod]
        public void PrepareOrders_LoadsIntoLemonStandCustomerDto_WhenGivenJsonCustomer_Test()
        {
            FakeLemonStandDownloader testObject = SetupPrepareOrderTests();
            Assert.AreEqual("Stan", testObject.Order.BillFirstName);
        }

        [TestMethod]
        public void PrepareOrders_LoadsIntoLemonStandBillingAddressDto_WhenGivenJsonCustomer_Test()
        {
            FakeLemonStandDownloader testObject = SetupPrepareOrderTests();
            Assert.AreEqual("Carbondale", testObject.Order.BillCity);
        }

        [TestMethod]
        public void LoadItems_LoadsIntoLemonStandItemDto_WhenGivenJsonItem_Test()
        {
            FakeLemonStandDownloader testObject = SetupPrepareOrderTests();
            Assert.AreEqual("Baseball cap", testObject.Order.OrderItems.First().Name);
        }

        [TestMethod]
        public void PrepareOrders_GetShipmentIsOnlyCalledOnce_WhenGivenSingleJsonOrder_Test()
        {
            SetupPrepareOrderTests();
            webClient.Verify(client => client.GetShipment("36"), Times.Exactly(1));
        }

        [TestMethod]
        public void LoadOrder_SqlAdapterIsCalledOnce_WhenOrderIsLoaded_Test()
        {
            FakeLemonStandDownloader testObject = new FakeLemonStandDownloader(store.Object, webClient.Object, adapter.Object);
            JObject jsonOrder = JObject.Parse(singleOrder);
            testObject.LoadOrder(jsonOrder);
            adapter.Verify(a => a.ExecuteWithRetry(It.IsAny<Action>()), Times.Exactly(1));
        }

        [TestMethod]
        public void LoadOrder_SqlAdapterSavesOrder_WhenGivenJsonOrder_Test()
        {
            FakeLemonStandDownloader testObject = new FakeLemonStandDownloader(store.Object, webClient.Object, adapter.Object);
            JObject jsonOrder = JObject.Parse(singleOrder);
            testObject.LoadOrder(jsonOrder);
            Assert.AreEqual(testObject.SavedOrder, testObject.Order);
        }
    }
}
