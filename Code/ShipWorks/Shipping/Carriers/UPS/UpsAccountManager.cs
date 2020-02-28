﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Utility;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Manages the registered UPS shippers
    /// </summary>
    public static class UpsAccountManager
    {
        static TableSynchronizer<UpsAccountEntity> synchronizer;
        static IEnumerable<IUpsAccountEntity> readOnlyAccounts;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize UpsAccountManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<UpsAccountEntity>();
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
                    synchronizer.EntityCollection.Sort((int) UpsAccountFieldIndex.Description, ListSortDirection.Ascending);
                }

                readOnlyAccounts = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();
            }

            needCheckForChanges = false;
        }

        /// <summary>
        /// Return the active list of ups accounts
        /// </summary>
        public static List<UpsAccountEntity> Accounts
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
        /// Return the active list of ups accounts
        /// </summary>
        public static IEnumerable<IUpsAccountEntity> AccountsReadOnly
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
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static UpsAccountEntity GetAccount(long accountID)
        {
            return Accounts.Where(s => s.UpsAccountID == accountID).FirstOrDefault();
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static IUpsAccountEntity GetAccountReadOnly(long accountID)
        {
            return AccountsReadOnly.Where(s => s.UpsAccountID == accountID).FirstOrDefault();
        }

        /// <summary>
        /// Save the UPS account to the database
        /// </summary>
        public static void SaveAccount(UpsAccountEntity account)
        {
            bool wasDirty = account.IsDirty;

            using (SqlAdapter adapter = new SqlAdapter())
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
        /// Delete the given account
        /// </summary>
        public static void DeleteAccount(UpsAccountEntity account)
        {
            ShipmentTypeCode shipmentTypeCode = account.ShipmentType;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();

            Messenger.Current.Send(new ShippingAccountsChangedMessage(null, shipmentTypeCode));
        }

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public static string GetDefaultDescription(UpsAccountEntity account)
        {
            StringBuilder description;

            if (account.ShipEngineCarrierId != null)
            {
                description = new StringBuilder("UPS from ShipWorks");
            }
            else
            {
                description = new StringBuilder(account.AccountNumber);

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
            }

            return description.ToString();
        }
    }
}
