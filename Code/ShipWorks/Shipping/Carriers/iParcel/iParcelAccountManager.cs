using System.ComponentModel;
using System.Linq;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using System.Collections.Generic;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model;
using ShipWorks.Messages;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Manages the available i-parcel accounts.
    /// </summary>
    public static class iParcelAccountManager
    {
        static TableSynchronizer<IParcelAccountEntity> synchronizer;
        static bool needCheckForChanges;

        /// <summary>
        /// Initialize iParcelAccountManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<IParcelAccountEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Return the active list of i-parcel accounts
        /// </summary>
        public static List<IParcelAccountEntity> Accounts
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
        /// Save the given i-parcel account
        /// </summary>
        public static void SaveAccount(IParcelAccountEntity account)
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
                    synchronizer.EntityCollection.Sort((int)IParcelAccountFieldIndex.IParcelAccountID, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static IParcelAccountEntity GetAccount(long accountID)
        {
            return Accounts.FirstOrDefault(a => a.IParcelAccountID == accountID);
        }

        /// <summary>
        /// Delete the given i-parcel account
        /// </summary>
        public static void DeleteAccount(IParcelAccountEntity account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();

            Messenger.Current.Send(new ShippingAccountsChangedMessage(null, ShipmentTypeCode.iParcel));
        }

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public static string GetDefaultDescription(IParcelAccountEntity account)
        {
            return string.Format("Username: {0}", account.Username);
        }
    }
}