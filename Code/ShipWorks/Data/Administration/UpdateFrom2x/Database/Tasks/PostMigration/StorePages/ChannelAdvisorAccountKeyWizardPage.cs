using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using Interapptive.Shared.UI;
using Interapptive.Shared.Net;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.StorePages
{
    /// <summary>
    /// Post-Migration step for having the user specify the ChannelAdvisor Account Key which wasn't
    /// required in V2 but is in v3.
    /// </summary>
    public partial class ChannelAdvisorAccountKeyWizardPage : WizardPage
    {
        static List<ChannelAdvisorStoreEntity> configurationMissing = null;

        // the store being configured by this instance
        ChannelAdvisorStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorAccountKeyWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving on
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!skipCheckBox.Checked)
            {
                if (String.IsNullOrWhiteSpace(accountKey.Text))
                {
                    MessageHelper.ShowError(this, "Please provide your ChannelAdvisor account key to proceed.");
                    e.NextPage = this;

                    return;
                }

                // begin wait
                Cursor.Current = Cursors.WaitCursor;

                // test the account key provided
                string key = accountKey.Text.Trim();
                if (!ChannelAdvisorClient.TestConnection(key))
                {
                    // show a message
                    MessageHelper.ShowError(this, "ShipWorks was unable to communicate with ChannelAdvisor using the provided account key.");

                    e.NextPage = this;

                    return;
                }

                store.AccountKey = key;
                StoreManager.SaveStore(store);
            }
        }

        /// <summary>
        /// Stepping into the wizard page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (store == null)
            {
                if (configurationMissing == null)
                {
                    LocateStoresMissingConfiguration();
                }

                // if there's nothing to configure, or nothing remaining, then move on
                if (configurationMissing.Count == 0)
                {
                    e.Skip = true;
                    return;
                }
                else
                {
                    // this page will be the first missing store
                    store = configurationMissing[0];

                    for (int i = 1; i < configurationMissing.Count; i++)
                    {
                        Wizard.Pages.Insert(Wizard.Pages.IndexOf(this) + 1, new ChannelAdvisorAccountKeyWizardPage() { store = configurationMissing[i] });
                    }
                }
            }

            // prepare this instance
            storeNameLabel.Text = String.Format("{0} Store: {1}", StoreTypeManager.GetType(store).StoreTypeName, store.StoreName);
        }

        /// <summary>
        /// Find the CA stores that are missing the AccountKey value
        /// </summary>
        private static void LocateStoresMissingConfiguration()
        {
            using (ChannelAdvisorStoreCollection stores = new ChannelAdvisorStoreCollection())
            {
                SqlAdapter.Default.FetchEntityCollection(stores, null);

                configurationMissing = new List<ChannelAdvisorStoreEntity>(stores.Where(s => String.IsNullOrWhiteSpace(s.AccountKey)));
            }
        }

        /// <summary>
        /// Load the ChannelAdvisor website for granting access to ShipWorks.
        /// </summary>
        private void OnClickGrantAccess(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://ssc.channeladvisor.com/support/grantAPI.php", this);
        }

        /// <summary>
        /// Toggle the account key enable/diable state
        /// </summary>
        private void OnSkipChanged(object sender, EventArgs e)
        {
            accountKey.Enabled = !skipCheckBox.Checked;
        }
    }
}
