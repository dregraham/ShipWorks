using System;
using System.Linq;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.BigCommerce.WizardPages
{
    /// <summary>
    /// Setup wizard page for configuring the BigCommerce account.
    /// </summary>
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
