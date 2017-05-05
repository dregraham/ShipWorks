using System.Windows.Media;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ShopSite;

namespace ShipWorks.Stores.UI.Platforms.ShopSite
{
    /// <summary>
    /// Account settings for ShopSite stores
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.ShopSite, ExternallyOwned = true)]
    public partial class ShopSiteAccountSettingsControl : AccountSettingsControlBase
    {
        private IShopSiteWebClient webClient;
        private ShopSiteAccountSettingsViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountSettingsControl()
        {
            InitializeComponent();

            // The background would sometimes go black if not explicitly set
            shopSiteAccountSettings.Background = new SolidColorBrush(Color.FromRgb(this.BackColor.R, this.BackColor.G, this.BackColor.B));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteAccountSettingsControl(IShopSiteWebClient shopSiteWebClient)
        {
            InitializeComponent();
            webClient = shopSiteWebClient;
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
            viewModel.LoadStore(store);

        /// <summary>
        /// Save the UI values to the given store.  Nothing is saved to the database.
        /// </summary>
        public override bool SaveToEntity(StoreEntity store) =>
            viewModel.SaveToEntity(store);
    }
}
