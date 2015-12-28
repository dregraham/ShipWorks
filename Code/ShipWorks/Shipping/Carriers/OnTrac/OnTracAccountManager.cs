using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// OnTrac Account Manager
    /// </summary>
    public static class OnTracAccountManager
    {
        static TableSynchronizer<OnTracAccountEntity> synchronizer;
        static bool needCheckForChanges;

        /// <summary>
        /// Initialize OnTracAccountManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<OnTracAccountEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Return the active list of OnTrac accounts
        /// </summary>
        public static List<OnTracAccountEntity> Accounts
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
        /// Save the given OnTrac account
        /// </summary>
        public static void SaveAccount(OnTracAccountEntity account)
        {
            bool wasDirty = account.IsDirty;

            using (var adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(account);
            }

            CheckForChangesNeeded();

            if (wasDirty)
            {
                Messenger.Current.Send(new ShippingAccountsChangedMessage(null, account.ShipmentType));
            }
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
        static void InternalCheckForChanges()
        {
            lock (synchronizer)
            {
                if (synchronizer.Synchronize())
                {
                    synchronizer.EntityCollection.Sort((int) OnTracAccountFieldIndex.OnTracAccountID, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static OnTracAccountEntity GetAccount(long accountID)
        {
            return Accounts.FirstOrDefault(a => a.OnTracAccountID == accountID);
        }

        /// <summary>
        /// Delete the given OnTrac account
        /// </summary>
        public static void DeleteAccount(OnTracAccountEntity account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();

            Messenger.Current.Send(new ShippingAccountsChangedMessage(null, ShipmentTypeCode.OnTrac));
        }

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public static string GetDefaultDescription(OnTracAccountEntity account)
        {
            return string.Format("Account #{0}", account.AccountNumber);
        }
    }
}