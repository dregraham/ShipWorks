using System;
using System.IO;
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
        Mock<ILemonStandWebClient> client = new Mock<ILemonStandWebClient>();
        Mock<ISqlAdapterRetry> adapter = new Mock<ISqlAdapterRetry>();
        Mock<StoreEntity> store = new Mock<StoreEntity>();
        private string lemonStandOrders;
        private string singleOrder;

        [TestInitialize]
        public void Initialize()
        { 
            lemonStandOrders = GetEmbeddedResourceJson("ShipWorks.Tests.Stores.LemonStand.Artifacts.LemonStandJsonOrderResponse.js");
            singleOrder = GetEmbeddedResourceJson("ShipWorks.Tests.Stores.LemonStand.Artifacts.LemonStandSingleOrderJson.js");         
            client.Setup(w => w.GetOrders()).Returns(lemonStandOrders);
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

        [TestMethod]
        public void LemonStandTest1()
        {
            FakeLemonStandDownloader testObject = new FakeLemonStandDownloader(store.Object, client.Object, adapter.Object);
            Assert.AreEqual(new DateTimeOffset(2015, 9, 10, 13, 59, 6, new TimeSpan(0, -5, 0, 0)), testObject.GetDate("2015-09-10T13:59:06-05:00"));
        }

        [TestMethod]
        public void LSTest2()
        {
            FakeLemonStandDownloader testObject = new FakeLemonStandDownloader(store.Object, client.Object, adapter.Object);
            JToken jsonOrder = singleOrder;
            testObject.Order = testObject.PrepareOrder(jsonOrder);
            Assert.AreEqual("36", testObject.Order.OrderNumber);
        }



    }
}
