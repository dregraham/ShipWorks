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
        /// <param name="shipmentTypeCode">The shipment type code the policy is going to be created for.</param>
        /// <param name="policyType">Type of the policy.</param>
        /// <returns>An instance of an IShippingPolicy.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the policy type is not recognized.</exception>
        public IShippingPolicy Create(ShipmentTypeCode shipmentTypeCode, string policyType)
        {
            ShippingPolicyType type = EnumHelper.GetEnumByApiValue<ShippingPolicyType>(policyType);

            switch (type)
            {
                case ShippingPolicyType.BestRateUpsRestriction:
                    return new BestRateUpsRestrictionShippingPolicy();

                case ShippingPolicyType.RateResultCount:
                {
                    if (shipmentTypeCode == ShipmentTypeCode.BestRate)
                    {
                        // Rate result count restrictions only applies to BestRate for now. The rate control, 
                        // BestRateResultTag, and the service control(s) will need to be refactored to 
                        // support this across all shipment types
                        return new RateResultCountShippingPolicy();
                    }
                    
                    return new NonRestrictedRateCountShippingPolicy();
                }

                default:
                    throw new InvalidOperationException(string.Format("Could not create a policy for type {0}", policyType));
            }
        }
    }
}