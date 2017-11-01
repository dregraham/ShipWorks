using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Utility;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Manage Dhl Express Accounts
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    [Component]
    public class AsendiaAccountManager : IInitializeForCurrentSession
    {
        static TableSynchronizer<AsendiaAccountEntity> synchronizer;
        static IEnumerable<IAsendiaAccountEntity> readOnlyAccounts;
        static bool needCheckForChanges;

        /// <summary>
        /// Initialize OnTracAccountManager
        /// </summary>
        public void InitializeForCurrentSession()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize AsendiaAccountManager
        /// </summary>
        public static void Initialize()
        {
            synchronizer = new TableSynchronizer<AsendiaAccountEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Return the active list of Asendia accounts
        /// </summary>
        public static List<AsendiaAccountEntity> Accounts
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
        /// Return the active list of Asendia accounts
        /// </summary>
        public static IEnumerable<IAsendiaAccountEntity> AccountsReadOnly
        {
            get
            {
                lock (synchronizer)
                {
                    if (needCheckForChanges)
                    {
                        InternalCheckForChanges();
                    }

                    return readOnlyAccounts;
                }
            }
        }

        /// <summary>
        /// Save the given Asendia account
        /// </summary>
        public static void SaveAccount(AsendiaAccountEntity account)
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
                    synchronizer.EntityCollection.Sort((int) AsendiaAccountFieldIndex.AsendiaAccountID, ListSortDirection.Ascending);
                }

                readOnlyAccounts = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static AsendiaAccountEntity GetAccount(long accountID)
        {
            return Accounts.FirstOrDefault(a => a.AsendiaAccountID == accountID);
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static IAsendiaAccountEntity GetAccountReadOnly(long accountID)
        {
            return AccountsReadOnly.FirstOrDefault(a => a.AsendiaAccountID == accountID);
        }

        /// <summary>
        /// Delete the given Asendia account
        /// </summary>
        public static void DeleteAccount(AsendiaAccountEntity account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();

            Messenger.Current.Send(new ShippingAccountsChangedMessage(null, ShipmentTypeCode.Asendia));
        }
    }
}
