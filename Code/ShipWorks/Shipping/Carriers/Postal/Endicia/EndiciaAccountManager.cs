using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using System.ComponentModel;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using System.Windows.Forms;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Utility class for loading and getting endicia accounts available in ShipWorks
    /// </summary>
    public static class EndiciaAccountManager
    {
        static TableSynchronizer<EndiciaAccountEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize EndicaAccountManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<EndiciaAccountEntity>();
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
                    synchronizer.EntityCollection.Sort((int)EndiciaAccountFieldIndex.Description, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get the endicia accounts in the system.  Optionally include those that have not yet totally completed signup where
        /// the user is yet to enter the account ID.
        /// </summary>
        public static List<EndiciaAccountEntity> GetAccounts(EndiciaReseller reseller, bool includeIncomplete)
        {
            lock (synchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                return EntityUtility.CloneEntityCollection(synchronizer.EntityCollection.Where(a => ((includeIncomplete || a.AccountNumber != null) && a.EndiciaReseller == (int)reseller)));
            }
        }

        /// <summary>
        /// Gets Configured accounts by Reseller type
        /// </summary>
        public static List<EndiciaAccountEntity> GetAccounts(EndiciaReseller endiciaReseller)
        {
            return GetAccounts(endiciaReseller, false);
        }

        /// <summary>
        /// Return the active list of endicia accounts
        /// </summary>
        public static List<EndiciaAccountEntity> EndiciaAccounts
        {
            get
            {
                return GetAccounts(EndiciaReseller.None, false);
            }
        }

        /// <summary>
        /// Return the active list of Express1 accounts
        /// </summary>
        public static List<EndiciaAccountEntity> Express1Accounts
        {
            get
            {
                return GetAccounts(EndiciaReseller.Express1, false);
            }
        }

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        public static EndiciaAccountEntity GetAccount(long accountID)
        {
            EndiciaAccountEntity endiciaAccount = EndiciaAccounts.Where(a => a.EndiciaAccountID == accountID).FirstOrDefault();

            if (endiciaAccount == null)
            {
                endiciaAccount = Express1Accounts.Where(a => a.EndiciaAccountID == accountID).FirstOrDefault();
            }

            return endiciaAccount;
        }

        /// <summary>
        /// Save the given account to the database
        /// </summary>
        public static void SaveAccount(EndiciaAccountEntity account)
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
        /// Delete the given account from shipworks
        /// </summary>
        public static void DeleteAccount(EndiciaAccountEntity account)
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
        public static string GetDefaultDescription(EndiciaAccountEntity account)
        {
            string descriptionBase = account.AccountNumber;

            // Express1 uses terribly long account numbers
            if (account.EndiciaReseller == (int)EndiciaReseller.Express1)
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
        /// Displays the appropriate setup wizard based on the Endicia Reseller
        /// </summary>
        public static bool DisplaySetupWizard(IWin32Window owner, EndiciaReseller endiciaReseller)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ShipmentType shipmentType = endiciaReseller == EndiciaReseller.Express1 ?
                    new Express1EndiciaShipmentType() :
                    new EndiciaShipmentType();

                using (Form dlg = shipmentType.CreateSetupWizard(lifetimeScope))
                {
                    return (dlg.ShowDialog(owner) == DialogResult.OK);
                }
            }
        }

        /// <summary>
        /// Gets the name of the Endicia Reseller.  "Endicia" if None.
        /// </summary>
        public static string GetResellerName(EndiciaReseller endiciaReseller)
        {
            if (endiciaReseller == EndiciaReseller.Express1)
            {
                return "Express1";
            }
            else
            {
                return "Endicia";
            }
        }
    }
}
