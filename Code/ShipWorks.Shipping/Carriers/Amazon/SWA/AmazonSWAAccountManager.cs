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

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Manage AmazonSWA Accounts
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    [Component(SingleInstance = true)]
    public class AmazonSWAAccountManager : IInitializeForCurrentSession, ICheckForChangesNeeded
    {
        static TableSynchronizer<AmazonSWAAccountEntity> synchronizer;
        static IEnumerable<IAmazonSWAAccountEntity> readOnlyAccounts;
        static bool needCheckForChanges;

        /// <summary>
        /// Initialize OnTracAccountManager
        /// </summary>
        public void InitializeForCurrentSession()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize AmazonSWAAccountManager
        /// </summary>
        public static void Initialize()
        {
            synchronizer = new TableSynchronizer<AmazonSWAAccountEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Return the active list of AmazonSWA accounts
        /// </summary>
        public static List<AmazonSWAAccountEntity> Accounts
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
        /// Return the active list of AmazonSWA accounts
        /// </summary>
        public static IEnumerable<IAmazonSWAAccountEntity> AccountsReadOnly
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
        /// Save the given AmazonSWA account
        /// </summary>
        public static void SaveAccount(AmazonSWAAccountEntity account)
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
        /// Non-static version of CheckForChanges used by ICheckForChangesNeeded in Heartbeat for IoC
        /// </summary>
        void ICheckForChangesNeeded.CheckForChangesNeeded()
        {
            CheckForChangesNeeded();
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
                    synchronizer.EntityCollection.Sort((int) AmazonSWAAccountFieldIndex.AmazonSWAAccountID, ListSortDirection.Ascending);
                }

                readOnlyAccounts = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static AmazonSWAAccountEntity GetAccount(long accountID)
        {
            return Accounts.FirstOrDefault(a => a.AmazonSWAAccountID == accountID);
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static IAmazonSWAAccountEntity GetAccountReadOnly(long accountID)
        {
            return AccountsReadOnly.FirstOrDefault(a => a.AmazonSWAAccountID == accountID);
        }

        /// <summary>
        /// Delete the given AmazonSWA account
        /// </summary>
        public static void DeleteAccount(AmazonSWAAccountEntity account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();

            Messenger.Current.Send(new ShippingAccountsChangedMessage(null, ShipmentTypeCode.AmazonSWA));
        }
    }
}
