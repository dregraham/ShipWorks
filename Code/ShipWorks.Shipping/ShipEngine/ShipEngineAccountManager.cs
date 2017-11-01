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

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Manage ShipEngine Accounts
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    [Component]
    public class ShipEngineAccountManager : IInitializeForCurrentSession
    {
        static TableSynchronizer<ShipEngineAccountEntity> synchronizer;
        static IEnumerable<IShipEngineAccountEntity> readOnlyAccounts;
        static bool needCheckForChanges;

        /// <summary>
        /// Initialize OnTracAccountManager
        /// </summary>
        public void InitializeForCurrentSession()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize ShipEngineAccountManager
        /// </summary>
        public static void Initialize()
        {
            synchronizer = new TableSynchronizer<ShipEngineAccountEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Return the active list of ShipEngine accounts
        /// </summary>
        public static List<ShipEngineAccountEntity> Accounts
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
        /// Return the active list of ShipEngine accounts
        /// </summary>
        public static IEnumerable<IShipEngineAccountEntity> AccountsReadOnly
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
        /// Save the given ShipEngine account
        /// </summary>
        public static void SaveAccount(ShipEngineAccountEntity account)
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
                    synchronizer.EntityCollection.Sort((int) ShipEngineAccountFieldIndex.ShipEngineAccountID, ListSortDirection.Ascending);
                }

                readOnlyAccounts = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static ShipEngineAccountEntity GetAccount(long accountID)
        {
            return Accounts.FirstOrDefault(a => a.ShipEngineAccountID == accountID);
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static IShipEngineAccountEntity GetAccountReadOnly(long accountID)
        {
            return AccountsReadOnly.FirstOrDefault(a => a.ShipEngineAccountID == accountID);
        }

        /// <summary>
        /// Delete the given ShipEngine account
        /// </summary>
        public static void DeleteAccount(ShipEngineAccountEntity account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();

            Messenger.Current.Send(new ShippingAccountsChangedMessage(null, account.ShipmentType));
        }
    }
}
