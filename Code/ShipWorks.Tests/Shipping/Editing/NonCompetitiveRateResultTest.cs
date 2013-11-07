using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Editing
{
    [TestClass]
    public class NonCompetitiveRateResultTest
    {
        [TestMethod]
        public void Constructor_CopiesAllProperties()
        {
            RateResult originalRate = new RateResult("Test", "3", 12.1m, "Foo") {ServiceLevel = ServiceLevelType.ThreeDays};
            NonCompetitiveRateResult testObject = new NonCompetitiveRateResult(originalRate);

            Assert.AreEqual("Test", testObject.Description);
            Assert.AreEqual("3", testObject.Days);
            Assert.AreEqual(12.1m, testObject.Amount);
            Assert.AreEqual("Foo", testObject.Tag);
            Assert.AreEqual(ServiceLevelType.ThreeDays, testObject.ServiceLevel);
        }

        [TestMethod]
        public void MaskDescription_ShowsDescription_WhenAllResultsAreNonCompetitive()
        {
            NonCompetitiveRateResult testObject = new NonCompetitiveRateResult(new RateResult("Bar", "5"));
            testObject.MaskDescription(new List<RateResult>{testObject, new NonCompetitiveRateResult(new RateResult("Foo", "3"))});

            Assert.AreEqual("Bar", testObject.Description);
        }

        [TestMethod]
        public void MaskDescription_MasksDescription_WhenOneResultIsNotNonCompetitive()
        {
            NonCompetitiveRateResult testObject = new NonCompetitiveRateResult(new RateResult("Bar", "5") { ServiceLevel = ServiceLevelType.ThreeDays});
            testObject.MaskDescription(new List<RateResult> { testObject, new RateResult("Foo", "3") });

            Assert.AreEqual("Undisclosed Three Days", testObject.Description);
        }

        [TestMethod]
        public void MaskDescription_MasksDescription_WhenNoResultsAreNonCompetitive()
        {
            NonCompetitiveRateResult testObject = new NonCompetitiveRateResult(new RateResult("Bar", "5") { ServiceLevel = ServiceLevelType.ThreeDays });
            testObject.MaskDescription(new List<RateResult> { new RateResult("Baz", "7"), new RateResult("Foo", "3") });

            Assert.AreEqual("Undisclosed Three Days", testObject.Description);
        }
    }
}
