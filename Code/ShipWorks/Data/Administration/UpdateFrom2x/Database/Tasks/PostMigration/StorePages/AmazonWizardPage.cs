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
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.StorePages
{
    /// <summary>
    /// Step for letting the user update their Amazon store to using Amazon MWS
    /// </summary>
    public partial class AmazonWizardPage : WizardPage
    {
        List<AmazonStoreEntity> legacyAmazonStores = null;

        // the store being configured by this instance
        AmazonStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into this page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (store == null)
            {
                if (legacyAmazonStores == null)
                {
                    LoadAmazonStores();
                }

                if (legacyAmazonStores.Count == 0)
                {
                    e.Skip = true;
                    return;
                }
                else
                {
                    // this page will be the first missing store
                    store = legacyAmazonStores[0];
                    
                    // add a page for each subsequent store
                    foreach (AmazonStoreEntity amazonStore in legacyAmazonStores.Skip(1))
                    {
                        Wizard.Pages.Insert(Wizard.Pages.IndexOf(this) + 1, new AmazonWizardPage() { store = amazonStore });
                    }
                }
            }

            // prepare the UI
            storeNameLabel.Text = String.Format("{0} Store: {1}", StoreTypeManager.GetType(store).StoreTypeName, store.StoreName);
            mwsAccountSettings.LoadStore(store);
        }

        /// <summary>
        /// Load the amazon stores that need to be converted to MWS in order to function
        /// </summary>
        private void LoadAmazonStores()
        {
            using (AmazonStoreCollection stores = new AmazonStoreCollection())
            {
                SqlAdapter.Default.FetchEntityCollection(stores, null);

                legacyAmazonStores = new List<AmazonStoreEntity>(stores.Where(s => s.AmazonApi == (int)AmazonApi.LegacySoap));
            }
        }

        /// <summary>
        /// Stepping to the next page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (!skipCheckBox.Checked)
            {
                if (mwsAccountSettings.SaveToEntity(store))
                {
                    store.AmazonApi = (int)AmazonApi.MarketplaceWebService;
                    SqlAdapter.Default.SaveAndRefetch(store);
                }
                else
                {
                    e.NextPage = this;
                }
            }
        }
    }
}
