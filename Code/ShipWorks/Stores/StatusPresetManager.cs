using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Manages all the available order and order item status values
    /// </summary>
    public static class StatusPresetManager
    {
        static TableSynchronizer<StatusPresetEntity> statusSynchronizer;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            statusSynchronizer = new TableSynchronizer<StatusPresetEntity>();
            CheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static bool CheckForChanges()
        {
            lock (statusSynchronizer)
            {
                if (statusSynchronizer.Synchronize())
                {
                    statusSynchronizer.EntityCollection.Sort((int) StatusPresetFieldIndex.StatusText, ListSortDirection.Ascending);

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Get the default preset for the specified store
        /// </summary>
        public static StatusPresetEntity GetStoreDefault(StoreEntity store, StatusPresetTarget presetTarget)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            lock (statusSynchronizer)
            {
                foreach (StatusPresetEntity preset in statusSynchronizer.EntityCollection)
                {
                    if (preset.Fields.State == SD.LLBLGen.Pro.ORMSupportClasses.EntityState.Deleted)
                    {
                        continue;
                    }

                    if (preset.StoreID == store.StoreID &&
                        preset.StatusTarget == (int) presetTarget &&
                        preset.IsDefault)
                    {
                        return EntityUtility.CloneEntity(preset);
                    }
                }
            }

            throw new InvalidOperationException(string.Format("No default status preset found for store {0}({1}) and target {2}.", store.StoreName, store.StoreID, presetTarget));
        }

        /// <summary>
        /// Get all the global presets the apply to the specfied type
        /// </summary>
        public static ICollection<StatusPresetEntity> GetGlobalPresets(StatusPresetTarget presetTarget)
        {
            return GetPresets(null, presetTarget);
        }

        /// <summary>
        /// Get all the presets for the specified store that apply to the given type
        /// </summary>
        public static ICollection<StatusPresetEntity> GetStorePresets(StoreEntity store, StatusPresetTarget presetTarget)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            return GetPresets(store, presetTarget);
        }

        /// <summary>
        /// Get a distinct sorted list of the global presets and all presets for every store
        /// </summary>
        public static List<string> GetAllPresets(StatusPresetTarget statusPresetTarget)
        {
            List<string> values = new List<string>();

            // Add all the globals
            values.AddRange(StatusPresetManager.GetGlobalPresets(statusPresetTarget).Select(p => p.StatusText));

            // Add all the store-specifics
            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                values.AddRange(StatusPresetManager.GetStorePresets(store, statusPresetTarget).Select(p => p.StatusText));
            }

            // Sort them
            values.Sort();

            // Remove dupes
            return values.Distinct().ToList();
        }

        /// <summary>
        /// Get all the presets matching the store and preset type
        /// </summary>
        private static ICollection<StatusPresetEntity> GetPresets(StoreEntity store, StatusPresetTarget presetTarget)
        {
            lock (statusSynchronizer)
            {
                List<StatusPresetEntity> presets = new List<StatusPresetEntity>();

                long? storeID = store != null ? store.StoreID : (long?) null;

                foreach (StatusPresetEntity preset in statusSynchronizer.EntityCollection)
                {
                    if (preset.StoreID == storeID &&
                        preset.StatusTarget == (int) presetTarget &&
                        preset.IsDefault == false)
                    {
                        presets.Add(EntityUtility.CloneEntity(preset));
                    }
                }

                return presets;
            }
        }
    }
}
