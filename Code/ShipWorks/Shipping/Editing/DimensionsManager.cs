using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Utility;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Provides access to the dimensions profiles
    /// </summary>
    public static class DimensionsManager
    {
        private static TableSynchronizer<DimensionsProfileEntity> synchronizer;
        private static bool needCheckForChanges = false;
        private static ImmutableList<IDimensionsProfileEntity> readOnlyEntities;

        /// <summary>
        /// Initialize DimensionsManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<DimensionsProfileEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (synchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (synchronizer)
            {
                if (synchronizer.Synchronize())
                {
                    synchronizer.EntityCollection.Sort((int) DimensionsProfileFieldIndex.Name, ListSortDirection.Ascending);
                    readOnlyEntities = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToImmutableList();
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Return all the dimensions profiles
        /// </summary>
        public static List<DimensionsProfileEntity> Profiles =>
            EnsureChangesLoaded(() => EntityUtility.CloneEntityCollection(synchronizer.EntityCollection));

        /// <summary>
        /// Return all the dimensions profiles
        /// </summary>
        public static IEnumerable<IDimensionsProfileEntity> ProfilesReadOnly =>
            EnsureChangesLoaded(() => readOnlyEntities);

        /// <summary>
        /// Get the profile with the specified ID, or null if not found.
        /// </summary>
        public static DimensionsProfileEntity GetProfile(long profileID) =>
            Profiles.FirstOrDefault(p => p.DimensionsProfileID == profileID);

        /// <summary>
        /// Get the profile with the specified ID, or null if not found.
        /// </summary>
        public static IDimensionsProfileEntity GetProfileReadOnly(long profileID) =>
            ProfilesReadOnly.FirstOrDefault(p => p.DimensionsProfileID == profileID);

        /// <summary>
        /// Ensure the given adapter has the latest info for its profile
        /// </summary>
        public static void UpdateDimensions(DimensionsAdapter adapter)
        {
            if (adapter.ProfileID == 0)
            {
                return;
            }

            DimensionsProfileEntity profile = GetProfile(adapter.ProfileID);
            if (profile == null)
            {
                adapter.ProfileID = 0;
                adapter.Length = 0;
                adapter.Weight = 0;
                adapter.Height = 0;
                adapter.Weight = 0;
            }
            else
            {
                adapter.Length = profile.Length;
                adapter.Width = profile.Width;
                adapter.Height = profile.Height;
                adapter.Weight = profile.Weight;
            }
        }

        /// <summary>
        /// Perform the operation, ensuring all changes are loaded
        /// </summary>
        private static T EnsureChangesLoaded<T>(Func<T> func)
        {
            lock (synchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                return func();
            }
        }
    }
}
