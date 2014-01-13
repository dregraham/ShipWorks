using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    [TestClass]
    public class BestRateEventsDescriptionTest
    {
        private BestRateEventsDescription testObject;

        [TestMethod]
        public void ToString_ReturnsNone_WhenNoneIsOnlyFlag_Test()
        {
            testObject = new BestRateEventsDescription(BestRateEventTypes.None);

            Assert.AreEqual(EnumHelper.GetDescription(BestRateEventTypes.None), testObject.ToString());
        }

        [TestMethod]
        public void ToString_ExcludesNone_WhenValueHasOneOtherValue_Test()
        {
            testObject = new BestRateEventsDescription(BestRateEventTypes.None | BestRateEventTypes.RatesCompared);

            Assert.AreEqual(EnumHelper.GetDescription(BestRateEventTypes.RatesCompared), testObject.ToString());
        }

        [TestMethod]
        public void ToString_ExcludesNone_WhenValueHasMultipleFlags_Test()
        {
            testObject = new BestRateEventsDescription(BestRateEventTypes.RatesCompared | BestRateEventTypes.RateSelected);

            string expected = string.Format("{0}, {1}", EnumHelper.GetDescription(BestRateEventTypes.RatesCompared), EnumHelper.GetDescription(BestRateEventTypes.RateSelected));
            Assert.AreEqual(expected, testObject.ToString());
        }
    }
}
