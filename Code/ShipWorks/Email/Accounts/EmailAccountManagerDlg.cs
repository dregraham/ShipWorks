using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.UI;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Window for managing email accounts in ShipWorks
    /// </summary>
    public partial class EmailAccountManagerDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EmailAccountManagerDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageEmailAccounts);

            LoadEmailAccounts();
            UpdateButtonState();
        }

        /// <summary>
        /// Load all the accounts into the grid
        /// </summary>
        private void LoadEmailAccounts()
        {
            accountGrid.Rows.Clear();

            foreach (EmailAccountEntity account in EmailAccountManager.EmailAccounts)
            {
                GridRow row = new GridRow(new string[] { account.AccountName, account.DisplayName, account.EmailAddress });
                accountGrid.Rows.Add(row);
                row.Tag = account;
            }

            // Can't configure defaults if there are'nt any
            configureDefaultAccounts.Enabled = accountGrid.Rows.Count > 0;
        }

        /// <summary>
        /// Reload the account manager, reload the list, and select the given account ID
        /// </summary>
        private void ReloadAccounts(long accountToSelect)
        {
            // Update the list in the manager
            EmailAccountManager.CheckForChangesNeeded();

            // Reload our list
            LoadEmailAccounts();

            // Select the one we just created
            foreach (GridRow row in accountGrid.Rows)
            {
                if (((EmailAccountEntity) row.Tag).EmailAccountID == accountToSelect)
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Update the button state based on the selection
        /// </summary>
        private void UpdateButtonState()
        {
            bool enabled = accountGrid.SelectedElements.Count > 0;

            edit.Enabled = enabled;
            delete.Enabled = enabled;
        }

        /// <summary>
        /// The grid selection has changed
        /// </summary>
        private void OnChangeSelectedAccount(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Edit the selected email account
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            EmailAccountEntity emailAccount = (EmailAccountEntity) accountGrid.SelectedElements[0].Tag;

            using (EmailAccountEditorDlg dlg = new EmailAccountEditorDlg(emailAccount))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ReloadAccounts(emailAccount.EmailAccountID);
                }
            }
        }

        /// <summary>
        /// Double-click on a grid row
        /// </summary>
        private void OnActivate(object sender, GridRowEventArgs e)
        {
            OnEdit(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Add a new email account
        /// </summary>
        private void OnNewEmailAccount(object sender, EventArgs e)
        {
            // Outbound email account's within ShipWorks doesn't matter what the incoming type is
            using (AddEmailAccountWizard dlg = new AddEmailAccountWizard(null))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ReloadAccounts(dlg.EmailAccount.EmailAccountID);
                }
            }
        }

        /// <summary>
        /// Delete the selected email account
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            EmailAccountEntity emailAccount = (EmailAccountEntity) accountGrid.SelectedElements[0].Tag;

            // But if the account is in use, we need to tell them that
            if (EmailAccountManager.IsAccountUsedByTemplates(emailAccount.EmailAccountID) ||
                EmailAccountManager.IsAccountUsedByUnsentEmail(emailAccount.EmailAccountID))
            {
                using (EmailAccountDeleteResolveDlg dlg = new EmailAccountDeleteResolveDlg(emailAccount))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                }
            }
            else
            {
                // By default we are just asking if they want to delete
                string question = string.Format("Delete the email account '{0}'?", emailAccount.AccountName);

                if (MessageHelper.ShowQuestion(this, question) != DialogResult.OK)
                {
                    return;
                }
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.DeleteEntity(emailAccount);
            }

            ReloadAccounts(-1);
        }

        /// <summary>
        /// Configure the default accounts to use for each store
        /// </summary>
        private void OnConfigureDefaultAccounts(object sender, EventArgs e)
        {
            using (EmailStoreDefaultAccountDlg dlg = new EmailStoreDefaultAccountDlg())
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
