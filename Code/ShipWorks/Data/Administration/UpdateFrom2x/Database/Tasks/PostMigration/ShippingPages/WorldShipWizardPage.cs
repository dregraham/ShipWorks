using System;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    /// <summary>
    /// Post migration page for configuring WorldShip usage
    /// </summary>
    public partial class WorldShipWizardPage : WizardPage
    {
        static bool anyWorldShip;

        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page, determine if it should even be shown.
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (e.FirstTime)
            {
                Cursor.Current = Cursors.WaitCursor;

                anyWorldShip = ShipmentCollection.GetCount(SqlAdapter.Default, ShipmentFields.ShipmentType == (int) ShipmentTypeCode.UpsWorldShip) > 0;
            }

            // If there are no WorldShip shipments just skip
            if (!anyWorldShip)
            {
                e.Skip = true;
            }
            else if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.UpsWorldShip))
            {
                ShowConfiguredUI();
            }
        }

        /// <summary>
        /// Configure the shipper.
        /// </summary>
        private void OnConfigureClick(object sender, EventArgs e)
        {
            using (UpsSetupWizard wizard = new UpsSetupWizard(ShipmentTypeCode.UpsWorldShip))
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.UpsWorldShip);

                    ShowConfiguredUI();
                }
            }
        }

        /// <summary>
        /// Show the UI that shows it's already configured
        /// </summary>
        private void ShowConfiguredUI()
        {
            linkConfigure.Visible = false;
            panelConfigured.Visible = true;
            panelConfigured.Top = linkConfigure.Top;
        }

        /// <summary>
        /// Moving next from the wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            // We'll only be here if we found UpsWorldShip shipments, in which case we have to at least mark the type as activated
            // so they can be seen in the shipment window.
            ShippingSettings.MarkAsActivated(ShipmentTypeCode.UpsWorldShip);
        }
    }
}
