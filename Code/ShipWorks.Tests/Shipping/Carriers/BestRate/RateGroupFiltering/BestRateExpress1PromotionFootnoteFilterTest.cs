using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    public class BestRateExpress1PromotionFootnoteFilterTest
    {
        private BestRateExpress1PromotionFootnoteFilter testObject;

        private Mock<IExpress1SettingsFacade> settings;

        [TestInitialize]
        public void Initialize()
        {
            settings = new Mock<IExpress1SettingsFacade>();

            testObject = new BestRateExpress1PromotionFootnoteFilter();
        }

        [Fact]
        public void Filter_RemovesDuplicatePromotionalFootnoteFactories_Test()
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
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new EndiciaShipmentType(), settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new UspsShipmentType(), settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            Assert.AreEqual(1, filteredRateGroup.FootnoteFactories.Count());
        }

        [Fact]
        public void Filter_RetainsOnePromotionalFootnoteFactory_WhenRateGroupHasMultiplePromoFootnoteFactories_Test()
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
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new EndiciaShipmentType(), settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new UspsShipmentType(), settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            // Already have a test verifying the count, so we know there is already only one item
            IRateFootnoteFactory factory = filteredRateGroup.FootnoteFactories.First();

            Assert.IsInstanceOfType(factory, typeof(Express1PromotionRateFootnoteFactory));
        }

        [Fact]
        public void Filter_RemovesNonEndiciaPromotionalFootnoteFactories_WhenRateGroupHasMultiplePromoFootnoteFactories_Test()
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
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new EndiciaShipmentType(), settings.Object));
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new UspsShipmentType(), settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            // Already have a test verifying the count, so we know there is already only one item
            IRateFootnoteFactory factory = filteredRateGroup.FootnoteFactories.First();

            Assert.AreEqual(ShipmentTypeCode.Endicia, factory.ShipmentType.ShipmentTypeCode);
        }

        [Fact]
        public void Filter_RetainsUspsPromotionalFootnoteFactory_WhenRateGroupOnlyHasUspsBasedPromoFootnoteFactory_Test()
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
            rateGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(new UspsShipmentType(), settings.Object));

            RateGroup filteredRateGroup = testObject.Filter(rateGroup);

            // Already have a test verifying the count, so we know there is already only one item
            IRateFootnoteFactory factory = filteredRateGroup.FootnoteFactories.First();

            Assert.AreEqual(ShipmentTypeCode.Usps, factory.ShipmentType.ShipmentTypeCode);
        }
    }
}
