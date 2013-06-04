using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using System.ComponentModel;
using ShipWorks.Data.Model;
using ShipWorks.Data;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// Manages and provides access to FTP accounts
    /// </summary>
    public static class FtpAccountManager
    {
        static TableSynchronizer<FtpAccountEntity> accountSynchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            accountSynchronizer = new TableSynchronizer<FtpAccountEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (accountSynchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (accountSynchronizer)
            {
                if (accountSynchronizer.Synchronize())
                {
                    accountSynchronizer.EntityCollection.Sort((int) FtpAccountFieldIndex.Host, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get all the FTP accounts loaded, optionally including ones that are only for internal use owned by other shipworks components.
        /// </summary>
        public static IList<FtpAccountEntity> GetFtpAccounts(bool includeOwned)
        {
            lock (accountSynchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                return EntityUtility.CloneEntityCollection(accountSynchronizer.EntityCollection.Where(a => includeOwned || a.InternalOwnerID == null));
            }
        }

        /// <summary>
        /// Get the current list of all FTP accounts
        /// </summary>
        public static IList<FtpAccountEntity> FtpAccounts
        {
            get
            {
                return GetFtpAccounts(false);
            }
        }

        /// <summary>
        /// Get the account by the specified ID. Returns null if no such account is found.
        /// </summary>
        public static FtpAccountEntity GetAccount(long accountID)
        {
            return GetFtpAccounts(true).SingleOrDefault(a => a.FtpAccountID == accountID);
        }
    }
}
