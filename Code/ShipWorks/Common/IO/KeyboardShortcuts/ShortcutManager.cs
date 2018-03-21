using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;
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
        /// Get shortcut for given hotkey
        /// </summary>
        public ShortcutEntity GetShortcut(VirtualKeys key, KeyboardShortcutModifiers modifierKeys)
        {
            key = TranslateKey(key);
            return Shortcuts.SingleOrDefault(s => s.VirtualKey == key && s.ModifierKeys == modifierKeys);
        }
		
        /// <summary>
        /// Get weigh shortcut
        /// </summary>
        /// <remarks>
        /// The first iteration of the weigh shortcut used Ctrl+W, but when other hotkeys were added, we only wanted
        /// users to use hotkeys with Ctrl+Shift as modifiers, so the shortcut was changed to Ctrl+Shift+W, however,
        /// for existing users, Ctrl+W will still be in the database. When getting the shortcut, we always want to
        /// return the Ctrl+Shift+W version. 
        /// </remarks>
        public ShortcutEntity GetWeighShortcut() => Shortcuts.FirstOrDefault(
            s => s.Action == KeyboardShortcutCommand.ApplyWeight &&
                 s.ModifierKeys == (KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift));

        /// <summary>
        /// If a numpad key is padded in, we return the "N" key. else we return the passed in key. 
        /// </summary>
        private VirtualKeys TranslateKey(VirtualKeys key)
        {
            if (key >= VirtualKeys.Numpad0 && key <= VirtualKeys.Numpad9)
            {
                key = VirtualKeys.N0 + (VirtualKeys.Numpad0 - key);
            }

            return key;
        }

        /// <summary>
        /// Get unused/available hotkeys
        /// </summary>
        public IEnumerable<KeyboardShortcutData> GetAvailableHotkeys()
        {
            // Create list of keyboard shortcut data with F5-F9 and Ctrl+Shift+A-Z
            List<KeyboardShortcutData> acceptedShortcuts = new List<KeyboardShortcutData>();

            for (VirtualKeys key = VirtualKeys.F5; key <= VirtualKeys.F9; key++)
            {
                acceptedShortcuts.Add(new KeyboardShortcutData(null, key, KeyboardShortcutModifiers.None));
            }

            for (VirtualKeys key = VirtualKeys.N0; key <= VirtualKeys.N9; key++)
            {
                acceptedShortcuts.Add(new KeyboardShortcutData(null, key, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift));
            }

            for (VirtualKeys key = VirtualKeys.A; key <= VirtualKeys.Z; key++)
            {
                acceptedShortcuts.Add(new KeyboardShortcutData(null, key, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift));
            }
            
            // Remove existing shortcuts from the list of available ones
            foreach (ShortcutEntity shortcut in Shortcuts)
            {
                acceptedShortcuts.RemoveWhere(s => s.ActionKey == shortcut.VirtualKey && s.Modifiers == shortcut.ModifierKeys);
            }
            
            return acceptedShortcuts;
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
