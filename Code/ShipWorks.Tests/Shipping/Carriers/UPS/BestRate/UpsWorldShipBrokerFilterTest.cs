using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.BestRate
{
    public class UpsWorldShipBrokerFilterTest
    {
        private UpsWorldShipBrokerFilter testObject;

        public UpsWorldShipBrokerFilterTest()
        {
            testObject = new UpsWorldShipBrokerFilter();
        }

        [Fact]
        public void Filter_RemovesWorldShipBroker_WhenListContainsUpsBrokerAndWorldShipBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new UpsBestRateBroker(),
                new WorldShipBestRateBroker()
            };

            List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

            Assert.Equal(1, fileredBrokers.Count);
            Assert.Equal(0, fileredBrokers.Count(b => b.GetType() == typeof(WorldShipBestRateBroker)));
        }

        [Fact]
        public void Filter_KeepsUpsBroker_WhenListContainsUpsBrokerAndWorldShipBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new UpsBestRateBroker(),
                new WorldShipBestRateBroker()
            };

            List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

            Assert.Equal(1, fileredBrokers.Count);
            Assert.Equal(1, fileredBrokers.Count(b => b.GetType() == typeof(UpsBestRateBroker)));
        }

        [Fact]
        public void Filter_DoesNotRemoveWorldShipBroker_WhenListOnlyContainsWorldShipBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new WorldShipBestRateBroker()
            };

            List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

            Assert.Equal(1, fileredBrokers.Count);
            Assert.Equal(1, fileredBrokers.Count(b => b.GetType() == typeof(WorldShipBestRateBroker)));
        }

        [Fact]
        public void Filter_KeepsUpsBroker_WhenListOnlyContainsUpsBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new UpsBestRateBroker()
            };

            List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

            Assert.Equal(1, fileredBrokers.Count);
            Assert.Equal(1, fileredBrokers.Count(b => b.GetType() == typeof(UpsBestRateBroker)));
        }
    }
}
