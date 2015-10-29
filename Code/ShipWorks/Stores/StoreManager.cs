using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using System.ComponentModel;
using System.Windows.Forms;
using ShipWorks.UI;
using System.Linq;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Platforms;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Stores.Management;
using ShipWorks.ApplicationCore.Setup;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Algorithms and functions for working with stores.
    /// </summary>
    public static class StoreManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(StoreManager));

        static TableSynchronizer<StoreEntity> storeSynchronizer;

        // These could easily be obtained each time from the store list, but since they can be called so often (due to being used by
        // grid column header drawing), i am caching it.
        static List<StoreType> uniqueStoreTypes;
        static List<StoreType> instanceStoreTypes;

        // Same as above, but restriced to store types that have stores that are enabled
        static List<StoreType> uniqueStoreTypesEnabled;

        // Increments every time there is a change
        static int version = 0;

        /// <summary>
        /// Initialize StoreManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            // These are basically a dependence of stores
            StatusPresetManager.InitializeForCurrentSession();

            storeSynchronizer = new TableSynchronizer<StoreEntity>();
            uniqueStoreTypes = new List<StoreType>();
            instanceStoreTypes = new List<StoreType>();

            UpdateInheritanceActiveTables();
            CheckForChanges();
            CleanupIncompleteStores();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static bool CheckForChanges()
        {
            // These are basically a dependence of stoers
            bool changes = StatusPresetManager.CheckForChanges();

            lock (storeSynchronizer)
            {
                if (storeSynchronizer.Synchronize())
                {
                    storeSynchronizer.EntityCollection.Sort((int) StoreFieldIndex.StoreName, ListSortDirection.Ascending);

                    changes = true;

                    // Update the instance-based list
                    instanceStoreTypes = GetAllStores()
                        .Select(s => StoreTypeManager.GetType(s)).ToList();

                    // Track the ones we had previously so we know what ones (if any) are new
                    List<StoreTypeCode> previousTypes = uniqueStoreTypes.Select(t => t.TypeCode).ToList();

                    // Get the unique list of types
                    uniqueStoreTypes = GetAllStores()
                        .Select(s => (StoreTypeCode) s.TypeCode)
                        .Distinct()
                        .Select(t => StoreTypeManager.GetType(t)).ToList();

                    // If there are now any store types present that we didnt used to have, we need to deal with some things.  This only detects new..
                    // doesn't detect if one went away.  That's ok.
                    if (uniqueStoreTypes.Any(current => !previousTypes.Contains(current.TypeCode)))
                    {
                        // First we need to clear the cache.  Due to our ActiveTableInheritance stuff, we only query for derived order specifc
                        // tables of storetypes that are active.  If the orders had been pulled in before the store type was found here, it would
                        // not be appropriately types as its derived type.  So we clear the cache to force it to be repulled.
                        DataProvider.ClearEntityCache();

                        // Now we need to update what the current active tables are
                        UpdateInheritanceActiveTables();
                    }

                    List<StoreEntity> enabledStores = GetEnabledStores();

                    // Save the ones that actually have stores that are enabled
                    uniqueStoreTypesEnabled = uniqueStoreTypes.Where(t => enabledStores.Any(s => s.TypeCode == (int) t.TypeCode)).ToList();
                }

                if (changes)
                {
                    version++;
                }
            }

            return changes;
        }

        /// <summary>
        /// The change version of the stores.  Increments every time the stores are refreshed from the database.
        /// </summary>
        public static int ChangeVersion
        {
            get { return version; }
        }

        /// <summary>
        /// Update the tables that should be considered when doing polymorphic fetches based on active store types
        /// </summary>
        private static void UpdateInheritanceActiveTables()
        {
            List<string> orderTables = new List<string>();
            List<string> itemTables = new List<string>();

            foreach (StoreType storeType in uniqueStoreTypes)
            {
                OrderEntity order = storeType.CreateOrder();
                if (order.GetType() != typeof(OrderEntity))
                {
                    if (!orderTables.Contains(order.LLBLGenProEntityName))
                    {
                        orderTables.Add(order.LLBLGenProEntityName);
                    }
                }

                OrderItemEntity item = storeType.CreateOrderItemInstance();
                if (item.GetType() != typeof(OrderItemEntity))
                {
                    if (!itemTables.Contains(item.LLBLGenProEntityName))
                    {
                        itemTables.Add(item.LLBLGenProEntityName);
                    }
                }
            }

            ActiveTableInheritanceManager.SetActiveTables(new OrderEntity().LLBLGenProEntityName, orderTables);
            ActiveTableInheritanceManager.SetActiveTables(new OrderItemEntity().LLBLGenProEntityName, itemTables);
        }

        /// <summary>
        /// Get the current list of stores.  All stores are returned, regardless of security.
        /// </summary>
        public static List<StoreEntity> GetAllStores()
        {
            lock (storeSynchronizer)
            {
                List<StoreEntity> collectionToClone = storeSynchronizer.EntityCollection.ToList();

                List<StoreEntity> stores = EntityUtility.CloneEntityCollection(collectionToClone).Where(s => s.SetupComplete).ToList();
                return stores;
            }
        }

        /// <summary>
        /// Get all stores, regardless of security, that are currently enabled for downloading and shipping
        /// </summary>
        public static List<StoreEntity> GetEnabledStores()
        {
            return GetAllStores().Where(s => s.Enabled).ToList();
        }

        /// <summary>
        /// Get a collection of each store type in use by the current database. These are just a distinct list of the non-instanced types... with no stores attached.
        /// </summary>
        public static IList<StoreType> GetUniqueStoreTypes(bool enabledOnly = false)
        {
            if (enabledOnly)
            {
                return uniqueStoreTypesEnabled.AsReadOnly();
            }
            else
            {
                return uniqueStoreTypes.AsReadOnly();
            }
        }

        /// <summary>
        /// Return an "instanced" (store-attached) StoreType for each Store in ShipWorks
        /// </summary>
        public static IList<StoreType> GetStoreTypeInstances()
        {
            return instanceStoreTypes.AsReadOnly();
        }

        /// <summary>
        /// Get the store with the given ID.  If it does not exist, null is returned
        /// </summary>
        public static StoreEntity GetStore(long storeID)
        {
            lock (storeSynchronizer)
            {
                foreach (StoreEntity store in storeSynchronizer.EntityCollection)
                {
                    if (store.StoreID == storeID)
                    {
                        return EntityUtility.CloneEntity(store);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Do any stores have address validation enabled
        /// </summary>
        public static bool DoAnyStoresHaveAutomaticValidationEnabled()
        {
            return GetEnabledStores().Any(ValidatedAddressManager.ShouldAutoValidate);
        }

        /// <summary>
        /// A little more efficient way to get the store by checking to see if the order is in cache first.  Will return zero if not found.
        /// </summary>
        public static StoreEntity GetRelatedStore(long entityID)
        {
            if (EntityUtility.GetEntityType(entityID) == EntityType.StoreEntity)
            {
                return GetStore(entityID);
            }

            if (EntityUtility.GetEntityType(entityID) == EntityType.OrderEntity)
            {
                return GetStore(DataProvider.GetOrderHeader(entityID).StoreID);
            }

            if (EntityUtility.GetEntityType(entityID) == EntityType.OrderItemEntity)
            {
                long orderID = DataProvider.GetRelatedKeys(entityID, EntityType.OrderEntity).First();

                return GetStore(DataProvider.GetOrderHeader(orderID).StoreID);
            }

            if (EntityUtility.GetEntityType(entityID) == EntityType.ShipmentEntity)
            {
                long orderID = DataProvider.GetRelatedKeys(entityID, EntityType.OrderEntity).First();

                return GetStore(DataProvider.GetOrderHeader(orderID).StoreID);
            }

            throw new InvalidOperationException("Invalid EntityType in GetRelatedStore: " + EntityUtility.GetEntityType(entityID));
        }


        /// <summary>
        /// Save the specified store, Translating known exceptions
        /// </summary>
        public static void SaveStore(StoreEntity store)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                SaveStore(store, adapter);
            }
        }

        /// <summary>
        /// Save the specified store, Translating known exceptions
        /// </summary>
        public static void SaveStore(StoreEntity store, SqlAdapter adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            try
            {
                adapter.SaveAndRefetch(store);
            }
            catch (Exception ex)
            {
                if (!TranslateException(ex))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get the most recent download times for each store
        /// </summary>
        public static Dictionary<long, DateTime?> GetLastDownloadTimes()
        {
            Dictionary<long, DateTime?> lastDownloadTimes = new Dictionary<long, DateTime?>();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                ResultsetFields resultFields = new ResultsetFields(2);
                resultFields.DefineField(DownloadFields.StoreID, 0, "StoreID", "");
                resultFields.DefineField(DownloadFields.Ended, 1, "Ended", "", AggregateFunction.Max);

                GroupByCollection groupBy = new GroupByCollection(DownloadFields.StoreID);

                // Do the fetch
                DataTable result = new DataTable();
                adapter.FetchTypedList(resultFields, result, new RelationPredicateBucket(), 0, null, true, groupBy);

                foreach (DataRow row in result.Rows)
                {
                    // This is a RARE case... This would only happen if there is a single log entry with a null ended time for the store.  This could
                    // happen if the first download for the store is in progress, or if sw totally crashed during a download.
                    if (row["Ended"] is DBNull)
                    {
                        continue;
                    }

                    lastDownloadTimes[(long) row["StoreID"]] = (DateTime) row["Ended"];
                }
            }

            // Make sure all stores are represented
            foreach (StoreEntity store in GetAllStores())
            {
                if (!lastDownloadTimes.ContainsKey(store.StoreID))
                {
                    lastDownloadTimes[store.StoreID] = null;
                }
            }

            return lastDownloadTimes;
        }

        /// <summary>
        /// Translate the given exception into a more specific store exception
        /// </summary>
        public static bool TranslateException(Exception ex)
        {
            if (ex == null)
            {
                return false;
            }

            if (ex is ORMQueryExecutionException)
            {
                if (ex.Message.Contains("IX_Store_StoreName"))
                {
                    throw new DuplicateNameException("The store name already exists.", ex);
                }
            }

            return false;
        }

        /// <summary>
        /// Does an initial check on the validity of the store name.  Does not garuntee that it will be OK
        /// once it gets to the database. If there is a problem, the message will be displayed with owner as the message box parent.
        /// </summary>
        public static bool CheckStoreName(string name, IWin32Window owner)
        {
            return CheckStoreName(name, null, owner);
        }

        /// <summary>
        /// Does an initial check on the validity of the store name.  Does not garuntee that it will be OK
        /// once it gets to the database. If there is a problem, the message will be displayed with owner as the message box parent.
        /// </summary>
        public static bool CheckStoreName(string name, StoreEntity ignoredStore, IWin32Window owner)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            // Has to have a name
            if (name.Length == 0)
            {
                MessageHelper.ShowInformation(owner, "Enter a name for your store.");

                return false;
            }

            IPredicate filter = StoreFields.StoreName == name;

            if (ignoredStore != null)
            {
                filter = new PredicateExpression(filter, PredicateExpressionOperator.And, StoreFields.StoreID != ignoredStore.StoreID);
            }

            bool exists = (StoreCollection.GetCount(SqlAdapter.Default, filter) > 0);

            // Has to be a unique name
            if (exists)
            {
                MessageHelper.ShowInformation(owner, "A store with the chosen name already exists.  Please enter a different name.");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the number of setup stores in the database.  This checks the database directly without using whats cached in the StoreManager.
        /// </summary>
        public static int GetDatabaseStoreCount()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                return StoreCollection.GetCount(adapter, StoreFields.SetupComplete == true);
            }
        }

        /// <summary>
        /// Cleanup any stores that did not make it all the way throught he AddStoreWizard.  This doesn't do
        /// anything if someone somewhere has an AddStoreWizard open.
        /// </summary>
        private static void CleanupIncompleteStores()
        {
            // If the add store wizard is not open and there is a non complete store, that basically
            // means sw crashed during the add store wizard and we need to clean it up.
            if (!ShipWorksSetupLock.IsLocked())
            {
                try
                {
                    // Have to take the lock while we are doing this. Otherwise there could be a race condition that after we checked the lock,
                    // a temp store was created.
                    using (ShipWorksSetupLock wizardLock = new ShipWorksSetupLock())
                    {
                        foreach (StoreEntity store in StoreCollection.Fetch(SqlAdapter.Default, StoreFields.SetupComplete == false))
                        {
                            log.InfoFormat("Deleting incomplete store {0} '{1}'", store.StoreID, store.StoreName);
                            DeletionService.DeleteStore(store);
                        }
                    }
                }
                catch (SqlAppResourceLockException)
                {
                    // Nothing to do - it will get cleaned up next time if there are any
                }
            }
        }
    }
}
