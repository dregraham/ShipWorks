using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    public class BestRateExpress1PromotionFootnoteFilterTest
    {
        private BestRateExpress1PromotionFootnoteFilter testObject;

        private Mock<IExpress1SettingsFacade> settings;

        public BestRateExpress1PromotionFootnoteFilterTest()
        {
            settings = new Mock<IExpress1SettingsFacade>();

            testObject = new BestRateExpress1PromotionFootnoteFilter();
        }

        [Fact]
        public void Filter_RemovesDuplicatePromotionalFootnoteFactories()
        {
            List<RateResult> rates = new List<RateResult>
            {
                new RateResult("Result 1",  "1", 3.14m, null),
                new RateResult("Result 2",  "2", 3.14m, null),
                new RateResult("Result 3",  "3", 3.14m, null),
                new RateResult("Result 4",  "4", 3.14m, null),
            };

            for (int i = 0; i < 3; i++)
            {
                rates[i].ShipmentType = ShipmentTypeCode.Endicia;
            }

            rates[3].ShipmentType = ShipmentTypeCode.Usps;

            RateGroup rateGroup = new RateGroup(rates);
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Endicia, settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Usps, settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            Assert.Equal(1, filteredRateGroup.FootnoteFactories.Count());
        }

        [Fact]
        public void Filter_RetainsOnePromotionalFootnoteFactory_WhenRateGroupHasMultiplePromoFootnoteFactories()
        {
            List<RateResult> rates = new List<RateResult>
            {
                new RateResult("Result 1",  "1", 3.14m, null),
                new RateResult("Result 2",  "2", 3.14m, null),
                new RateResult("Result 3",  "3", 3.14m, null),
                new RateResult("Result 4",  "4", 3.14m, null),
            };

            for (int i = 0; i < 3; i++)
            {
                rates[i].ShipmentType = ShipmentTypeCode.Endicia;
            }

            rates[3].ShipmentType = ShipmentTypeCode.Usps;

            RateGroup rateGroup = new RateGroup(rates);
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Endicia, settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Usps, settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            // Already have a test verifying the count, so we know there is already only one item
            IRateFootnoteFactory factory = filteredRateGroup.FootnoteFactories.First();

            Assert.IsAssignableFrom<Express1PromotionRateFootnoteFactory>(factory);
        }

        [Fact]
        public void Filter_RemovesNonEndiciaPromotionalFootnoteFactories_WhenRateGroupHasMultiplePromoFootnoteFactories()
        {
            List<RateResult> rates = new List<RateResult>
            {
                new RateResult("Result 1",  "1", 3.14m, null),
                new RateResult("Result 2",  "2", 3.14m, null),
                new RateResult("Result 3",  "3", 3.14m, null),
                new RateResult("Result 4",  "4", 3.14m, null),
            };

            for (int i = 0; i < 3; i++)
            {
                rates[i].ShipmentType = ShipmentTypeCode.Endicia;
            }

            rates[3].ShipmentType = ShipmentTypeCode.Usps;

            RateGroup rateGroup = new RateGroup(rates);
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Endicia, settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Usps, settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            // Already have a test verifying the count, so we know there is already only one item
            IRateFootnoteFactory factory = filteredRateGroup.FootnoteFactories.First();

            Assert.Equal(ShipmentTypeCode.Endicia, factory.ShipmentTypeCode);
        }

        [Fact]
        public void Filter_RetainsUspsPromotionalFootnoteFactory_WhenRateGroupOnlyHasUspsBasedPromoFootnoteFactory()
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


            RateGroup rateGroup = new RateGroup(rates);
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Usps, settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            // Already have a test verifying the count, so we know there is already only one item
            IRateFootnoteFactory factory = filteredRateGroup.FootnoteFactories.First();

            Assert.Equal(ShipmentTypeCode.Usps, factory.ShipmentTypeCode);
        }
    }
}
