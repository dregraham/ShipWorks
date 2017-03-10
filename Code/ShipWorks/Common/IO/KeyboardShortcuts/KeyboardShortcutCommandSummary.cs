using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Shared.IO.KeyboardShortcuts;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Summary data for a keyboard shortcut command
    /// </summary>
    public class KeyboardShortcutCommandSummary
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="command"></param>
        /// <param name="shortcuts"></param>
        public KeyboardShortcutCommandSummary(KeyboardShortcutCommand command, IEnumerable<string> shortcuts)
        {
            Command = command;
            Shortcuts = shortcuts.ToImmutableList();
            HasShortcuts = shortcuts.Any();
        }

        /// <summary>
        /// Command for this summary applies
        /// </summary>
        public KeyboardShortcutCommand Command { get; }

        /// <summary>
        /// Description of the command
        /// </summary>
        public string Description => $"Shortcut keys to {EnumHelper.GetDescription(Command)}";

        /// <summary>
        /// Default shortcut
        /// </summary>
        public string DefaultShortcut => Shortcuts.FirstOrDefault();

        /// <summary>
        /// All shortcuts
        /// </summary>
        public IEnumerable<string> Shortcuts { get; }

        /// <summary>
        /// Does the command have any shortcuts
        /// </summary>
        public bool HasShortcuts { get; }

        /// <summary>
        /// Formatted list of shortcuts
        /// </summary>
        public string FormattedShortcutList => String.Join(" or ", Shortcuts.ToArray());
    }
}
