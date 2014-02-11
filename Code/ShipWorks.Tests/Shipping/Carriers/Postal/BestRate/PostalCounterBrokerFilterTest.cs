using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.BestRate
{
    [TestClass]
    public class PostalCounterBrokerFilterTest
    {
        [TestMethod]
        public void Filter_WithMultipleEndiciaBrokers_ReturnsFirst()
        {
            var testBroker1 = new EndiciaCounterRatesBroker(new Mock<ICarrierAccountRepository<EndiciaAccountEntity>>().Object);
            var testBroker2 = new EndiciaCounterRatesBroker(new Mock<ICarrierAccountRepository<EndiciaAccountEntity>>().Object);
            var brokers = new List<IBestRateShippingBroker> {testBroker1, testBroker2};

            var testObject = new PostalCounterBrokerFilter();
            var results = testObject.Filter(brokers);

            Assert.AreEqual(testBroker1, results.Single());
        }

        [TestMethod]
        public void Filter_WithMultipleStampsBrokers_ReturnsFirst()
        {
            var testBroker1 = new StampsCounterRatesBroker(new Mock<ICarrierAccountRepository<StampsAccountEntity>>().Object);
            var testBroker2 = new StampsCounterRatesBroker(new Mock<ICarrierAccountRepository<StampsAccountEntity>>().Object);
            var brokers = new List<IBestRateShippingBroker> { testBroker1, testBroker2 };

            var testObject = new PostalCounterBrokerFilter();
            var results = testObject.Filter(brokers);

            Assert.AreEqual(testBroker1, results.Single());
        }

        [TestMethod]
        public void Filter_WithMultipleEndiciaAndStampsBrokers_ReturnsFirstEndicia()
        {
            var testBroker1 = new StampsCounterRatesBroker(new Mock<ICarrierAccountRepository<StampsAccountEntity>>().Object);
            var testBroker2 = new EndiciaCounterRatesBroker(new Mock<ICarrierAccountRepository<EndiciaAccountEntity>>().Object);
            var testBroker3 = new StampsCounterRatesBroker(new Mock<ICarrierAccountRepository<StampsAccountEntity>>().Object);
            var testBroker4 = new EndiciaCounterRatesBroker(new Mock<ICarrierAccountRepository<EndiciaAccountEntity>>().Object);
            var brokers = new List<IBestRateShippingBroker> { testBroker1, testBroker2, testBroker3, testBroker4 };

            var testObject = new PostalCounterBrokerFilter();
            var results = testObject.Filter(brokers);

            Assert.AreEqual(testBroker2, results.Single());
        }

        [TestMethod]
        public void Filter_WithNoPostalBrokers_ReturnsCopyOfOriginalList()
        {
            var testBroker1 = new UpsBestRateBroker();
            var testBroker2 = new FedExBestRateBroker();
            var brokers = new List<IBestRateShippingBroker> { testBroker1, testBroker2 };

            var testObject = new PostalCounterBrokerFilter();
            var results = testObject.Filter(brokers);

            Assert.AreEqual(testBroker1, results.First());
            Assert.AreEqual(testBroker2, results.Last());
        }
    }
}
