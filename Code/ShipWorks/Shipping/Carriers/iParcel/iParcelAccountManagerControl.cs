using System;
using System.Windows.Forms;
using Autofac;
using Divelements.SandGrid;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// UserControl for editing the list of iParcel shippers
    /// </summary>
    public partial class iParcelAccountManagerControl : UserControl
    {
        long initialAccountID = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public iParcelAccountManagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize()
        {
            LoadAccounts();
            UpdateButtonState();
        }

        /// <summary>
        /// Load all the accounts into the grid
        /// </summary>
        private void LoadAccounts()
        {
            sandGrid.Rows.Clear();

            foreach (IParcelAccountEntity account in iParcelAccountManager.Accounts)
            {
                GridRow row = new GridRow(new[] { account.Description });
                sandGrid.Rows.Add(row);
                row.Tag = account;

                if (account.IParcelAccountID == initialAccountID)
                {
                    row.Selected = true;
                }
            }

            if (sandGrid.SelectedElements.Count == 0 && sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }
        }

        /// <summary>
        /// Update the UI state of the buttons
        /// </summary>
        private void UpdateButtonState()
        {
            bool enabled = sandGrid.SelectedElements.Count > 0;

            edit.Enabled = enabled;
            delete.Enabled = enabled;

            bool allowAccountRegistration = ShipmentTypeManager.GetType(ShipmentTypeCode.iParcel).IsAccountRegistrationAllowed;

            if (!allowAccountRegistration)
            {
                add.Hide();

                // Adjust the location of the remove button based on the visiblity of the add button and
                // make sure it's on top of the add button.
                delete.Top = add.Top;
                delete.BringToFront();
            }
            else
            {
                add.Show();
                delete.Top = add.Bottom + 6;
            }
        }

        /// <summary>
        /// Selected sheet is changing
        /// </summary>
        private void OnChangeSelectedShipper(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Edit the selected sheet
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            try
            {
                IParcelAccountEntity account = (IParcelAccountEntity) sandGrid.SelectedElements[0].Tag;
                initialAccountID = account.IParcelAccountID;

                using (iParcelAccountEditorDlg dlg = new iParcelAccountEditorDlg(account))
                {
                    dlg.ShowDialog(this);
                }

                iParcelAccountManager.CheckForChangesNeeded();
                LoadAccounts();
            }
            catch (iParcelException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// Double-clicked a row (do Edit)
        /// </summary>
        private void OnActivate(object sender, GridRowEventArgs e)
        {
            OnEdit(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Add a new custom sheet
        /// </summary>
        private void OnAdd(object sender, EventArgs e)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShipmentTypeSetupWizard dlg = lifetimeScope.Resolve<IShipmentTypeSetupWizardFactory>()
                    .Create(ShipmentTypeCode.iParcel, OpenedFromSource.Manager);

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadAccounts();
                }
            }
        }

        /// <summary>
        /// Delete the selected custom sheet
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            GridRow gridRow = (GridRow) sandGrid.SelectedElements[0];
            IParcelAccountEntity account = (IParcelAccountEntity) gridRow.Tag;

            // By default we are just asking if they want to delete
            string question = string.Format("Remove account '{0}' from ShipWorks?", account.Username);

            DialogResult result = MessageHelper.ShowQuestion(this, question);

            if (result == DialogResult.OK)
            {
                iParcelAccountManager.DeleteAccount(account);
                LoadAccounts();
            }
        }
    }
}