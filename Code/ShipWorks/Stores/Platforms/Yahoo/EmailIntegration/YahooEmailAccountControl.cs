using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// UserControl for selecting the email account 
    /// </summary>
    public partial class YahooEmailAccountControl : UserControl
    {
        YahooStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public YahooEmailAccountControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the contol with the store that will have its account info setup
        /// </summary>
        public void InitializeForStore(YahooStoreEntity store)
        {
            this.store = store;

            if (store.YahooEmailAccount == null)
            {
                store.YahooEmailAccount = EmailAccountManager.GetAccount(store.YahooEmailAccountID);
            }

            if (store.YahooEmailAccount != null)
            {
                accountDescription.Text = store.YahooEmailAccount.EmailAddress;
            }
        }

        /// <summary>
        /// Add an email account to shipworks
        /// </summary>
        private void OnChange(object sender, EventArgs e)
        {
            if (YahooEmailUtility.SetupEmailAccount(store, this))
            {
                accountDescription.Text = store.YahooEmailAccount.EmailAddress;
            }
        }

        /// <summary>
        /// Edit the already configured email account
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            EmailAccountEntity account = store.YahooEmailAccount;

            using (EmailAccountEditorDlg dlg = new EmailAccountEditorDlg(account))
            {
                dlg.ShowDialog(this);

                accountDescription.Text = account.EmailAddress;
            }
        }
    }
}
