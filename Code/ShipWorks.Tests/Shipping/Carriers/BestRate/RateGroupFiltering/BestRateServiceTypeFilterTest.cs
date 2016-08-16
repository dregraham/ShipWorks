using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    public class BestRateServiceTypeFilterTest
    {
        static int createdRateCount = 0;

        [Fact]
        public void Filter_ReturnsCheapestRatePerServiceTypePerTag_WhenAllRatesAreSameCarrier()
        {
            var rates = new[]
            {
                CreateRateResult(1.23M, "Tag1", ServiceLevelType.Anytime, ShipmentTypeCode.UpsOnLineTools),
                CreateRateResult(0.23M, "Tag1", ServiceLevelType.ThreeDays, ShipmentTypeCode.UpsOnLineTools),
                CreateRateResult(5.23M, "Tag2", ServiceLevelType.ThreeDays, ShipmentTypeCode.UpsOnLineTools),
                CreateRateResult(6.23M, "Tag2", ServiceLevelType.Anytime, ShipmentTypeCode.UpsOnLineTools),
            };

            var testObject = new BestRateServiceTypeFilter();
            RateGroup rateGroup = testObject.Filter(new RateGroup(rates));

            Assert.Equal(rateGroup.Rates, new[] { rates[1], rates[2] });
        }

        [Fact]
        public void Filter_ReturnsUsps_WhenUspsAndEndiciaHaveSameCost()
        {
            var rates = new[]
            {
                CreateRateResult(0.23M, "Tag1", ServiceLevelType.Anytime, ShipmentTypeCode.Endicia),
                CreateRateResult(0.23M, "Tag1", ServiceLevelType.ThreeDays, ShipmentTypeCode.Usps),
            };

            var testObject = new BestRateServiceTypeFilter();
            RateGroup rateGroup = testObject.Filter(new RateGroup(rates));

            Assert.Equal(rateGroup.Rates, new[] { rates[1] });
        }

        [Fact]
        public void Filter_ReturnsEndicia_WhenEndiciaAndExpress1UspsSameCost()
        {
            var rates = new[]
            {
                CreateRateResult(0.23M, "Tag1", ServiceLevelType.Anytime, ShipmentTypeCode.Express1Usps),
                CreateRateResult(0.23M, "Tag1", ServiceLevelType.ThreeDays, ShipmentTypeCode.Endicia),
            };

            var testObject = new BestRateServiceTypeFilter();
            RateGroup rateGroup = testObject.Filter(new RateGroup(rates));

            Assert.Equal(rateGroup.Rates, new[] { rates[1] });
        }

        [Fact]
        public void Filter_ReturnsFirstInList_WhenRatesAreTheSameAndDoNotIncludeUspsOrEndicia()
        {
            var rates = new[]
            {
                CreateRateResult(0.23M, "Tag1", ServiceLevelType.Anytime, ShipmentTypeCode.Express1Endicia),
                CreateRateResult(0.23M, "Tag1", ServiceLevelType.ThreeDays, ShipmentTypeCode.Express1Usps),
            };

            var testObject = new BestRateServiceTypeFilter();
            RateGroup rateGroup = testObject.Filter(new RateGroup(rates));

            Assert.Equal(rateGroup.Rates, new[] { rates[0] });
        }

        private RateResult CreateRateResult(decimal amount, string tagResultKey, ServiceLevelType serviceLevel, ShipmentTypeCode shipmentType)
        {
            return new RateResult($"Rate {createdRateCount++}", "0", amount, new BestRateResultTag() { ResultKey = tagResultKey })
            {
                ServiceLevel = serviceLevel,
                ShipmentType = shipmentType
            };
        }
    }
}
