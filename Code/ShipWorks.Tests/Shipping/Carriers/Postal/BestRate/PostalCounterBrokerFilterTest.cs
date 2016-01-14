using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.BestRate
{
    public class PostalCounterBrokerFilterTest
    {
        [Fact]
        public void Filter_WithMultipleUspsBrokers_ReturnsFirst_Test()
        {
            var testBroker1 = new UspsCounterRatesBroker(new Mock<ICarrierAccountRepository<UspsAccountEntity>>().Object);
            var testBroker2 = new UspsCounterRatesBroker(new Mock<ICarrierAccountRepository<UspsAccountEntity>>().Object);
            var brokers = new List<IBestRateShippingBroker> { testBroker1, testBroker2 };

            var testObject = new PostalCounterBrokerFilter();
            var results = testObject.Filter(brokers);

            Assert.Equal(testBroker1, results.Single());
        }

        [Fact]
        public void Filter_WithNoPostalBrokers_ReturnsCopyOfOriginalList_Test()
        {
            var testBroker1 = new UpsBestRateBroker();
            var testBroker2 = new Mock<IBestRateShippingBroker>().Object;
            var brokers = new List<IBestRateShippingBroker> { testBroker1, testBroker2 };

            var testObject = new PostalCounterBrokerFilter();
            var results = testObject.Filter(brokers);

            Assert.Equal(testBroker1, results.First());
            Assert.Equal(testBroker2, results.Last());
        }
    }
}
