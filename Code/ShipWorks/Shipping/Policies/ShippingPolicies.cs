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
        /// <param name="policyConfiguration"></param>
        public static void Load(IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> policyConfiguration)
        {
            Load(policyConfiguration, new ShippingPolicyTypeEnumFactory());
        }

        /// <summary>
        /// Load a new set of shipping policies from the given configuration
        /// </summary>
        /// <param name="policyConfiguration"></param>
        /// <param name="shippingPolicyFactory">Factory that will be used to create the policies</param>
        public static void Load(IEnumerable<KeyValuePair<ShipmentTypeCode, IEnumerable<XElement>>> policyConfiguration, 
            IShippingPolicyFactory shippingPolicyFactory)
        {
            // Var is used here because the nested generic signature reduces readability
            var policies = policyConfiguration.SelectMany(shipmentPolicyConfiguration => shipmentPolicyConfiguration.Value
                .GroupBy(x => GetElementValue(x, "Type"))
                .Select(policyElements => CreatePolicy(shippingPolicyFactory, policyElements, shipmentPolicyConfiguration.Key)));

            Current = new ShippingPolicies(policies);
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
        /// Create a policy 
        /// </summary>
        private static KeyValuePair<ShipmentTypeCode, IShippingPolicy> CreatePolicy(IShippingPolicyFactory shippingPolicyFactory, 
            IGrouping<string, XElement> policyElements, ShipmentTypeCode shipmentType)
        {
            IShippingPolicy policy = shippingPolicyFactory.Create(policyElements.Key);

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
