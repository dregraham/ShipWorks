using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Shopify
{
    /// <summary>
    /// Shopify Account Settings Control
    /// </summary>
    [KeyedComponent(typeof(AccountSettingsControlBase), StoreTypeCode.Shopify)]
    public partial class ShopifyAccountSettingsMigrationControl : AccountSettingsControlBase
    {
        private readonly IShopifyCreateOrderSourceViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAccountSettingsMigrationControl(IShopifyCreateOrderSourceViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();
        }

        /// <summary>
        /// Nothing to save
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            return viewModel.Save((ShopifyStoreEntity) store);
        }

        /// <summary>
        /// Loads the store
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            viewModel.Load((ShopifyStoreEntity) store);
        }
    }
}