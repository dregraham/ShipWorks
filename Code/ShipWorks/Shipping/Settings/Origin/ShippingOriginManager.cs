using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using System.ComponentModel;
using ShipWorks.Data.Model;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Settings.Origin
{
    /// <summary>
    /// Provides access to postal shippers
    /// </summary>
    public static class ShippingOriginManager
    {
        static TableSynchronizer<ShippingOriginEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            synchronizer = new TableSynchronizer<ShippingOriginEntity>();
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
                    synchronizer.EntityCollection.Sort((int) ShippingOriginFieldIndex.Description, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Return the active list of origins
        /// </summary>
        public static List<ShippingOriginEntity> Origins
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
        /// Get the shipper with the specified ID, or null if not found.
        /// </summary>
        public static ShippingOriginEntity GetOrigin(long originID)
        {
            return Origins.Where(s => s.ShippingOriginID == originID).FirstOrDefault();
        }
    }
}
