using System;
using System.Collections.Generic;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Win32.Native;

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
        IEnumerable<Func<object, IShipWorksMessage>> GetCommands(IEnumerable<VirtualKeys> keys);

        /// <summary>
        /// Get a list of shortcuts for the given command
        /// </summary>
        IEnumerable<string> GetShortcuts(KeyboardShortcutCommand command);
    }
}
