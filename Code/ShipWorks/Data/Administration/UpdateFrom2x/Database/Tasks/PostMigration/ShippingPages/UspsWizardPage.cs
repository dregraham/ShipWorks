﻿using System;
using System.Windows.Forms;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    /// <summary>
    /// Post-migration wizard page for allowing the user to setup USPS.
    /// 
    /// This should only be presented if there are USPS shipments in the migrated 
    /// database.  V2 didn't have a "Stamps Shipper".
    /// </summary>
    public partial class UspsWizardPage : WizardPage
    {
        bool anyUsps;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsWizardPage()
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
                anyUsps = ShipmentCollection.GetCount(SqlAdapter.Default, ShipmentFields.ShipmentType == (int) ShipmentTypeCode.Usps) > 0;
            }

            // If there are no UPS accounts or shipments just skip
            if (!anyUsps)
            {
                e.Skip = true;
            }
            else if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Usps))
            {
                ShowConfiguredUI();
            }
        }

        /// <summary>
        /// Configure the shipper.
        /// </summary>
        private void OnConfigureClick(object sender, EventArgs e)
        {
            IRegistrationPromotion promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();
            using (UspsSetupWizard wizard = new UspsSetupWizard(promotion, true))
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Usps);

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
            ShippingSettings.MarkAsActivated(ShipmentTypeCode.Usps);
        }
    }
}