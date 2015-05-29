using System;
using System.Threading;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// UserControl for managing\editing USPS accounts
    /// </summary>
    public partial class UspsAccountManagerControl : PostalAccountManagerControlBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (UspsAccountManagerControl));

        private const long InitialAccountID = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsAccountManagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the current list of accounts into the control
        /// </summary>
        public override void Initialize()
        {
            LoadAccounts();
            UpdateButtonState();
        }

        /// <summary>
        /// Gets and sets whether this control will work with Express1 USPS accounts or regular USPS accounts
        /// </summary>
        public UspsResellerType UspsResellerType { get; set; }

        /// <summary>
        /// Determines if an Express1 account is being managed.
        /// </summary>
        private bool IsExpress1
        {
            get { return UspsResellerType == UspsResellerType.Express1; }
        }

        /// <summary>
        /// Load all the shippers into the grid
        /// </summary>
        private void LoadAccounts()
        {
            Cursor.Current = Cursors.WaitCursor;

            sandGrid.Rows.Clear();

            foreach (UspsAccountEntity account in UspsAccountManager.GetAccounts(UspsResellerType))
            {
                string contractType = EnumHelper.GetDescription((UspsAccountContractType)account.ContractType);
                GridRow row = new GridRow(new[] { account.Description, contractType, "Checking..." });
                sandGrid.Rows.Add(row);
                row.Tag = account;

                if (account.UspsAccountID == InitialAccountID)
                {
                    row.Selected = true;
                }

                ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(AsyncCheckAccountBalance), row);
            }

            if (sandGrid.SelectedElements.Count == 0 && sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// Get the account info for the account, or null on error
        /// </summary>
        private void AsyncCheckAccountBalance(object state)
        {
            string result = "";

            GridRow row = (GridRow) state;

            // Grab the account from the row and make a note of username for 
            // exception handling purposes
            UspsAccountEntity account = (UspsAccountEntity) row.Tag;
            string username = account.Username;

            if (account.Fields.State == EntityState.Fetched)
            {
                try
                {
                    PostageBalance postageBalance = new PostageBalance(new UspsPostageWebClient(account), new TangoWebClientWrapper());
                    result = StringUtility.FormatFriendlyCurrency(postageBalance.Value);
                }
                catch (UspsException ex)
                {
                    string logMessage = string.Format("Error updating grid with {0} account balance.", UspsAccountManager.GetResellerName((UspsResellerType)account.UspsReseller));
                    log.Error(logMessage, ex);
                }
                catch (ORMEntityIsDeletedException ex)
                {
                    // The call to obtain account info from the Stmaps.com API has been known to 
                    // take a few seconds, so a user could have deleted the account by the time
                    // the call from USPS completes.

                    // We don't have the account info anymore, so we can only use the username value
                    // that was cached above. 
                    string logMessage = string.Format("The USPS account ({0}) was deleted from ShipWorks while trying to obtain its account balance.", username);
                    log.Warn(logMessage, ex);
                }
            }

            Program.MainForm.BeginInvoke((MethodInvoker) delegate
                {
                    if (IsDisposed || !IsHandleCreated)
                    {
                        return;
                    }

                    InnerGrid innerGrid = row.Grid;
                    if (innerGrid != null)
                    {
                        SandGridBase sandGridInnerGrid = innerGrid.SandGrid;
                        if (sandGridInnerGrid != null)
                        {
                            if (row.Cells.Count > 1)
                            {
                                row.Cells[2].Text = result;
                            }
                        }
                    }
                });
        }

        /// <summary>
        /// Update the UI state of the buttons
        /// </summary>
        private void UpdateButtonState()
        {
            bool enabled = sandGrid.SelectedElements.Count > 0;

            edit.Enabled = enabled;
            remove.Enabled = enabled;

            bool allowAccountRegistration = ShipmentTypeManager.GetType(IsExpress1 ? ShipmentTypeCode.Express1Usps : ShipmentTypeCode.Usps).IsAccountRegistrationAllowed;
            
            if (!allowAccountRegistration)
            {
                add.Hide();

                // Adjust the location of the remove button based on the visiblity of the add button and
                // make sure it's on top of the add button. 
                remove.Top = add.Top;
                remove.BringToFront();
            }
            else
            {
                add.Show();
                remove.Top = add.Bottom + 6;
            }
        }

        /// <summary>
        /// Selected sheet is changing
        /// </summary>
        private void OnChangeSelectedAccount(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Double-clicked a row (do view)
        /// </summary>
        private void OnActivate(object sender, GridRowEventArgs e)
        {
            OnEdit(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Edit the selected account.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnEdit(object sender, EventArgs e)
        {
            UspsAccountEntity account = (UspsAccountEntity)sandGrid.SelectedElements[0].Tag;

            using (UspsAccountEditorDlg dlg = new UspsAccountEditorDlg(account))
            {
                dlg.ShowDialog(this);

                // Always reload Express1 accounts if OK is clicked, since the name is influenced by the address fields.
                if (dlg.AccountChanged || (IsExpress1 && dlg.DialogResult == DialogResult.OK))
                {
                    LoadAccounts();
                }
            }
        }

        /// <summary>
        /// Remove the selected account
        /// </summary>
        private void OnRemove(object sender, EventArgs e)
        {
            UspsAccountEntity account = (UspsAccountEntity) sandGrid.SelectedElements[0].Tag;

            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning,
                string.Format("Remove the account '{0}' from ShipWorks?\n\n" +
                "Note: This does not delete your account from {1}.",
                account.Description, UspsAccountManager.GetResellerName(UspsResellerType)));

            if (result == DialogResult.OK)
            {
                UspsAccountManager.DeleteAccount(account);
                LoadAccounts();
            }
        }

        /// <summary>
        /// Add a USPS account for use with ShipWorks
        /// </summary>
        private void OnAddAccount(object sender, EventArgs e)
        {
            if (UspsAccountManager.DisplaySetupWizard(this, UspsResellerType))
            {
                LoadAccounts();
            }
        }
    }
}
