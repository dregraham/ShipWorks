using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;
using ShipWorks.Shared.IO.KeyboardShortcuts;
using ShipWorks.Users;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Translate keyboard shortcut commands and keys
    /// </summary>
    [Component]
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    public class KeyboardShortcutTranslator : IKeyboardShortcutTranslator, IInitializeForCurrentSession
    {
        private readonly IUserSession userSession;

        private IEnumerable<KeyboardShortcutData> defaultShortcuts = new List<KeyboardShortcutData>
        {
            new KeyboardShortcutData(KeyboardShortcutCommand.ApplyWeight, VirtualKeys.W, KeyboardShortcutModifiers.Ctrl)
        };

        private Dictionary<VirtualKeys, Dictionary<KeyboardShortcutModifiers, ImmutableList<KeyboardShortcutCommand>>> shortcuts;
        private Dictionary<KeyboardShortcutCommand, ImmutableList<string>> shortcutText;

        /// <summary>
        /// Constructor
        /// </summary>
        public KeyboardShortcutTranslator(IUserSession userSession)
        {
            this.userSession = userSession;

            SetCurrentKeyboardShortcuts(Enumerable.Empty<KeyboardShortcutData>());
        }

        /// <summary>
        /// Initialize for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            List<KeyboardShortcutData> shortcutOverrides =
                userSession.User.ShortcutOverrides.Select(x => new KeyboardShortcutData(x)).ToList();

            SetCurrentKeyboardShortcuts(shortcutOverrides);
        }

        /// <summary>
        /// Set the current list of shortcuts using the given overrides
        /// </summary>
        private void SetCurrentKeyboardShortcuts(IEnumerable<KeyboardShortcutData> shortcutOverrides)
        {
            var mergedShortcuts = defaultShortcuts
                .Except(shortcutOverrides, GenericPropertyEqualityComparer.Create((KeyboardShortcutData x) => x.Command))
                .Concat(shortcutOverrides);

            shortcutText = mergedShortcuts.GroupBy(x => x.Command)
                .ToDictionary(x => x.Key, x => x.Select(shortcut => shortcut.ShortcutText).ToImmutableList());

            shortcuts = mergedShortcuts.GroupBy(x => x.ActionKey)
                .ToDictionary(x => x.Key, CreateCommandsForModifiers);
        }

        /// <summary>
        /// Create a list of commands for each modifier
        /// </summary>
        private Dictionary<KeyboardShortcutModifiers, ImmutableList<KeyboardShortcutCommand>> CreateCommandsForModifiers(IEnumerable<KeyboardShortcutData> shortcutList)
        {
            return shortcutList.GroupBy(x => x.Modifiers)
                .ToDictionary(x => x.Key, x => x.Select(k => k.Command).ToImmutableList());
        }

        /// <summary>
        /// Get a list of commands for the given keys
        /// </summary>
        public IEnumerable<KeyboardShortcutCommand> GetCommands(VirtualKeys actionKey, KeyboardShortcutModifiers modifiers)
        {
            Dictionary<KeyboardShortcutModifiers, ImmutableList<KeyboardShortcutCommand>> modifierCommands = null;

            if (shortcuts.TryGetValue(actionKey, out modifierCommands))
            {
                ImmutableList<KeyboardShortcutCommand> commands;

                if (modifierCommands.TryGetValue(modifiers, out commands))
                {
                    return commands;
                }
            }

            return Enumerable.Empty<KeyboardShortcutCommand>();
        }

        /// <summary>
        /// Get a list of shortcuts for the given command
        /// </summary>
        public KeyboardShortcutCommandSummary GetShortcuts(KeyboardShortcutCommand command)
        {
            ImmutableList<string> text;

            return new KeyboardShortcutCommandSummary(command,
                shortcutText.TryGetValue(command, out text) ? text : Enumerable.Empty<string>());
        }
    }
}
