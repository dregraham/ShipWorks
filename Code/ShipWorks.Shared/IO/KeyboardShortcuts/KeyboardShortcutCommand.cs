using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shared.IO.KeyboardShortcuts
{
    /// <summary>
    /// List of keyboard shortcut commands
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum KeyboardShortcutCommand
    {
        /// <summary>
        /// Apply the weight in a scale control
        /// </summary>
        [Description("apply weight")]
        ApplyWeight = 0,

        /// <summary>
        /// Focus on the quick search box
        /// </summary>
        /// <remarks>This is only used for testing at the moment</remarks>
        [Description("focus quick search")]
        FocusQuickSearch = 1,
    }
}
