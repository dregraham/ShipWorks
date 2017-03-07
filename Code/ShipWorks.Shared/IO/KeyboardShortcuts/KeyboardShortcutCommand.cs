using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shared.IO.KeyboardShortcuts
{
    /// <summary>
    /// List of keyboard shortcut commands
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum KeyboardShortcutCommand
    {
        /// <summary>
        /// Apply the weight in a scale control
        /// </summary>
        [Description("Apply weight")]
        ApplyWeight
    }
}
