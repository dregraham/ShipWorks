using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages
{
    /// <summary>
    /// Post-migration wizard page for kicking off a FedEx configuration if necessary
    /// </summary>
    public partial class FedExWizardPage : WizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// User is navigating into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            FedExAccountCollection accounts = FedExAccountCollection.Fetch(SqlAdapter.Default, null);

            // If there are no FedEx accounts to configure
            if (accounts.Count == 0)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = true;
            }

            // Load all incomplete accounts and add wizard pages for each
            else if (e.FirstTime)
            {
                int yPosition = 0;

                foreach (FedExAccountEntity account in accounts)
                {
                    FedExAccountLineControl line = new FedExAccountLineControl(account);
                    line.Location = new Point(0, yPosition);
                    lineContainer.Controls.Add(line);

                    yPosition = line.Bottom;
                }
            }
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            // If there are no accounts, we still need to see if there are shipments
            if (lineContainer.Controls.OfType<FedExAccountLineControl>().Count() == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    var shipments = new ShipmentCollection();

                    adapter.FetchEntityCollection(
                        shipments,
                        new RelationPredicateBucket(ShipmentFields.ShipmentType == (int) ShipmentTypeCode.FedEx),
                        1,
                        null);

                    // Even though its not configured, we do consider it Activated, and thus ShipWorks is able to display shipment data
                    // of the type.
                    if (shipments.Count > 0)
                    {
                        ShippingSettings.MarkAsActivated(ShipmentTypeCode.FedEx);
                    }
                }
            }
            else
            {
                // If some are still pending warn them
                // BN: We don't warn on the other pages - gonna try it without and see how it goes... It will tell them when they try to ship anyway...
                /*
                if (lineContainer.Controls.Cast<FedExAccountLineControl>().Any(l => l.Account.Is2xMigrationPending))
                {
                    DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning, "Some FedEx accounts have not yet been configured for ShipWorks 3.\n\nContinue anyway?");

                    if (result != DialogResult.OK)
                    {
                        e.NextPage = this;
                        return;
                    }
                }
                */

                // Either way we have FedEx accounts - so we have to make sure its marked as activated, even if they havn't gone through the wizard yet.
                ShippingSettings.MarkAsActivated(ShipmentTypeCode.FedEx);
            }
        }
    }
}
