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
        private ILookup<ShipmentTypeCode, IEnumerable<IShippingPolicy>> shippingPolicies;

        internal ShippingPolicies(IEnumerable<KeyValuePair<ShipmentTypeCode, List<XElement>>> policyConfiguration)
        {
               
        }

        /// <summary>
        /// Load a new set of shipping policies from the given configuration
        /// </summary>
        /// <param name="policyConfiguration"></param>
        public static void Load(IEnumerable<KeyValuePair<ShipmentTypeCode, List<XElement>>> policyConfiguration)
        {
            Current = new ShippingPolicies(policyConfiguration);
        }

        /// <summary>
        /// Apply any applicable shipping policies 
        /// </summary>
        /// <param name="shipmentType"></param>
        /// <param name="target"></param>
        public void Apply(ShipmentTypeCode shipmentType, Object target)
        {

        }

        /// <summary>
        /// Current ShippingPolicies
        /// </summary>
        public static ShippingPolicies Current { get; private set; }
    }
}
