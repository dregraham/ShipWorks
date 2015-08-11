using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram.Rules;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Tests.Stores.eBay.Shipping.GlobalShippingProgram.Rules
{
    public class SelectedShippingMethodRuleTest
    {
        private SelectedShippingMethodRule testObject;

        public SelectedShippingMethodRuleTest()
        {
            testObject = new SelectedShippingMethodRule();
        }

        [Fact]
        public void Evaluate_ReturnsTrue_WhenSelectedShippingMethodIsGSP_Test()
        {
            EbayOrderEntity ebayOrder = new EbayOrderEntity();
            ebayOrder.SelectedShippingMethod = (int)EbayShippingMethod.GlobalShippingProgram;

            Assert.IsTrue(testObject.Evaluate(ebayOrder));
        }

        [Fact]
        public void Evaluate_ReturnsFalse_WhenSelectedShippingMethodIsDirectToBuyer_Test()
        {
            EbayOrderEntity ebayOrder = new EbayOrderEntity();
            ebayOrder.SelectedShippingMethod = (int)EbayShippingMethod.DirectToBuyer;

            Assert.IsFalse(testObject.Evaluate(ebayOrder));
        }

        [Fact]
        public void Evaluate_ReturnsFalse_WhenOrderIsNull_Test()
        {
            EbayOrderEntity ebayOrder = null;
            Assert.IsFalse(testObject.Evaluate(ebayOrder));
        }
    }
}
