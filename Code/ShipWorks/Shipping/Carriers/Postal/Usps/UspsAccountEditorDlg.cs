using System;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public partial class UspsAccountEditorDlg : Form
    {
        private readonly UspsAccountEntity account;
        private bool passwordUpdated;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsAccountEditorDlg"/> class.
        /// </summary>
        /// <param name="account">The account.</param>
        public UspsAccountEditorDlg(UspsAccountEntity account)
        {
            InitializeComponent();

            this.account = account;
            passwordUpdated = false;

            // Adjust the note text based on the carrier/reseller
            string carrierName = StampsAccountManager.GetResellerName((UspsResellerType)account.UspsReseller);
            labelStamps.Text = carrierName;
            labelNote.Text = string.Format("Any changes made to the address are only for ShipWorks. Your address information with {0} is not updated.", carrierName);
            
            base.Text = string.Format("{0} Account", carrierName);
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnLoad(object sender, EventArgs e)
        {
            accountInfoControl.Initialize(account);

            int originalTop = accountPanel.Top;

            // The account info control is dynamic based on the account entity, so adjust 
            // the account panel based on the info control to avoid empty space
            accountPanel.Top = accountInfoControl.Bottom;
            Height = Height - (originalTop - accountPanel.Top);

            if (account.CountryCode == string.Empty)
            {
                // Default the country code to US for any accounts that were created
                // prior to ShipWorks collecting person data for a stamps account
                account.CountryCode = "US";
            }

            personControl.LoadEntity(new PersonAdapter(account, string.Empty));

            mailingPostalCode.Text = account.MailingPostalCode;
        }

        /// <summary>
        /// Gets a value indicating whether [account changed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [account changed]; otherwise, <c>false</c>.
        /// </value>
        public bool AccountChanged
        {
            get { return passwordUpdated || accountInfoControl.PostagePurchased; }
        }

        /// <summary>
        /// Called when [update password].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnUpdatePassword(object sender, EventArgs e)
        {
            using (UspsAccountUpdatePasswordDlg dlg = new UspsAccountUpdatePasswordDlg(account))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    accountInfoControl.Initialize(account);
                    passwordUpdated = true;
                }
            }
        }

        /// <summary>
        /// Called when [OK].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnOK(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            personControl.SaveToEntity();

            if (account.FirstName.Length == 0 || account.LastName.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a first and last name for the account.");
                return;
            }

            if (account.Street1.Length == 0)
            {
                MessageHelper.ShowError(this, "Enter a street address for the account.");
                return;
            }

            account.MailingPostalCode = mailingPostalCode.Text.Trim();
            
            try
            {
                StampsAccountManager.SaveAccount(account);
                DialogResult = DialogResult.OK;
            }
            catch (ORMConcurrencyException)
            {
                MessageHelper.ShowError(this, "Your changes cannot be saved because another user has deleted the account.");
                DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Rollback changes if not saved
            if (DialogResult != DialogResult.OK)
            {
                account.RollbackChanges();
                StampsAccountManager.CheckForChangesNeeded();
            }
        }
    }
}
