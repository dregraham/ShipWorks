using System;
using Xunit;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Shipping.Policies
{
    public class ShippingPolicyTypeEnumFactoryTest
    {
        [Fact]
        public void Create_CreatesRateResultCountPolicy_WithRateResultCountString_WhenShipmentTypeIsBestRate_Test()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.BestRate, "RateResultCount");

            Assert.IsInstanceOfType(policy, typeof(RateResultCountShippingPolicy));
        }

        [Fact]
        public void Create_CreatesNullShippingPolicy_WhenShipmentTypeIsNotBestRate_Test()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.Usps, "RateResultCount");

            Assert.IsInstanceOfType(policy, typeof(NonRestrictedRateCountShippingPolicy));
        }

        [Fact]
        public void Create_CreatesBestRateUpsRestrictionPolicy_WithBestRateUpsRestrictionString_WhenShipmentTypeIsBestRate_Test()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.BestRate, "BestRateUpsRestriction");

            Assert.IsInstanceOfType(policy, typeof(BestRateUpsRestrictionShippingPolicy));
        }

        [Fact]
        public void Create_CreatesBestRateUpsRestrictionPolicy_WithBestRateUpsRestrictionString_WhenShipmentTypeIsNotBestRate_Test()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.FedEx, "BestRateUpsRestriction");

            Assert.IsInstanceOfType(policy, typeof(BestRateUpsRestrictionShippingPolicy));
        }

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Create_ThrowsInvalidOperationException_WhenPolicyTypeIsUnknown()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            factory.Create(ShipmentTypeCode.UpsOnLineTools, "InvalidPolicy");
        }
    }
}
