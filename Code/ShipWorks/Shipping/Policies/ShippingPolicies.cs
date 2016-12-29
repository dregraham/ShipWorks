using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// Central store of shipping policies
    /// </summary>
    public class ShippingPolicies
    {
        private readonly ILookup<ShipmentTypeCode, IShippingPolicy> shippingPolicies;
        private static readonly ShippingPoliciesCache cache = new ShippingPoliciesCache();

        /// <summary>
        /// Constructor. Consuming code should use ShippingPolicies.Current instead of creating a new instance
        /// </summary>
        public ShippingPolicies(IEnumerable<KeyValuePair<ShipmentTypeCode, IShippingPolicy>> policies)
        {
            shippingPolicies = (policies ?? new List<KeyValuePair<ShipmentTypeCode, IShippingPolicy>>())
                .ToLookup(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Load a new set of shipping policies from the given configuration
        /// </summary>
        /// <param name="storeId">Id of the store to which the policies apply</param>
        /// <param name="policyConfiguration"></param>
        public static void Load(long storeId, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> policyConfiguration) =>
            cache.Load(storeId, policyConfiguration);

        /// <summary>
        /// Load a new set of shipping policies from the given configuration
        /// </summary>
        /// <param name="storeId">Id of the store to which the policies apply</param>
        /// <param name="policyConfiguration"></param>
        /// <param name="shippingPolicyFactory">Factory that will be used to create the policies</param>
        public static void Load(long storeId, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> policyConfiguration,
            IShippingPolicyFactory shippingPolicyFactory) =>
            cache.Load(storeId, policyConfiguration, shippingPolicyFactory);

        /// <summary>
        /// Unload a store from the cache
        /// </summary>
        /// <param name="storeId">Id of the store whose policies should be unloaded</param>
        public static void Unload(long storeId) => cache.Unload(storeId);

        /// <summary>
        /// Unload a store from the cache
        /// </summary>
        /// <param name="storeId">Id of the store whose policies should be unloaded</param>
        /// <param name="shippingPolicyFactory"></param>
        public static void Unload(long storeId, IShippingPolicyFactory shippingPolicyFactory) =>
            cache.Unload(storeId, shippingPolicyFactory);

        /// <summary>
        /// Clear the cache of shipping policy data
        /// </summary>
        /// <remarks>This is primarily meant to be used by tests</remarks>
        public static void ClearCache() => cache.ClearCache();

        /// <summary>
        /// Apply any applicable shipping policies
        /// </summary>
        /// <param name="shipmentType">Shipment type</param>
        /// <param name="target">Target to which the policy should apply</param>
        public void Apply(ShipmentTypeCode shipmentType, Object target)
        {
            foreach (IShippingPolicy policy in shippingPolicies[shipmentType].Where(x => x.IsApplicable(target)))
            {
                policy.Apply(target);
            }
        }

        /// <summary>
        /// Current ShippingPolicies
        /// </summary>
        public static ShippingPolicies Current => cache.Current;
    }
}
