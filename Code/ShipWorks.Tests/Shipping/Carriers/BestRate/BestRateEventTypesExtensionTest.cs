﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    [TestClass]
    public class BestRateEventTypesExtensionTest
    {
        private BestRateEventTypes testObject;

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsNone_WhenOnlyEventIsNone_Test()
        {
            testObject = BestRateEventTypes.None;

            Assert.AreEqual(BestRateEventTypes.None, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRatesCompared_WhenOnlyEventIsRatesCompared_Test()
        {
            testObject = BestRateEventTypes.RatesCompared;

            Assert.AreEqual(BestRateEventTypes.RatesCompared, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRatesCompared_NoneAndRatesComparedAreSet_Test()
        {
            testObject = BestRateEventTypes.None | BestRateEventTypes.RatesCompared;

            Assert.AreEqual(BestRateEventTypes.RatesCompared, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRateSelected_WhenOnlyEventIsRateSelected_Test()
        {
            testObject = BestRateEventTypes.RateSelected;

            Assert.AreEqual(BestRateEventTypes.RateSelected, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRateSelected_NoneAndRateSelectedAreSet_Test()
        {
            testObject = BestRateEventTypes.None |BestRateEventTypes.RateSelected;

            Assert.AreEqual(BestRateEventTypes.RateSelected, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRateSelected_NoneAndRatesComparedAndRateSelectedAreSet_Test()
        {
            testObject = BestRateEventTypes.None | BestRateEventTypes.RatesCompared | BestRateEventTypes.RateSelected;

            Assert.AreEqual(BestRateEventTypes.RateSelected, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRateAutoSelectedAndProcessed_WhenOnlyEventIsRateAutoSelectedAndProcessed_Test()
        {
            testObject = BestRateEventTypes.RateAutoSelectedAndProcessed;

            Assert.AreEqual(BestRateEventTypes.RateAutoSelectedAndProcessed, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRateAutoSelectedAndProcessed_NoneAndAutoSelectedAndProcessedAreSet_Test()
        {
            testObject = BestRateEventTypes.None | BestRateEventTypes.RateAutoSelectedAndProcessed;

            Assert.AreEqual(BestRateEventTypes.RateAutoSelectedAndProcessed, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRateAutoSelectedAndProcessed_NoneAndRatesComparedAndAutoSelectedAndProcessedAreSet_Test()
        {
            testObject = BestRateEventTypes.None | BestRateEventTypes.RatesCompared | BestRateEventTypes.RateAutoSelectedAndProcessed;

            Assert.AreEqual(BestRateEventTypes.RateAutoSelectedAndProcessed, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRateAutoSelectedAndProcessed_NoneAndRateSelectedAndRateAutoSelectedAndProcessedAreSet_Test()
        {
            testObject = BestRateEventTypes.None | BestRateEventTypes.RateSelected | BestRateEventTypes.RateAutoSelectedAndProcessed;

            Assert.AreEqual(BestRateEventTypes.RateAutoSelectedAndProcessed, testObject.GetLatestBestRateEvent());
        }

        [TestMethod]
        public void GetLatestBestRateEvent_ReturnsRateAutoSelectedAndProcessed_NoneAndRatesComparedAndRateSelectedAndRateAutoSelectedAndProcessedAreSet_Test()
        {
            testObject = BestRateEventTypes.None | BestRateEventTypes.RatesCompared | BestRateEventTypes.RateSelected | BestRateEventTypes.RateAutoSelectedAndProcessed;

            Assert.AreEqual(BestRateEventTypes.RateAutoSelectedAndProcessed, testObject.GetLatestBestRateEvent());
        }
    }
}
