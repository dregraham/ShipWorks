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
        [Description("Apply Weight")]
        ApplyWeight = 0,

        /// <summary>
        /// Focus on the quick search box
        /// </summary>
        [Description("Focus Quick Search")]
        FocusQuickSearch = 1,

        /// <summary>
        /// Apply a profile
        /// </summary>
        [Description("Apply Profile")]
        ApplyProfile = 2,
        
        /// <summary>
        /// Create a label
        /// </summary>
        [Description("Create Label")]
        CreateLabel = 3,
        
        /// <summary>
        /// Simulates a Tab key press
        /// </summary>
        [Description("Press Tab Key")]
        Tab = 4,
        
        /// <summary>
        /// Simulates an Escape key press
        /// </summary>
        [Description("Press Escape Key")]
        Escape = 5,
        
        /// <summary>
        /// Simulates an Enter key press
        /// </summary>
        [Description("Press Enter Key")]
        Enter = 6,
        
        /// <summary>
        /// Toggles auto print on/off
        /// </summary>
        [Description("Toggle Single Scan Auto Print")]
        ToggleAutoPrint = 7,

        /// <summary>
        /// Clears the quick search box
        /// </summary>
        [Description("Clear Quick Search")]
        ClearQuickSearch = 8
    }
}
