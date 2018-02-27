using System.Collections.Generic;
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
        List<Hotkey> GetAvailableHotkeys();

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
        void Delete(ShortcutEntity profile, ISqlAdapter adapter);

        /// <summary>
        /// Direct shortcut manager to update shortcuts from database before Shortcuts are returned
        /// </summary>
        void CheckForChangesNeeded();
    }
}