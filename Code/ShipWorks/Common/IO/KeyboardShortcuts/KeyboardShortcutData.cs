using System;
using System.Reflection;
using Interapptive.Shared.Win32.Native;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shared.IO.KeyboardShortcuts;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Container for shortcut data
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    internal class KeyboardShortcutData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardShortcutData(KeyboardShortcutCommand command,
            VirtualKeys actionKey, KeyboardShortcutModifiers modifiers)
        {
            Command = command;
            ActionKey = actionKey;
            Modifiers = modifiers;
        }

        /// <summary>
        /// Keyboard shortcut command
        /// </summary>
        public KeyboardShortcutCommand Command { get; }

        /// <summary>
        /// Action key for keyboard shortcut
        /// </summary>
        public VirtualKeys ActionKey { get; }

        /// <summary>
        /// Modifiers for the action key
        /// </summary>
        public KeyboardShortcutModifiers Modifiers { get; }

        /// <summary>
        /// Get the shortcut text
        /// </summary>
        public string ShortcutText
        {
            get
            {
                return (Modifiers.HasFlag(KeyboardShortcutModifiers.Ctrl) ? "Ctrl+" : string.Empty) +
                    (Modifiers.HasFlag(KeyboardShortcutModifiers.Alt) ? "Alt+" : string.Empty) +
                    (Modifiers.HasFlag(KeyboardShortcutModifiers.Shift) ? "Shift+" : string.Empty) +
                    ActionKey.ToString();
            }
        }
    }
}
