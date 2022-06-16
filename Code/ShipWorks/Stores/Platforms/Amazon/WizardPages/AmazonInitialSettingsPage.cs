using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Amazon.WizardPages
{
    /// <summary>
    /// Select the country that an Amazon store is in
    /// </summary>
    [KeyedComponent(typeof(WizardPage), StoreTypeCode.Amazon, ExternallyOwned = true)]
    [Order(typeof(WizardPage), 1)]
    public partial class AmazonInitialSettingsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonInitialSettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Step Next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            var amazonStore = GetStore<AmazonStoreEntity>();

            if (!storeCountryControl.SaveToEntity(amazonStore) || !storeInitialDownloadDaysControl.SaveToEntity(amazonStore))
            {
                e.NextPage = this;
            }
        }
    }
}
