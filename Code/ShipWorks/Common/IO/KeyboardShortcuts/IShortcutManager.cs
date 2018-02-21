using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// Is the barcode already used by a shortcut?
        /// </summary>
        bool IsBarcodeAvailable(string barcode);

        /// <summary>
        /// Save shortcut
        /// </summary>
        Task Save(ShortcutEntity shortcut);

        /// <summary>
        /// Delete the shortcut associated with the ShippingProfileEntity
        /// </summary>
        Task DeleteShortcutForProfile(ShippingProfileEntity profile, ISqlAdapter adapter);
    }
}