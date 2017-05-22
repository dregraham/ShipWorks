using System;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Keyboard shortcut modifiers
    /// </summary>
    [Flags]
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum KeyboardShortcutModifiers
    {
        /// <summary>
        /// No modifiers
        /// </summary>
        [Description("None")]
        None = 0x00,

        /// <summary>
        /// Control key
        /// </summary>
        [Description("Ctrl")]
        Ctrl = 0x01,

        /// <summary>
        /// Shift key
        /// </summary>
        [Description("Shift")]
        Shift = 0x02,

        /// <summary>
        /// Alt key
        /// </summary>
        [Description("Alt")]
        Alt = 0x04
    }
}
