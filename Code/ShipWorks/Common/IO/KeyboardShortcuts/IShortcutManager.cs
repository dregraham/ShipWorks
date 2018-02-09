using System.Collections.Generic;
using System.Threading.Tasks;
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
        List<ShortcutHotkey> GetAvailableHotkeys();

        /// <summary>
        /// Is the barcode already used by a shortcut?
        /// </summary>
        bool IsBarcodeAvailable(string barcode);

        /// <summary>
        /// Save shortcut
        /// </summary>
        Task Save(ShortcutEntity shortcut);
    }
}