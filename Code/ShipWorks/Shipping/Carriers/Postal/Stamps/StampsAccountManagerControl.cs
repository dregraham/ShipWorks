using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// UserControl for managing\editing stamps.com accounts
    /// </summary>
    public partial class StampsAccountManagerControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(StampsAccountManagerControl));

        long initialAccountID = -1;

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
        public void Initialize()
        {
            LoadAccounts();
            UpdateButtonState();
        }

        /// <summary>
        /// Load all the shippers into the grid
        /// </summary>
        private void LoadAccounts()
        {
            Cursor.Current = Cursors.WaitCursor;

            sandGrid.Rows.Clear();

            foreach (StampsAccountEntity account in StampsAccountManager.StampsAccounts)
            {
                GridRow row = new GridRow(new string[] { account.Username, "Checking..." });
                sandGrid.Rows.Add(row);
                row.Tag = account;

                if (account.StampsAccountID == initialAccountID)
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
            StampsAccountEntity account = (StampsAccountEntity) row.Tag;

            if (account.Fields.State == EntityState.Fetched)
            {
                try
                {
                    AccountInfo accountInfo = StampsApiSession.GetAccountInfo(account);

                    result = accountInfo.PostageBalance.AvailablePostage.ToString("c");
                }
                catch (StampsException ex)
                {
                    log.Error("Error updating grid with stamps account balance.", ex);
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
                                row.Cells[1].Text = result;
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
            //OnView(sender, EventArgs.Empty);
            OnEdit(sender, EventArgs.Empty);
        }

        
        /// <summary>
        /// Edit the selected account.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnEdit(object sender, EventArgs e)
        {
            StampsAccountEntity account = (StampsAccountEntity)sandGrid.SelectedElements[0].Tag;

            using (StampsAccountEditorDlg dlg = new StampsAccountEditorDlg(account))
            {
                dlg.ShowDialog(this);

                if (dlg.AccountChanged)
                {
                    LoadAccounts();
                }
            }
        }

        /// <summary>
        /// Remvoe the selected account
        /// </summary>
        private void OnRemove(object sender, EventArgs e)
        {
            StampsAccountEntity account = (StampsAccountEntity) sandGrid.SelectedElements[0].Tag;

            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning,
                string.Format("Remove the account '{0}' from ShipWorks?\n\n" +
                "Note: This does not delete your account from Stamps.com.",
                account.Username));

            if (result == DialogResult.OK)
            {
                StampsAccountManager.DeleteAccount(account);
                LoadAccounts();
            }
        }

        /// <summary>
        /// Add a stamps.com account for use with shipworks
        /// </summary>
        private void OnAddAccount(object sender, EventArgs e)
        {
            using (StampsSetupWizard wizard = new StampsSetupWizard())
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    LoadAccounts();
                }
            }
        }
    }
}
