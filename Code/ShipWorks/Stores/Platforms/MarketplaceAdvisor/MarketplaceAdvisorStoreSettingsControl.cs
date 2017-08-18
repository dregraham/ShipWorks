using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// UserControl for MarketplaceAdvisor store settings
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public partial class MarketplaceAdvisorStoreSettingsControl : StoreSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorStoreSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load data from the store into the UI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            flagsControl.LoadFlags((MarketplaceAdvisorStoreEntity) store);
        }

        /// <summary>
        /// Save data from the UI into the store
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            ((MarketplaceAdvisorStoreEntity) store).DownloadFlags = (int) flagsControl.ReadFlags();

            return true;
        }
    }
}
