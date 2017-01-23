using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.BestRate
{
    public class PostalCounterBrokerFilterTest
    {
        [Fact]
        public void Filter_WithMultipleUspsBrokers_ReturnsFirst()
        {
            var testBroker1 =
                new UspsCounterRatesBroker(
                    new Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>().Object);
            var testBroker2 =
                new UspsCounterRatesBroker(
                    new Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>().Object);
            var brokers = new List<IBestRateShippingBroker> {testBroker1, testBroker2};

            var testObject = new PostalCounterBrokerFilter();
            var results = testObject.Filter(brokers);

            Assert.Equal(testBroker1, results.Single());
        }

        [Fact]
        public void Filter_WithNoPostalBrokers_ReturnsCopyOfOriginalList()
        {
            // Save a copy of all the shipment entities passed into the GetRates method so we can inspect them later
            var shipmentType = new Mock<WorldShipShipmentType>();

            var testBroker1 = new UpsCounterRatesBroker(shipmentType.Object);
            var testBroker2 = new Mock<IBestRateShippingBroker>().Object;
            var brokers = new List<IBestRateShippingBroker> {testBroker1, testBroker2};

            var testObject = new PostalCounterBrokerFilter();
            var results = testObject.Filter(brokers);

            Assert.Equal(testBroker1, results.First());
            Assert.Equal(testBroker2, results.Last());
        }
    }
}

