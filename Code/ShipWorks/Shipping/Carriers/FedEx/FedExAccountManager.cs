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

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Manages FedEx shipper records
    /// </summary>
    public static class FedExAccountManager
    {
        static TableSynchronizer<FedExAccountEntity> synchronizer;
        static bool needCheckForChanges = false;
        static IEnumerable<IFedExAccountEntity> readOnlyAccounts;

        /// <summary>
        /// Initialize FedExAccountManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<FedExAccountEntity>();
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
                    synchronizer.EntityCollection.Sort((int) FedExAccountFieldIndex.Description, ListSortDirection.Ascending);
                }

                readOnlyAccounts = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Return the active list of fedex accounts
        /// </summary>
        public static List<FedExAccountEntity> Accounts
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
        /// Return the active list of fedex accounts
        /// </summary>
        public static IEnumerable<IFedExAccountEntity> AccountsReadOnly
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
        public static FedExAccountEntity GetAccount(long accountID)
        {
            return Accounts.FirstOrDefault(s => s.FedExAccountID == accountID);
        }

        /// <summary>
        /// Get a read only version of the specified account
        /// </summary>
        internal static IFedExAccountEntity GetAccountReadOnly(long accountID)
        {
            return AccountsReadOnly.FirstOrDefault(s => s.FedExAccountID == accountID);
        }

        /// <summary>
        /// Save the given account to the database
        /// </summary>
        public static void SaveAccount(FedExAccountEntity account)
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
        public static void DeleteAccount(FedExAccountEntity account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();

            Messenger.Current.Send(new ShippingAccountsChangedMessage(null, ShipmentTypeCode.FedEx));
        }

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public static string GetDefaultDescription(FedExAccountEntity account)
        {
            StringBuilder description = new StringBuilder(account.AccountNumber);

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
