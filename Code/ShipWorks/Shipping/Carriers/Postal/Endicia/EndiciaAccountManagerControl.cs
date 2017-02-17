﻿using System;
using System.Threading;
using System.Windows.Forms;
using Autofac;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// UserControl for managing\editing endicia accounts
    /// </summary>
    [Component(RegistrationType.Self)]
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
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    ITangoWebClient tangoWebClient = lifetimeScope.Resolve<ITangoWebClient>();

                    try
                    {
                        result = (new PostageBalance(new EndiciaPostageWebClient(account), tangoWebClient)).Value.ToString("c");
                    }
                    catch (EndiciaException ex)
                    {
                        log.Error("Error updating grid with endicia account balance.", ex);
                    }
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

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                EndiciaAccountEditorDlg dlg = lifetimeScope.Resolve<EndiciaAccountEditorDlg>();
                dlg.LoadAccount(account);

                var result = dlg.ShowDialog(this);

                if (result == DialogResult.OK || dlg.PostagePurchased)
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
            EndiciaAccountEntity account = (EndiciaAccountEntity) sandGrid.SelectedElements[0].Tag;

            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning,
                string.Format("Remove the account '{0}' from ShipWorks?\n\n" +
                "Important: This does not close your account with {1}.  After removing the account from ShipWorks " +
                "you need to log on to www.endicia.com to close your account.",
                account.Description, (endiciaReseller == EndiciaReseller.Express1) ? "Express1" : "Endicia"));

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
