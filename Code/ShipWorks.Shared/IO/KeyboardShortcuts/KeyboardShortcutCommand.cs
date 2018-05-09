using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.IO.KeyboardShortcuts
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

        /// <summary>
        /// Apply a profile
        /// </summary>
        [Description("apply profile")]
        ApplyProfile = 2,
        
        /// <summary>
        /// Create a label
        /// </summary>
        [Description("create label")]
        CreateLabel = 3,
        
        /// <summary>
        /// Simulates a Tab key press
        /// </summary>
        [Description("Press Tab key")]
        Tab = 4,
        
        /// <summary>
        /// Simulates an Escape key press
        /// </summary>
        [Description("Press Escape key")]
        Escape = 5,
        
        /// <summary>
        /// Simulates an Enter key press
        /// </summary>
        [Description("Press Enter key")]
        Enter = 6
    }
}
