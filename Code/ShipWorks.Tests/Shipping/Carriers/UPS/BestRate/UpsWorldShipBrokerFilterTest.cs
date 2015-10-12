using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;
using ShipWorks.Tests.Shared;

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
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var worldShipBestRateBroker = mock.Create<WorldShipBestRateBroker>();
                var upsBestRateBroker = mock.Create<UpsBestRateBroker>();

                List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
                {
                    upsBestRateBroker,
                    worldShipBestRateBroker
                };

                List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

                Assert.Equal(1, fileredBrokers.Count);
                Assert.Equal(0, fileredBrokers.Count(b => b.GetType() == typeof(WorldShipBestRateBroker)));
            }
        }

        [Fact]
        public void Filter_KeepsUpsBroker_WhenListContainsUpsBrokerAndWorldShipBroker_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var worldShipBestRateBroker = mock.Create<WorldShipBestRateBroker>();
                var upsBestRateBroker = mock.Create<UpsBestRateBroker>();

                List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
                {
                    upsBestRateBroker,
                    worldShipBestRateBroker
                };

                List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

                Assert.Equal(1, fileredBrokers.Count);
                Assert.Equal(1, fileredBrokers.Count(b => b.GetType() == typeof(UpsBestRateBroker)));
            }
        }

        [Fact]
        public void Filter_DoesNotRemoveWorldShipBroker_WhenListOnlyContainsWorldShipBroker_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var worldShipBestRateBroker = mock.Create<WorldShipBestRateBroker>();

                List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
                {
                    worldShipBestRateBroker
                };

                List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

                Assert.Equal(1, fileredBrokers.Count);
                Assert.Equal(1, fileredBrokers.Count(b => b.GetType() == typeof(WorldShipBestRateBroker)));
            }

        }

        [Fact]
        public void Filter_KeepsUpsBroker_WhenListOnlyContainsUpsBroker_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var bestRateBroker = new UpsBestRateBroker(mock.Create<UpsOltShipmentType>());

                List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
                {
                    bestRateBroker
                };

                List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

                Assert.Equal(1, fileredBrokers.Count);
                Assert.Equal(1, fileredBrokers.Count(b => b.GetType() == typeof(UpsBestRateBroker)));
            }
        }
    }
}
