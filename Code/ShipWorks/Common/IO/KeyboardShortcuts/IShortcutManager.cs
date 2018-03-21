using System.Collections.Generic;
using System.Windows.Input;
using Interapptive.Shared.Win32.Native;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.IO.KeyboardShortcuts;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Manage shortcuts
    /// </summary>
    public interface IShortcutManager
    {
        /// <summary>
        /// Get unused/available hotkeys
        /// </summary>
        IEnumerable<KeyboardShortcutData> GetAvailableHotkeys();

        /// <summary>
        /// All of the shortcuts
        /// </summary>
        /// <returns></returns>
        IEnumerable<ShortcutEntity> Shortcuts { get; }

        /// <summary>
        /// Save shortcut
        /// </summary>
        void Save(ShortcutEntity shortcut, ISqlAdapter adapter);

        /// <summary>
        /// Delete the shortcut
        /// </summary>
        void Delete(ShortcutEntity shortcut, ISqlAdapter adapter);

        /// <summary>
        /// Direct shortcut manager to update shortcuts from database before Shortcuts are returned
        /// </summary>
        void CheckForChangesNeeded();

        /// <summary>
        /// Get shortcut for given hotkey
        /// </summary>
        ShortcutEntity GetShortcut(VirtualKeys key, ModifierKeys modifierKeys);
    }
}