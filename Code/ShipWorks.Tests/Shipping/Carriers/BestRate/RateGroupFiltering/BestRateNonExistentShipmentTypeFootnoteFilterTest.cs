﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    [TestClass]
    public class BestRateNonExistentShipmentTypeFootnoteFilterTest
    {
        private BestRateNonExistentShipmentTypeFootnoteFilter testObject;
        private Mock<IExpress1SettingsFacade> settings;

        [TestInitialize]
        public void Initialize()
        {
            settings = new Mock<IExpress1SettingsFacade>();

            testObject = new BestRateNonExistentShipmentTypeFootnoteFilter();
        }

        [TestMethod]
        public void Filter_RemovesFootnoteFactories_ForShipmentTypesNotInRates_Test()
        {
            List<RateResult> rates = new List<RateResult>
            {
                new RateResult("Result 1",  "1", 3.14m, null),
                new RateResult("Result 2",  "2", 3.14m, null),
                new RateResult("Result 3",  "3", 3.14m, null),
                new RateResult("Result 4",  "4", 3.14m, null),
            };

            foreach (RateResult rate in rates)
            {
                rate.ShipmentType = ShipmentTypeCode.Stamps;
            }

            // Setup our rate group to have footnote factories for shipment types other than the ones in the rate results 
            RateGroup rateGroup = new RateGroup(rates);

            rateGroup.AddFootnoteFactory(new Express1DiscountedRateFootnoteFactory(new EndiciaShipmentType(), rates, rates));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new EndiciaShipmentType(), settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new StampsShipmentType(), settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            // Shouldn't have any footnote factories that aren't for stamps
            Assert.IsFalse(filteredRateGroup.FootnoteFactories.Any(f => f.ShipmentType.ShipmentTypeCode != ShipmentTypeCode.Stamps));
        }


        [TestMethod]
        public void Filter_RetainsFootnoteFactories_ForShipmentTypesInRates_Test()
        {
            List<RateResult> rates = new List<RateResult>
            {
                new RateResult("Result 1",  "1", 3.14m, null),
                new RateResult("Result 2",  "2", 3.14m, null),
                new RateResult("Result 3",  "3", 3.14m, null),
                new RateResult("Result 4",  "4", 3.14m, null),
            };

            foreach (RateResult rate in rates)
            {
                rate.ShipmentType = ShipmentTypeCode.Stamps;
            }

            // Setup our rate group to have footnote factories for shipment types other than the ones in the rate results 
            RateGroup rateGroup = new RateGroup(rates);

            rateGroup.AddFootnoteFactory(new Express1DiscountedRateFootnoteFactory(new EndiciaShipmentType(), rates, rates));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new EndiciaShipmentType(), settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new StampsShipmentType(), settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            Assert.AreEqual(ShipmentTypeCode.Stamps, filteredRateGroup.FootnoteFactories.First().ShipmentType.ShipmentTypeCode);
        }
    }
}
