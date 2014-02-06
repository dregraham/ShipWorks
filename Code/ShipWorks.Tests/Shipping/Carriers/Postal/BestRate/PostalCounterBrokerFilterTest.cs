using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;

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
    }
}
