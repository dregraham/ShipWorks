namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// Factory that creates shipping policies
    /// </summary>
    public interface IShippingPolicyFactory
    {
        /// <summary>
        /// Create a shipping policy from the given type string
        /// </summary>
        /// <param name="shipmentTypeCode">The shipment type code the policy is going to be created for.</param>
        /// <param name="policyType">Type of the policy.</param>
        /// <returns>An instance of an IShippingPolicy.</returns>
        IShippingPolicy Create(ShipmentTypeCode shipmentTypeCode, string policyType);
    }
}