﻿using System.Collections.Generic;
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

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Manage Dhl Express Accounts
    /// </summary>
    [Order(typeof(IInitializeForCurrentSession), 1)]
    [Component(SingleInstance = true)]
    public class DhlExpressAccountManager : IInitializeForCurrentSession, ICheckForChangesNeeded
    {
        static TableSynchronizer<DhlExpressAccountEntity> synchronizer;
        static IEnumerable<IDhlExpressAccountEntity> readOnlyAccounts;
        static bool needCheckForChanges;

        /// <summary>
        /// Initialize OnTracAccountManager
        /// </summary>
        public void InitializeForCurrentSession()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize DhlExpressAccountManager
        /// </summary>
        public static void Initialize()
        {
            synchronizer = new TableSynchronizer<DhlExpressAccountEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Return the active list of DhlExpress accounts
        /// </summary>
        public static List<DhlExpressAccountEntity> Accounts
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
        /// Return the active list of DhlExpress accounts
        /// </summary>
        public static IEnumerable<IDhlExpressAccountEntity> AccountsReadOnly
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
        /// Save the given DhlExpress account
        /// </summary>
        public static void SaveAccount(DhlExpressAccountEntity account)
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
                    synchronizer.EntityCollection.Sort((int) DhlExpressAccountFieldIndex.DhlExpressAccountID, ListSortDirection.Ascending);
                }

                readOnlyAccounts = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static DhlExpressAccountEntity GetAccount(long accountID)
        {
            return Accounts.FirstOrDefault(a => a.DhlExpressAccountID == accountID);
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static IDhlExpressAccountEntity GetAccountReadOnly(long accountID)
        {
            return AccountsReadOnly.FirstOrDefault(a => a.DhlExpressAccountID == accountID);
        }

        /// <summary>
        /// Delete the given DhlExpress account
        /// </summary>
        public static void DeleteAccount(DhlExpressAccountEntity account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();

            Messenger.Current.Send(new ShippingAccountsChangedMessage(null, ShipmentTypeCode.DhlExpress));
        }
    }
}
