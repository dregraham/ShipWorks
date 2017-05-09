using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Shared.IO.KeyboardShortcuts;
using ShipWorks.Users;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Translate keyboard shortcut commands and keys
    /// </summary>
    [Component]
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    public class KeyboardShortcutTranslator : IKeyboardShortcutTranslator
    {
        private readonly Dictionary<VirtualKeys, Dictionary<KeyboardShortcutModifiers, KeyboardShortcutCommand>> shortcuts =
            new Dictionary<VirtualKeys, Dictionary<KeyboardShortcutModifiers, KeyboardShortcutCommand>>
            {
                {
                    VirtualKeys.W,
                    new Dictionary<KeyboardShortcutModifiers, KeyboardShortcutCommand>
                    {
                        {
                            KeyboardShortcutModifiers.Ctrl, KeyboardShortcutCommand.ApplyWeight
                        }
                    }
                }
            };
        private Dictionary<KeyboardShortcutCommand, string> shortcutText;

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardShortcutTranslator()
        {
            shortcutText = shortcuts.SelectMany(CreateShortcutText).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Get a list of commands for the given keys
        /// </summary>
        public KeyboardShortcutCommand? GetCommand(VirtualKeys actionKey, KeyboardShortcutModifiers modifiers)
        {
            Dictionary<KeyboardShortcutModifiers, KeyboardShortcutCommand> modifierCommands = null;

            if (shortcuts.TryGetValue(actionKey, out modifierCommands))
            {
                KeyboardShortcutCommand command;

                if (modifierCommands.TryGetValue(modifiers, out command))
                {
                    return command;
                }
            }

            return null;
        }

        /// <summary>
        /// Get a list of shortcuts for the given command
        /// </summary>
        public string GetShortcut(KeyboardShortcutCommand command)
        {
            string text;

            return shortcutText.TryGetValue(command, out text) ? text : string.Empty;
        }

        /// <summary>
        /// Create text for all commands associated with an action key
        /// </summary>
        private IEnumerable<KeyValuePair<KeyboardShortcutCommand, string>> CreateShortcutText(KeyValuePair<VirtualKeys, Dictionary<KeyboardShortcutModifiers, KeyboardShortcutCommand>> actionKeyShortcuts)
        {
            return actionKeyShortcuts.Value
                .Select(c => new KeyValuePair<KeyboardShortcutCommand, string>(c.Value, new KeyboardShortcutData(c.Value, actionKeyShortcuts.Key, c.Key).ShortcutText));
        }
    }
}
