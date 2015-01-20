using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Shipping.Policies
{
    [TestClass]
    public class ShippingPolicyTypeEnumFactoryTest
    {
        [TestMethod]
        public void Create_CreatesRateResultCountPolicy_WithRateResultCountString()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create("RateResultCount");
            Assert.IsInstanceOfType(policy, typeof(RateResultCountShippingPolicy));
        }

        [TestMethod]
        public void Create_CreatesBestRateUpsRestrictionPolicy_WithBestRateUpsRestrictionString()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create("BestRateUpsRestriction");
            Assert.IsInstanceOfType(policy, typeof(BestRateUpsRestrictionShippingPolicy));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Create_ThrowsInvalidOperationException_WhenPolicyTypeIsUnknown()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            factory.Create("InvalidPolicy");
        }
    }
}
