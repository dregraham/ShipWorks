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
using ShipWorks.Stores;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Adapter.Custom;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.StorePages
{
    /// <summary>
    /// Step for letting the user configure their Yahoo store's email account.  V2 may not have had an outgoing server setup
    /// and this needs to be configured.
    /// </summary>
    public partial class YahooEmailWizardPage : WizardPage
    {
        static List<YahooStoreEntity> configurationMissing = null;

        // the store being configured by this instance
        YahooStoreEntity store;

        public YahooEmailWizardPage()
        {
            InitializeComponent();
        }

        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!skipCheckBox.Checked)
            {
                if (String.IsNullOrEmpty(store.YahooEmailAccount.OutgoingServer))
                {
                    MessageHelper.ShowError(this, "The selected email account is not completely configured.  Configure it now or check the checkbox to set it up when the upgrade completes.");

                    e.NextPage = this;
                }
            }
        }

        /// <summary>
        /// Stepping into the wizard page for the first time
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (store == null)
            {
                if (configurationMissing == null)
                {
                    LocateStoresMissingConfiguration();
                }

                // if there's nothing to configure, or nothing remaining to configure move on
                if (configurationMissing.Count == 0)
                {
                    e.Skip = true;
                    return;
                }
                else
                {
                    // This page will be the first missing store
                    store = configurationMissing[0];

                    // Add a page for each subsequent one
                    for (int i = 1; i < configurationMissing.Count; i++)
                    {
                        Wizard.Pages.Insert(Wizard.Pages.IndexOf(this) + 1, new YahooEmailWizardPage() { store = configurationMissing[i] });
                    }
                }
            }

            // prepare
            storeNameLabel.Text = String.Format("{0} Store: {1}", StoreTypeManager.GetType(store).StoreTypeName, store.StoreName);
            emailAccountControl.InitializeForStore(store);
        }

        /// <summary>
        /// Find Yahoo stores pointed to email accounts without an smtp server
        /// </summary>
        private static void LocateStoresMissingConfiguration()
        {
            using (YahooStoreCollection stores = new YahooStoreCollection())
            {

                PrefetchPath2 prefetch = new PrefetchPath2(EntityType.YahooStoreEntity);
                prefetch.Add(YahooStoreEntity.PrefetchPathYahooEmailAccount);

                SqlAdapter.Default.FetchEntityCollection(stores, null, prefetch);

                configurationMissing = new List<YahooStoreEntity>(stores.Where( s => String.IsNullOrEmpty(s.YahooEmailAccount.OutgoingServer)));
            }
        }
    }
}
