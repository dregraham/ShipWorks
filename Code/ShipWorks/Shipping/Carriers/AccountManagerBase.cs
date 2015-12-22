using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Utility;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Account manager base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AccountManagerBase<T> where T : EntityBase2
    {
        TableSynchronizer<T> synchronizer;
        bool needCheckForChanges;

        /// <summary>
        /// Initialize Account manager
        /// </summary>
        public void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<T>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Return the active list of accounts
        /// </summary>
        public IEnumerable<T> Accounts
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
        /// Save the given account
        /// </summary>
        public void SaveAccount(T account)
        {
            using (var adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(account);
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public void CheckForChangesNeeded()
        {
            lock (synchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        void InternalCheckForChanges()
        {
            lock (synchronizer)
            {
                if (synchronizer.Synchronize())
                {
                    synchronizer.EntityCollection.Sort(AccountIdFieldIndex, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public T GetAccount(long accountID)
        {
            return Accounts.FirstOrDefault(a => EntityUtility.GetEntityId(a) == accountID);
        }

        /// <summary>
        /// Delete the given account
        /// </summary>
        public void DeleteAccount(T account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public abstract string GetDefaultDescription(T account);

        /// <summary>
        /// Get the index of the id field
        /// </summary>
        protected abstract int AccountIdFieldIndex { get; }
    }
}