using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.BestRate
{
    [TestClass]
    public class Express1BrokerFilterTest
    {
        private Express1BrokerFilter testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new Express1BrokerFilter();
        }

        [TestMethod]
        public void Filter_RemovesAllExpress1CounterRateBrokers_WhenCollectionContainsRealExpress1StampsBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new Express1EndiciaCounterRatesBroker(),
                new Express1StampsBestRateBroker()
            };

            List<IBestRateShippingBroker> filteredBrokers = testObject.Filter(brokers).ToList();

            Assert.AreEqual(1, filteredBrokers.Count);
            Assert.AreEqual(0, filteredBrokers.OfType<Express1EndiciaCounterRatesBroker>().Count());
        }

        [TestMethod]
        public void Filter_RemovesAllStampsCounterRateBrokers_WhenCollectionContainsRealExpress1EndiciaBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new Express1EndiciaBestRateBroker(),
                new Express1StampsCounterRatesBroker()
            };

            List<IBestRateShippingBroker> filteredBrokers = testObject.Filter(brokers).ToList();

            Assert.AreEqual(1, filteredBrokers.Count);
            Assert.AreEqual(0, filteredBrokers.OfType<Express1StampsCounterRatesBroker>().Count());
        }

        [TestMethod]
        public void Filter_RemovesExpress1EndiciaCounterRatesBroker_WhenCollectionContainsExpress1StampsCounterRatesBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new Express1EndiciaCounterRatesBroker(),
                new Express1StampsCounterRatesBroker()
            };

            List<IBestRateShippingBroker> filteredBrokers = testObject.Filter(brokers).ToList();

            Assert.AreEqual(1, filteredBrokers.Count);
            Assert.AreEqual(0, filteredBrokers.OfType<Express1EndiciaCounterRatesBroker>().Count());
        }

        [TestMethod]
        public void Filter_RetainsNonExpress1Brokers_AndRemovesExpress1EndiciaCounterRatesBroker_WhenCollectionContainsExpress1StampsCounterRatesBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new Express1EndiciaCounterRatesBroker(),
                new FedExBestRateBroker(),
                new UpsCounterRatesBroker(),
                new Express1StampsCounterRatesBroker()
            };

            List<IBestRateShippingBroker> filteredBrokers = testObject.Filter(brokers).ToList();

            Assert.AreEqual(3, filteredBrokers.Count);
            Assert.AreEqual(0, filteredBrokers.OfType<Express1EndiciaCounterRatesBroker>().Count());
            Assert.AreEqual(1, filteredBrokers.OfType<FedExBestRateBroker>().Count());
            Assert.AreEqual(1, filteredBrokers.OfType<UpsCounterRatesBroker>().Count());

        }

        [TestMethod]
        public void Filter_RetainsExpress1Brokers_WhenCollectionContainsAllRealExpress1Brokers_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new Express1EndiciaBestRateBroker(),
                new Express1StampsBestRateBroker()
            };

            List<IBestRateShippingBroker> filteredBrokers = testObject.Filter(brokers).ToList();

            Assert.AreEqual(2, filteredBrokers.Count);
            Assert.AreEqual(1, filteredBrokers.OfType<Express1StampsBestRateBroker>().Count());
            Assert.AreEqual(1, filteredBrokers.OfType<Express1EndiciaBestRateBroker>().Count());
        }


    }
}
