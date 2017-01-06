using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// Cache of shipping policies
    /// </summary>
    /// <remarks>
    /// This was extracted from the static methods of ShippingPolicies to help make tests more stable.
    /// In the future, this could be used directly though IoC.
    /// </remarks>
    public class ShippingPoliciesCache
    {
        private readonly ConcurrentDictionary<long, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>>> policyCache;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingPoliciesCache()
        {
            policyCache = new ConcurrentDictionary<long, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>>>();
        }

        /// <summary>
        /// Load a new set of shipping policies from the given configuration
        /// </summary>
        /// <param name="storeId">Id of the store to which the policies apply</param>
        /// <param name="policyConfiguration"></param>
        public void Load(long storeId, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> policyConfiguration)
        {
            Load(storeId, policyConfiguration, new ShippingPolicyTypeEnumFactory());
        }

        /// <summary>
        /// Load a new set of shipping policies from the given configuration
        /// </summary>
        /// <param name="storeId">Id of the store to which the policies apply</param>
        /// <param name="policyConfiguration"></param>
        /// <param name="shippingPolicyFactory">Factory that will be used to create the policies</param>
        public void Load(long storeId, IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> policyConfiguration,
            IShippingPolicyFactory shippingPolicyFactory)
        {
            policyCache.AddOrUpdate(storeId, id => policyConfiguration, (id, orig) => policyConfiguration);

            UpdateCurrentPolicies(shippingPolicyFactory);
        }

        /// <summary>
        /// Unload a store from the cache
        /// </summary>
        /// <param name="storeId">Id of the store whose policies should be unloaded</param>
        public void Unload(long storeId)
        {
            Unload(storeId, new ShippingPolicyTypeEnumFactory());
        }

        /// <summary>
        /// Unload a store from the cache
        /// </summary>
        /// <param name="storeId">Id of the store whose policies should be unloaded</param>
        /// <param name="shippingPolicyFactory"></param>
        public void Unload(long storeId, IShippingPolicyFactory shippingPolicyFactory)
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
        public void ClearCache()
        {
            policyCache.Clear();

            UpdateCurrentPolicies(null);
        }

        /// <summary>
        /// Current ShippingPolicies
        /// </summary>
        public ShippingPolicies Current { get; private set; }

        /// <summary>
        /// Update the policy collection with the currently cached data
        /// </summary>
        /// <param name="shippingPolicyFactory"></param>
        private void UpdateCurrentPolicies(IShippingPolicyFactory shippingPolicyFactory)
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
        private KeyValuePair<ShipmentTypeCode, IShippingPolicy> CreatePolicy(IShippingPolicyFactory shippingPolicyFactory,
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
        private string GetElementValue(XContainer x, string elementName)
        {
            XElement element = x.Element(elementName);
            return element == null ? string.Empty : element.Value;
        }
    }
}
