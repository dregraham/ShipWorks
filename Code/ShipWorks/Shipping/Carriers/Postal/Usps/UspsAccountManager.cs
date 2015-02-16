using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Manages the available stamps.com accounts
    /// </summary>
    public static class UspsAccountManager
    {
        static TableSynchronizer<UspsAccountEntity> synchronizer;
        static bool needCheckForChanges;

        /// <summary>
        /// Initialize StampsAccountManager
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

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the Stamps.com accounts in the system.  Optionally include those that have not yet totally completed signup where
        /// the user is yet to enter the account ID.
        /// </summary>
        public static List<UspsAccountEntity> GetAccounts(UspsResellerType stampsResellerType, bool includeIncomplete)
        {
            lock (synchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                return EntityUtility.CloneEntityCollection(synchronizer.EntityCollection.Where(a => ((includeIncomplete || a.Username != null) && a.UspsReseller == (int)stampsResellerType)));
            }
        }

        /// <summary>
        /// Get the Stamps.com accounts in the system.
        /// </summary>
        public static List<UspsAccountEntity> GetAccounts(UspsResellerType stampsResellerType)
        {
            return GetAccounts(stampsResellerType, false);
        }

        /// <summary>
        /// Return the active list of Express1 accounts
        /// </summary>
        public static List<UspsAccountEntity> Express1Accounts
        {
            get
            {
                return GetAccounts(UspsResellerType.Express1, false);
            }
        }

        /// <summary>
        /// Return the active list of USPS accounts.
        /// </summary>
        public static List<UspsAccountEntity> UspsAccounts
        {
            get
            {
                return GetAccounts(UspsResellerType.None, false);
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static UspsAccountEntity GetAccount(long accountID)
        {
            return UspsAccounts.FirstOrDefault(a => a.UspsAccountID == accountID) ??
                   Express1Accounts.FirstOrDefault(a => a.UspsAccountID == accountID);
        }

        /// <summary>
        /// Save the given stamps account
        /// </summary>
        public static void SaveAccount(UspsAccountEntity account)
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
        public static void DeleteAccount(UspsAccountEntity account)
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
        public static string GetDefaultDescription(UspsAccountEntity account)
        {
            string descriptionBase = account.UspsAccountID.ToString(CultureInfo.InvariantCulture);

            // Express1 uses terribly long account numbers
            if (account.UspsReseller == (int)UspsResellerType.Express1)
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
        public static bool DisplaySetupWizard(IWin32Window owner, UspsResellerType stampsResellerType)
        {
            ShipmentType shipmentType;
            switch (stampsResellerType)
            {
                case UspsResellerType.None:
                    shipmentType = new UspsShipmentType();
                    break;
                case UspsResellerType.Express1:
                    shipmentType = new Express1UspsShipmentType();
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
        public static string GetResellerName(UspsResellerType stampsResellerType)
        {
            switch (stampsResellerType)
            {
                case UspsResellerType.None:
                    return "USPS";
                case UspsResellerType.Express1:
                    return "Express1";
                default:
                    throw new ArgumentOutOfRangeException("stampsResellerType");
            }
        }
    }
}
