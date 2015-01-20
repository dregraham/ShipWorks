﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Shipping.Policies
{
    [TestClass]
    public class RateResultCountShippingPolicyTest
    {
        private readonly RateResultCountShippingPolicy testObject;

        private RateControl rateControl;

        public RateResultCountShippingPolicyTest()
        {
            testObject = new RateResultCountShippingPolicy();
        }

        [TestInitialize]
        public void Initialize()
        {
            rateControl = new RateControl();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Configure_ThrowsArgumentException_WhenDataIsDecimalFormat_Test()
        {
            testObject.Configure("3.3");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Configure_ThrowsArgumentException_WhenDataHasAlphaCharacters_Test()
        {
            testObject.Configure("3ABC");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Configure_ThrowsArgumentException_WhenDataContainsUnTrimmableWhitespace_Test()
        {
            testObject.Configure("3 4");
        }

        [TestMethod]
        public void Configure_AssignsQuantity_WhenDataIsInteger_Test()
        {
            testObject.Configure("34");
            Assert.AreEqual(34, testObject.RateResultQuantity);
        }

        [TestMethod]
        public void Configure_AssignsQuantity_WhenDataIsInteger_AndContainsTrimmableWhitespace_Test()
        {
            testObject.Configure(" 34 ");
            Assert.AreEqual(34, testObject.RateResultQuantity);
        }

        [TestMethod]
        public void Configure_RetainsGreatestQuantity_WhenPolicyIsConfiguredMultipleTimes_Test()
        {
            testObject.Configure("12");
            testObject.Configure("3");
            testObject.Configure("16");
            testObject.Configure("2");

            Assert.AreEqual(16, testObject.RateResultQuantity);
        }

        [TestMethod]
        public void Configure_QuantityIsOne_WhenDataIsIntegerLessThanOne_Test()
        {
            testObject.Configure("0");

            Assert.AreEqual(1, testObject.RateResultQuantity);
        }

        [TestMethod]
        public void IsApplicable_ReturnsFalse_WhenTargetIsNotRateControl_Test()
        {
            testObject.IsApplicable("a string");
        }

        [TestMethod]
        public void IsApplicable_ReturnsTrue_WhenTargetIsRateControl_Test()
        {
            testObject.IsApplicable(rateControl);
        }

        [TestMethod]
        public void Apply_AssignsShowAllRatesToFalse_Test()
        {
            testObject.Apply(rateControl);

            Assert.IsFalse(rateControl.ShowAllRates);
        }

        [TestMethod]
        public void Apply_AssignsRateResultQuantity_ToRateControlRestrictedRateCount_Test()
        {
            testObject.Configure("400");
            testObject.Apply(rateControl);

            Assert.AreEqual(testObject.RateResultQuantity, rateControl.RestrictedRateCount);
        }

        [TestMethod]
        public void Apply_AssignsShowSingleRateToFalse_WhenConfiguredQuantityIsNotOne_Test()
        {
            testObject.Configure("3");
            testObject.Apply(rateControl);

            Assert.IsFalse(rateControl.ShowSingleRate);
        }

        [TestMethod]
        public void Apply_AssignsShowSingleRateToTrue_WhenConfiguredQuantityIsOne_Test()
        {
            testObject.Configure("1");
            testObject.Apply(rateControl);

            Assert.IsTrue(rateControl.ShowSingleRate);
        }

        [TestMethod]
        public void Apply_DoesNotThrowException_WhenTargetIsNotRateControl_Test()
        {
            testObject.Apply("not a rate control");
        }
    }
}
