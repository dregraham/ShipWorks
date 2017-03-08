﻿using System;
using System.Collections.Generic;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Win32.Native;
using ShipWorks.Shared.IO.KeyboardShortcuts;

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
        IEnumerable<Func<object, IShipWorksMessage>> GetCommands(VirtualKeys actionKey, KeyboardShortcutModifiers modifiers);

        /// <summary>
        /// Get a list of shortcuts for the given command
        /// </summary>
        IEnumerable<string> GetShortcuts(KeyboardShortcutCommand command);
    }
}
