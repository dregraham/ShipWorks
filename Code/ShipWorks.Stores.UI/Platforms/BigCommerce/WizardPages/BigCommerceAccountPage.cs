using System;
using System.Linq;
using Autofac;
using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.BigCommerce.AccountSettings;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce.WizardPages
{
    /// <summary>
    /// Setup wizard page for configuring the BigCommerce account.
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.BigCommerce)]
    [Order(typeof(WizardPage), 1)]
    public partial class BigCommerceAccountPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lifetimeScope"></param>
        public BigCommerceAccountPage(BigCommerceAccountSettingsViewModel viewModel) : this()
        {
            accountSettingsControl.SetViewModel(viewModel);
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        protected void OnStepNext(object sender, WizardStepEventArgs e)
        {
            BigCommerceStoreEntity store = GetStore<BigCommerceStoreEntity>();

            // Ask the account settings control to save it's info to the store.  If anything is invalid, stay on this page.
            if (!accountSettingsControl.SaveToEntity(store))
            {
                e.NextPage = this;
            }
        }
    }
}
