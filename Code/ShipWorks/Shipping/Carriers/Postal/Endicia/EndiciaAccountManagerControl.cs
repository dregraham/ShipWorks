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
using Divelements.SandGrid;
using ShipWorks.UI;
using ShipWorks.Data.Connection;
using System.Threading;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// UserControl for managing\editing endicia accounts
    /// </summary>
    public partial class EndiciaAccountManagerControl : PostalAccountManagerControlBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EndiciaAccountManagerControl));

        long initialAccountID = -1;

        // Reseller type this control is editing
        EndiciaReseller endiciaReseller = EndiciaReseller.None;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaAccountManagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the current list of accounts into the control
        /// </summary>
        public void Initialize(EndiciaReseller reseller)
        {
            this.endiciaReseller = reseller;

            // if this is Endicia Label Server, no reseller, enforce the limit
            if (reseller == EndiciaReseller.None)
            {
                editionGuiHelper.RegisterElement(add, Editions.EditionFeature.EndiciaAccountLimit, () => EndiciaAccountManager.EndiciaAccounts.Count + 1);
            }

            LoadAccounts();
        }

        /// <summary>
        /// Load all the shippers into the grid
        /// </summary>
        private void LoadAccounts()
        {
            Cursor.Current = Cursors.WaitCursor;

            sandGrid.Rows.Clear();

            foreach (EndiciaAccountEntity account in EndiciaAccountManager.GetAccounts(endiciaReseller))
            {
                GridRow row = new GridRow(new string[] { account.Description, "Checking..." });
                sandGrid.Rows.Add(row);
                row.Tag = account;

                if (account.EndiciaAccountID == initialAccountID)
                {
                    row.Selected = true;
                }

                if (account.TestAccount && !EndiciaApiClient.UseTestServer)
                {
                    row.Cells[1].Text = "(Test Account)";
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(AsyncCheckAccountBalance), row);
                }
            }

            if (sandGrid.SelectedElements.Count == 0 && sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }

            UpdateButtonState();
        }

        /// <summary>
        /// Get the account info for the account, or null on error
        /// </summary>
        private void AsyncCheckAccountBalance(object state)
        {
            string result = "";

            GridRow row = (GridRow) state;
            EndiciaAccountEntity account = (EndiciaAccountEntity) row.Tag;

            if (account.Fields.State == EntityState.Fetched)
            {
                try
                {
                    result = (new PostageBalance(new EndiciaPostageWebClient(account), new TangoWebClientWrapper())).Value.ToString("c");
                }
                catch (EndiciaException ex)
                {
                    log.Error("Error updating grid with endicia account balance.", ex);
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

            details.Enabled = enabled;
            remove.Enabled = enabled;

            bool allowAccountRegistration = ShipmentTypeManager.GetType(IsExpress1 ? ShipmentTypeCode.Express1Endicia : ShipmentTypeCode.Endicia).IsAccountRegistrationAllowed;

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

            editionGuiHelper.UpdateUI();
        }

        /// <summary>
        /// Determines if this carrier is an Express1 carrier
        /// </summary>
        private bool IsExpress1
        {
            get
            {
                return endiciaReseller != EndiciaReseller.None;
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
        /// View the selected account
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            EndiciaAccountEntity account = (EndiciaAccountEntity) sandGrid.SelectedElements[0].Tag;

            if (endiciaReseller == EndiciaReseller.None && (account.IsDazzleMigrationPending || !ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Endicia)))
            {
                DialogResult result = MessageHelper.ShowQuestion(this, "This account was migrated from ShipWorks 2.\n\nWould you like to configure the account for use in ShipWorks 3?");

                if (result == DialogResult.OK)
                {
                    using (EndiciaSetupWizard setupWizard = new EndiciaSetupWizard())
                    {
                        if (setupWizard.ShowDialog(this) == DialogResult.OK)
                        {
                            LoadAccounts();
                        }
                    }
                }
            }
            else
            {
                using (EndiciaAccountEditorDlg dlg = new EndiciaAccountEditorDlg(account))
                {
                    var result = dlg.ShowDialog(this);

                    if (result == DialogResult.OK || dlg.PostagePurchased)
                    {
                        LoadAccounts();
                    }
                }
            }
        }

        /// <summary>
        /// Remvoe the selected account
        /// </summary>
        private void OnRemove(object sender, EventArgs e)
        {
            EndiciaAccountEntity account = (EndiciaAccountEntity) sandGrid.SelectedElements[0].Tag;

            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning,
                string.Format("Remove the account '{0}' from ShipWorks?\n\n" +
                "Important: This does not close your account with {1}.  After removing the account from ShipWorks " +
                "you need to log on to www.endicia.com to close your account.",
                account.Description, (endiciaReseller == EndiciaReseller.Express1) ? "Express1" : "Endicia" ));

            if (result == DialogResult.OK)
            {
                EndiciaAccountManager.DeleteAccount(account);
                LoadAccounts();
            }
        }

        /// <summary>
        /// Add a USPS account for use with shipworks
        /// </summary>
        private void OnAddAccount(object sender, EventArgs e)
        {
            if (EndiciaAccountManager.DisplaySetupWizard(this, endiciaReseller))
            {
                LoadAccounts();
            }
        }
    }
}
