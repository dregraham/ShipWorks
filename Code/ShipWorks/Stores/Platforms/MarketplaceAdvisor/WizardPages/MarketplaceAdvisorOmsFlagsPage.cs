using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.WizardPages
{
    /// <summary>
    /// Wizard page for deciding which flags need to be set when a MarketplaceAdvisor order is downloaded
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public partial class MarketplaceAdvisorOmsFlagsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOmsFlagsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            MarketplaceAdvisorStoreEntity store = GetStore<MarketplaceAdvisorStoreEntity>();

            if (store.AccountType != (int) MarketplaceAdvisorAccountType.OMS)
            {
                e.Skip = true;
                return;
            }
        }

        /// <summary>
        /// The page is now visible
        /// </summary>
        private void OnPageShown(object sender, EventArgs e)
        {
            MarketplaceAdvisorStoreEntity store = GetStore<MarketplaceAdvisorStoreEntity>();

            flagsControl.LoadFlags(store);
        }

        /// <summary>
        /// Stepping next from the flags page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            MarketplaceAdvisorStoreEntity store = GetStore<MarketplaceAdvisorStoreEntity>();

            store.DownloadFlags = (int) flagsControl.ReadFlags();
        }
    }
}
