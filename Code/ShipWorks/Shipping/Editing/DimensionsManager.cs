using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model;
using System.ComponentModel;
using ShipWorks.Data;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Provides access to the dimensions profiles
    /// </summary>
    public static class DimensionsManager 
    {
        static TableSynchronizer<DimensionsProfileEntity> synchronizer;
        static bool needCheckForChanges = false;

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
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Return all the dimensions profiles
        /// </summary>
        public static List<DimensionsProfileEntity> Profiles
        {
            get
            {
                lock (synchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return EntityUtility.CloneEntityCollection(synchronizer.EntityCollection);
                }
            }
        }

        /// <summary>
        /// Get the profile with the specified ID, or null if not found.
        /// </summary>
        public static DimensionsProfileEntity GetProfile(long profileID)
        {
            return Profiles.Where(p => p.DimensionsProfileID == profileID).FirstOrDefault();
        }

        /// <summary>
        /// Ensure the givnen adapter has the latest info for its profile
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
    }
}
