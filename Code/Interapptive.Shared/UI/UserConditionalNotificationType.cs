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

        /// <summary>
        /// Display indicator for shortcuts
        /// </summary>
        ShortcutIndicator = 4,

        /// <summary>
        /// Presort label change
        /// </summary>
        PresortLabelChange = 5,

        /// <summary>
        /// Global post label change
        /// </summary>
        GlobalPostChange = 6,

        /// <summary>
        /// Document envelope customs change
        /// </summary>
        GlobalPostAdvantageChange = 7,
    }
}
