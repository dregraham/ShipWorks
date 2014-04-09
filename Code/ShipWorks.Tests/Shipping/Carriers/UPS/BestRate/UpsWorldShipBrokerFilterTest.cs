﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.BestRate
{
    [TestClass]
    public class UpsWorldShipBrokerFilterTest
    {
        private UpsWorldShipBrokerFilter testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new UpsWorldShipBrokerFilter();
        }

        [TestMethod]
        public void Filter_RemovesWorldShipBroker_WhenListContainsUpsBrokerAndWorldShipBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new UpsBestRateBroker(),
                new WorldShipBestRateBroker()
            };

            List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

            Assert.AreEqual(1, fileredBrokers.Count);
            Assert.AreEqual(0, fileredBrokers.Count(b => b.GetType() == typeof(WorldShipBestRateBroker)));
        }

        [TestMethod]
        public void Filter_KeepsUpsBroker_WhenListContainsUpsBrokerAndWorldShipBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new UpsBestRateBroker(),
                new WorldShipBestRateBroker()
            };

            List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

            Assert.AreEqual(1, fileredBrokers.Count);
            Assert.AreEqual(1, fileredBrokers.Count(b => b.GetType() == typeof(UpsBestRateBroker)));
        }

        [TestMethod]
        public void Filter_DoesNotRemoveWorldShipBroker_WhenListOnlyContainsWorldShipBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new WorldShipBestRateBroker()
            };

            List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

            Assert.AreEqual(1, fileredBrokers.Count);
            Assert.AreEqual(1, fileredBrokers.Count(b => b.GetType() == typeof(WorldShipBestRateBroker)));
        }

        [TestMethod]
        public void Filter_KeepsUpsBroker_WhenListOnlyContainsUpsBroker_Test()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>
            {
                new UpsBestRateBroker()
            };

            List<IBestRateShippingBroker> fileredBrokers = testObject.Filter(brokers).ToList();

            Assert.AreEqual(1, fileredBrokers.Count);
            Assert.AreEqual(1, fileredBrokers.Count(b => b.GetType() == typeof(UpsBestRateBroker)));
        }
    }
}
