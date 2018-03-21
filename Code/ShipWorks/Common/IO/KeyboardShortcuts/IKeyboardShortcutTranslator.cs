﻿using System.Collections.Generic;
using Interapptive.Shared.Win32.Native;
using ShipWorks.IO.KeyboardShortcuts;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Translate keyboard shortcut commands and keys
    /// </summary>
    public interface IKeyboardShortcutTranslator
    {
        /// <summary>
        /// Get a list of commands for the given keys
        /// </summary>
        KeyboardShortcutCommand? GetCommand(VirtualKeys actionKey, KeyboardShortcutModifiers modifiers);

        /// <summary>
        /// Get a list of shortcuts for the given command
        /// </summary>
        string GetShortcut(KeyboardShortcutCommand command);
    }
}
