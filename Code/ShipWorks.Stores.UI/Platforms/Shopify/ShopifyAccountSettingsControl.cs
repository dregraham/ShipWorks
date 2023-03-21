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
    public partial class ShopifyAccountSettingsControl : AccountSettingsControlBase
    {
        private readonly IShopifyAccountSettingsViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAccountSettingsControl(IShopifyAccountSettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();            
        }

        /// <summary>
        /// Nothing to save
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            // nothing to save, but underlying implementation throws
            return true;
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