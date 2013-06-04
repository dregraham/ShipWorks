using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Window for resolving issues with accounts that are going to be deleted
    /// </summary>
    public partial class EmailAccountDeleteResolveDlg : Form
    {
        EmailAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailAccountDeleteResolveDlg(EmailAccountEntity account)
        {
            InitializeComponent();

            this.account = account;

            labelAccountName.Text = account.AccountName;
            labelAccountEmail.Text = account.EmailAddress;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            bool templateUsage = EmailAccountManager.IsAccountUsedByTemplates(account.EmailAccountID);
            bool unsentUsage = EmailAccountManager.IsAccountUsedByUnsentEmail(account.EmailAccountID);

            if (!unsentUsage)
            {
                panelUnsentMessages.Visible = false;
                panelTemplates.Top = panelUnsentMessages.Top;

                Height -= panelUnsentMessages.Height;
            }
            else if (!templateUsage)
            {
                panelTemplates.Visible = false;

                Height -= panelTemplates.Height;
            }

            LoadOtherAccounts(radioUnsentChange, accountsForUnsent, radioUnsentDelete);
            LoadOtherAccounts(radioTemplatesChange, accountsForTemplates, radioTemplatesDelete);
        }

        /// <summary>
        /// Load the options for the other accounts that the user can select from when one is going away
        /// </summary>
        private void LoadOtherAccounts(RadioButton changeAccount, ComboBox accountList, RadioButton dontChange)
        {
            EmailUtility.LoadEmailAccounts(accountList, EmailAccountManager.EmailAccounts.Where(a => a.EmailAccountID != account.EmailAccountID));

            if (accountList.Items.Count == 0)
            {
                changeAccount.Enabled = false;
                dontChange.Checked = true;
            }
            else
            {
                changeAccount.Checked = true;
            }
        }

        /// <summary>
        /// One of the radio options was changed - update the UI
        /// </summary>
        private void OnChangeRadioOption(object sender, EventArgs e)
        {
            accountsForUnsent.Enabled = radioUnsentChange.Checked;
            accountsForTemplates.Enabled = radioTemplatesChange.Checked;
        }

        /// <summary>
        /// Signal the account should be deleted, but first perform the request actions.
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            // Do the unsent stuff
            if (EmailAccountManager.IsAccountUsedByUnsentEmail(account.EmailAccountID))
            {
                RelationPredicateBucket predicate = new RelationPredicateBucket(
                    EmailOutboundFields.SendStatus != (int) EmailOutboundStatus.Sent &
                    EmailOutboundFields.AccountID == account.EmailAccountID);

                if (radioUnsentChange.Checked)
                {
                    EmailAccountEntity changeTo = EmailAccountManager.GetAccount((long) accountsForUnsent.SelectedValue);

                    EmailOutboundEntity prototype = new EmailOutboundEntity();
                    prototype.AccountID = changeTo.EmailAccountID;
                    prototype.FromAddress = string.Format("\"{0}\" <{1}>", changeTo.DisplayName, changeTo.EmailAddress);

                    // Update all the outgoing emails to use the new account
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.UpdateEntitiesDirectly(prototype, predicate);
                    }
                }
                else
                {
                    // Delete all unsent messages that were being used by the account that's going to be deleted
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.DeleteEntitiesDirectly(typeof(EmailOutboundEntity), predicate);
                    }
                }
            }

            // Do the template stuff
            if (EmailAccountManager.IsAccountUsedByTemplates(account.EmailAccountID))
            {
                long accountID = -1;

                if (radioTemplatesChange.Checked)
                {
                    accountID = (long) accountsForTemplates.SelectedValue;
                }

                TemplateStoreSettingsEntity prototype = new TemplateStoreSettingsEntity();
                prototype.EmailAccountID = accountID;

                // Update all the settings using the old account id to the new one
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.UpdateEntitiesDirectly(prototype, new RelationPredicateBucket(TemplateStoreSettingsFields.EmailAccountID == account.EmailAccountID));
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
