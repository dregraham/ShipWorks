using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram.Rules;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Stores.eBay.Shipping.GlobalShippingProgram.Rules
{
    [TestClass]
    public class EligibilityRuleTest
    {
        private EligibilityRule testObject;

        public EligibilityRuleTest()
        {
            testObject = new EligibilityRule();
        }

        [TestMethod]
        public void Evaluate_ReturnsTrue_WhenIsEligibleForGlobalShippingIsTrue_Test()
        {
            EbayOrderEntity ebayOrder = new EbayOrderEntity();
            ebayOrder.GspEligible = true;

            Assert.IsTrue(testObject.Evaluate(ebayOrder));
        }

        [TestMethod]
        public void Evaluate_ReturnsFalse_WhenIsEligibleForGlobalShippingIsFalse_Test()
        {
            EbayOrderEntity ebayOrder = new EbayOrderEntity();
            ebayOrder.GspEligible = false;

            Assert.IsFalse(testObject.Evaluate(ebayOrder));
        }

        [TestMethod]
        public void Evaluate_ReturnsFalse_WhenOrderIsNull_Test()
        {
            EbayOrderEntity ebayOrder = null;
            Assert.IsFalse(testObject.Evaluate(ebayOrder));
        }
    }
}
