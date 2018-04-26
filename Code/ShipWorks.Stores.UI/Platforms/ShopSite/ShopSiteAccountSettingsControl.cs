using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ShopSite.AccountSettings;

namespace ShipWorks.Stores.UI.Platforms.ShopSite
{
    /// <summary>
    /// Account settings for ShopSite stores
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.ShopSite, ExternallyOwned = true)]
    public partial class ShopSiteAccountSettingsControl : AccountSettingsControlBase
    {
        private ShopSiteAccountSettingsViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountSettingsControl(ShopSiteAccountSettingsViewModel viewModel) : this()
        {
            SetViewModel(viewModel);
        }

        /// <summary>
        /// Set the view model on the control
        /// </summary>
        public void SetViewModel(ShopSiteAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
            shopSiteAccountSettings.DataContext = viewModel;
        }

        /// <summary>
        /// Load the account settings UI from the given store
        /// </summary>
        public override void LoadStore(StoreEntity store) =>
            viewModel.LoadStore(store as ShopSiteStoreEntity);

        /// <summary>
        /// Save the UI values to the given store.  Nothing is saved to the database.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store) =>
            viewModel.SaveToEntity(store as ShopSiteStoreEntity);
    }
}
