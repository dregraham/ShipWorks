using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using Divelements.SandGrid;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Window for configuring the default email account to use for each store
    /// </summary>
    public partial class EmailStoreDefaultAccountDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EmailStoreDefaultAccountDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            accountDropDown.Enabled = false;

            // Load all the stores for the default list
            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                EmailAccountEntity defaultAccount = EmailAccountManager.GetStoreDefault(store.StoreID);
                string name = defaultAccount != null ? defaultAccount.AccountName : "";

                GridRow row = new GridRow(new string[] { store.StoreName, name });
                row.Tag = store;

                storeGrid.Rows.Add(row);
            }

            // Load the accounts menu
            foreach (EmailAccountEntity account in EmailAccountManager.EmailAccounts)
            {
                ToolStripItem item = accountsContextMenu.Items.Add(account.AccountName);

                item.Tag = account;
                item.Click += new EventHandler(OnSetEmailAccount);
            }

            // Cant set if nothing exists
            accountsContextMenu.Enabled = accountsContextMenu.Items.Count > 0;
        }

        /// <summary>
        /// Update the UI based on selection
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            accountDropDown.Enabled = storeGrid.SelectedElements.Count == 1;
        }

        /// <summary>
        /// Set the default email account to use for the selected store
        /// </summary>
        void OnSetEmailAccount(object sender, EventArgs e)
        {
            ToolStripItem toolStripItem = (ToolStripItem) sender;
            GridRow row = (GridRow) storeGrid.SelectedElements[0];

            StoreEntity store = (StoreEntity) row.Tag;
            EmailAccountEntity account = (EmailAccountEntity) toolStripItem.Tag;

            EmailAccountManager.SetStoreDefault(store.StoreID, account);
            row.Cells[1].Text = account.AccountName;
        }
    }
}
