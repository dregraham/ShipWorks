﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.ComponentModel;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Manages the available stamps.com accounts
    /// </summary>
    public static class StampsAccountManager
    {
        static TableSynchronizer<StampsAccountEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize StampsAccountManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<StampsAccountEntity>();
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
                    synchronizer.EntityCollection.Sort((int) StampsAccountFieldIndex.Username, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the Stamps.com accounts in the system.  Optionally include those that have not yet totally completed signup where
        /// the user is yet to enter the account ID.
        /// </summary>
        public static List<StampsAccountEntity> GetAccounts(StampsResellerType stampsResellerType, bool includeIncomplete)
        {
            lock (synchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                return EntityUtility.CloneEntityCollection(synchronizer.EntityCollection.Where(a => ((includeIncomplete || a.Username != null) && a.StampsReseller == (int)stampsResellerType)));
            }
        }

        /// <summary>
        /// Get the Stamps.com accounts in the system.
        /// </summary>
        public static List<StampsAccountEntity> GetAccounts(StampsResellerType stampsResellerType)
        {
            return GetAccounts(stampsResellerType, false);
        }

        /// <summary>
        /// Return the active list of stamps.com accounts
        /// </summary>
        public static List<StampsAccountEntity> StampsAccounts
        {
            get
            {
                return GetAccounts(StampsResellerType.None, false);
            }
        }

        /// <summary>
        /// Return the active list of Express1 accounts
        /// </summary>
        public static List<StampsAccountEntity> Express1Accounts
        {
            get
            {
                return GetAccounts(StampsResellerType.Express1, false);
            }
        }

        /// <summary>
        /// Return the active list of Stamps.com Expedited accounts
        /// </summary>
        public static List<StampsAccountEntity> StampsExpeditedAccounts
        {
            get
            {
                return GetAccounts(StampsResellerType.StampsExpedited, false);
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static StampsAccountEntity GetAccount(long accountID)
        {
            StampsAccountEntity stampsAccount = StampsAccounts.Where(a => a.StampsAccountID == accountID).FirstOrDefault();

            if (stampsAccount == null)
            {
                stampsAccount = Express1Accounts.Where(a => a.StampsAccountID == accountID).FirstOrDefault();
            }

            return stampsAccount;
        }

        /// <summary>
        /// Save the given stamps account
        /// </summary>
        public static void SaveAccount(StampsAccountEntity account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(account);
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Delete the given stamps account
        /// </summary>
        public static void DeleteAccount(StampsAccountEntity account)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(account);
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public static string GetDefaultDescription(StampsAccountEntity account)
        {
            string descriptionBase = account.StampsAccountID.ToString();

            // Express1 uses terribly long account numbers
            if (account.StampsReseller == (int)StampsResellerType.Express1)
            {
                // only shorten so long as we know they're still using long account numbers.
                if (descriptionBase.Length == 36)
                {
                    descriptionBase = "Express1";
                }
            }

            StringBuilder description = new StringBuilder(descriptionBase);

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

        /// <summary>
        /// Displays the appropriate setup wizard based on the Stamps Reseller
        /// </summary>
        public static bool DisplaySetupWizard(IWin32Window owner, StampsResellerType stampsResellerType)
        {
            ShipmentType shipmentType;
            switch (stampsResellerType)
            {
                case StampsResellerType.None:
                    shipmentType = new StampsShipmentType();
                    break;
                case StampsResellerType.Express1:
                    shipmentType = new Express1StampsShipmentType();
                    break;
                case StampsResellerType.StampsExpedited:
                    shipmentType = new UspsShipmentType();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("stampsResellerType");
            }

            using (Form dlg = shipmentType.CreateSetupWizard())
            {
                return (dlg.ShowDialog(owner) == DialogResult.OK);
            }
        }

        /// <summary>
        /// Gets the name of the Stamps reseller
        /// </summary>
        public static string GetResellerName(StampsResellerType stampsResellerType)
        {
            switch (stampsResellerType)
            {
                case StampsResellerType.None:
                    return "Stamps.com";
                case StampsResellerType.Express1:
                    return "Express1";
                case StampsResellerType.StampsExpedited:
                    return "USPS";
                default:
                    throw new ArgumentOutOfRangeException("stampsResellerType");
            }
        }
    }
}
