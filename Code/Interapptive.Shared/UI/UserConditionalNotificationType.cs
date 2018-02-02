using System.Reflection;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Types of user conditional notifications
    /// </summary>
    [Obfuscation(ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum UserConditionalNotificationType
    {
        /// <summary>
        /// Combine orders confirmation dialog
        /// </summary>
        CombineOrders = 1,

        /// <summary>
        /// Split orders confirmation dialog
        /// </summary>
        SplitOrders = 2,

        /// <summary>
        /// Insurance behavior change notification dialog
        /// </summary>
        InsuranceBehaviorChange = 3,
    }
}
