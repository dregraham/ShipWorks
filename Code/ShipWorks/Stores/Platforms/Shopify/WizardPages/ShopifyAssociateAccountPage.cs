using System;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Shopify.WizardPages
{
    /// <summary>
    /// Page to associate Etsy Account token with ShipWorks Account
    /// </summary>
    public partial class ShopifyAssociateAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAssociateAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On Stepping in
        /// </summary>
        private void OnSteppingIntoAccountPage(object sender, WizardSteppingIntoEventArgs e)
        {
            ShopifyStoreEntity store = GetStore<ShopifyStoreEntity>();

            shopifyManageToken.LoadStore(store);
        }

        /// <summary>
        /// Handles Import Token Click Event. Imports Token...
        /// </summary>
        private void OnImportTokenFileClick(object sender, EventArgs e)
        {
            shopifyManageToken.ImportToken();
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNextShopifyAssociateAccountPage(object sender, WizardStepEventArgs e)
        {
            ShopifyStoreEntity store = GetStore<ShopifyStoreEntity>();
            shopifyManageToken.VerifyToken();

            if (!shopifyManageToken.IsTokenValid)
            {
                e.NextPage = this;

                MessageHelper.ShowError(this, "Please create or import an Shopify login token.");
            }
            else
            {
                if (string.IsNullOrEmpty(store.StoreName))
                {

                    using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                    {
                        var webClient = lifetimeScope.Resolve<IShopifyWebClient>(TypedParameter.From(store));
                        webClient.RetrieveShopInformation();
                    }
                }
            }
        }
    }
}