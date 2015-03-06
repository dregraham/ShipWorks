using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    /// <summary>
    /// Page for configuring Express1 during the V2 to V3 upgrade.
    /// </summary>
    public partial class Express1WizardPage : WizardPage
    {
        bool anyExpress1 = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1WizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the wizard page for the first time
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (e.FirstTime)
            {
                anyExpress1 = EndiciaAccountCollection.Fetch(SqlAdapter.Default, EndiciaAccountFields.EndiciaReseller == (int)EndiciaReseller.Express1).Any();

                if (!anyExpress1)
                {
                    RelationPredicateBucket bucket = new RelationPredicateBucket(ShipmentFields.ShipmentType == (int)ShipmentTypeCode.Express1Endicia);
                    anyExpress1 = SqlAdapter.Default.GetDbCount(new ShipmentEntityFactory().CreateFields(), bucket) > 0;
                }
            }

            // If there are no endicia accounts or shipments just skip
            if (!anyExpress1)
            {
                e.Skip = true;
            }
            else if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Express1Endicia))
            {
                ShowConfiguredUI();
            }
        }

        /// <summary>
        /// Configure the shipper.
        /// </summary>
        private void OnConfigureClick(object sender, EventArgs e)
        {
            // We could have gotten through the "Signup" phase - which means we're not pending anymore, but still have not been fully Configured
            // if they canceled before getting there endicia account# emailed to them.

            // There wasn't an Express1 for USPS in ShipWorks 2x, so we can explicitly call out the Express1 for Endicia 
            // shipment type here
            using (Form wizard = new Express1EndiciaShipmentType().CreateSetupWizard())
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Express1Endicia);

                    Wizard.MoveNext();
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
            ShippingSettings.MarkAsActivated(ShipmentTypeCode.Express1Endicia);
        }
    }
}
