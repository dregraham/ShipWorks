using System.Reflection;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Types of user conditional notifications
    /// </summary>
    [Obfuscation(ApplyToMembers = true)]
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
    }
}
