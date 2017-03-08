using System;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Keyboard shortcut modifiers
    /// </summary>
    [Flags]
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
