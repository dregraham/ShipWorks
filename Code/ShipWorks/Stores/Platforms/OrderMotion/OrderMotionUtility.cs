using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Win32;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using ShipWorks.Email.Accounts;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Utility functions and properties for yahoo
    /// </summary>
    public static class OrderMotionUtility
    {
        /// <summary>
        /// Controls if messages get deleted off the server after they are downloaded
        /// </summary>
        public static bool DeleteMessagesAfterDownload
        {
            get { return InterapptiveOnly.Registry.GetValue("OrderMotionDeleteMessages", true); }
            set { InterapptiveOnly.Registry.SetValue("OrderMotionDeleteMessages", value); }
        }

        /// <summary>
        /// Setup the email account for the given store
        /// </summary>
        public static bool SetupEmailAccount(OrderMotionStoreEntity store, IWin32Window owner)
        {
            using (AddEmailAccountWizard dlg = new AddEmailAccountWizard(EmailIncomingServerType.Pop3))
            {
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    EmailAccountEntity account = dlg.EmailAccount;

                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        // If the store had an account before, we have to delete it
                        if (store.OrderMotionEmailAccount != null)
                        {
                            // Don't delete it directly, b\c the account manager (and maybe others) could
                            // still be using the entity.  They can keep using it, and then they'll pick it up later.
                            adapter.DeleteEntity(new EmailAccountEntity(store.OrderMotionEmailAccountID));

                            store.OrderMotionEmailAccount = null;
                        }

                        // Update the store to use this new account
                        store.OrderMotionEmailAccount = account;

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