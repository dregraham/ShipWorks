using System.Windows.Forms;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// Utility functions and properties for yahoo
    /// </summary>
    public static class YahooEmailUtility
    {
        /// <summary>
        /// Controls if messages get deleted off the server after they are downloaded
        /// </summary>
        public static bool DeleteMessagesAfterDownload
        {
            get { return InterapptiveOnly.Registry.GetValue("YahooDeleteMessages", true); }
            set { InterapptiveOnly.Registry.SetValue("YahooDeleteMessages", value); }
        }

        /// <summary>
        /// Setup the email account for the given store
        /// </summary>
        public static bool SetupEmailAccount(YahooStoreEntity store, IWin32Window owner)
        {
            using (AddEmailAccountWizard dlg = new AddEmailAccountWizard(EmailIncomingServerType.Pop3))
            {
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    EmailAccountEntity account = dlg.EmailAccount;

                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        // If the store had an account before, we have to delete it
                        if (store.YahooEmailAccount != null)
                        {
                            // Don't delete it directly, b\c the account manager (and maybe others) could
                            // still be using the entity.  They can keep using it, and then they'll pick it up later.
                            adapter.DeleteEntity(new EmailAccountEntity(store.YahooEmailAccountID));

                            store.YahooEmailAccount = null;
                        }

                        // Update the store to use this new account
                        store.YahooEmailAccount = account;

                        // We own this account
                        account.InternalOwnerID = store.StoreID;

                        adapter.SaveAndRefetch(store);
                        adapter.SaveAndRefetch(account);

                        adapter.Commit();
                    }

                    // Need to have it pickup the updated email account
                    EmailAccountManager.CheckForChangesNeeded();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
