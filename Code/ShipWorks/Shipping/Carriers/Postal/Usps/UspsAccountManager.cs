﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Utility;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Manages the available USPS accounts
    /// </summary>
    public static class UspsAccountManager
    {
        static TableSynchronizer<UspsAccountEntity> synchronizer;
        static IEnumerable<IUspsAccountEntity> readOnlyAccounts;
        static bool needCheckForChanges;

        /// <summary>
        /// Initialize UspsAccountManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<UspsAccountEntity>();
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
                    synchronizer.EntityCollection.Sort((int) UspsAccountFieldIndex.Username, ListSortDirection.Ascending);
                }

                readOnlyAccounts = synchronizer.EntityCollection.Select(x => x.AsReadOnly()).ToReadOnly();

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the USPS accounts in the system.  Optionally include those that have not yet totally completed signup where
        /// the user is yet to enter the account ID.
        /// </summary>
        private static List<UspsAccountEntity> GetAccounts(UspsResellerType uspsResellerType, bool includePending)
        {
            lock (synchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                IEnumerable<UspsAccountEntity> filterredAccounts =
                    synchronizer.EntityCollection.Where(account => account.UspsReseller == (int) uspsResellerType);

                if (!includePending)
                {
                    filterredAccounts =
                        filterredAccounts.Where(account => account.PendingInitialAccount != (int) UspsPendingAccountType.Create);
                }

                return EntityUtility.CloneEntityCollection(filterredAccounts);
            }
        }

        /// <summary>
        /// Get the USPS accounts in the system.  Optionally include those that have not yet totally completed signup where
        /// the user is yet to enter the account ID.
        /// </summary>
        private static IEnumerable<IUspsAccountEntity> GetAccountsReadOnly(UspsResellerType uspsResellerType)
        {
            lock (synchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                return readOnlyAccounts.Where(account => account.UspsReseller == (int) uspsResellerType);
            }
        }

        /// <summary>
        /// Gets the Usps account where Account is pending (Create or Existing)
        /// </summary>
        public static UspsAccountEntity GetPendingUspsAccount()
        {
            List<UspsAccountEntity> uspsAccountEntities = GetAccounts(UspsResellerType.None, true);

            return uspsAccountEntities.FirstOrDefault(account => account.PendingInitialAccount != (int) UspsPendingAccountType.None);
        }

        /// <summary>
        /// Get the USPS accounts in the system.
        /// </summary>
        public static List<UspsAccountEntity> GetAccounts(UspsResellerType uspsResellerType)
        {
            return GetAccounts(uspsResellerType, false);
        }

        /// <summary>
        /// Return the active list of Express1 accounts
        /// </summary>
        public static List<UspsAccountEntity> Express1Accounts => GetAccounts(UspsResellerType.Express1);

        /// <summary>
        /// Return the active list of USPS accounts.
        /// </summary>
        public static List<UspsAccountEntity> UspsAccounts => GetAccounts(UspsResellerType.None);

        /// <summary>
        /// Return the active list of USPS accounts.
        /// </summary>
        public static IEnumerable<IUspsAccountEntity> UspsAccountsReadOnly => GetAccountsReadOnly(UspsResellerType.None);

        /// <summary>
        /// Return the active list of Express1 accounts
        /// </summary>
        public static IEnumerable<IUspsAccountEntity> Express1AccountsReadOnly => GetAccountsReadOnly(UspsResellerType.Express1);

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static UspsAccountEntity GetAccount(long accountID)
        {
            return UspsAccounts.FirstOrDefault(a => a.UspsAccountID == accountID) ??
                   Express1Accounts.FirstOrDefault(a => a.UspsAccountID == accountID);
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static IUspsAccountEntity GetAccountReadOnly(long accountID)
        {
            return UspsAccountsReadOnly.FirstOrDefault(a => a.UspsAccountID == accountID) ??
                   Express1AccountsReadOnly.FirstOrDefault(a => a.UspsAccountID == accountID);
        }

        /// <summary>
        /// Save the given USPS account
        /// </summary>
        public static void SaveAccount(UspsAccountEntity account)
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
        /// Delete the given USPS account
        /// </summary>
        public static void DeleteAccount(UspsAccountEntity account)
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
        public static string GetDefaultDescription(UspsAccountEntity account)
        {
            string descriptionBase = account.Username;

            // only shorten so long as we know they're still using long account numbers.
            if (descriptionBase.Length == 36)
            {
                descriptionBase = String.Empty;
            }

            StringBuilder description = new StringBuilder(descriptionBase);

            if (!string.IsNullOrEmpty(account.Street1))
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(account.Street1);
            }

            if (!string.IsNullOrEmpty(account.PostalCode))
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(account.PostalCode);
            }

            return description.ToString();
        }

        /// <summary>
        /// Displays the appropriate setup wizard based on the USPS Reseller
        /// </summary>
        public static bool DisplaySetupWizard(IWin32Window owner, UspsResellerType uspsResellerType)
        {
            ShipmentType shipmentType;
            switch (uspsResellerType)
            {
                case UspsResellerType.None:
                    shipmentType = new UspsShipmentType();
                    break;
                case UspsResellerType.Express1:
                    shipmentType = new Express1UspsShipmentType();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("uspsResellerType");
            }

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShipmentTypeSetupWizard dlg = lifetimeScope.Resolve<IShipmentTypeSetupWizardFactory>()
                    .Create(shipmentType.ShipmentTypeCode, OpenedFromSource.Manager);

                return (dlg.ShowDialog(owner) == DialogResult.OK);
            }
        }

        /// <summary>
        /// Gets the name of the USPS reseller
        /// </summary>
        public static string GetResellerName(UspsResellerType uspsResellerType)
        {
            switch (uspsResellerType)
            {
                case UspsResellerType.None:
                    return "USPS";
                case UspsResellerType.Express1:
                    return "Express1";
                default:
                    throw new ArgumentOutOfRangeException("uspsResellerType");
            }
        }
    }
}
