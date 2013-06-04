using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Email.Accounts;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Email
{
    /// <summary>
    /// Utility class for the email file source for generic input
    /// </summary>
    public static class GenericFileSourceEmailUtility
    {
        /// <summary>
        /// Setup the email account for the given store
        /// </summary>
        public static bool SetupNewAccount(GenericFileStoreEntity store, IWin32Window owner)
        {
            using (AddEmailAccountWizard dlg = new AddEmailAccountWizard(EmailIncomingServerType.Imap))
            {
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    EmailAccountEntity account = dlg.EmailAccount;

                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        // If the store had an account before, we have to delete it
                        if (store.EmailAccountID != null)
                        {
                            adapter.DeleteEntity(new EmailAccountEntity(store.EmailAccountID.Value));
                            store.EmailAccountID = null;
                        }

                        // Update the store to use this new account
                        store.EmailAccountID = account.EmailAccountID;

                        // We own this account
                        account.InternalOwnerID = store.StoreID;

                        adapter.SaveAndRefetch(store);
                        adapter.SaveAndRefetch(account);

                        adapter.Commit();
                    }

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
