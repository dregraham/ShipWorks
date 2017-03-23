using System;
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
        None = 0x00,

        /// <summary>
        /// Control key
        /// </summary>
        Ctrl = 0x01,

        /// <summary>
        /// Shift key
        /// </summary>
        Shift = 0x02,

        /// <summary>
        /// Alt key
        /// </summary>
        Alt = 0x04
    }
}
