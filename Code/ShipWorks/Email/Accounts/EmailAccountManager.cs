using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Stores;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Manages and provides access to email accounts
    /// </summary>
    public static class EmailAccountManager
    {
        static TableSynchronizer<EmailAccountEntity> accountSynchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize EmailAccountManager
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            accountSynchronizer = new TableSynchronizer<EmailAccountEntity>();
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
                    accountSynchronizer.EntityCollection.Sort((int) EmailAccountFieldIndex.DisplayName, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Get all the email accounts loaded, optionally including ones that are only for internal use owned by other shipworks components.
        /// </summary>
        public static IList<EmailAccountEntity> GetEmailAccounts(bool includeOwned)
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
        /// Get the current list of all email accoutns
        /// </summary>
        public static IList<EmailAccountEntity> EmailAccounts
        {
            get
            {
                return GetEmailAccounts(false);
            }
        }

        /// <summary>
        /// Indicates if the email account is configured to be the account to send with for any templates
        /// </summary>
        public static bool IsAccountUsedByTemplates(long accountID)
        {
            return TemplateStoreSettingsCollection.GetCount(SqlAdapter.Default, TemplateStoreSettingsFields.EmailAccountID == accountID) > 0;
        }

        /// <summary>
        /// Indicates if the email account is configured to be used with some unsent email messages
        /// </summary>
        public static bool IsAccountUsedByUnsentEmail(long accountID)
        {
            return EmailOutboundCollection.GetCount(SqlAdapter.Default,
                EmailOutboundFields.AccountID == accountID &
                EmailOutboundFields.SendStatus != (int) EmailOutboundStatus.Sent) > 0;
        }

        /// <summary>
        /// Get the account by the specified ID. Returns null if no such account is found.
        /// </summary>
        public static EmailAccountEntity GetAccount(long accountID)
        {
            return GetEmailAccounts(true).SingleOrDefault(a => a.EmailAccountID == accountID);
        }

        /// <summary>
        /// Get the default account to use for the given store.  If there are no accounts in the system, null
        /// will be returned.
        /// </summary>
        public static EmailAccountEntity GetStoreDefault(long storeID)
        {
            StoreEntity store = StoreManager.GetStore(storeID);
            if (store == null)
            {
                return null;
            }

            EmailAccountEntity account = GetAccount(store.DefaultEmailAccountID);
            if (account != null)
            {
                return account;
            }

            if (EmailAccounts.Count > 0)
            {
                return EmailAccounts[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set the default account to use for the given store
        /// </summary>
        public static void SetStoreDefault(long storeID, EmailAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            StoreEntity store = StoreManager.GetStore(storeID);
            if (store != null)
            {
                store.DefaultEmailAccountID = account.EmailAccountID;

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(store);
                }
            }
        }
    }
}
