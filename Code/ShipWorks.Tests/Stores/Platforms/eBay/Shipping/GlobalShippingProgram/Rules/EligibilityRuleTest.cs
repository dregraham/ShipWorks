using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram.Rules;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Stores.eBay.Shipping.GlobalShippingProgram.Rules
{
    public class EligibilityRuleTest
    {
        private EligibilityRule testObject;

        public EligibilityRuleTest()
        {
            testObject = new EligibilityRule();
        }

        [Fact]
        public void Evaluate_ReturnsTrue_WhenIsEligibleForGlobalShippingIsTrue()
        {
            EbayOrderEntity ebayOrder = new EbayOrderEntity();
            ebayOrder.GspEligible = true;

            Assert.True(testObject.Evaluate(ebayOrder));
        }

        [Fact]
        public void Evaluate_ReturnsFalse_WhenIsEligibleForGlobalShippingIsFalse()
        {
            EbayOrderEntity ebayOrder = new EbayOrderEntity();
            ebayOrder.GspEligible = false;

            Assert.False(testObject.Evaluate(ebayOrder));
        }

        [Fact]
        public void Evaluate_ReturnsFalse_WhenOrderIsNull()
        {
            EbayOrderEntity ebayOrder = null;
            Assert.False(testObject.Evaluate(ebayOrder));
        }
    }
}
