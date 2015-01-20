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
        IShippingPolicy Create(string policyType);
    }
}