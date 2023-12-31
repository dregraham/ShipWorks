﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32.Native;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Utility;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Users;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Manage shortcuts
    /// </summary>
    [Component(SingleInstance = true)]
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    public class ShortcutManager : IInitializeForCurrentSession, ICheckForChangesNeeded, IShortcutManager
    {
        private TableSynchronizer<ShortcutEntity> tableSynchronizer;
        private bool needCheckForChanges;
        private ReadOnlyCollection<IShortcutEntity> readOnlyEntities;
        private readonly Func<ICurrentUserSettings> currentUserSettingsFactory;
        private readonly IMessageHelper messageHelper;

        // Shortcuts reserved for future use
        private readonly KeyboardShortcutData[] reservedShortcuts =
        {
            new KeyboardShortcutData(null, VirtualKeys.A, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null),
            new KeyboardShortcutData(null, VirtualKeys.C, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null),
            new KeyboardShortcutData(null, VirtualKeys.D, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null),
            new KeyboardShortcutData(null, VirtualKeys.F, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null),
            new KeyboardShortcutData(null, VirtualKeys.O, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null),
            new KeyboardShortcutData(null, VirtualKeys.P, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null),
            new KeyboardShortcutData(null, VirtualKeys.V, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null),
            new KeyboardShortcutData(null, VirtualKeys.W, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null)
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public ShortcutManager(Func<ICurrentUserSettings> currentUserSettingsFactory, IMessageHelper messageHelper)
        {
            this.currentUserSettingsFactory = currentUserSettingsFactory;
            this.messageHelper = messageHelper;
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
                try
                {
                    if (tableSynchronizer.Synchronize())
                    {
                        tableSynchronizer.EntityCollection.Sort((int) ShortcutFieldIndex.Action, ListSortDirection.Ascending);
                        readOnlyEntities = tableSynchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();
                    }

                    needCheckForChanges = false;
                }
                catch (ORMQueryExecutionException ex) when (ex.Message.Contains("Invalid object name \'dbo.Shortcut\'"))
                {
                    // This happens when a user is logged on and has dbo.Shortcut, but then upgrades
                    // to a version of ShipWorks that doesn't have dbo.Shortcut and hits a key
                }
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
            => Shortcuts.SingleOrDefault(s => s.VirtualKey == key && s.ModifierKeys == modifierKeys);

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
        /// Get unused/available hotkeys
        /// </summary>
        public IEnumerable<KeyboardShortcutData> GetAvailableHotkeys()
        {
            List<KeyboardShortcutData> acceptedShortcuts = CreateAcceptedShortcutList();

            RemoveExistingShortcuts(acceptedShortcuts);

            return acceptedShortcuts;
        }

        /// <summary>
        /// Remove existing and reserved shortcuts
        /// </summary>
        private void RemoveExistingShortcuts(List<KeyboardShortcutData> acceptedShortcuts)
        {
            // Remove existing shortcuts from the list of available ones
            foreach (ShortcutEntity shortcut in Shortcuts)
            {
                acceptedShortcuts.RemoveWhere(s => s.ActionKey == shortcut.VirtualKey && s.Modifiers == shortcut.ModifierKeys);
            }

            // Remove reserved shortcuts from the list of available ones
            foreach (KeyboardShortcutData shortcut in reservedShortcuts)
            {
                acceptedShortcuts.RemoveWhere(s => s.ActionKey == shortcut.ActionKey && s.Modifiers == shortcut.Modifiers);
            }
        }

        /// <summary>
        /// Create list of keyboard shortcut data with F5-F9 and Ctrl+Shift+A-Z
        /// </summary>
        private static List<KeyboardShortcutData> CreateAcceptedShortcutList()
        {
            List<KeyboardShortcutData> acceptedShortcuts = new List<KeyboardShortcutData>();

            for (VirtualKeys key = VirtualKeys.F5; key <= VirtualKeys.F9; key++)
            {
                acceptedShortcuts.Add(new KeyboardShortcutData(null, key, KeyboardShortcutModifiers.None, null));
            }

            for (VirtualKeys key = VirtualKeys.N1; key <= VirtualKeys.N9; key++)
            {
                acceptedShortcuts.Add(new KeyboardShortcutData(null, key, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null));
            }

            for (VirtualKeys key = VirtualKeys.A; key <= VirtualKeys.Z; key++)
            {
                acceptedShortcuts.Add(new KeyboardShortcutData(null, key, KeyboardShortcutModifiers.Ctrl | KeyboardShortcutModifiers.Shift, null));
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

        /// <summary>
        /// All the shortcuts
        /// </summary>
        public IEnumerable<IShortcutEntity> ShortcutsReadOnly
        {
            get
            {
                lock (tableSynchronizer)
                {
                    if (needCheckForChanges)
                    {
                        CheckForChanges();
                    }

                    return readOnlyEntities;
                }
            }
        }

        /// <summary>
        /// Get tooltip text for tooltip
        /// </summary>
        public (string Title, string Description) GetCreateLabelToolTipText()
        {
            string hotKey = new KeyboardShortcutData(Shortcuts.FirstOrDefault(s =>
                                s.Action == KeyboardShortcutCommand.CreateLabel))?.ShortcutText ?? "No Shortcut Defined";

            return ($"Create Label ({hotKey})", "Create a shipping label for the selected order.");
        }

        /// <summary>
        /// Show shortcut indicator
        /// </summary>
        public void ShowShortcutIndicator(ShortcutMessage shortcutMessage)
        {
            if (currentUserSettingsFactory().ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator))
            {
                string actionName = EnumHelper.GetDescription(shortcutMessage.Shortcut.Action);

                if (shortcutMessage.Trigger == ShortcutTriggerType.Hotkey)
                {
                    messageHelper.ShowKeyboardPopup($"{shortcutMessage.Value}: {actionName}");
                }
                else
                {
                    messageHelper.ShowBarcodePopup($"Barcode: {actionName}");
                }
            }
        }
    }
}
