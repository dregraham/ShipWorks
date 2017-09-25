using System;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Etsy.WizardPages
{
    /// <summary>
    /// Page to associate Etsy Account token with ShipWorks Account
    /// </summary>
    public partial class EtsyAssociateAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyAssociateAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On Stepping in
        /// </summary>
        private void OnSteppingIntoAccountPage(object sender, WizardSteppingIntoEventArgs e)
        {
            EtsyStoreEntity store = GetStore<EtsyStoreEntity>();

            etsyManageToken.LoadStore(store);
        }

        /// <summary>
        /// Handles Import Token Click Event. Imports Token...
        /// </summary>
        private void OnImportTokenFileClick(object sender, EventArgs e)
        {
            etsyManageToken.ImportToken();
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNextEtsyAssociateAccountPage(object sender, WizardStepEventArgs e)
        {
            EtsyStoreEntity store = GetStore<EtsyStoreEntity>();

            if (!etsyManageToken.IsTokenValid)
            {
                e.NextPage = this;

                MessageHelper.ShowError(this, "Please create or import an Etsy login token.");
            }
            else
            {
                if (string.IsNullOrEmpty(store.StoreName))
                {

                    using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                    {
                        var webClient = lifetimeScope.Resolve<IEtsyWebClient>(TypedParameter.From(store));
                        webClient.RetrieveShopInformation();
                    }
                }
            }
        }
    }
}