using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.ThreeDCart.WizardPages
{
    /// <summary>
    /// Wizard page for setting the store time zone
    /// </summary>
    public partial class ThreeDCartTimeZonePage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartTimeZonePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving to the next wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ThreeDCartStoreEntity store = GetStore<ThreeDCartStoreEntity>();

            store.TimeZoneID = timeZoneControl.SelectedTimeZone.Id;
        }

        /// <summary>
        /// Load the TimeZone information from the store entity
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            ThreeDCartStoreEntity store = GetStore<ThreeDCartStoreEntity>();

            timeZoneControl.SelectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(store.TimeZoneID);
        }
    }
}
