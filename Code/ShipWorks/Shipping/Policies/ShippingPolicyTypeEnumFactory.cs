using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// Create shipping policies based on the ShippingPolicyType enumeration
    /// </summary>
    public class ShippingPolicyTypeEnumFactory : IShippingPolicyFactory
    {
        /// <summary>
        /// Create a shipping policy from the given type string
        /// </summary>
        public IShippingPolicy Create(string policyType)
        {
            ShippingPolicyType type = EnumHelper.GetEnumByApiValue<ShippingPolicyType>(policyType);

            switch (type)
            {
                case ShippingPolicyType.BestRateUpsRestriction:
                    return new BestRateUpsRestrictionShippingPolicy();
                case ShippingPolicyType.RateResultCount:
                    return new RateResultCountShippingPolicy();
                default:
                    throw new InvalidOperationException(string.Format("Could not create a policy for type {0}", policyType));
            }
        }
    }
}