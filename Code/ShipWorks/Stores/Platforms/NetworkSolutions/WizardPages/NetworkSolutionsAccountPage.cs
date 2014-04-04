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
using Interapptive.Shared.UI;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.NetworkSolutions.WebServices;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.WizardPages
{
    /// <summary>
    /// Page for configuring the NetworkSolutions token
    /// </summary>
    public partial class NetworkSolutionsAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the wizard page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            NetworkSolutionsStoreEntity store = GetStore<NetworkSolutionsStoreEntity>();

            // no token yet
            if (store.UserToken.Length == 0)
            {
                tokenImport.InitializeForStore(store);
            }
            // we have a token
            else
            {
                panelCreate.Visible = false;

                manageTokenControl.Top = panelCreate.Top;
                manageTokenControl.Visible = true;

                manageTokenControl.InitializeForStore(store);
            }
        }

        /// <summary>
        /// Moving to the next wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            NetworkSolutionsStoreEntity store = GetStore<NetworkSolutionsStoreEntity>();

            if (store.UserToken.Length == 0)
            {
                MessageHelper.ShowError(this, "You must create a Login Token before continuing.");
                e.NextPage = this;
                return;
            }

            // retrieve store details
            Cursor.Current = Cursors.WaitCursor;
            NetworkSolutionsWebClient webClient = new NetworkSolutionsWebClient(store);
            try
            {
                SiteSettingType site = webClient.GetSiteSettings();

                store.Company = site.CompanyName;
                store.StoreName = site.CompanyName;
                store.Website = site.StoreUrl;

                // for licensing
                store.StoreUrl = site.StoreUrl;

                if (string.IsNullOrEmpty(store.StoreUrl))
                {
                    throw new NetworkSolutionsException("ShipWorks was able to connect to your store, but the Store URL returned was empty.");
                }
            }
            catch (NetworkSolutionsException ex)
            {
                MessageHelper.ShowError(this, String.Format("Unable to retrieve store details: {0}", ex.Message));
            }

            // refresh status codes so the next wizard page can use them
            try
            {
                NetworkSolutionsStatusCodeProvider statusCodes = new NetworkSolutionsStatusCodeProvider(store);
                statusCodes.UpdateFromOnlineStore();
            }
            catch (NetworkSolutionsException)
            {
                MessageHelper.ShowError(this, String.Format("ShipWorks was unable to retrieve the store status codes."));
            }
        }

        /// <summary>
        /// Import the token file instead of creating a new one
        /// </summary>
        private void OnImportTokenFile(object sender, EventArgs e)
        {
            if (NetworkSolutionsWebClient.ImportTokenFile(GetStore<NetworkSolutionsStoreEntity>(), this))
            {
                tokenImport.CancelWaiting();

                OnCreateTokenCompleted(null, EventArgs.Empty);

                MessageHelper.ShowInformation(this, "The token file has been imported.  Click 'Next' to continue.");
            }
        }

        /// <summary>
        /// Token has been imported
        /// </summary>
        private void OnCreateTokenCompleted(object sender, EventArgs e)
        {
            tokenImport.Enabled = false;
            linkImportTokenFile.Enabled = false;
        }
    }
}
