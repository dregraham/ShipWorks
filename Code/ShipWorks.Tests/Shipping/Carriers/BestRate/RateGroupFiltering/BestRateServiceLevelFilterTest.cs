using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    public class BestRateServiceLevelFilterTest
    {
        private BestRateServiceLevelFilter testObject;

        [Fact]
        public void Filter_RatesAreOrderedFromCheapestToMostExpensive()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "12", 34.30M, "SomeRateResult", ServiceLevelType.Anytime),
                CreateRateResult("Rate xyz", "12", 4.23M, "SomeRateResult2", ServiceLevelType.Anytime),
                CreateRateResult("Rate 123", "probably 7", 9.87M, "SomeRateResult3", ServiceLevelType.Anytime)
            };

            RateGroup rateGroup = new RateGroup(rates);

            testObject = new BestRateServiceLevelFilter(ServiceLevelType.Anytime);
            List<RateResult> filteredRates = testObject.Filter(rateGroup).Rates;

            Assert.Equal(rates[1], filteredRates[0]);
            Assert.Equal(rates[2], filteredRates[1]);
            Assert.Equal(rates[0], filteredRates[2]);
        }

        [Fact]
        public void Filter_RatesWithSameCost_AreOrderedByServiceLevel()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "SomeRateResult", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult3", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", 4.23M, "SomeRateResult4", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult5", ServiceLevelType.TwoDays ),
            };

            RateGroup rateGroup = new RateGroup(rates);

            testObject = new BestRateServiceLevelFilter(ServiceLevelType.Anytime);
            List<RateResult> filteredRates = testObject.Filter(rateGroup).Rates;

            Assert.Equal(rates[2], filteredRates[0]);
            Assert.Equal(rates[4], filteredRates[1]);
            Assert.Equal(rates[0], filteredRates[2]);
            Assert.Equal(rates[3], filteredRates[3]);
            Assert.Equal(rates[1], filteredRates[4]);
        }

        [Fact]
        public void Filter_ReturnsAllRates_WhenLessThanFiveRatesAreAvailable()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", 4.23M, "SomeRateResult3", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult4", ServiceLevelType.TwoDays ),
            };

            RateGroup rateGroup = new RateGroup(rates);

            testObject = new BestRateServiceLevelFilter(ServiceLevelType.Anytime);
            List<RateResult> filteredRates = testObject.Filter(rateGroup).Rates;

            Assert.Equal(rates.Count, filteredRates.Count);
        }

        [Fact]
        public void Filter_ReturnsFirstFiveRates_WhenMoreThanFiveRatesAreAvailable()
        {

            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult", ServiceLevelType.TwoDays ),
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult2", ServiceLevelType.TwoDays ),
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult3", ServiceLevelType.TwoDays ),
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult4", ServiceLevelType.TwoDays ),  

                // These are the rates that should be returned
                CreateRateResult("Rate abc", "3", 4.23M, "SomeRateResult5", ServiceLevelType.ThreeDays ),
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult6", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult7", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", 4.23M, "SomeRateResult8", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult9", ServiceLevelType.TwoDays ),
            };

            RateGroup rateGroup = new RateGroup(rates);

            testObject = new BestRateServiceLevelFilter(ServiceLevelType.Anytime);
            List<RateResult> filteredRates = testObject.Filter(rateGroup).Rates;

            Assert.Equal(rates[6], filteredRates[0]);
            Assert.Equal(rates[8], filteredRates[1]);
            Assert.Equal(rates[4], filteredRates[2]);
            Assert.Equal(rates[7], filteredRates[3]);
            Assert.Equal(rates[5], filteredRates[4]);
        }

        [Fact]
        public void Filter_ReturnsOneAndTwoDayRates_When2DaysAreSpecifiedAndExpectedDateIsNull()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult", ServiceLevelType.TwoDays ),
                CreateRateResult("Rate 789", "2", 6.88M, "SomeRateResult2", ServiceLevelType.TwoDays ),
                CreateRateResult("Rate 789", "2", 6.89M, "SomeRateResult3", ServiceLevelType.TwoDays ),
                CreateRateResult("Rate 789", "2", 6.90M, "SomeRateResult4", ServiceLevelType.TwoDays ),

                CreateRateResult("Rate abc", "3", 4.23M, "SomeRateResult5", ServiceLevelType.ThreeDays ),
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult6", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult7", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", 4.23M, "SomeRateResult8", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult9", ServiceLevelType.Anytime ),
            };

            RateGroup rateGroup = new RateGroup(rates);

            testObject = new BestRateServiceLevelFilter(ServiceLevelType.TwoDays);
            List<RateResult> filteredRates = testObject.Filter(rateGroup).Rates;

            Assert.Equal(rates[6], filteredRates[0]);
            Assert.Equal(rates[0], filteredRates[1]);
            Assert.Equal(rates[1], filteredRates[2]);
            Assert.Equal(rates[2], filteredRates[3]);
            Assert.Equal(rates[3], filteredRates[4]);
        }

        [Fact]
        public void Filter_ReturnsTwoDayAnd4DayRates_When2DaysAreSpecifiedAndA2DayServiceArivesAfter4DayService()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate 789", "2", 6.87M, "SomeRateResult", ServiceLevelType.TwoDays, DateTime.Today.AddDays(3) ),
                CreateRateResult("Rate 789", "2", 6.88M, "SomeRateResult2", ServiceLevelType.TwoDays ),
                CreateRateResult("Rate 789", "2", 6.89M, "SomeRateResult3", ServiceLevelType.TwoDays ),
                CreateRateResult("Rate 789", "2", 6.90M, "SomeRateResult4", ServiceLevelType.TwoDays ),

                CreateRateResult("Rate abc", "3", 4.23M, "SomeRateResult5", ServiceLevelType.ThreeDays ),
                CreateRateResult("Rate xyz", "It will get there when it gets there", 4.23M, "SomeRateResult6", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "SomeRateResult7", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "Soon", .23M, "SomeRateResult8", ServiceLevelType.FourToSevenDays, DateTime.Today.AddDays(3)),
                CreateRateResult("Rate 789", "2", 4.23M, "SomeRateResult9", ServiceLevelType.Anytime ),
            };

            RateGroup rateGroup = new RateGroup(rates);

            testObject = new BestRateServiceLevelFilter(ServiceLevelType.TwoDays);
            List<RateResult> filteredRates = testObject.Filter(rateGroup).Rates;

            Assert.Equal(rates[7], filteredRates[0]);
            Assert.Equal(rates[6], filteredRates[1]);
            Assert.Equal(rates[0], filteredRates[2]);
            Assert.Equal(rates[1], filteredRates[3]);
            Assert.Equal(rates[2], filteredRates[4]);
        }

        // Helper methods for creating rate results
        private RateResult CreateRateResult(string description, string days, decimal amount, string tagResultKey)
        {
            return new RateResult(description, days, amount, new BestRateResultTag() { ResultKey = tagResultKey });
        }

        private RateResult CreateRateResult(string description, string days, decimal amount, string tagResultKey, ServiceLevelType serviceLevel)
        {
            RateResult rateResult = CreateRateResult(description, days, amount, tagResultKey);
            rateResult.ServiceLevel = serviceLevel;
            return rateResult;
        }

        private RateResult CreateRateResult(string description, string days, decimal amount, string tagResultKey, ServiceLevelType serviceLevel, DateTime expectedDeliveryDate)
        {
            RateResult rateResult = CreateRateResult(description, days, amount, tagResultKey, serviceLevel);
            rateResult.ExpectedDeliveryDate = expectedDeliveryDate;
            return rateResult;
        }
    }
}
