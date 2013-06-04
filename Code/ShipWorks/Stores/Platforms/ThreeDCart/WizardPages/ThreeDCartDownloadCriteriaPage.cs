using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.ThreeDCart.WizardPages
{
    /// <summary>
    /// Wizard page for setting the store time zone
    /// </summary>
    public partial class ThreeDCartDownloadCriteriaPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartDownloadCriteriaPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving to the next wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ThreeDCartStoreEntity store = GetStore<ThreeDCartStoreEntity>();

            threeDCartDownloadCriteriaControl.SaveToEntity(store);
        }

        /// <summary>
        /// Load the TimeZone information from the store entity
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            ThreeDCartStoreEntity store = GetStore<ThreeDCartStoreEntity>();

            threeDCartDownloadCriteriaControl.LoadStore(store);
        }
    }
}
