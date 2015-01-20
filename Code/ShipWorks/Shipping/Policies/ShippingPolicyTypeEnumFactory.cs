using System;

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
            throw new NotImplementedException();
        }
    }
}