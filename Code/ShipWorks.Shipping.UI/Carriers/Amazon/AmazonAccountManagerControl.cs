using System;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// UserControl for editing the list of Amazon shippers
    /// </summary>
    public partial class AmazonAccountManagerControl : UserControl
    {
        public IAmazonAccountManager AccountManager { get; set; }

        long initialShipperID = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonAccountManagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize()
        {
            LoadShippers();
            UpdateButtonState();
        }

        /// <summary>
        /// Load all the shippers into the grid
        /// </summary>
        private void LoadShippers()
        {
            sandGrid.Rows.Clear();

            foreach (AmazonAccountEntity shipper in AccountManager.Accounts)
            {
                GridRow row = new GridRow(new string[] { shipper.MerchantID });
                sandGrid.Rows.Add(row);
                row.Tag = shipper;

                if (shipper.AmazonAccountID == initialShipperID)
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

            bool allowAccountRegistration = new AmazonShipmentType().IsAccountRegistrationAllowed;

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
        private void OnChangeSelectedShipper(object sender, Divelements.SandGrid.SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Edit the selected sheet
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            if (sandGrid.SelectedElements.Count != 1)
            {
                return;
            }

            using (ILifetimeScope lifetimeScope = IoC.Current.BeginLifetimeScope())
            {
                using (AmazonAccountEditorDlg dialog = lifetimeScope.Resolve<AmazonAccountEditorDlg>())
                {
                    dialog.LoadAccount(sandGrid.SelectedElements[0].Tag as AmazonAccountEntity);
                    dialog.ShowDialog();
                }
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

            using (ILifetimeScope lifetimeScope = IoC.Current.BeginLifetimeScope())
            {
                using (ShipmentTypeSetupWizardForm dlg = lifetimeScope.ResolveKeyed<ShipmentTypeSetupWizardForm>(ShipmentTypeCode.Amazon))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadShippers();
                    }
                }
            }
        }

        /// <summary>
        /// Delete the selected custom sheet
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            GridRow row = (GridRow) sandGrid.SelectedElements[0];
            AmazonAccountEntity shipper = (AmazonAccountEntity)row.Tag;

            // By default we are just asking if they want to delete
            string question = string.Format("Delete the shipper '{0}'?", shipper.MerchantID);

            DialogResult result = MessageHelper.ShowQuestion(this, question);

            if (result == DialogResult.OK)
            {
                AccountManager.DeleteAccount(shipper);
                LoadShippers();
            }
        }
    }
}
