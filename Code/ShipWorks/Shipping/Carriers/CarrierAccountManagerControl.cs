using Autofac;
using Divelements.SandGrid;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Settings;
using System.Linq;
using System.Windows.Forms;
using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Control for managing carrier accounts
    /// </summary>
    [Component(RegistrationType.Self)]
    public partial class CarrierAccountManagerControl : UserControl
    {
        private  ICarrierAccountRetrieverFactory carrierAccountRetrieverFactory;
        private long initialShipperID = -1;
        private bool initialized;
        private ShipmentTypeCode shipmentTypeCode;
        private ICarrierAccountRetriever repo;

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierAccountManagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize(ShipmentTypeCode shipmentTypeCode)
        {
            if (!initialized)
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    carrierAccountRetrieverFactory = scope.Resolve<ICarrierAccountRetrieverFactory>();
                    repo = carrierAccountRetrieverFactory.Create(shipmentTypeCode);
                    this.shipmentTypeCode = shipmentTypeCode;

                    if (shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools ||
                        shipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
                    {
                        editionGuiHelper.RegisterElement(add, EditionFeature.UpsAccountLimit, () => repo.AccountsReadOnly.Count() + 1);
                    }

                    LoadShippers();
                }

                initialized = true;
            }
        }

        /// <summary>
        /// Load all the shippers into the grid
        /// </summary>
        private void LoadShippers()
        {
            sandGrid.Rows.Clear();

            foreach (ICarrierAccount shipper in repo.Accounts)
            {
                GridRow row = new GridRow(new [] { shipper.AccountDescription });
                sandGrid.Rows.Add(row);
                row.Tag = shipper;

                if (shipper.AccountId == initialShipperID)
                {
                    row.Selected = true;
                }
            }

            if (sandGrid.SelectedElements.Count == 0 && sandGrid.Rows.Count > 0)
            {
                sandGrid.Rows[0].Selected = true;
            }

            UpdateButtonState();
        }

        /// <summary>
        /// Update the UI state of the buttons
        /// </summary>
        private void UpdateButtonState()
        {
            bool enabled = sandGrid.SelectedElements.Count > 0;

            edit.Enabled = enabled;
            delete.Enabled = enabled;
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                bool allowAccountRegistration = scope.Resolve<IShipmentTypeManager>().Get(shipmentTypeCode).IsAccountRegistrationAllowed;

                if (!allowAccountRegistration)
                {
                    add.Hide();

                    // Adjust the location of the remove button based on the visibility of the add button and
                    // make sure it's on top of the add button.
                    delete.Top = add.Top;
                    delete.BringToFront();
                }
                else
                {
                    add.Show();
                    delete.Top = add.Bottom + 6;
                }

                editionGuiHelper.UpdateUI();
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
            ICarrierAccount shipper = (ICarrierAccount) sandGrid.SelectedElements[0].Tag;
            initialShipperID = shipper.AccountId;
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                using (ICarrierAccountEditorDlg dlg = scope.ResolveKeyed<ICarrierAccountEditorDlg>(shipmentTypeCode, TypedParameter.From(shipper)))
                {
                    dlg.ShowDialog(this);

                    repo.CheckForChangesNeeded();
                    LoadShippers();
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
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShipmentTypeSetupWizard dlg = lifetimeScope.Resolve<IShipmentTypeSetupWizardFactory>()
                    .Create(shipmentTypeCode, OpenedFromSource.Manager, TypedParameter.From(true));

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadShippers();
                }
            }
        }

        /// <summary>
        /// Delete the selected custom sheet
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            GridRow row = (GridRow) sandGrid.SelectedElements[0];
            ICarrierAccount shipper = (ICarrierAccount) row.Tag;

            // By default we are just asking if they want to delete
            string question = string.Format("Delete the shipper '{0}'?", shipper.AccountDescription);

            DialogResult result = MessageHelper.ShowQuestion(this, question);

            if (result == DialogResult.OK)
            {
                repo.DeleteAccount(shipper);
                LoadShippers();
            }
        }
    }
}
