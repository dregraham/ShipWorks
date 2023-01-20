using System;
using System.Linq;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Shopify.WizardPages
{
    /// <summary>
    /// Setup wizard page for configuring the Shopify account.
    /// </summary>
    public partial class ShopifyAssociateAccountPageOld : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAssociateAccountPageOld()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingIntoAccountPage(object sender, WizardSteppingIntoEventArgs e)
        {
            ShopifyStoreEntity store = GetStore<ShopifyStoreEntity>();

            createTokenControl.InitializeForStore(store);
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNextAccountPage(object sender, WizardStepEventArgs e)
        {
            if (!createTokenControl.IsAccessTokenValid)
            {
                e.NextPage = this;
                MessageHelper.ShowError(this, "Please create or import a valid token.");
                return;
            }
        }

        /// <summary>
        /// Import a token from file
        /// </summary>
        private void OnImportTokenFile(object sender, EventArgs e)
        {
            if (ShopifyTokenUtility.ImportTokenFile(GetStore<ShopifyStoreEntity>(), this))
            {
                createTokenControl.UpdateStatusDisplay();
            }
        }
    }
}
