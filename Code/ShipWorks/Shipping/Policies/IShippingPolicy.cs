
namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// An interface for defining a shipping policy. The shipping policy concept is intended to
    /// be an extension point to dynamically alter behavior at run-time on various aspects of 
    /// shipping. For example, a policy could be created to alter the behavior of the number 
    /// of rate results that appear in the rate grid. The target is the object that the policy
    /// will be altering. In the example of the rate grid, the target may be a grid class of 
    /// some sort. Other examples of targets could be a list of objects and a policy would add
    /// or remove items from the list as needed.
    /// </summary>
    public interface IShippingPolicy
    {
        /// <summary>
        /// Uses the configuration data provided to configure the shipping policy. 
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        void Configure(string configuration);

        /// <summary>
        /// Determines whether the specified target is applicable to the shipping policy.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns><c>true</c> if the specified target is applicable; otherwise, <c>false</c>.</returns>
        bool IsApplicable(object target);

        /// <summary>
        /// Applies the policy to the specified target.
        /// </summary>
        /// <param name="target">The object the policy will be applied towards.</param>
        void Apply(object target);
    }
}
