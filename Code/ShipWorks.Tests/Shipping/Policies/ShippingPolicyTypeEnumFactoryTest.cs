using System;
using Xunit;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Policies;

namespace ShipWorks.Tests.Shipping.Policies
{
    public class ShippingPolicyTypeEnumFactoryTest
    {
        [Fact]
        public void Create_CreatesRateResultCountPolicy_WithRateResultCountString_WhenShipmentTypeIsBestRate()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.BestRate, "RateResultCount");

            Assert.IsAssignableFrom<RateResultCountShippingPolicy>(policy);
        }

        [Fact]
        public void Create_CreatesNullShippingPolicy_WhenShipmentTypeIsNotBestRate()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.Usps, "RateResultCount");

            Assert.IsAssignableFrom<NonRestrictedRateCountShippingPolicy>(policy);
        }

        [Fact]
        public void Create_CreatesBestRateUpsRestrictionPolicy_WithBestRateUpsRestrictionString_WhenShipmentTypeIsBestRate()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.BestRate, "BestRateUpsRestriction");

            Assert.IsAssignableFrom<BestRateUpsRestrictionShippingPolicy>(policy);
        }

        [Fact]
        public void Create_CreatesBestRateUpsRestrictionPolicy_WithBestRateUpsRestrictionString_WhenShipmentTypeIsNotBestRate()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            IShippingPolicy policy = factory.Create(ShipmentTypeCode.FedEx, "BestRateUpsRestriction");

            Assert.IsAssignableFrom<BestRateUpsRestrictionShippingPolicy>(policy);
        }

        [Fact]
        public void Create_ThrowsInvalidOperationException_WhenPolicyTypeIsUnknown()
        {
            ShippingPolicyTypeEnumFactory factory = new ShippingPolicyTypeEnumFactory();
            Assert.Throws<InvalidOperationException>(() => factory.Create(ShipmentTypeCode.UpsOnLineTools, "InvalidPolicy"));
        }
    }
}
