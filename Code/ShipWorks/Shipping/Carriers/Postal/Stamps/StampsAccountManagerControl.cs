using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using Divelements.SandGrid;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using System.Threading;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// UserControl for managing\editing stamps.com accounts
    /// </summary>
    public partial class StampsAccountManagerControl : PostalAccountManagerControlBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (StampsAccountManagerControl));

        private long initialAccountID = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsAccountManagerControl()
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
        /// Gets and sets whether this control will work with Express1 Stamps accounts or regular Stamps accounts
        /// </summary>
        public StampsResellerType StampsResellerType { get; set; }

        /// <summary>
        /// Determines if an Express1 account is being managed.
        /// </summary>
        private bool IsExpress1
        {
            get { return StampsResellerType == StampsResellerType.Express1; }
        }

        /// <summary>
        /// Load all the shippers into the grid
        /// </summary>
        private void LoadAccounts()
        {
            Cursor.Current = Cursors.WaitCursor;

            sandGrid.Rows.Clear();

            foreach (UspsAccountEntity account in StampsAccountManager.GetAccounts(StampsResellerType))
            {
                string contractType = EnumHelper.GetDescription((UspsAccountContractType)account.ContractType);
                GridRow row = new GridRow(new string[] { account.Description, contractType, "Checking..." });
                sandGrid.Rows.Add(row);
                row.Tag = account;

                if (account.UspsAccountID == initialAccountID)
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
                    result = (new PostageBalance(new UspsPostageWebClient(account), new TangoWebClientWrapper())).Value.ToString("c");
                }
                catch (StampsException ex)
                {
                    string logMessage = string.Format("Error updating grid with {0} account balance.", StampsAccountManager.GetResellerName((StampsResellerType)account.UspsReseller));
                    log.Error(logMessage, ex);
                }
                catch (ORMEntityIsDeletedException ex)
                {
                    // The call to obtain account info from the Stmaps.com API has been known to 
                    // take a few seconds, so a user could have deleted the account by the time
                    // the call from Stamps.com completes.

                    // We don't have the account info anymore, so we can only use the username value
                    // that was cached above. 
                    string logMessage = string.Format("The Stamps.com account ({0}) was deleted from ShipWorks while trying to obtain its account balance.", username);
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
                        SandGridBase sandGrid = innerGrid.SandGrid;
                        if (sandGrid != null)
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

            bool allowAccountRegistration = ShipmentTypeManager.GetType(IsExpress1 ? ShipmentTypeCode.Express1Stamps : ShipmentTypeCode.Stamps).IsAccountRegistrationAllowed;
            
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
        private void OnChangeSelectedAccount(object sender, Divelements.SandGrid.SelectionChangedEventArgs e)
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

            using (StampsAccountEditorDlg dlg = new StampsAccountEditorDlg(account))
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
                account.Description, StampsAccountManager.GetResellerName(StampsResellerType)));

            if (result == DialogResult.OK)
            {
                StampsAccountManager.DeleteAccount(account);
                LoadAccounts();
            }
        }

        /// <summary>
        /// Add a stamps.com account for use with ShipWorks
        /// </summary>
        private void OnAddAccount(object sender, EventArgs e)
        {
            if (StampsAccountManager.DisplaySetupWizard(this, StampsResellerType))
            {
                LoadAccounts();
            }
        }
    }
}
