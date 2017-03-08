using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Win32.Native;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Shared.IO.KeyboardShortcuts;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Translate keyboard shortcut commands and keys
    /// </summary>
    [Component]
    public class KeyboardShortcutTranslator : IKeyboardShortcutTranslator
    {
        /// <summary>
        /// Get a list of commands for the given keys
        /// </summary>
        public IEnumerable<Func<object, IShipWorksMessage>> GetCommands(VirtualKeys actionKey, KeyboardShortcutModifiers modifiers)
        {
            // Dummy implementation of the keyboard translation for testing
            // TODO: Replace with actual implementation
            if (actionKey == VirtualKeys.W && modifiers == KeyboardShortcutModifiers.Ctrl)
            {
                return new Func<object, IShipWorksMessage>[] { x => new ApplyWeightMessage(x) };
            }

            return Enumerable.Empty<Func<object, IShipWorksMessage>>();
        }

        /// <summary>
        /// Get a list of shortcuts for the given command
        /// </summary>
        public IEnumerable<string> GetShortcuts(KeyboardShortcutCommand command)
        {
            return Enumerable.Empty<string>();
        }
    }
}
