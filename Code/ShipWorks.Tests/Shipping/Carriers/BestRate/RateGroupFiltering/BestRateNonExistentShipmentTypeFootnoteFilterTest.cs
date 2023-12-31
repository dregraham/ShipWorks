﻿using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    public class BestRateNonExistentShipmentTypeFootnoteFilterTest
    {
        private BestRateNonExistentShipmentTypeFootnoteFilter testObject;
        private Mock<IExpress1SettingsFacade> settings;

        public BestRateNonExistentShipmentTypeFootnoteFilterTest()
        {
            settings = new Mock<IExpress1SettingsFacade>();

            testObject = new BestRateNonExistentShipmentTypeFootnoteFilter();
        }

        [Fact]
        public void Filter_RemovesFootnoteFactories_ForShipmentTypesNotInRates()
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
                rate.ShipmentType = ShipmentTypeCode.Usps;
            }

            // Setup our rate group to have footnote factories for shipment types other than the ones in the rate results
            RateGroup rateGroup = new RateGroup(rates);

            rateGroup.AddFootnoteFactory(new Express1DiscountedRateFootnoteFactory(ShipmentTypeCode.Endicia, rates, rates));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Endicia, settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Usps, settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            // Shouldn't have any footnote factories that aren't for USPS
            Assert.False(filteredRateGroup.FootnoteFactories.Any(f => f.ShipmentTypeCode != ShipmentTypeCode.Usps));
        }


        [Fact]
        public void Filter_RetainsFootnoteFactories_ForShipmentTypesInRates()
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
                rate.ShipmentType = ShipmentTypeCode.Usps;
            }

            // Setup our rate group to have footnote factories for shipment types other than the ones in the rate results
            RateGroup rateGroup = new RateGroup(rates);

            rateGroup.AddFootnoteFactory(new Express1DiscountedRateFootnoteFactory(ShipmentTypeCode.Endicia, rates, rates));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Endicia, settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Usps, settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            Assert.Equal(ShipmentTypeCode.Usps, filteredRateGroup.FootnoteFactories.First().ShipmentTypeCode);
        }

        [Fact]
        public void Filter_RetainsShippingAccountRequiredFootnoteFactory_ForShipmentTypesNotInRates()
        {
            RateGroup rateGroup = RateGroup.ShippingAccountRequiredRateGroup(ShipmentTypeCode.BestRate);

            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Endicia, settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Usps, settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            Assert.Equal(ShipmentTypeCode.BestRate, filteredRateGroup.FootnoteFactories.First().ShipmentTypeCode);
            Assert.IsAssignableFrom<ShippingAccountRequiredForRatingFootnoteFactory>(filteredRateGroup.FootnoteFactories.First());
        }
    }
}
