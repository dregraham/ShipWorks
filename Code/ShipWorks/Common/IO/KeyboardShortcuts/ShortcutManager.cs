using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.IO.KeyboardShortcuts;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Manage shortcuts
    /// </summary>
    [Component(SingleInstance = true)]
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    public class ShortcutManager : IInitializeForCurrentSession, ICheckForChangesNeeded, IShortcutManager
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private TableSynchronizer<ShortcutEntity> tableSynchronizer;
        private bool needCheckForChanges;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShortcutManager(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Initializes the object
        /// </summary>
        public void InitializeForCurrentSession()
        {
            tableSynchronizer = new TableSynchronizer<ShortcutEntity>();
            CheckForChanges();
        }

        /// <summary>
        /// Save shortcut
        /// </summary>
        public void Save(ShortcutEntity shortcut, ISqlAdapter adapter)
        {
            adapter.SaveAndRefetch(shortcut);
            CheckForChangesNeeded();
        }

        /// <summary>
        /// Get unused/available hotkeys
        /// </summary>
        public List<Hotkey> GetAvailableHotkeys() =>
            EnumHelper.GetEnumList<Hotkey>().Select(h => h.Value)
                .Where(hotkey => Shortcuts.None(s => s.Hotkey == hotkey))
                .ToList();

        /// <summary>
        /// Is the barcode already used by a shortcut?
        /// </summary>
        public bool IsBarcodeAvailable(string barcode) =>
            Shortcuts.None(s => s.Barcode.Equals(barcode, StringComparison.InvariantCulture));

        /// <summary>
        /// Update local version of shortcuts from the database
        /// </summary>
        private void CheckForChanges()
        {
            lock (tableSynchronizer)
            {
                if (tableSynchronizer.Synchronize())
                {
                    tableSynchronizer.EntityCollection.Sort((int) ShortcutFieldIndex.Action, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Direct shortcut manager to update shortcuts from database before Shortcuts are returned
        /// </summary>
        public void CheckForChangesNeeded()
        {
            lock (tableSynchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Delete the shortcut
        /// </summary>
        public void Delete(ShortcutEntity shortcut, ISqlAdapter adapter)
        {
            adapter.DeleteEntity(shortcut);
            CheckForChangesNeeded();
        }

        /// <summary>
        /// All the shortcuts
        /// </summary>
        public IEnumerable<ShortcutEntity> Shortcuts
        {
            get
            {
                lock (tableSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        CheckForChanges();
                    }

                    return tableSynchronizer.EntityCollection;
                }
            }
        }
    }
}
