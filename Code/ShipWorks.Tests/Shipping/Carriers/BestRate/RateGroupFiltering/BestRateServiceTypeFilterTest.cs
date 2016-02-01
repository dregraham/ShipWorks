﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.RateGroupFiltering
{
    public class BestRateServiceTypeFilterTest
    {
        private BestRateServiceTypeFilter testObject;

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

            testObject = new BestRateServiceTypeFilter();
            List<RateResult> filteredRates = testObject.Filter(rateGroup).Rates;

            Assert.Equal(rates.Count, filteredRates.Count);
        }

        [Fact]
        public void Filter_RatesWithDifferentCosts_ReturnsOneRatePerServiceTypePerTag()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 0.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 1.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 2.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 3.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ), 
                
                CreateRateResult("Rate 2abc", "3", 5.23M, "Tag2", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate 2xyz", "A", 6.23M, "Tag2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 2123", "1", 7.23M, "Tag2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 2456", "S", 8.23M, "Tag2", ServiceLevelType.FourToSevenDays ),
            };

            RateGroup rateGroup = new RateGroup(rates);

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rateGroup, new List<string>() { "Rate abc", "Rate 2abc" });
        }

        [Fact]
        public void Filter_RatesWithSameCost_ReturnsEndiciaForEachResultKey()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ), 
                
                CreateRateResult("Rate 2abc", "3", 4.23M, "Tag2", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate 2xyz", "A", 4.23M, "Tag2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 2123", "1", 4.23M, "Tag2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 2456", "S", 4.23M, "Tag2", ServiceLevelType.FourToSevenDays ),
            };
            rates[0].ShipmentType = ShipmentTypeCode.FedEx;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Usps;
            rates[3].ShipmentType = ShipmentTypeCode.Usps;
            rates[4].ShipmentType = ShipmentTypeCode.Endicia;

            rates[5].ShipmentType = ShipmentTypeCode.FedEx;
            rates[6].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[7].ShipmentType = ShipmentTypeCode.Endicia;
            rates[8].ShipmentType = ShipmentTypeCode.Usps;

            RateGroup rateGroup = new RateGroup(rates);

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rateGroup, new List<string>() { "Rate 789", "Rate 2123" });
        }
        
        [Fact]
        public void Filter_RatesWithSameCost_ReturnsExpress1UspsForResultKey()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ),                
            };
            rates[0].ShipmentType = ShipmentTypeCode.Endicia;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Express1Usps;
            rates[3].ShipmentType = ShipmentTypeCode.Usps;
            rates[4].ShipmentType = ShipmentTypeCode.Endicia;

            RateGroup rateGroup = new RateGroup(rates);

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rateGroup, new List<string>() { "Rate 123" });
        }

        [Fact]
        public void Filter_RatesWithSameCost_ReturnsExpress1EndiciaForEachResultKey()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ), 
                
                CreateRateResult("Rate 2abc", "3", 4.23M, "Tag2", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate 2xyz", "A", 4.23M, "Tag2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 2123", "1", 4.23M, "Tag2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 2456", "S", 4.23M, "Tag2", ServiceLevelType.FourToSevenDays ),
            };
            rates[0].ShipmentType = ShipmentTypeCode.Express1Endicia;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Usps;
            rates[3].ShipmentType = ShipmentTypeCode.Endicia;
            rates[4].ShipmentType = ShipmentTypeCode.Express1Usps;

            rates[5].ShipmentType = ShipmentTypeCode.FedEx;
            rates[6].ShipmentType = ShipmentTypeCode.Express1Endicia;
            rates[7].ShipmentType = ShipmentTypeCode.Express1Usps;
            rates[8].ShipmentType = ShipmentTypeCode.Usps;

            RateGroup rateGroup = new RateGroup(rates);

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rateGroup, new List<string>() { "Rate abc", "Rate 2xyz" });
        }

        [Fact]
        public void Filter_RatesWithSameCost_ReturnsEndiciaForResultKey()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ),                
            };
            rates[0].ShipmentType = ShipmentTypeCode.FedEx;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Usps;
            rates[3].ShipmentType = ShipmentTypeCode.Usps;
            rates[4].ShipmentType = ShipmentTypeCode.Endicia;

            RateGroup rateGroup = new RateGroup(rates);

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rateGroup, new List<string>() { "Rate 789" });
        }

        [Fact]
        public void Filter_RatesWithSameCost_ReturnsExpress1EndiciaForResultKey()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ),    
                CreateRateResult("Rate 000", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),  
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),            
            };
            rates[0].ShipmentType = ShipmentTypeCode.Express1Usps;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Endicia;
            rates[3].ShipmentType = ShipmentTypeCode.Express1Usps;
            rates[4].ShipmentType = ShipmentTypeCode.Express1Endicia;

            RateGroup rateGroup = new RateGroup(rates);

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rateGroup, new List<string>() { "Rate 123" });
        }

        [Fact]
        public void Filter_RatesWithSameCost_ReturnsExpress1UspsForEachResultKey()
        {
            // Setup the broker to return specific rates
            List<RateResult> rates = new List<RateResult>
            {
                CreateRateResult("Rate abc", "3", 4.23M, "Tag1", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate xyz", "A", 4.23M, "Tag1", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 123", "1", 4.23M, "Tag1", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 456", "S", 4.23M, "Tag1", ServiceLevelType.FourToSevenDays ),
                CreateRateResult("Rate 789", "2", 4.23M, "Tag1", ServiceLevelType.TwoDays ), 
                
                CreateRateResult("Rate 2abc", "3", 4.23M, "Tag2", ServiceLevelType.ThreeDays),
                CreateRateResult("Rate 2xyz", "A", 4.23M, "Tag2", ServiceLevelType.Anytime ),
                CreateRateResult("Rate 2123", "1", 4.23M, "Tag2", ServiceLevelType.OneDay ),
                CreateRateResult("Rate 2456", "S", 4.23M, "Tag2", ServiceLevelType.FourToSevenDays ),
            };
            rates[0].ShipmentType = ShipmentTypeCode.FedEx;
            rates[1].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[2].ShipmentType = ShipmentTypeCode.Usps;
            rates[3].ShipmentType = ShipmentTypeCode.Usps;
            rates[4].ShipmentType = ShipmentTypeCode.Express1Usps;

            rates[5].ShipmentType = ShipmentTypeCode.FedEx;
            rates[6].ShipmentType = ShipmentTypeCode.UpsOnLineTools;
            rates[7].ShipmentType = ShipmentTypeCode.Express1Usps;
            rates[8].ShipmentType = ShipmentTypeCode.Usps;

            RateGroup rateGroup = new RateGroup(rates);

            // To test that order in the list doesn't matter, we'll create a queue and loaded it with the initial list
            // Then we'll go into a for loop where get get rates, then shift the last RateResult to the top of the queue
            // and get rates again.
            RunQueueTest(rateGroup, new List<string>() { "Rate 789", "Rate 2123" });
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

                testObject = new BestRateServiceTypeFilter();
                RateGroup filteredGroup = testObject.Filter(rateGroup);

                // Find the list of correct results based on the rate result description passed in.
                List<RateResult> correctRateResults = rateGroup.Rates.Join(correctRateResultDescriptions,
                                                                 rr => rr.Description,
                                                                 correct => correct,
                                                                 (rr, correct) => rr).ToList();

                // Make sure the counts of correct results matches the rates returned.
                Assert.Equal(correctRateResultDescriptions.Count(), filteredGroup.Rates.Count());

                // Check each of the correct results with the returned results to make sure they are correct.
                correctRateResults.ForEach(rr => Assert.Equal(rr,
                                                                 filteredGroup.Rates.First(r => r.Description == rr.Description)));

                // Shift the last entry to the first.
                testQueue.Enqueue(testQueue.Dequeue());
                rates = testQueue.ToList();
            }
        }
    }
}
