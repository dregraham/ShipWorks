using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Shipping.Policies
{
    [TestClass]
    public class ShippingPolicyTypeEnumFactoryTest
    {
        [TestMethod]
        public void Create_CreatesRateResultCountPolicy_WithRateResultCountString_WhenShipmentTypeIsBestRate_Test()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.BestRate, "RateResultCount");

            Assert.IsInstanceOfType(policy, typeof(RateResultCountShippingPolicy));
        }

        [TestMethod]
        public void Create_CreatesNullShippingPolicy_WhenShipmentTypeIsNotBestRate_Test()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.Stamps, "RateResultCount");

            Assert.IsInstanceOfType(policy, typeof(NonRestrictedRateCountShippingPolicy));
        }

        [TestMethod]
        public void Create_CreatesBestRateUpsRestrictionPolicy_WithBestRateUpsRestrictionString_WhenShipmentTypeIsBestRate_Test()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.BestRate, "BestRateUpsRestriction");

            Assert.IsInstanceOfType(policy, typeof(BestRateUpsRestrictionShippingPolicy));
        }

        [TestMethod]
        public void Create_CreatesBestRateUpsRestrictionPolicy_WithBestRateUpsRestrictionString_WhenShipmentTypeIsNotBestRate_Test()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.FedEx, "BestRateUpsRestriction");

            Assert.IsInstanceOfType(policy, typeof(BestRateUpsRestrictionShippingPolicy));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Create_ThrowsInvalidOperationException_WhenPolicyTypeIsUnknown()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            factory.Create(ShipmentTypeCode.UpsOnLineTools, "InvalidPolicy");
        }
    }
}
