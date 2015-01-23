using System;
using System.Collections.Concurrent;
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
        private static readonly ConcurrentDictionary<long, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>>> policyCache 
            = new ConcurrentDictionary<long, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>>>();

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
        public static void Load(long storeId, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> policyConfiguration)
        {
            Load(storeId, policyConfiguration, new ShippingPolicyTypeEnumFactory());
        }

        /// <summary>
        /// Load a new set of shipping policies from the given configuration
        /// </summary>
        /// <param name="storeId">Id of the store to which the policies apply</param>
        /// <param name="policyConfiguration"></param>
        /// <param name="shippingPolicyFactory">Factory that will be used to create the policies</param>
        public static void Load(long storeId, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> policyConfiguration, 
            IShippingPolicyFactory shippingPolicyFactory)
        {
            policyCache.AddOrUpdate(storeId, id => policyConfiguration, (id, orig) => policyConfiguration);

            UpdateCurrentPolicies(shippingPolicyFactory);
        }

        /// <summary>
        /// Unload a store from the cache
        /// </summary>
        /// <param name="storeId">Id of the store whose policies should be unloaded</param>
        public static void Unload(long storeId)
        {
            Unload(storeId, new ShippingPolicyTypeEnumFactory());
        }

        /// <summary>
        /// Unload a store from the cache
        /// </summary>
        /// <param name="storeId">Id of the store whose policies should be unloaded</param>
        /// <param name="shippingPolicyFactory"></param>
        public static void Unload(long storeId, IShippingPolicyFactory shippingPolicyFactory)
        {
            IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> removed = null;
            if (policyCache.TryRemove(storeId, out removed))
            {
                UpdateCurrentPolicies(shippingPolicyFactory);
            }
        }

        /// <summary>
        /// Clear the cache of shipping policy data
        /// </summary>
        /// <remarks>This is primarily meant to be used by tests</remarks>
        public static void ClearCache()
        {
            policyCache.Clear();

            UpdateCurrentPolicies(null);
        }

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
        public static ShippingPolicies Current { get; private set; }

        /// <summary>
        /// Update the policy collection with the currently cached data
        /// </summary>
        /// <param name="shippingPolicyFactory"></param>
        private static void UpdateCurrentPolicies(IShippingPolicyFactory shippingPolicyFactory)
        {
            // Var is used here because the nested generic signature reduces readability
            var policies = policyCache.SelectMany(x => x.Value).SelectMany(shipmentPolicyConfiguration => shipmentPolicyConfiguration.Value
                .GroupBy(x => GetElementValue(x, "Type"))
                .Select(policyElements => CreatePolicy(shippingPolicyFactory, policyElements, shipmentPolicyConfiguration.Key)));

            Current = new ShippingPolicies(policies);
        }

        /// <summary>
        /// Create a policy 
        /// </summary>
        private static KeyValuePair<ShipmentTypeCode, IShippingPolicy> CreatePolicy(IShippingPolicyFactory shippingPolicyFactory, 
            IGrouping<string, XElement> policyElements, ShipmentTypeCode shipmentType)
        {
            IShippingPolicy policy = shippingPolicyFactory.Create(shipmentType, policyElements.Key);

            foreach (string value in policyElements.Select(x => GetElementValue(x, "Config")))
            {
                policy.Configure(value);
            }

            return new KeyValuePair<ShipmentTypeCode, IShippingPolicy>(shipmentType, policy);
        }

        /// <summary>
        /// Get the value of an element with the given name, or an empty string if there is none
        /// </summary>
        private static string GetElementValue(XContainer x, string elementName)
        {
            XElement element = x.Element(elementName);
            return element == null ? string.Empty : element.Value;
        }
    }
}
