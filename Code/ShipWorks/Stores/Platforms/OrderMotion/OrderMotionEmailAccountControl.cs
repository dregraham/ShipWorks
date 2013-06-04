using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Email.Accounts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// UserControl for selecting the email account 
    /// </summary>
    public partial class OrderMotionEmailAccountControl : UserControl
    {
        OrderMotionStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionEmailAccountControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the contol with the store that will have its account info setup
        /// </summary>
        public void InitializeForStore(OrderMotionStoreEntity store)
        {
            this.store = store;

            if (store.OrderMotionEmailAccount == null)
            {
                store.OrderMotionEmailAccount = EmailAccountManager.GetAccount(store.OrderMotionEmailAccountID);
            }

            if (store.OrderMotionEmailAccount != null)
            {
                accountDescription.Text = store.OrderMotionEmailAccount.EmailAddress;
            }
        }

        /// <summary>
        /// Add an email account to shipworks
        /// </summary>
        private void OnChange(object sender, EventArgs e)
        {
            if (OrderMotionUtility.SetupEmailAccount(store, this))
            {
                accountDescription.Text = store.OrderMotionEmailAccount.EmailAddress;
            }
        }

        /// <summary>
        /// Edit the already configured email account
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            EmailAccountEntity account = store.OrderMotionEmailAccount;

            using (EmailAccountEditorDlg dlg = new EmailAccountEditorDlg(account))
            {
                dlg.ShowDialog(this);

                accountDescription.Text = account.EmailAddress;
            }
        }
    }
}
