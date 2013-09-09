using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model;
using System.ComponentModel;
using ShipWorks.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// Manages EquaShip account records
    /// </summary>
    public static class EquaShipAccountManager
    {
        static TableSynchronizer<EquaShipAccountEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initializes EquaShipAccountManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<EquaShipAccountEntity>();
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
                    synchronizer.EntityCollection.Sort((int)EquaShipAccountFieldIndex.Username, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Return the active list of EquaShip accounts
        /// </summary>
        public static List<EquaShipAccountEntity> Accounts
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
        /// Gets the account wtih the specified ID, or null if not found
        /// </summary>
        public static EquaShipAccountEntity GetAccount(long accountID)
        {
            return Accounts.Where(s => s.EquaShipAccountID == accountID).FirstOrDefault();
        }

        /// <summary>
        /// saves the given account ot the database
        /// </summary>
        public static void SaveAccount(EquaShipAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(account);
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Deletes the given account
        /// </summary>
        public static void DeleteAccount(EquaShipAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public static string GetDefaultDescription(EquaShipAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            StringBuilder description = new StringBuilder(account.Username);

            if (account.Street1.Length > 0)
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(account.Street1);
            }

            if (account.PostalCode.Length > 0)
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(account.PostalCode);
            }

            return description.ToString();
        }
    }
}
