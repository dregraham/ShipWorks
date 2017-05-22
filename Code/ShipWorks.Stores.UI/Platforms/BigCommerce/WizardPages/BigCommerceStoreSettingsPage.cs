using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce.WizardPages
{
    /// <summary>
    /// Wizard page for setting the store settings
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.BigCommerce)]
    [Order(typeof(WizardPage), 2)]
    public partial class BigCommerceStoreSettingsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceStoreSettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving to the next wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            BigCommerceStoreEntity store = GetStore<BigCommerceStoreEntity>();

            // Ask the store settings control to save it's info to the store.  If anything is invalid, stay on this page.
            if (!storeSettingsControl.SaveToEntity(store))
            {
                e.NextPage = this;
            }

            // Ask the store download criteria control to save it's info to the store.  If anything is invalid, stay on this page.
            if (!downloadCriteriaControl.SaveToEntity(store))
            {
                e.NextPage = this;
            }
        }

        /// <summary>
        /// Load the Store Settings information from the store entity
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            BigCommerceStoreEntity store = GetStore<BigCommerceStoreEntity>();
            storeSettingsControl.LoadStore(store);
            downloadCriteriaControl.LoadStore(store);
        }
    }
}
