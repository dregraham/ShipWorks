﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    [TestClass]
    public class BestRateServiceLevelFilterTest
    {
        private BestRateServiceLevelFilter testObject;

        [TestMethod]
        public void Filter_RatesAreOrderedFromCheapestToMostExpensive_Test()
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

            Assert.AreEqual(rates[1], filteredRates[0]);
            Assert.AreEqual(rates[2], filteredRates[1]);
            Assert.AreEqual(rates[0], filteredRates[2]);
        }

        [TestMethod]
        public void Filter_RatesWithSameCost_AreOrderedByServiceLevel_Test()
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

            Assert.AreEqual(rates[2], filteredRates[0]);
            Assert.AreEqual(rates[4], filteredRates[1]);
            Assert.AreEqual(rates[0], filteredRates[2]);
            Assert.AreEqual(rates[3], filteredRates[3]);
            Assert.AreEqual(rates[1], filteredRates[4]);
        }

        [TestMethod]
        public void Filter_ReturnsAllRates_WhenLessThanFiveRatesAreAvailable_Test()
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

            Assert.AreEqual(rates.Count, filteredRates.Count);
        }

        [TestMethod]
        public void Filter_ReturnsFirstFiveRates_WhenMoreThanFiveRatesAreAvailable_Test()
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

            Assert.AreEqual(rates[6], filteredRates[0]);
            Assert.AreEqual(rates[8], filteredRates[1]);
            Assert.AreEqual(rates[4], filteredRates[2]);
            Assert.AreEqual(rates[7], filteredRates[3]);
            Assert.AreEqual(rates[5], filteredRates[4]);
        }

        [TestMethod]
        public void Filter_ReturnsOneAndTwoDayRates_When2DaysAreSpecifiedAndExpectedDateIsNull_Test()
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

            Assert.AreEqual(rates[6], filteredRates[0]);
            Assert.AreEqual(rates[0], filteredRates[1]);
            Assert.AreEqual(rates[1], filteredRates[2]);
            Assert.AreEqual(rates[2], filteredRates[3]);
            Assert.AreEqual(rates[3], filteredRates[4]);
        }

        [TestMethod]
        public void Filter_ReturnsTwoDayAnd4DayRates_When2DaysAreSpecifiedAndA2DayServiceArivesAfter4DayService_Test()
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

            Assert.AreEqual(rates[7], filteredRates[0]);
            Assert.AreEqual(rates[6], filteredRates[1]);
            Assert.AreEqual(rates[0], filteredRates[2]);
            Assert.AreEqual(rates[1], filteredRates[3]);
            Assert.AreEqual(rates[2], filteredRates[4]);
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

        private void RunQueueTest(RateGroup rateGroup, List<string> correctRateResultDescriptions)
        {
            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            Queue<RateResult> testQueue = new Queue<RateResult>();
            List<RateResult> rates = rateGroup.Rates;

            rates.ForEach(testQueue.Enqueue);

            for (int i = 0; i < rateGroup.Rates.Count; i++)
            {
                Debug.WriteLine("====================================Iteration: " + i + ", " + rates.First().ShipmentType + rates.First().Description);

                testObject = new BestRateServiceLevelFilter(ServiceLevelType.Anytime);
                RateGroup filteredGroup = testObject.Filter(rateGroup);

                // Find the list of correct results based on the rate result description passed in.
                List<RateResult> correctRateResults = rateGroup.Rates.Join(correctRateResultDescriptions,
                                                                 rr => rr.Description,
                                                                 correct => correct,
                                                                 (rr, correct) => rr).ToList();

                // Make sure the counts of correct results matches the rates returned.
                Assert.AreEqual(correctRateResultDescriptions.Count(), filteredGroup.Rates.Count());

                // Check each of the correct results with the returned results to make sure they are correct.
                correctRateResults.ForEach(rr => Assert.AreEqual(rr,
                                                                 filteredGroup.Rates.First(r => r.Description == rr.Description)));

                // Shift the last entry to the first.
                testQueue.Enqueue(testQueue.Dequeue());
                rates = testQueue.ToList();
            }
        }
    }
}
