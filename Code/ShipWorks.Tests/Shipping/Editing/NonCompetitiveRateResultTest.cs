using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Editing
{
    [TestClass]
    public class NonCompetitiveRateResultTest
    {
        [TestMethod]
        public void Constructor_CopiesAllProperties()
        {
            RateResult originalRate = new RateResult("Test", "3", 12.1m, "Foo") { ServiceLevel = ServiceLevelType.ThreeDays, IsCounterRate = true};
            NoncompetitiveRateResult testObject = new NoncompetitiveRateResult(originalRate, "MaskedBar");

            Assert.AreEqual("Test", testObject.Description);
            Assert.AreEqual("3", testObject.Days);
            Assert.AreEqual(12.1m, testObject.Amount);
            Assert.AreEqual("Foo", testObject.Tag);
            Assert.AreEqual(ServiceLevelType.ThreeDays, testObject.ServiceLevel);
            Assert.AreEqual(originalRate, testObject.OriginalRate);
            Assert.AreEqual(originalRate.IsCounterRate, testObject.IsCounterRate);
        }

        [TestMethod]
        public void MaskDescription_ShowsDescription_WhenAllResultsAreNonCompetitive()
        {
            NoncompetitiveRateResult testObject = new NoncompetitiveRateResult(new RateResult("Bar", "5"), "MaskedBar");
            testObject.MaskDescription(new List<RateResult> { testObject, new NoncompetitiveRateResult(new RateResult("Foo", "3"), "MaskedFoo") });

            Assert.AreEqual("Bar", testObject.Description);
        }

        [TestMethod]
        public void MaskDescription_MasksDescription_WhenOneResultIsNotNonCompetitive()
        {
            NoncompetitiveRateResult testObject = new NoncompetitiveRateResult(new RateResult("Bar", "5") { ServiceLevel = ServiceLevelType.ThreeDays }, "MaskedBar");
            testObject.MaskDescription(new List<RateResult> { testObject, new RateResult("Foo", "3") });

            Assert.AreEqual("Undisclosed MaskedBar", testObject.Description);
        }

        [TestMethod]
        public void MaskDescription_MasksDescription_WhenNoResultsAreNonCompetitive()
        {
            NoncompetitiveRateResult testObject = new NoncompetitiveRateResult(new RateResult("Bar", "5") { ServiceLevel = ServiceLevelType.ThreeDays }, "MaskedBar");
            testObject.MaskDescription(new List<RateResult> { new RateResult("Baz", "7"), new RateResult("Foo", "3") });

            Assert.AreEqual("Undisclosed MaskedBar", testObject.Description);
        }
    }
}
