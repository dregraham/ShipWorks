using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ShopSite.AccountSettings;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.ShopSite.WizardPages
{
    /// <summary>
    /// Wizard page for setting up the login information for ShopSite
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.ShopSite)]
    [Order(typeof(WizardPage), 1)]
    public partial class ShopSiteAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountPage(ShopSiteAccountSettingsViewModel viewModel) : this()
        {
            accountSettingsControl.SetViewModel(viewModel);
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ShopSiteStoreEntity store = GetStore<ShopSiteStoreEntity>();

            if (!accountSettingsControl.SaveToEntity(store))
            {
                e.NextPage = this;
                return;
            }

            store.StoreName = "ShopSite Store";
            store.Website = new Uri(store.ApiUrl).Host;
        }
    }
}
