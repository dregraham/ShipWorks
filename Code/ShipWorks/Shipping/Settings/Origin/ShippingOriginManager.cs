using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Utility;

namespace ShipWorks.Shipping.Settings.Origin
{
    /// <summary>
    /// Provides access to postal shippers
    /// </summary>
    public static class ShippingOriginManager
    {
        static TableSynchronizer<ShippingOriginEntity> synchronizer;
        static IEnumerable<IShippingOriginEntity> readOnlyOrigins;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize ShippingOriginManager
        /// </summary>
        public static void InitializeForCurrentSession()
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

                readOnlyOrigins = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();

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
        /// Return the active list of origins
        /// </summary>
        public static IEnumerable<IShippingOriginEntity> OriginsReadOnly
        {
            get
            {
                lock (synchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return readOnlyOrigins;
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

        /// <summary>
        /// Get the shipper with the specified ID, or null if not found.
        /// </summary>
        public static IShippingOriginEntity GetOriginReadOnly(long originID)
        {
            return OriginsReadOnly.FirstOrDefault(x => x.ShippingOriginID == originID);
        }
    }
}
